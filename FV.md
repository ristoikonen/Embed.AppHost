# FV Method - AWS Lambda function

AWS Lambda function  **FV** , a C# financial  function designed to calculate the Future Value (FV) of an investment. This calculation is based on a series of periodic, constant payments and a constant interest rate.
It the future value of an investment, with regular payments and an initial present value. It's a fundamental financial function often used in scenarios like calculating the future worth of a savings plan or an annuity.


## Method Exposure

**FV** is exposed through Amazon API Gateway as a HTTP *Get* operation.
```html
https://lee4cnqs6ryonvdsu2dogpmzuy0uzzjw.lambda-url.ap-southeast-2.on.aws/fv?rate=0.08&pv=2000&nper=5
```
---
## Method Signature

```csharp
public double FV(double Rate, double NPer, double Pmt, double PV = 0, bool PmtAtEndOfPeriod = true)
```

---
## Parameters

* `Rate`: A `Double` representing the **interest rate per period**. For example, if you have an annual interest rate of 6% and payments are made monthly, the `Rate` would be `0.06 / 12`.
* `NPer`: A `Double` representing the **total number of payment periods** in the annuity. For instance, for a 5-year annuity with monthly payments, `NPer` would be `5 * 12`.
* `Pmt`: A `Double` representing the **payment made each period**. This value typically represents an outflow of cash, so it should be entered as a **negative number**. For example, if you make a payment of $100, `Pmt` would be `-100`.
* `PV`: (Optional) A `Double` representing the **present value**, or the lump-sum amount that a series of future payments is worth right now. This also typically represents an outflow and should be a **negative number** if it's money you've invested. If omitted, `PV` defaults to `0`.
* `PmtAtEndOfPeriod`: (Optional) A `Boolean` that specifies when payments are due.
    * `true` (default): Payments are due at the **end of the period**.
    * `false`: Payments are due at the **beginning of the period**.

---
## Return Value

Returns a `Double` representing the **future value** of the investment.

---
## Remarks

* The `FV` method is an financial function. While functional, it might be part of a larger financial library and not intended for direct external consumption without understanding the broader context.
* The `Rate` and `NPer` parameters must be expressed in consistent units. If you calculate `Rate` using months, `NPer` must also be in months.
* The `Pmt` parameter represents payments that are constant throughout the life of the investment.
* Cash paid out (such as deposits to savings or an initial investment) should be represented by **negative numbers**. Cash received (such as dividends or the future value of the investment) should be represented by **positive numbers**.

---
## Example - Just for demo puposes as here, we're calling FV internally

```csharp
// Example: Calculate the future value of a savings account
// where you deposit $100 at the end of each month for 5 years
// with an annual interest rate of 3%.

double annualRate = 0.03;
double monthlyRate = annualRate / 12; // 0.0025
double numberOfYears = 5;
double numberOfPeriods = numberOfYears * 12; // 60 periods
double monthlyPayment = -100; // outflow
double presentValue = 0; // Starting with no initial lump sum

// Calculate FV with payments at the end of the period (default)
double futureValue = FV(monthlyRate, numberOfPeriods, monthlyPayment, presentValue, true);

Console.WriteLine($"Future Value (payments at end of period): {futureValue:C}");
// Expected output will be a positive number, indicating money received.
// For example, if FV_Internal returns approximately $6,464.67, the output would be:
// Future Value (payments at end of period): $6,464.67

// Example with payments at the beginning of the period
futureValue = FV(monthlyRate, numberOfPeriods, monthlyPayment, presentValue, false);
Console.WriteLine($"Future Value (payments at beginning of period): {futureValue:C}");
```


