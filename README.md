# GitHub Repository Analyzer

This project is a .NET 8 web API that connects to the GitHub API to fetch statistics from the `lodash/lodash` repository. It analyzes the frequency of letters in the JavaScript/TypeScript files, sorted in decreasing order.

## Features
- Fetches JavaScript/TypeScript files from a GitHub repository.
- Analyzes letter frequency in the file contents.
- Error handling middleware for robust API responses.

---

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- GitHub Personal Access Token (PAT) with `public_repo` scope

---

## Setup

### 1. Clone the Repository
```bash
git clone https://github.com/omidrh/Infinit.Assessment.git
cd repository-name
```

### 2. Configure the Application

Create a `appsettings.Development.json` file:
```json
"GitHub": {
	"PersonalAccessToken": "your_personal_access_token"
}
```
Add it to `.gitignore` if it's not there:
```plaintext
appsettings.Development.json
```

### 3. Build and Run
```bash
dotnet build
dotnet run --project Infinit.Assessment.Api
```

---

## API Endpoints

### Base URL
`https://localhost:5001/api/stats`

### Endpoints
1. **GET `/filtered-files`**
   - Fetches JavaScript/TypeScript files.
   - Example:
     ```json
     {
       "e": 262122,
       "t": 211570,
       "a": 165579,
       "r": 163042,
       "n": 140384,
       "s": 139006,
       . . .
     }
     ```

2. **POST `/analyze`**
   - Analyzes letter frequency.
   - **Request Body**:
     ```json
     {
       "repositoryOwner": "lodash",
       "repositoryName": "lodash",
       "branch": "main"
     }
     ```
   - **Response**:
     ```json
     [
       {
         "path": ".markdown-doctest-setup.js",
         "type": "blob",
         "size": 218,
         "url": "https://api.github.com/repos/lodash/lodash/git/blobs/cdb0bbb5c7f42d4fcf7338db60d5891de11cf78d"
       },
       {
         "path": "dist/lodash.core.js",
         "type": "blob",
         "size": 115957,
         "url": "https://api.github.com/repos/lodash/lodash/git/blobs/be1d567d629fdfda05efcaa6adfb480d93702889"
       },
       . . .
     ]
     ```

---

## Testing
Run tests with:
```bash
dotnet test
```

---

## Notes
- Ensure the PAT is set in `appsettings.Development.json`.
- Use `.gitignore` to prevent sensitive data from being committed.

---

## Folder Structure
```
Infinit.Assessment
├── Infinit.Assessment.Api        # API project
├── Infinit.Assessment.Services   # Services
└── Infinit.Assessment.Tests      # Tests
```

---

## Author
Omid REZAEI HANJANI
