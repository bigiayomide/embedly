# Key errors to fix:

1. Customer.Email vs Customer.EmailAddress
2. WebhookEvent.Timestamp vs WebhookEvent.CreatedAt
3. ErrorDetails vs ErrorResponse
4. Country properties
5. CustomerVerificationProperties properties
6. KycUpgradeResult properties
7. Request model properties
8. DateTime vs DateTimeOffset
9. Missing using statements
10. Service provider generic methods

The main issues are property name mismatches between my test code and the actual SDK models.