# JWT Authentication Implementation Guide

## 🔐 Overview

JWT (JSON Web Token) authentication has been successfully implemented in your Healthcare API. This guide explains how to use the authentication system.

---

## 📚 What is JWT?

JWT is a stateless authentication mechanism that encodes user information in a cryptographically signed token. When a client logs in:

1. **Login** → Server validates credentials
2. **Token Generation** → Server creates a signed JWT token
3. **Token Storage** → Client stores the token (localStorage, sessionStorage, etc.)
4. **API Requests** → Client includes token in Authorization header
5. **Token Validation** → Server validates the token signature

---

## 🚀 Quick Start

### Step 1: Get a Token

**Request:**
```bash
POST https://localhost:5001/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Login successful",
  "statusCode": 200,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "username": "admin",
    "expiresAt": "2024-03-04T15:45:32.123Z",
    "tokenType": "Bearer"
  }
}
```

### Step 2: Use Token in API Requests

**Add to Authorization Header:**
```bash
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Example - Get All Patients:**
```bash
GET https://localhost:5001/api/patients
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Step 3: Refresh Token (When Expired)

**Request:**
```bash
POST https://localhost:5001/api/auth/refresh
Authorization: Bearer <your-current-token>
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Token refreshed successfully",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "username": "admin",
    "expiresAt": "2024-03-04T16:45:32.123Z",
    "tokenType": "Bearer"
  }
}
```

---

## 👥 Test Credentials

For development and testing, use these credentials:

| Username | Password | Role |
|----------|----------|------|
| admin | admin123 | Admin |
| doctor | doctor123 | Doctor |
| patient | patient123 | Patient |
| user | password123 | User |

**Get Test Credentials Endpoint:**
```bash
GET https://localhost:5001/api/auth/test-credentials
```

---

## 🔌 Authentication Endpoints

### 1. Login
```
POST /api/auth/login
```

**Request Body:**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Response:** JWT Token with expiration

**Status Codes:**
- `200 OK` - Login successful
- `401 Unauthorized` - Invalid credentials
- `400 Bad Request` - Missing credentials

### 2. Refresh Token
```
POST /api/auth/refresh
Header: Authorization: Bearer <token>
```

**Response:** New JWT Token

**Status Codes:**
- `200 OK` - Token refreshed
- `401 Unauthorized` - Invalid/expired token

### 3. Get Test Credentials
```
GET /api/auth/test-credentials
```

**Response:** List of test users (development only)

---

## 🛡️ Protected Endpoints

All API endpoints now require authentication with a valid JWT token.

### Example: Get Patients (Requires Auth)

**Without Token (401 Unauthorized):**
```bash
GET https://localhost:5001/api/patients
```

**Response:**
```json
{
  "success": false,
  "message": "Unauthorized",
  "statusCode": 401,
  "data": null
}
```

**With Token (200 OK):**
```bash
GET https://localhost:5001/api/patients
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response:**
```json
{
  "success": true,
  "message": "Patients retrieved successfully",
  "statusCode": 200,
  "data": [
    {
      "id": 1,
      "firstName": "John",
      "lastName": "Doe",
      "email": "john@example.com",
      ...
    }
  ]
}
```

---

## 🧪 Testing with Swagger

### 1. Open Swagger UI
Navigate to: `https://localhost:5001/swagger`

### 2. Login First
- Click on `AuthController` → `POST /api/auth/login`
- Enter test credentials (e.g., admin/admin123)
- Execute
- Copy the token from response

### 3. Authorize Swagger
- Click the green "Authorize" button in the top-right
- Paste: `Bearer <your-token>`
- Click "Authorize"

### 4. Test Protected Endpoints
- Now you can test any endpoint
- The token is automatically included in headers

---

## 🛠️ Testing with Postman

### 1. Create Login Request
```
POST https://localhost:5001/api/auth/login
Body (JSON):
{
  "username": "admin",
  "password": "admin123"
}
```

### 2. Copy Token
- From response, copy the `token` value

### 3. Create Authorization Header
- Go to any protected endpoint (e.g., GET /api/patients)
- Headers tab → Add new header:
  - Key: `Authorization`
  - Value: `Bearer <paste-token-here>`

### 4. Send Request
- Should now receive 200 OK with data

---

## 🛠️ Testing with cURL

```bash
# 1. Login and get token
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# 2. Use token in request
curl -H "Authorization: Bearer <your-token>" \
  https://localhost:5001/api/patients
```

---

## ⚙️ Configuration

JWT settings are configured in `appsettings.Development.json` and `appsettings.Production.json`:

```json
{
  "Jwt": {
    "SecretKey": "your-secret-key-development-only-change-in-production",
    "Issuer": "HealthCareAPI",
    "Audience": "HealthCareClient",
    "ExpirationMinutes": 60
  }
}
```

### Configuration Parameters:

| Parameter | Description | Default |
|-----------|-------------|---------|
| SecretKey | Secret key for token signing | your-secret-key... |
| Issuer | Token issuer identifier | HealthCareAPI |
| Audience | Token audience identifier | HealthCareClient |
| ExpirationMinutes | Token expiration time | 60 minutes |

---

## 🔒 Security Best Practices

### In Development:
- ✅ Use simple passwords for testing
- ✅ Use default test credentials
- ✅ Log all authentication attempts

### In Production:
- ❌ **CHANGE SECRET KEY** - Use a strong, unique key
- ✅ **Hash passwords** - Never store plain-text passwords
- ✅ **Use HTTPS** - Always transmit tokens over HTTPS
- ✅ **Implement rate limiting** - Prevent brute-force attacks
- ✅ **Add user database** - Replace in-memory user store
- ✅ **Implement refresh tokens** - Use separate short-lived tokens
- ✅ **Add role-based access control** - Restrict endpoints by role
- ✅ **Log security events** - Monitor authentication failures
- ✅ **Rotate secrets** - Change secret keys periodically
- ✅ **Use token blacklist** - Invalidate tokens on logout

---

## 📝 Token Structure

A JWT token consists of 3 parts separated by dots:

```
header.payload.signature
```

### Example Token:
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.
eyJzdWIiOiJhZG1pbiIsIm5hbWUiOiJhZG1pbiIsImlhdCI6MTcwOTYwMjkzMn0.
jqL4mLjR6n7zK8pQ9wX2yZ3aB4cD5eF6gH7iJ8kL9mN0
```

### Decode (for reference):

**Header:**
```json
{
  "alg": "HS256",
  "typ": "JWT"
}
```

**Payload (Claims):**
```json
{
  "iss": "HealthCareAPI",
  "aud": "HealthCareClient",
  "sub": "admin",
  "name": "admin",
  "iat": 1709602932,
  "exp": 1709606532
}
```

**Signature:** (cryptographically signed)

---

## 🔄 Token Lifecycle

```
┌─────────────────────────────────────────────────┐
│           Token Lifecycle                       │
└─────────────────────────────────────────────────┘

1. USER LOGIN
   └─→ POST /api/auth/login
       └─→ Credentials validated
       └─→ JWT token generated
       └─→ Return token to client

2. TOKEN STORED
   └─→ Client stores token (localStorage, memory, etc.)

3. API REQUESTS
   └─→ Include token in Authorization header
   └─→ Server validates token signature
   └─→ Server validates token expiration
   └─→ Request processed if valid

4. TOKEN EXPIRATION
   └─→ Automatic after 60 minutes (default)
   └─→ Client receives 401 Unauthorized

5. TOKEN REFRESH
   └─→ POST /api/auth/refresh
   └─→ New token generated with extended expiration
   └─→ Process repeats

6. LOGOUT (Optional)
   └─→ Client deletes stored token
   └─→ No server-side action needed (stateless)
```

---

## 🐛 Common Issues & Solutions

### Issue 1: 401 Unauthorized on API Calls

**Problem:** Token not being sent or is invalid

**Solutions:**
1. Verify token is included in Authorization header
2. Check token hasn't expired
3. Ensure format is: `Bearer <token>`
4. Get a new token by logging in again

### Issue 2: Token Validation Failed

**Problem:** Token signature or claims are invalid

**Solutions:**
1. Regenerate token (logout and login again)
2. Verify SecretKey matches in appsettings
3. Check token wasn't modified/corrupted

### Issue 3: Token Expired

**Problem:** Token lifetime has passed

**Solutions:**
1. Call refresh endpoint: `POST /api/auth/refresh`
2. Or login again to get new token
3. Implement automatic refresh in client application

### Issue 4: Invalid Credentials

**Problem:** Username or password doesn't match

**Solutions:**
1. Verify username and password are correct
2. Check test credentials: `GET /api/auth/test-credentials`
3. Ensure no typos in credentials

---

## 🚀 Client Implementation Examples

### JavaScript/TypeScript (Fetch API)

```typescript
// 1. Login
async function login(username: string, password: string) {
  const response = await fetch('https://localhost:5001/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ username, password })
  });
  
  const data = await response.json();
  if (data.success) {
    localStorage.setItem('token', data.data.token);
    return data.data.token;
  }
}

// 2. Make API call with token
async function getPatients() {
  const token = localStorage.getItem('token');
  const response = await fetch('https://localhost:5001/api/patients', {
    method: 'GET',
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  return await response.json();
}

// 3. Refresh token
async function refreshToken() {
  const token = localStorage.getItem('token');
  const response = await fetch('https://localhost:5001/api/auth/refresh', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  
  const data = await response.json();
  if (data.success) {
    localStorage.setItem('token', data.data.token);
  }
}
```

### C# HttpClient

```csharp
// 1. Login
var client = new HttpClient();
var loginRequest = new { username = "admin", password = "admin123" };
var json = JsonSerializer.Serialize(loginRequest);
var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await client.PostAsync(
  "https://localhost:5001/api/auth/login", 
  content);
var loginResponse = await response.Content.ReadAsStringAsync();

// Extract token
var jsonDoc = JsonDocument.Parse(loginResponse);
var token = jsonDoc.RootElement.GetProperty("data").GetProperty("token").GetString();

// 2. Use token in request
var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/api/patients");
request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
response = await client.SendAsync(request);
```

---

## 📋 API Response Codes

When using JWT authentication, you may receive these status codes:

| Code | Meaning | When |
|------|---------|------|
| 200 | OK | Login successful, token valid |
| 201 | Created | Resource created |
| 400 | Bad Request | Missing/invalid credentials |
| 401 | Unauthorized | Missing or invalid token |
| 403 | Forbidden | Valid token but insufficient permissions |
| 404 | Not Found | Resource not found |
| 500 | Server Error | Server-side error |

---

## 🔄 Refresh Token Strategy

For production applications, implement automatic token refresh:

```typescript
// Example: Auto-refresh before expiration
async function apiCall(url, options = {}) {
  const token = localStorage.getItem('token');
  const expiresAt = new Date(localStorage.getItem('tokenExpiresAt'));
  
  // Refresh if expiring in next 5 minutes
  if (new Date() > new Date(expiresAt.getTime() - 5 * 60000)) {
    await refreshToken();
  }
  
  // Make API call with token
  const response = await fetch(url, {
    ...options,
    headers: {
      ...options.headers,
      'Authorization': `Bearer ${token}`
    }
  });
  
  return response;
}
```

---

## 📚 Protected Endpoints

The following endpoints now require JWT authentication:

### REQUIRED AUTHENTICATION:
- ✅ GET /api/departments
- ✅ GET /api/departments/{id}
- ✅ POST /api/departments
- ✅ PUT /api/departments/{id}
- ✅ DELETE /api/departments/{id}
- ✅ GET /api/patients
- ✅ POST /api/patients
- ... (all other API endpoints)

### NO AUTHENTICATION REQUIRED:
- ❌ POST /api/auth/login
- ❌ GET /api/auth/test-credentials

---

## ⚙️ Production Checklist

Before deploying to production:

- [ ] Change `Jwt:SecretKey` to a strong, unique value
- [ ] Implement proper user database with password hashing (bcrypt)
- [ ] Move credentials from in-memory dictionary to database
- [ ] Implement password hashing and verification
- [ ] Add rate limiting for login endpoint
- [ ] Implement token blacklist for logout
- [ ] Add HTTPS/TLS enforcement
- [ ] Implement role-based access control (RBAC)
- [ ] Add security logging and monitoring
- [ ] Implement refresh token rotation
- [ ] Add CORS restrictions (specify allowed origins)
- [ ] Test token expiration and refresh flows
- [ ] Implement token revocation mechanism
- [ ] Add audit logging for authentication events

---

## 📞 Support

For issues or questions about JWT implementation:

1. Check the **Common Issues** section above
2. Review the configuration in `appsettings.json`
3. Check server logs for authentication errors
4. Verify token format in Authorization header

---

**JWT Authentication Implementation Complete! ✅**

Your API is now protected with industry-standard JWT authentication.
