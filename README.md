# MyTrack

Personal Enterprise Productivity Portal built using the latest Microsoft technologies.

---

## Project Goal

MyTrack is a personal productivity platform created to learn enterprise software development from scratch.

The application will help me:

- Track daily work
- Generate daily, weekly, and monthly summaries
- Calculate payroll and taxes
- Generate reports
- Learn enterprise application architecture
- Practice GitHub, CI/CD, testing, and deployment

---

# Technology Stack

| Layer | Technology |
|--------|------------|
| Frontend | Angular (Later) |
| Backend | ASP.NET Core Web API (.NET 10) |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Authentication | ASP.NET Identity |
| Testing | xUnit |
| Source Control | GitHub |
| API Testing | Swagger |

---

# Development Roadmap

- [x] Stage 1 - Solution Setup
- [ ] Stage 2 - Architecture
- [ ] Stage 3 - Database
- [ ] Stage 4 - Work Log Module
- [ ] Stage 5 - Payroll Module
- [ ] Stage 6 - Reports
- [ ] Stage 7 - Authentication
- [ ] Stage 8 - Testing
- [ ] Stage 9 - CI/CD
- [ ] Stage 10 - Deployment

---

# Stage 1 - Solution Setup

## Objective

Create the initial enterprise solution and verify that the Web API runs successfully.

### Completed

- Created GitHub repository
- Created MyTrack solution
- Added ASP.NET Core Web API project
- Enabled Swagger
- Successfully ran the API locally

### Folder Structure

```text
MyTrack
│
├── MyTrack.Api
└── README.md
```

### What I Learned

- How to create a GitHub repository
- Difference between a Repository and a Solution
- How ASP.NET Core Web API starts
- What Swagger is
- What Program.cs does
- Why Visual Studio creates WeatherForecast by default

### Challenges

- Visual Studio threw an HRESULT error while creating the solution.
- Swagger wasn't configured initially.

### Resolution

- Reopened the solution successfully.
- Installed Swagger support.
- Updated Program.cs.
- Configured launchSettings.json.

---

## Commits

| Commit | Description |
|---------|-------------|
| Initial Commit | Created repository |
| Stage 1 | Created Web API and enabled Swagger |

---

## Next Stage

Enterprise Architecture