# Multi-Tenant E-Commerce API

A REST API project for practicing ASP .NET Web API, built with **ASP.NET Core 10** and **Entity Framework Core**, deployed live on **Azure**, with SQL Server Database connection (Azure), JWT Authentication, Password Hashing etc.

Multiple companies (tenants) can run on the same platform. Each company has its own inventory and staff. Users are divided into SuperAdmin, StoreAdmin and Customers. SuperAdmins have entire access, StoreAdmins have access to their company specific areas, Customers can shop across any company. Every role sees only what they're allowed to see.

🟢 **Live at:** [https://multitenantecommerceapi20260418205629-chfqfka6enhwbef2.canadacentral-01.azurewebsites.net/scalar/v1](https://multitenantecommerceapi20260418205629-chfqfka6enhwbef2.canadacentral-01.azurewebsites.net/scalar/v1)

---

## Try it live

The API is fully interactive via the Scalar UI at the link above, no setup needed. I also had implemented Swagger but due to JWT authentication Scalar is more viable.

### Test Users, Companies and Items Values
USERS:
1. Super Admin 1: Email: suadmin1@email.com, Password: suadmin1@123, UserID (Guid): 1ED29FCD-7E3B-F111-ADFC-FE00B8F5346A
2. Store Admin 1: Email: stadmin1@email.com, Password: stadmin1@123, UserID (Guid): 246844AA-803B-F111-ADFC-FE00B8F5346A
3. Customer 1: Email: c1@email.com, Password: c1@123, UserID (Guid): 807A525B-7F3B-F111-ADFC-FE00B8F5346A

Companies:
1. Name: Snapchat, CompanyID (Guid): E46B7D1A-7F3B-F111-ADFC-FE00B8F5346A
2. Name: Microsoft, CompanyID (Guid): 1D587F30-7F3B-F111-ADFC-FE00B8F5346A

Items:
1. Name: Camera, Company: Snapchat, ItemID (Guid): 978C05FC-803B-F111-ADFC-FE00B8F5346A
2. Name: Filter, Company: Snapchat, ItemID (Guid): FF9BA504-813B-F111-ADFC-FE00B8F5346A
3. Name: Laptop, Company: Microsoft, ItemID (Guid): 47566125-813B-F111-ADFC-FE00B8F5346A
4. Name: Tablet, Company: Microsoft, ItemID (Guid): 2FEA252C-813B-F111-ADFC-FE00B8F5346A

### Step 1 — Log in

Call the login endpoint with your role, email, and password:

```
POST /api/UserLogin/login/{role}_{email}_{password}
```

Roles are: `SuperAdmin (0)`, `StoreAdmin (1)`, `Customer (2)`

The request will look like this:

```
POST /api/UserLogin/login/0_suadmin1@email.com_suadmin1@123
```

The response will look like this:

```json
{
  "token": "Bearer eyJhbGci...",
  "userId": "...",
  "email": "..."
}
```

Copy the full token value including the word `Bearer` (it is necessary for API calls and it will be active for 60 minutes after generation).

### Step 2 — Authorize in Scalar

1. For every API call, in the Key section add **Authorization**
2. Paste your token into the field, make sure it starts with `Bearer ` followed by the token
3. Only then the API call will work.
4. Read the endpoints that the Users can access (view in the document below), otherwise if wrongly authorized or not accessible, it will return `401 Unauthorized` or `403 Forbidden` respectively.

## How roles work

There are three roles. Each one can do more or less depending on their level:

| Role | What they can do |
|---|---|
| **SuperAdmin** | Everything — manage all companies, all users, all items across the whole platform |
| **StoreAdmin** | Manage their own company only — items, staff, and view their finance dashboard |
| **Customer** | Browse items, manage their cart, checkout, and view their order history |

Your role is locked into your JWT token at login. The API checks it on every request, you cannot call endpoints above your role level.

**StoreAdmin tenant isolation:** even if a StoreAdmin passes another company's ID in the URL, the API checks ownership before doing anything and returns `403 Forbidden` if it doesn't match.

---

## Tech stack

| What | Why |
|---|---|
| ASP.NET Core 10 | API framework |
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

**Tenant isolation in one place**, instead of checking company ownership in every service method, there is a single `TenantAuthorizationService`. Every controller endpoint that takes a `companyId` runs that check first. One place to maintain, impossible to forget.

**Soft deletes**, nothing is permanently deleted. Every entity has an `IsDeleted` flag. Deleted records are hidden from queries but can be restored by an admin.

**Dual primary keys**, every entity has an internal `int Id` for fast database joins, and a public facing `Guid` for URLs and API responses. This prevents users from guessing sequential IDs to enumerate data.

**Optimized database queries**, implemented database queries according to specific data needed and as required, and maintained less overhead.

**Order snapshots**, when a customer checks out, item name, price, and image are copied into the `OrderItem` record. Future changes to the item don't affect historical orders.

---

## Run it locally

**Prerequisites:** .NET 10 SDK, SQL Server

```bash
# 1. Clone
git clone https://github.com/usamaqh/Multi-Tenant-E-Commerce-API.git
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

For local you can also write as: 
```
"DefaultConnection": "Server={LocalServerName};Database={DBName};TrustServerCertificate=True;Trusted_Connection=True;"
```

---
