# VMS — Volunteer Management System (CIS 4327 Pre-Semester Assignment)

Admin features for a volunteer management system: **Manage Volunteer Profiles** and **Manage Opportunities**. Due Aug 18.

See `docs/project-plan.md` for the full technical plan, role split, and timeline.

## Stack

- ASP.NET Core (.NET 10) MVC
- EF Core (code-first + migrations) on SQLite
- Simple cookie auth (single seeded admin — no ASP.NET Identity)

## Getting started

Prereqs: [Git](https://git-scm.com/downloads) and the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)


```bash
git clone https://github.com/yeeron/CIS4327-Pre-Semester-Assignment.git
cd CIS4327-Pre-Semester-Assignment
dotnet restore
dotnet tool install --global dotnet-ef   # once, if you don't have it
dotnet ef database update --project VolunteerMS   # creates vms.db. SKIP until migration #1 is merged
dotnet run --project VolunteerMS
```
Then open the localhost URL it prints. Stop the app with Ctrl+C.

The SQLite database (`vms.db`) is git-ignored — everyone generates their own from migrations. Never commit it.

## Workflow

- Branch off `main` → small PRs → 1 review → merge. No direct pushes to `main`.
- Blocked more than 30 minutes? Post in the group chat.

## Assumptions

The assignment allows assumptions about missing requirements. We document every one here as we make them:

- Volunteers do not log in; the username/password fields on a volunteer profile are data managed by the admin.
- "Volunteer opportunity matches" are created manually by the admin (assign/remove). *(May upgrade to criteria-based auto-matching if time allows.)*
- (add more as decided)
