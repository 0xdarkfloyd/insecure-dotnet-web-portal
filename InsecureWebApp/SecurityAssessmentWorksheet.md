# Security Assessment Worksheet

## Team Information
- **Team Name**: _______________
- **Team Members**: _______________
- **Phase**: _______________
- **Date**: _______________

## Phase 1: Vulnerability Identification

### Instructions
Review the codebase and identify security vulnerabilities. For each vulnerability found, complete the following information:

| Vulnerability # | Location (File:Line) | Description | Risk Level | OWASP Category | CWE ID | Potential Impact |
|----------------|---------------------|-------------|------------|----------------|---------|------------------|
| 1 | | | ☐ Critical ☐ High ☐ Medium ☐ Low | | | |
| 2 | | | ☐ Critical ☐ High ☐ Medium ☐ Low | | | |
| 3 | | | ☐ Critical ☐ High ☐ Medium ☐ Low | | | |
| 4 | | | ☐ Critical ☐ High ☐ Medium ☐ Low | | | |
| 5 | | | ☐ Critical ☐ High ☐ Medium ☐ Low | | | |
| 6 | | | ☐ Critical ☐ High ☐ Medium ☐ Low | | | |
| 7 | | | ☐ Critical ☐ High ☐ Medium ☐ Low | | | |
| 8 | | | ☐ Critical ☐ High ☐ Medium ☐ Low | | | |
| 9 | | | ☐ Critical ☐ High ☐ Medium ☐ Low | | | |
| 10 | | | ☐ Critical ☐ High ☐ Medium ☐ Low | | | |

### Risk Level Definitions
- **Critical**: Immediate threat that could lead to complete system compromise
- **High**: Significant security risk that could lead to data breach or system damage
- **Medium**: Security weakness that could be exploited under certain conditions
- **Low**: Minor security issue with limited impact

## Phase 2: Architecture Analysis

### Current Architecture Issues
List the main architectural security weaknesses:

1. _______________________________________________
2. _______________________________________________
3. _______________________________________________
4. _______________________________________________
5. _______________________________________________

### Proposed Secure Architecture

#### Authentication Strategy
- **Current Issues**: _______________________________________________
- **Proposed Solution**: _______________________________________________
- **Implementation Approach**: _______________________________________________

#### Authorization Strategy
- **Current Issues**: _______________________________________________
- **Proposed Solution**: _______________________________________________
- **Implementation Approach**: _______________________________________________

#### Data Protection
- **Current Issues**: _______________________________________________
- **Proposed Solution**: _______________________________________________
- **Implementation Approach**: _______________________________________________

#### Input Validation & Sanitization
- **Current Issues**: _______________________________________________
- **Proposed Solution**: _______________________________________________
- **Implementation Approach**: _______________________________________________

#### Configuration Management
- **Current Issues**: _______________________________________________
- **Proposed Solution**: _______________________________________________
- **Implementation Approach**: _______________________________________________

#### Error Handling & Logging
- **Current Issues**: _______________________________________________
- **Proposed Solution**: _______________________________________________
- **Implementation Approach**: _______________________________________________

## Phase 3: Implementation Planning

### Team Assignment: _______________

### Assigned Vulnerabilities to Fix
1. _______________________________________________
2. _______________________________________________
3. _______________________________________________
4. _______________________________________________

### Implementation Tasks
| Task | Priority | Estimated Time | Assigned To | Status |
|------|----------|----------------|-------------|--------|
| | ☐ High ☐ Medium ☐ Low | | | ☐ Not Started ☐ In Progress ☐ Complete |
| | ☐ High ☐ Medium ☐ Low | | | ☐ Not Started ☐ In Progress ☐ Complete |
| | ☐ High ☐ Medium ☐ Low | | | ☐ Not Started ☐ In Progress ☐ Complete |
| | ☐ High ☐ Medium ☐ Low | | | ☐ Not Started ☐ In Progress ☐ Complete |
| | ☐ High ☐ Medium ☐ Low | | | ☐ Not Started ☐ In Progress ☐ Complete |

### Code Changes Made
Document the key changes implemented:

#### File: _______________
**Changes Made**:
- _______________________________________________
- _______________________________________________

#### File: _______________
**Changes Made**:
- _______________________________________________
- _______________________________________________

#### File: _______________
**Changes Made**:
- _______________________________________________
- _______________________________________________

## Phase 4: OWASP ASVS Compliance Review

### V2: Authentication
- [ ] 2.1.1 User identity verification mechanisms
- [ ] 2.1.3 Generic error messages for authentication
- [ ] 2.2.1 Strong authentication mechanisms
- [ ] 2.2.2 Multi-factor authentication for administrative accounts
- [ ] 2.3.1 Secure credential storage (hashing)

**Comments**: _______________________________________________

### V3: Session Management
- [ ] 3.2.1 Framework-provided session management
- [ ] 3.2.2 Session tokens generated using approved cryptographic algorithms
- [ ] 3.2.3 Session tokens with sufficient entropy
- [ ] 3.3.1 Logout functionality that invalidates sessions

**Comments**: _______________________________________________

### V4: Access Control
- [ ] 4.1.1 Principle of least privilege
- [ ] 4.1.2 Access control decisions made at trusted layer
- [ ] 4.1.3 Deny by default principle
- [ ] 4.2.1 Sensitive data and APIs protected against IDOR attacks

**Comments**: _______________________________________________

### V5: Validation, Sanitization and Encoding
- [ ] 5.1.1 Input validation architecture using positive validation
- [ ] 5.1.2 Validation of structured data against defined schema
- [ ] 5.3.4 Data validated on the server side for SQL injection prevention
- [ ] 5.3.5 Protection against SQL injection in stored procedures

**Comments**: _______________________________________________

### V7: Error Handling and Logging
- [ ] 7.1.1 Generic error messages without sensitive information
- [ ] 7.1.2 Error handling that doesn't disclose stack traces
- [ ] 7.4.1 Logging of high-value transactions with integrity controls
- [ ] 7.4.2 Log data protected against injection attacks

**Comments**: _______________________________________________

### V8: Data Protection
- [ ] 8.2.1 Data classification and protection measures
- [ ] 8.2.2 Protection of sensitive data from unauthorized disclosure
- [ ] 8.3.1 Sensitive data stored server-side and not in client-side storage
- [ ] 8.3.4 Memory containing sensitive data is zeroed out after use

**Comments**: _______________________________________________

### V9: Communication
- [ ] 9.1.1 TLS used for all client connectivity with sensitive data
- [ ] 9.1.2 Latest recommended TLS versions and cipher suites
- [ ] 9.2.1 Server certificate properly configured and valid
- [ ] 9.2.2 TLS settings configured for security

**Comments**: _______________________________________________

## Summary & Reflection

### Key Learning Points
1. _______________________________________________
2. _______________________________________________
3. _______________________________________________

### Most Critical Vulnerabilities Found
1. _______________________________________________
2. _______________________________________________
3. _______________________________________________

### Challenges Encountered
_______________________________________________
_______________________________________________

### Recommendations for Real Projects
1. _______________________________________________
2. _______________________________________________
3. _______________________________________________

### Team Score
**Vulnerabilities Identified**: ___/15
**ASVS Controls Implemented**: ___/20
**Code Quality**: ☐ Excellent ☐ Good ☐ Fair ☐ Needs Improvement

### Instructor Feedback
_______________________________________________
_______________________________________________
_______________________________________________