# Insecure .NET Web Application - Security Training Exercise

⚠️ **WARNING: This application contains intentional security vulnerabilities and should NEVER be deployed to production or exposed to the internet!**

## Overview

This application demonstrates common security vulnerabilities in .NET web applications. It serves as a training exercise for developers to:

1. **Identify security risks** in the codebase
2. **Propose architecture-level fixes** following OWASP guidelines
3. **Implement secure coding practices** aligned with OWASP ASVS controls

## Security Vulnerabilities Present

### 🔴 Critical Vulnerabilities

1. **Command Injection (AdminController)** - CWE-78
   - Direct execution of user-provided commands
   - No input validation or sanitization
   - Full system access through web interface

2. **SQL Injection** - CWE-89
   - Raw SQL queries with user input concatenation
   - Present in AuthController.Login and ProductsController.GetProducts
   - No parameterized queries or input validation

3. **Hardcoded Credentials** - CWE-798
   - Database connection strings with credentials in source code
   - Weak JWT signing key exposed in code
   - No configuration management

### 🟠 High Risk Vulnerabilities

4. **No HTTPS Enforcement** - CWE-319
   - HTTP traffic not redirected to HTTPS
   - Sensitive data transmitted in plain text
   - Authentication tokens vulnerable to interception

5. **Insecure Authentication** - CWE-287
   - JWT tokens without expiration
   - Weak secret key for token signing
   - No token validation (issuer, audience, lifetime)

6. **Sensitive Data Exposure** - CWE-200
   - Plain text password storage
   - SSN and credit card data in database/API responses
   - Sensitive information in JWT claims
   - Detailed error messages with stack traces

7. **Broken Authorization** - CWE-862
   - No authentication required for sensitive endpoints
   - IDOR vulnerabilities (users can access others' data)
   - Admin functionality without authorization checks

### 🟡 Medium Risk Vulnerabilities

8. **No CSRF Protection** - CWE-352
   - State-changing operations without CSRF tokens
   - Vulnerable to cross-site request forgery

9. **Insecure CORS Configuration** - CWE-942
   - AllowAnyOrigin policy
   - No restrictions on headers or methods

10. **Mass Assignment** - CWE-915
    - Direct binding of user input to domain models
    - No input validation or filtering

11. **Information Disclosure** - CWE-200
    - System information exposed through admin endpoints
    - Database structure and connection details revealed
    - Environment variables accessible via API

## Exercise Instructions

### Phase 1: Risk Identification (30 minutes)

**Individual Work:**
1. Review the codebase systematically
2. Identify all security vulnerabilities
3. Classify each vulnerability by:
   - **Risk Level** (Critical/High/Medium/Low)
   - **OWASP Top 10 Category**
   - **CWE Classification**
   - **Potential Impact**

**Deliverable:** Risk assessment document

### Phase 2: Architecture Analysis (45 minutes)

**Team Discussion:**
1. Analyze the current architecture for security weaknesses
2. Propose secure architecture patterns:
   - **Authentication & Authorization** strategy
   - **Data Protection** mechanisms
   - **Configuration Management** approach
   - **API Security** controls
   - **Logging & Monitoring** strategy

**Deliverable:** Secure architecture proposal

### Phase 3: Implementation (Pair Programming - 2 hours)

**Working in Pairs:**
Implement fixes for assigned vulnerability categories:

#### Team A: Authentication & Authorization
- Implement secure JWT with proper validation
- Add role-based authorization
- Secure password hashing (bcrypt/Argon2)
- Session management

#### Team B: Data Protection & Input Validation
- Remove hardcoded connection strings
- Implement input validation and sanitization
- Add parameterized queries
- Encrypt sensitive data fields

#### Team C: Infrastructure & Configuration
- Enable HTTPS enforcement
- Implement CSRF protection
- Secure CORS configuration
- Add rate limiting and monitoring

#### Team D: API Security & Error Handling
- Implement proper error handling
- Add API versioning and documentation
- Remove information disclosure
- Add audit logging

### Phase 4: OWASP ASVS Review (30 minutes)

**Cross-Team Review:**
Map implemented fixes against OWASP ASVS controls:

#### V1: Architecture, Design and Threat Modeling
- [ ] 1.1.1 Secure SDLC processes
- [ ] 1.1.2 Threat modeling for design changes
- [ ] 1.2.1 Security architecture documentation

#### V2: Authentication
- [ ] 2.1.1 User identity verification
- [ ] 2.1.3 Generic error messages
- [ ] 2.2.1 Strong authentication mechanisms
- [ ] 2.3.1 Secure credential storage

#### V3: Session Management
- [ ] 3.2.1 Framework session management
- [ ] 3.2.2 Session token generation
- [ ] 3.3.1 Session logout functionality

#### V4: Access Control
- [ ] 4.1.1 Principle of least privilege
- [ ] 4.1.2 Access control at trusted layer
- [ ] 4.1.3 Deny by default principle

#### V5: Validation, Sanitization and Encoding
- [ ] 5.1.1 Input validation architecture
- [ ] 5.1.2 Structured data validation
- [ ] 5.3.4 SQL injection prevention

#### V7: Error Handling and Logging
- [ ] 7.1.1 Generic error messages
- [ ] 7.1.2 Sensitive information in errors
- [ ] 7.4.1 High-value transaction logging

#### V8: Data Protection
- [ ] 8.2.1 Data classification
- [ ] 8.2.2 Sensitive data protection
- [ ] 8.3.1 Server-side data protection

#### V9: Communication
- [ ] 9.1.1 TLS for sensitive data
- [ ] 9.1.2 Latest TLS versions
- [ ] 9.2.1 Server certificate validation

#### V10: Malicious Code
- [ ] 10.3.1 Input validation for uploads
- [ ] 10.3.2 File type validation

## Running the Application

### Prerequisites
- .NET 8.0 SDK
- SQL Server or SQL Server Express
- Visual Studio or VS Code

### Setup
1. Clone/download the project
2. Navigate to the InsecureWebApp directory
3. Update connection string if needed (though it's hardcoded - this is a vulnerability!)
4. Run the application:

```bash
dotnet restore
dotnet run
```

### Testing Vulnerabilities

#### SQL Injection Examples:
```bash
# Login bypass
POST /api/auth/login
{
  "email": "admin@test.com' OR '1'='1' --",
  "password": "anything"
}

# Product search injection
GET /api/products?search='; DROP TABLE Products; --
```

#### Command Injection Example:
```bash
POST /api/admin/execute-command
{
  "command": "dir & whoami & ipconfig"
}
```

#### IDOR Examples:
```bash
# Access other users' orders
GET /api/orders?userId=1
GET /api/orders?userId=2

# Access user details
GET /api/auth/users/1
GET /api/auth/users/2
```

## Security Assessment Checklist

### Code Review Focus Areas:
- [ ] Input validation and sanitization
- [ ] Authentication and authorization mechanisms
- [ ] Error handling and information disclosure
- [ ] Configuration and secrets management
- [ ] Data protection and encryption
- [ ] Session management
- [ ] CSRF and CORS protections
- [ ] SQL injection prevention
- [ ] Command injection prevention
- [ ] File upload security

### Architecture Review Focus Areas:
- [ ] Defense in depth implementation
- [ ] Principle of least privilege
- [ ] Secure by default configuration
- [ ] Fail-safe mechanisms
- [ ] Audit and monitoring capabilities
- [ ] Data flow security
- [ ] External dependency security
- [ ] Infrastructure security

## Learning Objectives

By completing this exercise, participants will:

1. **Develop Security Awareness**
   - Recognize common vulnerability patterns
   - Understand attack vectors and exploitation methods
   - Appreciate the importance of secure coding practices

2. **Apply OWASP Guidelines**
   - Map vulnerabilities to OWASP Top 10
   - Implement ASVS controls
   - Use security testing methodologies

3. **Practice Secure Development**
   - Implement secure authentication and authorization
   - Apply input validation and output encoding
   - Configure secure infrastructure settings

4. **Collaborate on Security**
   - Conduct security code reviews
   - Share security knowledge across teams
   - Build security into development workflows

## References

- [OWASP Top 10 2021](https://owasp.org/www-project-top-ten/)
- [OWASP Application Security Verification Standard (ASVS)](https://owasp.org/www-project-application-security-verification-standard/)
- [OWASP Cheat Sheet Series](https://cheatsheetseries.owasp.org/)
- [Microsoft Security Development Lifecycle](https://www.microsoft.com/en-us/securityengineering/sdl)
- [CWE/SANS Top 25 Most Dangerous Software Errors](https://cwe.mitre.org/top25/)

---

**Remember: This is a training exercise. Never use these insecure patterns in production code!**