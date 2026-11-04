# StayFinder security design

How the API is protected, layer by layer. The structure follows the Programming Advices course
"Secure Your APIs: JWT, Roles & Policies in ASP.NET Core".

## Controls

| Layer | Control | Where |
|---|---|---|
| Transport | HTTPS redirection, HSTS outside development | `Program.cs` |
| Browser access | CORS limited to the origins listed in `Cors:AllowedOrigins` | `Program.cs`, `appsettings.json` |
| Passwords | Salted PBKDF2 hashing (ASP.NET Core Identity hasher), strength rules | `IdentityPasswordHasher`, `RegisterRequestValidator` |
| Authentication | JWT, 15 minute access tokens, signature, issuer, audience, lifetime and algorithm checked | `Program.cs`, `JwtTokenService` |
| Sessions | Refresh tokens: random, only the SHA-256 hash is stored, rotated on every use, reuse revokes all sessions | `AuthService` |
| Authorization | Roles (Guest, Host, Admin), policies `RequireHost` and `RequireAdmin` | `Policies.cs` |
| Ownership | `EnsureOwnerOrAdmin` check in services, so users only touch their own data | `CurrentUserExtensions` |
| Abuse protection | Rate limiting: 10 per minute per IP on auth endpoints, 120 per minute overall, 429 response | `Program.cs` |
| Logging | Serilog to console and rolling files, failed logins and token reuse logged, e-mails masked, no secrets or tokens | `AuthService`, `LogSanitizer` |
| Auditing | `AuditLogs` table: who, what, when, from which IP | `AuditService` |
| Secrets | JWT key kept in user-secrets (development) and environment variables or a key vault (production) | `Program.cs` |
| Errors | One middleware, generic 500 message, no stack traces to clients | `ExceptionHandlingMiddleware` |
| Mass assignment | Request DTOs only, the role is never read from a request | `RegisterRequest`, `AuthService` |

## Endpoint security map (planned)

| Endpoint | Who | Extra rule |
|---|---|---|
| `POST /api/auth/register`, `login`, `refresh`, `logout` | Public | Rate limited |
| `GET /api/users/me` | Logged in | Returns only the caller's own data |
| `GET /api/listings`, `/{id}`, reviews, availability | Public | Active listings only |
| `POST /api/users/me/become-host` | Logged in | Changes only the caller's own role |
| Create, edit, deactivate listings, images, blocked dates | Host | Owner of the listing only |
| `POST /api/bookings`, `/quote` | Logged in | Guest is the caller, cannot book own listing |
| `GET /api/bookings/mine` | Logged in | Own bookings only |
| `POST /api/bookings/{id}/cancel` | Logged in | The booking's guest, or the listing's host |
| `POST /api/bookings/{id}/confirm`, `/decline` | Host | Owner of the booked listing only |
| `GET /api/host/bookings` | Host | Bookings of own listings only |
| `POST /api/bookings/{id}/review` | Logged in | The booking's guest, stay completed, one per booking |
| Favorites | Logged in | Own favorites only |
| `/api/admin/*` | Admin | Every action written to the audit log |

When the caller does not own a resource, prefer answering 404 over 403 so the API does not reveal that the resource exists.

## Known trade-offs

- Registering with an existing e-mail returns 409, which reveals that the e-mail is registered. Rate limiting reduces abuse.
- Rate limits are per IP. Behind a proxy or load balancer, forwarded headers must be configured at deployment.
- No account lockout and no e-mail confirmation yet.
