# GitHub Repository Search Application

A full-stack web application designed for searching GitHub repositories, featuring JWT-based authentication and a custom server-side session bookmarking service.

## Tech Stack
- **Client:** Angular (Standalone Components, Angular Material UI, Reactive Forms / RxJS)
- **Server:** ASP.NET Core (.NET Web API / Minimal APIs)
- **External API:** GitHub Search REST API
- **Security:** JSON Web Tokens (JWT) & Claims-based Authorization

## Features
1. **JWT Authentication:** Secure login flow with token issuance, client-side storage, and authorization headers on all protected endpoints.
2. **Server-Side GitHub Integration:** Proxy requests through ASP.NET Core using an authenticated/configured HttpClient (including mandatory GitHub User-Agent headers).
3. **Repository Gallery:** Displays repositories in a responsive Material Card grid showing repository name, owner avatar, stars, description, and direct repository links.
4. **Session-Based Bookmarking:** Custom thread-safe in-memory session store (ConcurrentDictionary) managing user-specific bookmarks.
5. **Bookmark Management (Bonus):** Dedicated view to browse, review, and remove saved repositories.

## Prerequisites
- .NET SDK (8.0 or later)
- Node.js (v18.x or v20.x+)
- Angular CLI (`npm install -g @angular/cli`)

## Getting Started

### 1. Run the Backend (.NET Server)
Open a terminal in the root project folder and run:
cd GithubSearch.Server
dotnet restore
dotnet run

### 2. Run the Frontend (Angular Client)
Open a new terminal window in the root project folder and run:
cd github-search-client
npm install
ng serve

Once running, navigate to http://localhost:4200/ in your browser.
