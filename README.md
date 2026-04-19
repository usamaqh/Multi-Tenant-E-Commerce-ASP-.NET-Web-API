# Multi-Tenant E-Commerce API

A production-deployed REST API built with **ASP.NET Core** and **Entity Framework Core**, implementing a multitenant e-commerce platform where multiple companies can operate independently on a shared infrastructure. Live on **Azure App Service** with a **SQL Server** backend.

---

## What it does

The API supports three distinct user roles — SuperAdmin, StoreAdmin, and Customer — each with their own scoped permissions. Multiple companies (tenants) can exist on the platform simultaneously. Each company manages its own inventory, and customers can browse items, manage a cart, and place orders across any company.

A dedicated tenant authorization layer ensures that StoreAdmins can only read and modify data belonging to their own company, regardless of what IDs they pass in the request.

---

## Tech stack

- **ASP.NET Core 8** — REST API framework
- **Entity Framework Core** — ORM with SQL Server provider
- **SQL Server on Azure** — relational database
- **Azure App Service** — hosting
- **JWT (HMAC-SHA256)** — stateless authentication
- **ASP.NET Core Identity PasswordHasher** — password hashing

---

## Architecture

```
Controllers
    └── extract claims → call TenantAuthorizationService → call Service layer

TenantAuthorizationService
    └── single source of truth for company ownership checks

Service layer (CompanyService, ItemsService, UserService, CustomerCartService, FinanceService)
    └── business logic + EF Core queries → DTOs

AppDbContext (EF Core)
    └── SQL Server
```

The API uses a **layered service pattern** with constructor-injected dependencies throughout. All services are registered as scoped in `Program.cs` to align with the EF Core `DbContext` lifetime.

---

## Role hierarchy

| Role | What they can do |
|---|---|
| **SuperAdmin** | Full access — manage all companies, all users, all items |
| **StoreAdmin** | Manage their own company's items, users, and view finance dashboard |
| **Customer** | Browse items, manage cart, checkout, view purchase history |

Role is embedded in the JWT at login and validated on every request via `[Authorize(Roles = ...)]`. Tenant isolation for StoreAdmin is enforced separately in `TenantAuthorizationService` before any service method is called.

---

## Key design decisions

### Tenant isolation
Rather than scattering company-ownership checks across individual services, all authorization logic lives in a single `TenantAuthorizationService`. Every controller endpoint that accepts a `companyId` runs `CanAccessCompany(userId, role, companyId)` before calling the service layer. SuperAdmins bypass the check; StoreAdmins must own the company; Customers are always denied at this layer.

### Soft deletes
No data is permanently deleted. All entities carry an `IsDeleted` flag and all queries filter on `!IsDeleted`. Admins can restore deleted records via undelete endpoints.

### Dual primary keys
Every entity uses an `int Id` as the actual database primary key (clustered index, efficient joins) alongside a `Guid` public search key exposed in URLs and responses. This prevents sequential ID enumeration in the API while keeping the database fast.

### EF Core query hygiene
- `.AsNoTracking()` on all read-only queries — eliminates change tracker overhead when projecting to DTOs
- `.Select()` projections push column selection to SQL — only the fields needed by the DTO are fetched
- `AnyAsync()` used instead of `FirstOrDefaultAsync()` for existence checks — no column data fetched
- Tracked entities use implicit change detection — no redundant `.Update()` calls before `SaveChangesAsync()`

### Cart + stock consistency
Stock is decremented atomically with the cart write in a single `SaveChangesAsync()` call. The `UpdateCartItem` method restores the previous quantity to stock before applying the new quantity, keeping inventory counts accurate across updates and removals. When a cart is emptied, the cart record itself is removed.

### N+1 prevention
The checkout and cart removal flows load all cart items and their related item entities in a single query using `.Include().ThenInclude()`, then operate on the in-memory graph. No per-item database queries inside loops.

### Finance reporting
`GetCustomerPurchaseHistory` uses a correlated subquery inside `Select()` to load orders and their line items in a single database round-trip. `GetCompanyFinanceDashboard` aggregates revenue and sold units from the `Items` table, which are updated at checkout time rather than computed on the fly.

---

## Endpoints

### Authentication
| Method | Route | Access |
|---|---|---|
| POST | `/api/UserLogin/login/{role}_{email}_{password}` | Public |

Returns a `Bearer` JWT token and basic user info. Token expires after 60 minutes.

### Companies
| Method | Route | Access |
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
| Method | Route | Access |
|---|---|---|
| GET | `/api/Items/get_items/{companyId}` | SuperAdmin, StoreAdmin (own company) |
| GET | `/api/Items/get_item_by_id/{companyId}_{itemId}` | SuperAdmin, StoreAdmin (own company) |
| GET | `/api/Items/get_item_by_name/{companyId}_{itemName}` | SuperAdmin, StoreAdmin (own company) |
| POST | `/api/Items/add` | SuperAdmin, StoreAdmin (own company) |
| PUT | `/api/Items/update/{companyId}_{itemId}` | SuperAdmin, StoreAdmin (own company) |
| DELETE | `/api/Items/delete/{companyId}_{itemId}` | SuperAdmin, StoreAdmin (own company) |
| PUT | `/api/Items/undelete/{companyId}_{itemId}` | SuperAdmin, StoreAdmin (own company) |

### Users
| Method | Route | Access |
|---|---|---|
| GET | `/api/User/get_all` | SuperAdmin, StoreAdmin (own company users) |
| GET | `/api/User/get_all_by_roll/{userRole}` | SuperAdmin |
| GET | `/api/User/get_by_userid/{userId}` | SuperAdmin, StoreAdmin |
| GET | `/api/User/get_by_name/{fname}_{lname}` | SuperAdmin, StoreAdmin, Customer (own profile) |
| GET | `/api/User/get_by_email/{email}` | SuperAdmin, StoreAdmin, Customer (own profile) |
| POST | `/api/User/add` | SuperAdmin, StoreAdmin |
| PUT | `/api/User/update/{userId}` | SuperAdmin, StoreAdmin, Customer (own profile) |
| DELETE | `/api/User/delete/{userId}` | SuperAdmin, StoreAdmin, Customer (own profile) |
| PUT | `/api/User/undelete/{userId}` | SuperAdmin |

### Cart
| Method | Route | Access |
|---|---|---|
| GET | `/api/CustomerCart/get_cart_details` | All roles |
| POST | `/api/CustomerCart/add_item` | Customer |
| PUT | `/api/CustomerCart/update_item` | Customer |
| DELETE | `/api/CustomerCart/remove_item/{cartItemId}` | Customer |
| DELETE | `/api/CustomerCart/remove_cart` | All roles |
| GET | `/api/CustomerCart/checkout` | Customer |

### Finance
| Method | Route | Access |
|---|---|---|
| GET | `/api/Finances/get_customer_purchase_history` | All roles |
| GET | `/api/Finances/get_company_finance_dashboard/{companyId}` | SuperAdmin, StoreAdmin (own company) |

---

## Data model

```
User ──────────────── Company
 │  (StoreAdmin has    │
 │   CompanyId Guid)   │
 │                     │
 └── Cart              └── Item
      └── CartItem          └── CartItem
           └── Item
                
Order
 └── OrderItem (snapshot of item data at purchase time)
```

`OrderItem` stores a point-in-time snapshot of item name, price, and image at the moment of purchase — so order history is unaffected by future item edits or deletions.

---

## Running locally

**Prerequisites:** .NET 8 SDK, SQL Server (local or Azure)

1. Clone the repo
```bash
git clone https://github.com/your-username/Multi-Tenant-E-Commerce-API.git
cd Multi-Tenant-E-Commerce-API
```

2. Copy the example config and fill in your values
```bash
cp appsettings.example.json appsettings.json
```

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;..."
  },
  "Jwt": {
    "Key": "your-secret-key-min-32-chars",
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "Subject": "your-subject"
  }
}
```

3. Apply migrations
```bash
dotnet ef database update
```

4. Run
```bash
dotnet run
```

Swagger UI available at `https://localhost:{port}/swagger` in development.

---

## What I'd add next

- **Refresh tokens** — current JWTs expire after 60 minutes with no renewal path
- **Pagination** on list endpoints — `GetAllUsers` and `GetAllItems` return unbounded result sets
- **Integration tests** — service layer has no test coverage currently
- **Rate limiting** on the login endpoint — no brute-force protection at the moment
- **Blob storage for images** — currently saved to `wwwroot/uploads` on the server filesystem, which doesn't survive Azure App Service restarts without a persistent volume
