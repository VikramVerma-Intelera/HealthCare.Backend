# 🔐 Password Requirements & Validation Guide

## What Password Should User Enter?

When a user creates an account or logs in with "Password123", here are the complete password requirements:

---

## 📋 Password Requirements Overview

Your Healthcare API enforces **strong password security** standards:

| Requirement | Rule | Example |
|-------------|------|---------|
| **Minimum Length** | At least **8 characters** | ❌ pass123, ✅ Pass@123word |
| **Maximum Length** | No more than **128 characters** | ✅ up to 128 chars |
| **Uppercase** | At least **1 uppercase letter (A-Z)** | ✅ **P**ass@123 |
| **Lowercase** | At least **1 lowercase letter (a-z)** | ✅ p**ass**@123 |
| **Numbers** | At least **1 digit (0-9)** | ✅ pass@**123** |
| **Special Chars** | At least **1 special character** | ✅ pass@123 |
| **No Common Patterns** | Avoid weak passwords | ❌ password, 123456, qwerty |
| **No Sequences** | Avoid abc, 123, etc | ❌ abc123, password123 |

---

## ✅ Valid Password Examples

All of these passwords meet the requirements:

```
✅ Password123!      - All requirements met
✅ MyHealth@2024     - Strong and secure
✅ Doc@2024secure    - Good combination
✅ P@ssw0rd!Next     - Complex & secure
✅ Secure#Pass99     - Numbers + special char
✅ AdminAccess@2024  - Mixed case + special
```

---

## ❌ Invalid Password Examples

These passwords will be **rejected**:

```
❌ password          - No uppercase, numbers, or special char
❌ Password123       - No special character (!@#$%^&*)
❌ password123       - No uppercase letter
❌ PASSWORD123!      - No lowercase letter
❌ Pass@123          - Too similar to common pattern
❌ pass@123          - Only 8 chars, too simple
❌ Pass              - No numbers or special chars
❌ abc123!           - Sequential characters (abc, 123)
❌ pass123!A         - Sequential characters (123)
```

---

## 🎯 Allowed Special Characters

You can use any of these special characters:

```
! @ # $ % ^ & * ( ) _ + - = [ ] { } | ; ' : " < > , . ? /
```

**Examples:**
- `Pass!123`
- `Account@123`
- `Secure#Pass99`
- `Medical$2024`
- `Health(Care)123`

---

## 🚀 Getting Password Requirements (API)

### Endpoint 1: Get Password Requirements
```bash
GET /api/auth/password-requirements
```

**Response:**
```json
{
  "success": true,
  "data": {
    "requirements": [
      "At least 8 characters long",
      "No more than 128 characters",
      "At least one uppercase letter (A-Z)",
      "At least one lowercase letter (a-z)",
      "At least one digit (0-9)",
      "At least one special character: !@#$%^&*()_+-=[]{}|;':\"<>,.?/",
      "No common patterns (password, 123456, qwerty, etc.)",
      "No sequential characters (abc, 123, etc.)"
    ],
    "minimumLength": 8,
    "maximumLength": 128
  }
}
```

---

## ✔️ Validating Passwords (API)

### Endpoint 2: Validate Password Before Registration

**Request:**
```bash
POST /api/auth/validate-password
Content-Type: application/json

{
  "password": "Password123!"
}
```

**Response (Valid):**
```json
{
  "success": true,
  "data": {
    "isValid": true,
    "message": "Password is valid",
    "requirements": [...]
  }
}
```

**Response (Invalid):**
```json
{
  "success": true,
  "data": {
    "isValid": false,
    "message": "Password must contain at least one special character: !@#$%^&*()_+-=[]{}|;':\"<>,.?/",
    "requirements": [...]
  }
}
```

---

## 📝 Password Validation Rules Explained

### 1. Length Requirements
- **Minimum: 8 characters** - Prevents very weak passwords
- **Maximum: 128 characters** - Allows passphrases

```
✅ Password123!        - 13 characters (valid)
❌ Pass1!              - 6 characters (too short)
```

### 2. Character Type Requirements

#### Must Have Uppercase (A-Z)
```
✅ Pass123!    - P is uppercase
❌ pass123!    - No uppercase
```

#### Must Have Lowercase (a-z)
```
✅ Pass123!    - ass is lowercase
❌ PASS123!    - No lowercase
```

#### Must Have Digit (0-9)
```
✅ Pass123!    - 123 are digits
❌ Password!   - No digits
```

#### Must Have Special Character
```
✅ Pass123!    - ! is special
❌ Pass123     - No special character
```

### 3. Common Pattern Protection
Prevents weak passwords containing common words:
```
❌ password    - Blocked (common word)
❌ 123456      - Blocked (common number)
❌ qwerty      - Blocked (keyboard pattern)
❌ admin123    - Blocked (contains "admin")
```

### 4. Sequential Character Protection
Prevents obvious sequences:
```
❌ abc123      - Sequential abc and 123
❌ pass123abc  - Contains sequential 123
❌ qwerty123   - Contains sequential letters
```

---

## 🎓 Password Strategy Examples

### Example 1: Simple Format
```
Verb + Noun + Number + Special Character

MyDog@2024     ✅ Valid
- My: Custom word
- Dog: Memorable
- @: Special character
- 2024: Current year
```

### Example 2: Phrase Format
```
Take first letters of a phrase + Numbers + Special

H3alth!Care    ✅ Valid (Health Care)
- H: First letter of Health
- 3: Looks like E (l33t speak)
- alth: Rest of Health
- !: Special character
- Care: Full word
```

### Example 3: Personal Format
```
Name + Event + Number + Special

John@Holiday2024   ✅ Valid
- John: Personal
- Holiday: Important date
- 2024: Year
- @: Special character
```

---

## 🔐 For Healthcare Users

Since this is a healthcare system, consider passwords related to:

```
✅ MedCare#2024        - Medical + Care + Year
✅ Patient@Doc2024     - Related to healthcare roles
✅ Health$Care99       - Healthcare specific
✅ Clinical!Lab@2024   - Department specific
✅ Appointment#2024    - Healthcare related
```

---

## 📱 Registering with Strong Password

### Step-by-Step

```
1. Choose a memorable phrase:
   "My favorite doctor is great"

2. Take first letters:
   M f d i g → MfdIg

3. Add number and special:
   MfdIg@2024

4. Verify requirements:
   ✅ Length: 11 characters (>= 8)
   ✅ Uppercase: M, I (2 letters)
   ✅ Lowercase: f, d, g (3 letters)
   ✅ Digit: 2, 0, 2, 4 (4 numbers)
   ✅ Special: @ (1 special char)
   ✅ No common patterns
   ✅ No sequences

5. Ready to register!
```

---

## 🛠️ API Usage Example

### JavaScript Example
```javascript
// 1. Get password requirements
async function showPasswordRequirements() {
  const response = await fetch('https://localhost:5001/api/auth/password-requirements');
  const data = await response.json();
  console.log('Requirements:', data.data.requirements);
}

// 2. Validate password while typing
async function validatePassword(password) {
  const response = await fetch('https://localhost:5001/api/auth/validate-password', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ password })
  });
  
  const data = await response.json();
  return data.data.isValid;
}

// 3. Register with password
async function register(username, password) {
  // First validate
  const isValid = await validatePassword(password);
  if (!isValid) {
    alert('Password does not meet requirements');
    return;
  }
  
  // Then register
  const response = await fetch('https://localhost:5001/api/auth/register', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ username, password })
  });
  
  const data = await response.json();
  if (data.success) {
    console.log('Registered! Token:', data.data.token);
  }
}
```

---

## 🎯 Testing Passwords

### Use Swagger UI to Test

1. Navigate to: `https://localhost:5001/swagger`
2. Scroll to **AuthController**
3. Click **POST /api/auth/password-requirements**
4. Click **Try it out** → **Execute**
5. See all requirements
6. Click **POST /api/auth/validate-password**
7. Enter test password: `{"password":"Password123!"}`
8. Click **Execute** → See if valid

### Test with cURL
```bash
# Get requirements
curl https://localhost:5001/api/auth/password-requirements --insecure

# Validate password
curl -X POST https://localhost:5001/api/auth/validate-password \
  -H "Content-Type: application/json" \
  -d '{"password":"Password123!"}' \
  --insecure
```

---

## 🔒 Security Notes

### Why These Requirements?
- **Length (8+)**: Prevents brute-force attacks
- **Uppercase + Lowercase + Numbers + Special**: Increases complexity
- **No Common Patterns**: Prevents dictionary attacks
- **No Sequences**: Prevents predictable passwords

### NIST Guidelines
These requirements follow NIST SP 800-63B recommendations for:
- Length > 8 characters
- Character diversity
- No common patterns
- No sequential characters

---

## 📋 Summary

| Question | Answer |
|----------|--------|
| **What is minimum length?** | 8 characters |
| **What is maximum length?** | 128 characters |
| **Must have uppercase?** | Yes, at least 1 (A-Z) |
| **Must have lowercase?** | Yes, at least 1 (a-z) |
| **Must have numbers?** | Yes, at least 1 (0-9) |
| **Must have special char?** | Yes, at least 1 (!@#$%^&*...) |
| **Can I use spaces?** | No, special chars don't include space |
| **Can I reuse old passwords?** | Yes (not tracked in demo) |
| **How long is password valid?** | Until user changes it |

---

## ✅ Ready to Register!

**Valid Password Examples:**
- `Password123!`
- `MyHealth@2024`
- `SecurePass#99`
- `Doctor@2024!`
- `Healthcare!123`

**Try registering with any of these and enjoy secure access!**

---

**Password Security: ✅ IMPLEMENTED & ENFORCED**
**Validation: 🔐 PRODUCTION-READY**
