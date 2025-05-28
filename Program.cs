using Amazon;
using Aspire.Hosting.AWS.Lambda;

#pragma warning disable CA2252

var builder = DistributedApplication.CreateBuilder(args);

// builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);


// Embed.AWSServerless

var awsConfig = builder.AddAWSSDKConfig()
                        .WithProfile("Release")
                        .WithRegion(RegionEndpoint.APSoutheast2);


//var defaultRouteLambda = builder.AddAWSLambdaFunction<Projects.AWSServerless1>("LambdaDefaultRoute", 
//    lambdaHandler: "Default");



//var serverlessHandler = builder.AddAWSLambdaFunction<Projects.AWSServerless1>("ServerlessHandler",
//        lambdaHandler: "ServerlessHandler::ServerlessHandler.Functions::Add");


//var functionHandler = builder.AddAWSLambdaFunction<Projects.Encode_Lambda>("FunctionHandler",
//        lambdaHandler: "Encode.Lambda::Encode.Lambda.Function::FunctionHandler");

//var responseHandler = builder.AddAWSLambdaFunction<Projects.Encode_Lambda>("ResponseHandler",
//        lambdaHandler: "Encode.Lambda::Encode.Lambda.Function::ResponseHandler");

var webFV = builder.AddAWSLambdaFunction<Projects.Embed_AWSless>("FV",
        lambdaHandler: "Embed.AWSless::Embed.AWSless.Functions::FV");
                        
//var getCallingIPAsync = builder.AddAWSLambdaFunction<Projects.AWSServerless2>("GetCallingIPAsync",
//        lambdaHandler: "AWSServerless2::AWSServerless2.Functions::GetCallingIPAsync");

//var getFunctionHandler = builder.AddAWSLambdaFunction<Projects.AWSServerless2>("GetFunctionHandler",
//        lambdaHandler: "AWSServerless2::AWSServerless2.Functions::GetFunctionHandler");

//var webDefault = builder.AddAWSLambdaFunction<Projects.AWSServerless2>("Default", lambdaHandler: "Default");

//var responseHandler = builder.AddAWSLambdaFunction<Projects.AWSServerless2>("ResponseHandler",
//        lambdaHandler: "AWSServerless2::AWSServerless2.Functions::ResponseHandler");

builder.AddAWSAPIGatewayEmulator("APIGatewayEmulator", Aspire.Hosting.AWS.Lambda.APIGatewayType.HttpV2)
    .WithReference(webFV, Method.Get, "/fv");

//.WithReference(responseHandler, Aspire.Hosting.AWS.Lambda.Method.Get, "/r")
//.WithReference(getCallingIPAsync, Aspire.Hosting.AWS.Lambda.Method.Get, "/ip");

//.WithReference(responseHandler, Aspire.Hosting.AWS.Lambda.Method.Get, "/f");
//.WithReference(defaultRouteLambda, Method.Get, "/");
//.WithReference(serverlessHandler, Aspire.Hosting.AWS.Lambda.Method.Get, "/add/{x}/{y}");



builder.Build().Run();
