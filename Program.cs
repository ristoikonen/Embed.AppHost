using Amazon;
using Aspire.Hosting.AWS.Lambda;

#pragma warning disable CA2252

var builder = DistributedApplication.CreateBuilder(args);

var awsConfig = builder.AddAWSSDKConfig()
                        .WithProfile("Release")
                        .WithRegion(RegionEndpoint.APSoutheast2);

var webFV = builder.AddAWSLambdaFunction<Projects.Embed_AWSless>("FV",
        lambdaHandler: "Embed.AWSless::Embed.AWSless.Functions::FV");

builder.AddAWSAPIGatewayEmulator("APIGatewayEmulator", Aspire.Hosting.AWS.Lambda.APIGatewayType.HttpV2)
    .WithReference(webFV, Method.Get, "/fv");

builder.Build().Run();










// var qparams_FV = @"?{rate}&{pv}&{nper}";
// {rate}{nper}{pv}
// builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
// Embed.AWSServerless

//var webDefault = builder.AddAWSLambdaFunction<Projects.AWSServerless2>("Default", lambdaHandler: "Default");


//var serverlessHandler = builder.AddAWSLambdaFunction<Projects.AWSServerless1>("ServerlessHandler",
//        lambdaHandler: "ServerlessHandler::ServerlessHandler.Functions::Add");


//var functionHandler = builder.AddAWSLambdaFunction<Projects.Encode_Lambda>("FunctionHandler",
//        lambdaHandler: "Encode.Lambda::Encode.Lambda.Function::FunctionHandler");

//var responseHandler = builder.AddAWSLambdaFunction<Projects.Encode_Lambda>("ResponseHandler",
//        lambdaHandler: "Encode.Lambda::Encode.Lambda.Function::ResponseHandler");


//var getFunctionHandler = builder.AddAWSLambdaFunction<Projects.AWSServerless2>("GetFunctionHandler",
//        lambdaHandler: "AWSServerless2::AWSServerless2.Functions::GetFunctionHandler");


//.WithReference(serverlessHandler, Aspire.Hosting.AWS.Lambda.Method.Get, "/add/{x}/{y}");

