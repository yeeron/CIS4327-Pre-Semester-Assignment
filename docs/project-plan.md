# VMS Pre-Semester Project — Plan & Role Selection

**CIS 4327 Senior Project I · Due Aug 18 · Team of 4**

This doc covers the technical decisions made so far (with the reasoning), the proposed work split, and the timeline. Read the whole thing, then claim a role at the bottom.

---

## What we're building

The admin portion of a Volunteer Management System, per the two use cases in the assignment doc:

1. **Manage Volunteer Profiles** — admin login, volunteer list with filters (Approved / Pending / Disapproved / Inactive / All), search, add/edit volunteers (~18-field profile), view a volunteer's opportunity matches.
2. **Manage Opportunities** — opportunity list with filters (Most Recent 60 days / By Center), search, add/edit/delete opportunities, view an opportunity's matched volunteers.

The spec allows us to make assumptions where it's vague — we'll document every assumption in the README, since mentors read that.

---

## Decisions made (and why)

| Decision | Choice | Why |
|---|---|---|
| Framework | **ASP.NET Core MVC** | It's what we all built with in class. Controllers/views split cleanly across 4 people. |
| Data access | **EF Core, code-first + migrations** | The repo *is* the database — `dotnet ef database update` and everyone has an identical schema. No passing DB files around. |
| Database | **SQLite** | Single file, zero install, identical on every OS. |
| Auth | **Simple cookie login** (no ASP.NET Identity) | Spec needs exactly one thing: admin logs in, gets a session, `[Authorize]` protects admin pages. Identity would add ~15 tables of machinery we'd never use. Password is hashed with the built-in `PasswordHasher` — no plaintext. |
| Volunteer login | **Not building it** | Volunteers have username/password *fields* in their profile, but per the spec they never log in. Those are just data the admin edits. |
| Matching | **Manual, minimal** | A `VolunteerOpportunity` join table + an "Assign" button. "View Matches" reads from it. If we finish early, auto-matching (by shared Center/Skills) is a bolt-on LINQ query — the schema below is designed so nothing needs ripping out. |

## Schema (the contract we all build against)

Core entities:

- **Volunteer** — the big profile (name, username, hashed password, address, phones, email, education, licenses, emergency contact fields, DL-on-file / SS-card-on-file flags, ApprovalStatus enum, IsActive flag)
- **Opportunity** — name, description, date, **Center FK**
- **Center** — real table, not a text field
- **Skill** — real table, not a text field
- **VolunteerCenter**, **VolunteerSkill** — many-to-many joins (checkbox lists on the forms)
- **VolunteerOpportunity** — the match join table
- **AdminUser** — username + hashed password, seeded

Availability, educational background, and licenses stay as plain text fields — nothing ever queries them. Centers and Skills are real tables *now* because retrofitting them later (if we add auto-match) would mean migrations + form rewrites; doing it up front costs ~2 hours.

---

## How we split the work

**Rule: vertical slices, not layers.** Everyone owns a feature end-to-end (model → controller → views). Nobody does "just the views" — that makes every feature depend on three people and nobody able to demo anything alone.

**Week 1 is not split four ways.** One or two people build the foundation; four people on an empty repo just merge-conflict each other in `Program.cs`.

### The roles — pick one

**🔧 Foundation (Week 1, then becomes Matching + Glue)**
Project skeleton, EF models + first migration, cookie auth, base layout/nav, seed data. After week 1: the `VolunteerOpportunity` join table, assign/remove UI, both "View Matches" screens, PR review, unblocking people. *[Dave — taking this one as lead unless someone objects]*

**🧑‍🤝‍🧑 Volunteers CRUD** — the biggest slice
Volunteer list page, Add + Edit forms (all ~18 fields, checkbox lists for Centers/Skills), server-side validation, cancel flows, "return to where you were" navigation. The heaviest lift on the project — the 18-field form with validation is genuinely fiddly.

**📋 Opportunities CRUD** — same shape, smaller
Opportunity list, Add/Edit forms, **Delete** (with confirm), validation, cancel flows. Roughly 60% the size of the Volunteers slice — the form is much smaller. Whoever takes Volunteers CRUD produces patterns this slice can reuse (and vice versa — coordinate so you're not both inventing the same form layout).

**🔍 Filters + Search (both pages)**
The 6 volunteer filters, the 2 opportunity filters, both search boxes, the "no results found" messages, keeping filter/search state when you navigate away and back. Isolated by nature — it's query logic layered on top of the two list pages. Depends on the CRUD list pages existing, so week 2 starts with helping seed test data / writing the filter queries against the schema directly.

### Timeline

| Week | What |
|---|---|
| **1** (now → ~Jul 24) | Foundation built + PR'd. Everyone else: clone, run, migrate, app boots on your machine. Review the schema PR so you understand the models. |
| **2–3** (→ ~Aug 7) | Build your slice. Small PRs, reviewed within 24h. If you're blocked >30 min, say so in the group chat — don't sit on it. |
| **4** (→ Aug 15) | **Feature freeze.** Validation edge cases, error messages, README (setup steps + documented assumptions), demo run-through as a team. |
| Aug 18 | Submit repo link on Canvas, share with mentor. |

~5 hrs/week each per the assignment = ~90 person-hours total. Enough if week 1 doesn't get wasted; tight if it does.

---

## Claim your role

Reply in the group chat with your pick, in order of preference. If two people want the same slice, whoever's less confident takes Opportunities CRUD (it's the best one to learn from since Volunteers CRUD is the same pattern at larger scale).

Open questions welcome — this plan is a draft until we all agree.
