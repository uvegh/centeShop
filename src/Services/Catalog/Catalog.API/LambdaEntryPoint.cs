using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.Core;

namespace Catalog.API;

/// <summary>
/// This class extends from APIGatewayHttpApiV2ProxyFunction which contains the method FunctionHandlerAsync which is the 
/// actual Lambda function entry point. The Lambda handler field should be set to:
/// 
/// Catalog.API::Catalog.API.LambdaEntryPoint::FunctionHandlerAsync
/// </summary>
public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
{
    /// <summary>
    /// The builder has configuration, logging and Amazon API Gateway already configured. The startup class
    /// needs to be configured in this method using the UseStartup<>() method.
    /// </summary>
    /// <param name="builder"></param>
    protected override void Init(IHostBuilder builder)
    {
        builder
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables();
            });
    }
}