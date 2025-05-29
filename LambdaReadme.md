# AWS Serverless Application - Financial Lambda function


Visual Studio Project consists of:
* serverless.template - An AWS CloudFormation Serverless Application Model template file for declaring your Serverless functions and other AWS resources
* Function.cs - Class file containing the C# method mapped to the single function declared in the template file
* Finacial.cs - Class file containing the C# finacial methods like FV.
* Startup.cs - Class file that can be used to configure services that can be injected for either the Lambda container lifetime or a single function invocation
* aws-lambda-tools-defaults.json - Default argument settings for use with Visual Studio and command line deployment tools for AWS

Has Dockerfile to use Amazon ECR provided .NET 8.0 lambda/dotnet base image for Lambda that contains all the required components to run functions packaged as container images on AWS Lambda. 
These base images contain the Amazon Linux Base operating system, the runtime for a given language, dependencies and the Lambda Runtime Interface Client (RIC), which implements the Lambda Runtime API. 

Contains AWS Lambda function **FV** that will be exposed through Amazon API Gateway as a HTTP *Get* operation. 


---
# FV Method

Project has AWS Lambda function  **FV** , a C# financial  function designed to calculate the Future Value (FV) of an investment. This calculation is based on a series of periodic, constant payments and a constant interest rate.
It the future value of an investment, with regular payments and an initial present value. It's a fundamental financial function often used in scenarios like calculating the future worth of a savings plan or an annuity.

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
## Example for demo puposes as we're calling FC internally

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




# How to work with AWS Serverless Application in Visual Studio


## Here are some steps to follow from Visual Studio:

To deploy your Serverless application, right click the project in Solution Explorer and select *Publish to AWS Lambda*.
To view your deployed application open the Stack View window by double-clicking the stack name shown beneath the AWS CloudFormation node in the AWS Explorer tree. The Stack View also displays the root URL to your published application.

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```


Deploy application
```
    cd "Embed.AWSless/src/Embed.AWSless"
    dotnet lambda deploy-serverless
```

## Lambda Annotations
This template uses [Lambda Annotations](https://github.com/aws/aws-lambda-dotnet/blob/master/Libraries/src/Amazon.Lambda.Annotations/README.md) to bridge the gap between the Lambda programming model and a more idiomatic .NET model.

This automatically handles reading parameters from an APIGatewayProxyRequest and returning an APIGatewayProxyResponse. 

It also generates the function resources in a JSON or YAML CloudFormation template based on your function definitions, and keeps them updated.

### Using Annotations without API Gateway
You can still use Lambda Annotations without integrating with API Gateway. For example, this Lambda function processes messages from an Amazon Simple Queue Service (Amazon SQS) queue:
```
[LambdaFunction(Policies = "AWSLambdaSQSQueueExecutionRole", MemorySize = 512, Timeout = 30)]
public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context) 
{ 
    foreach(var message in evnt.Records) 
    { 
      await ProcessMessageAsync(message, context);
    }
}
```

### Reverting to not using Annotations
If you wish to use the former style of function instead of annotations, replace the Lambda function with:
```
public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
{
    context.Logger.LogInformation("Handling the 'Get' Request");

    var response = new APIGatewayProxyResponse
    {
        StatusCode = (int)HttpStatusCode.OK,
        Body = "Hello AWS Serverless",
        Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
    };

    return response;
}
```

You must also replace the function resource in `serverless.template` with:
```
    "Get": {
      "Type": "AWS::Serverless::Function",
      "Properties": {
        "Handler": "<ASSEMBLY>::<TYPE>.Functions::Get",
        "Runtime": "dotnet8",
        "CodeUri": "",
        "MemorySize": 512,
        "Timeout": 30,
        "Role": null,
        "Policies": [
          "AWSLambdaBasicExecutionRole"
        ],
        "Events": {
          "RootGet": {
            "Type": "Api",
            "Properties": {
              "Path": "/",
              "Method": "GET"
            }
          }
        }
      }
    }
  }
```

You may also want to:
1. Update the generated test code to match the new `Get` Signature.
2. Remove the package reference and `using` statements related to `Amazon.Lambda.Annotations`.
