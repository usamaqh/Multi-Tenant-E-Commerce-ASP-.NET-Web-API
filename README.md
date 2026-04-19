# Multi-Tenant E-Commerce API

A REST API built with **ASP.NET Core 10** and **Entity Framework Core**, deployed live on **Azure**.

Multiple companies (tenants) can run on the same platform. Each company has its own inventory and staff. Users are divided into SuperAdmin, StoreAdmin and Customers. SuperAdmins have entire access, StoreAdmins have access to their company specific areas, Customers can shop across any company. Every role sees only what they're allowed to see.

🟢 **Live at:** [https://multitenantecommerceapi20260418205629-chfqfka6enhwbef2.canadacentral-01.azurewebsites.net/scalar/v1](https://multitenantecommerceapi20260418205629-chfqfka6enhwbef2.canadacentral-01.azurewebsites.net/scalar/v1)

---

## Try it live

The API is fully interactive via the Scalar UI at the link above — no setup needed.

### Step 1 — Log in

Call the login endpoint with your role, email, and password:

```
POST /api/UserLogin/login/{role}_{email}_{password}
```

Roles are: `SuperAdmin`, `StoreAdmin`, `Customer`

The response will look like this:

```json
{
  "token": "Bearer eyJhbGci...",
  "userId": "...",
  "email": "..."
}
```

Copy the full token value including the word `Bearer`.

### Step 2 — Authorize in Scalar

1. Click the **Authorize** button (🔒) at the top of the Scalar page
2. Paste your token into the field — make sure it starts with `Bearer ` followed by the token
3. Click **Authorize**

All subsequent requests will now include your token automatically.

### Step 3 — Call endpoints

You're now authenticated. Every endpoint in the Scalar UI shows which roles can access it. Trying to call an endpoint your role doesn't have access to will return `401 Unauthorized` or `403 Forbidden`.

---

## How roles work

There are three roles. Each one can do more or less depending on their level:

| Role | What they can do |
|---|---|
| **SuperAdmin** | Everything — manage all companies, all users, all items across the whole platform |
| **StoreAdmin** | Manage their own company only — items, staff, and view their finance dashboard |
| **Customer** | Browse items, manage their cart, checkout, and view their order history |

Your role is locked into your JWT token at login. The API checks it on every request — you cannot call endpoints above your role level.

**StoreAdmin tenant isolation:** even if a StoreAdmin passes another company's ID in the URL, the API checks ownership before doing anything and returns `403 Forbidden` if it doesn't match.

---

## Tech stack

| What | Why |
|---|---|
| ASP.NET Core 8 | API framework |
| Entity Framework Core | Database ORM |
| SQL Server on Azure | Database |
| Azure App Service | Hosting |
| JWT (HMAC-SHA256) | Authentication tokens |
| ASP.NET Core PasswordHasher | Password hashing |

---

## Endpoints

### Authentication
| Method | Route | Who |
|---|---|---|
| POST | `/api/UserLogin/login/{role}_{email}_{password}` | Public |

### Companies
| Method | Route | Who |
|---|---|---|
| GET | `/api/Company/get_all` | SuperAdmin |
| GET | `/api/Company/get_by_id/{companyId}` | SuperAdmin |
| GET | `/api/Company/get_by_name/{companyName}` | SuperAdmin |
| GET | `/api/Company/get_by_adminid/{adminId}` | SuperAdmin |
| POST | `/api/Company/add` | SuperAdmin |
| PUT | `/api/Company/update/{companyId}` | SuperAdmin |
| DELETE | `/api/Company/delete/{companyId}` | SuperAdmin |
| PUT | `/api/Company/undelete/{companyId}` | SuperAdmin |

### Items
| Method | Route | Who |
|---|---|---|
| GET | `/api/Items/get_items/{companyId}` | SuperAdmin, StoreAdmin (own company) |
| GET | `/api/Items/get_item_by_id/{companyId}_{itemId}` | SuperAdmin, StoreAdmin (own company) |
| GET | `/api/Items/get_item_by_name/{companyId}_{itemName}` | SuperAdmin, StoreAdmin (own company) |
| POST | `/api/Items/add` | SuperAdmin, StoreAdmin (own company) |
| PUT | `/api/Items/update/{companyId}_{itemId}` | SuperAdmin, StoreAdmin (own company) |
| DELETE | `/api/Items/delete/{companyId}_{itemId}` | SuperAdmin, StoreAdmin (own company) |
| PUT | `/api/Items/undelete/{companyId}_{itemId}` | SuperAdmin, StoreAdmin (own company) |

### Users
| Method | Route | Who |
|---|---|---|
| GET | `/api/User/get_all` | SuperAdmin, StoreAdmin (own company) |
| GET | `/api/User/get_all_by_roll/{userRole}` | SuperAdmin |
| GET | `/api/User/get_by_userid/{userId}` | SuperAdmin, StoreAdmin |
| GET | `/api/User/get_by_name/{fname}_{lname}` | SuperAdmin, StoreAdmin, Customer (own profile) |
| GET | `/api/User/get_by_email/{email}` | SuperAdmin, StoreAdmin, Customer (own profile) |
| POST | `/api/User/add` | SuperAdmin, StoreAdmin |
| PUT | `/api/User/update/{userId}` | SuperAdmin, StoreAdmin, Customer (own profile) |
| DELETE | `/api/User/delete/{userId}` | SuperAdmin, StoreAdmin, Customer (own profile) |
| PUT | `/api/User/undelete/{userId}` | SuperAdmin |

### Cart
| Method | Route | Who |
|---|---|---|
| GET | `/api/CustomerCart/get_cart_details` | All roles |
| POST | `/api/CustomerCart/add_item` | Customer |
| PUT | `/api/CustomerCart/update_item` | Customer |
| DELETE | `/api/CustomerCart/remove_item/{cartItemId}` | Customer |
| DELETE | `/api/CustomerCart/remove_cart` | All roles |
| GET | `/api/CustomerCart/checkout` | Customer |

### Finance
| Method | Route | Who |
|---|---|---|
| GET | `/api/Finances/get_customer_purchase_history` | All roles |
| GET | `/api/Finances/get_company_finance_dashboard/{companyId}` | SuperAdmin, StoreAdmin (own company) |

---

## Notable design decisions

**Tenant isolation in one place** — instead of checking company ownership in every service method, there is a single `TenantAuthorizationService`. Every controller endpoint that takes a `companyId` runs that check first. One place to maintain, impossible to forget.

**Soft deletes** — nothing is permanently deleted. Every entity has an `IsDeleted` flag. Deleted records are hidden from queries but can be restored by an admin.

**Dual primary keys** — every entity has an internal `int Id` for fast database joins, and a public-facing `Guid` for URLs and API responses. This prevents users from guessing sequential IDs to enumerate data.

**No N+1 queries** — checkout and cart operations load everything needed in one query using `.Include().ThenInclude()`, then work in memory. No database call per item inside a loop.

**Order snapshots** — when a customer checks out, item name, price, and image are copied into the `OrderItem` record. Future changes to the item don't affect historical orders.

**Single `SaveChanges` per operation** — all mutations within one request are committed together in one database round-trip. No partial writes.

---

## Run it locally

**Prerequisites:** .NET 8 SDK, SQL Server

```bash
# 1. Clone
git clone https://github.com/your-username/Multi-Tenant-E-Commerce-API.git
cd Multi-Tenant-E-Commerce-API

# 2. Set up config
cp appsettings.example.json appsettings.json
# Fill in your connection string and JWT values in appsettings.json

# 3. Apply database migrations
dotnet ef database update

# 4. Run
dotnet run
```

Scalar UI will be available at `https://localhost:{port}/scalar/v1`.

### Config reference (`appsettings.example.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:{ServerName};Initial Catalog={DBName};User ID={UserID};Password={Password};Encrypt=True;"
  },
  "Jwt": {
    "Key": "{your-secret-key-minimum-32-characters}",
    "Issuer": "{Issuer}",
    "Audience": "{Audience}",
    "Subject": "{Subject}"
  }
}
```

---

## What I'd improve next

- **Refresh tokens** — JWT tokens expire after 60 minutes with no way to renew without logging in again
- **Pagination** — list endpoints like `get_all` return all records with no limit
- **Integration tests** — no automated test coverage on the service layer yet
- **Rate limiting** on login — no protection against brute-force password attempts
- **Azure Blob Storage for images** — images are currently stored on the server filesystem, which resets on Azure App Service restarts
