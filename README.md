# GitHub Repository Search Application

A full-stack web application for searching GitHub repositories, authenticating users with JWT, and saving repositories to a custom server-side in-memory bookmark store.

## Features

- **Repository Search:** Search GitHub repositories by keyword through the backend.
- **Repository Gallery:** Display repository names, owner avatars, descriptions, star counts, and links to GitHub.
- **JWT Authentication:** Authenticate users and protect bookmark endpoints.
- **Bookmark Management:** Add and remove bookmarked repositories.
- **Bookmarks Page:** View and manage saved repositories in a dedicated page.
- **User-Specific Storage:** Keep bookmarks separated by user in a custom in-memory server-side store.

## Technology Stack

### Frontend
- Angular 21
- TypeScript
- Angular Material
- RxJS

### Backend
- ASP.NET Core .NET 10
- Minimal APIs
- `HttpClient` for GitHub API requests
- JWT authentication and claims-based authorization
- Custom in-memory bookmark storage using `ConcurrentDictionary`

### External API
GitHub Search API:

`https://api.github.com/search/repositories?q={keyword}`

The frontend sends search requests to the backend, which communicates with the GitHub API.

## Prerequisites

Install the following before running the application:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) — a version supported by Angular 21
- npm, included with Node.js

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/yaeligo/test_1.git
cd test_1
```

### 2. Configure the backend

Open a terminal in the repository root and navigate to the server project:

```bash
cd GithubSearch.Server
```

The application requires the following configuration values:

- `Jwt:SecretKey` — a secret key of at least 32 bytes.
- `DemoAuth:Username` — the demo login username.
- `DemoAuth:Password` — the demo login password.

For local development, you can set these values using PowerShell before starting the backend:

```powershell
$env:Jwt__SecretKey = "replace-with-a-local-secret-key-of-32-chars-or-more"
$env:DemoAuth__Username = "demo"
$env:DemoAuth__Password = "replace-with-a-local-demo-password"
```

These are example values for local development. Use the same username and password when signing in to the application.

Do not commit real credentials or production secrets to GitHub.

### 3. Run the backend

From the `GithubSearch.Server` directory, run:

```bash
dotnet restore
dotnet run --urls http://localhost:5000
```

Keep this terminal open while using the application.

### 4. Run the frontend

Open a second terminal in the repository root:

```bash
cd github-search-client
npm install
npm start
```

If the project does not define an `npm start` script, run:

```bash
npx ng serve
```

### 5. Open the application

Navigate to:

[http://localhost:4200](http://localhost:4200)

Sign in using the demo credentials configured for the backend.

## Demo Login Credentials

Use the following credentials to sign in to the application:

Username: demo

Password: demopass123

These credentials are intended for demonstration and testing purposes only.

## Application Workflow

1. Sign in to the application.
2. Enter a repository keyword in the search field.
3. Press the Search button or Enter.
4. Browse the matching repositories.
5. Use the bookmark button to save or remove a repository.
6. Open the Bookmarks page to view and manage saved repositories.

## Bookmark Storage

Bookmarks are stored on the server in a custom in-memory store, organized by user identity.

This implementation does not use a database. Because the data is held in memory, bookmarks are lost when the backend process restarts.

## Project Structure

```text
test_1/
├── GithubSearch.Server/
│   └── ASP.NET Core backend and Minimal APIs
├── github-search-client/
│   └── Angular frontend
└── README.md
```

## Notes

- The backend must be running for search, authentication, and bookmark operations to work.
- The frontend is configured to communicate with the backend at `http://localhost:5000`.
- The application uses the GitHub Search API to retrieve repository results.
