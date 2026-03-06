# JWT Authentication - Quick Reference & Testing Guide

## 🎯 Quick Authentication Flow

```
┌─────────────┐
│   CLIENT    │
└──────┬──────┘
       │ 1. POST /api/auth/login
       │    {"username":"admin","password":"admin123"}
       ▼
┌──────────────────┐
│  AUTH SERVICE    │
│ - Validate creds │
│ - Generate JWT   │
└──────┬───────────┘
       │ 2. Return Token
       │    eyJhbGciOiJIUzI1NiI...
       ▼
┌─────────────────────┐
│ CLIENT STORES TOKEN │
│ localStorage, etc   │
└──────┬──────────────┘
       │ 3. GET /api/patients
       │    Authorization: Bearer <token>
       ▼
┌──────────────────────┐
│  API ENDPOINT        │
│ - Validate token     │
│ - Check expiration   │
│ - Process request    │
└──────────────────────┘
```

---

## 🚀 30-Second Setup

### Start Application
```powershell
cd C:\Users\VikramVerma\source\repos\HealthCare
dotnet run
```

### Open Swagger UI
```
https://localhost:5001/swagger
```

### Test Authentication
1. Click **AuthController** section
2. Click **POST /api/auth/login**
3. Click **Try it out**
4. Enter:
   ```json
   {
     "username": "admin",
     "password": "admin123"
   }
   ```
5. Click **Execute**
6. Copy token from response

---

## 🧪 Testing Workflow

### Via Swagger UI

#### Step 1: Login
```
Controller: AuthController
Endpoint: POST /api/auth/login
Request:
{
  "username": "admin",
  "password": "admin123"
}
Response: 200 OK
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "username": "admin",
    "expiresAt": "2024-03-04T15:45:32Z",
    "tokenType": "Bearer"
  }
}
```

#### Step 2: Authorize in Swagger
- Click green **Authorize** button
- Paste: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
- Click **Authorize**

#### Step 3: Test Protected Endpoint
- Click **DepartmentsController**
- Click **GET /api/departments**
- Click **Try it out**
- Click **Execute**
- Should return: 200 OK with departments list

---

## 💻 Testing with cURL

### 1. Get Token
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}' \
  --insecure
```

### 2. Extract Token
```bash
# On Windows PowerShell
$response = curl -X POST https://localhost:5001/api/auth/login `
  -H "Content-Type: application/json" `
  -d '{"username":"admin","password":"admin123"}' `
  --insecure | ConvertFrom-Json
$token = $response.data.token
```

### 3. Use Token in Request
```bash
curl -H "Authorization: Bearer $token" \
  https://localhost:5001/api/patients \
  --insecure
```

---

## 📮 Testing with Postman

### 1. Create Login Request
```
Method: POST
URL: https://localhost:5001/api/auth/login
Body (JSON):
{
  "username": "admin",
  "password": "admin123"
}
```

### 2. Send and Copy Token
- Click **Send**
- From response, copy `data.token` value

### 3. Add Token to Header
```
Key: Authorization
Value: Bearer <paste-token-here>
```

### 4. Test Protected Endpoint
```
Method: GET
URL: https://localhost:5001/api/patients
Headers: (token already added from step 3)
```

---

## 🧩 JavaScript Client Example

```javascript
// 1. Login and store token
async function login() {
  const response = await fetch('https://localhost:5001/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      username: 'admin',
      password: 'admin123'
    })
  });
  
  const data = await response.json();
  const token = data.data.token;
  
  // Store token
  localStorage.setItem('token', token);
  localStorage.setItem('expiresAt', data.data.expiresAt);
}

// 2. Make authenticated API call
async function getPatients() {
  const token = localStorage.getItem('token');
  
  const response = await fetch('https://localhost:5001/api/patients', {
    method: 'GET',
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  
  const data = await response.json();
  return data;
}

// 3. Refresh token when needed
async function refreshToken() {
  const token = localStorage.getItem('token');
  
  const response = await fetch('https://localhost:5001/api/auth/refresh', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  
  const data = await response.json();
  const newToken = data.data.token;
  
  localStorage.setItem('token', newToken);
  localStorage.setItem('expiresAt', data.data.expiresAt);
}

// Usage
await login();           // Get token
const patients = await getPatients();  // Use token
await refreshToken();    // Get new token when needed
```

---

## 🔑 Test Credentials

| Username | Password | Use Case |
|----------|----------|----------|
| `admin` | `admin123` | Full admin access |
| `doctor` | `doctor123` | Doctor operations |
| `patient` | `patient123` | Patient data |
| `user` | `password123` | General user |

**Endpoint to get credentials:**
```bash
GET https://localhost:5001/api/auth/test-credentials
(No authentication required)
```

---

## 📊 Response Code Reference

### Success Responses
| Code | Meaning | When |
|------|---------|------|
| 200 | OK | Login successful, API call successful |
| 201 | Created | Resource created |

### Error Responses
| Code | Meaning | When | Solution |
|------|---------|------|----------|
| 400 | Bad Request | Missing credentials | Provide username & password |
| 401 | Unauthorized | Invalid credentials or token | Verify credentials or get new token |
| 404 | Not Found | Resource doesn't exist | Check URL and ID |
| 500 | Server Error | Server error | Check server logs |

---

## 🔄 Refresh Token Flow

### When Token Expires
```
Client API Call
    ↓
Server responds: 401 Unauthorized
    ↓
Client calls: POST /api/auth/refresh
    ↓
Server generates new token
    ↓
Client stores new token
    ↓
Client retries original request
```

### Auto-Refresh Implementation (JavaScript)
```javascript
async function apiCallWithRefresh(url, options = {}) {
  const token = localStorage.getItem('token');
  const expiresAt = new Date(localStorage.getItem('expiresAt'));
  
  // Check if token expires in next 5 minutes
  if (new Date() > new Date(expiresAt.getTime() - 5 * 60000)) {
    await refreshToken();
  }
  
  // Make API call with current token
  const response = await fetch(url, {
    ...options,
    headers: {
      ...options.headers,
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    }
  });
  
  return response;
}
```

---

## 🐛 Troubleshooting

### Problem: "Invalid username or password"
**Check:**
1. Username and password are correct
2. Using test credentials from table above
3. No typos in credentials

**Fix:**
1. Get test credentials: `GET /api/auth/test-credentials`
2. Try different user
3. Check spelling

### Problem: "401 Unauthorized" on API call
**Check:**
1. Token is in Authorization header
2. Format is: `Bearer <token>` (with space)
3. Token hasn't expired
4. Token is complete (full string)

**Fix:**
1. Get new token: `POST /api/auth/login`
2. Verify Authorization header format
3. Refresh token: `POST /api/auth/refresh`

### Problem: "Token validation failed"
**Check:**
1. Token not modified
2. Token not truncated
3. Secret key matches in appsettings

**Fix:**
1. Get new token from login
2. Copy entire token string
3. Verify configuration

### Problem: Can't access Swagger
**Check:**
1. Application is running
2. Using correct URL: `https://localhost:5001/swagger`
3. Not using `http://` (should be `https://`)

**Fix:**
1. Start application: `dotnet run`
2. Wait for message: "Now listening on: https://localhost:5001"
3. Navigate to URL

---

## 📋 API Endpoints Summary

### Authentication (No Token Required)
```
POST   /api/auth/login                  - Get JWT token
GET    /api/auth/test-credentials       - Get test users
```

### Token Management (Token Required)
```
POST   /api/auth/refresh                - Refresh expired token
```

### Patient Management (Token Required)
```
GET    /api/patients                    - Get all patients
GET    /api/patients/{id}               - Get patient by ID
POST   /api/patients                    - Create patient
PUT    /api/patients/{id}               - Update patient
DELETE /api/patients/{id}               - Delete patient
```

### Doctor Management (Token Required)
```
GET    /api/doctors                     - Get all doctors
GET    /api/doctors/{id}                - Get doctor by ID
POST   /api/doctors                     - Create doctor
PUT    /api/doctors/{id}                - Update doctor
DELETE /api/doctors/{id}                - Delete doctor
```

### And more...
```
(All other endpoints require token)
```

---

## ⏱️ Token Expiration

### Default Settings
- **Expiration:** 60 minutes from creation
- **Refresh:** Get new token via `/api/auth/refresh`
- **Configuration:** `appsettings.json` → `Jwt:ExpirationMinutes`

### Example Timeline
```
09:00 - User logs in, gets token (expires at 10:00)
09:30 - Using token for API calls (valid)
09:55 - Token still valid, but expires soon
10:00 - Token expires, API returns 401
10:00 - Call /api/auth/refresh to get new token
10:01 - Using new token (expires at 11:01)
```

---

## 🔐 Security Checklist

### Development ✅
- [x] Test credentials available
- [x] Default secret key set
- [x] Token generation working
- [x] Token validation working

### Production ⚠️
- [ ] Secret key changed to strong value
- [ ] HTTPS enabled
- [ ] User database with password hashing
- [ ] Rate limiting on login
- [ ] Token blacklist for logout
- [ ] Security logging enabled
- [ ] CORS properly configured

---

## 📞 Quick Links

- **Full Guide:** JWT_AUTHENTICATION_GUIDE.md
- **Implementation:** JWT_IMPLEMENTATION_SUMMARY.md
- **Complete Summary:** JWT_COMPLETE_SUMMARY.md
- **Swagger UI:** https://localhost:5001/swagger

---

## 🎯 Success Criteria

✅ **You're successful when:**
1. Application runs without errors
2. Can login via `POST /api/auth/login`
3. Receive valid JWT token in response
4. Can use token to access protected endpoints
5. Token refresh works on `/api/auth/refresh`
6. Swagger UI shows Bearer token scheme
7. Can test endpoints with token in Swagger

---

**Created: March 4, 2024**
**Status: ✅ READY FOR TESTING**
**Next: Follow the testing workflow above to verify implementation**
