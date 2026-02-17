# BankRUs API - Quick Test Endpoints

## Prerequisites
**Start smtp4dev first:**
```bash
smtp4dev
```
Then open http://localhost:5000 to see welcome emails.

---

## 1. Open Account
```http
POST https://localhost:8000/api/accounts
Content-Type: application/json

{
  "firstName": "Jane",
  "lastName": "Doe",
  "socialSecurityNumber": "19900101-2013",
  "email": "jane@doe.com"
}
```

---
→ Get the `accountNumber` from Welcome email in smtp4de.
## 2. Get Account Details (by account number)
```http
GET https://localhost:8000/api/bank-accounts?accountNumber={accountNumber}
```
→ Save the `accountId` (Guid) from response

---

## 3. Deposit Money (US05)
```http
POST https://localhost:8000/api/bank-accounts/{accountId}/deposits
Content-Type: application/json

{
  "amount": 1000.00,
  "reference": "Lön januari"
}
```

---

## 4. Withdraw Money (US06)
```http
POST https://localhost:8000/api/bank-accounts/{accountId}/withdrawals
Content-Type: application/json

{
  "amount": 200.00,
  "reference": "Uttag bankomat"
}
```

---

## 5. List Transactions (US07)
```http
GET https://localhost:8000/api/bank-accounts/{accountId}/transactions
```
