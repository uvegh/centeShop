using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace ApiGateway.Transforms;

public class UserContextTransforms:ITransformProvider
{
    public void ValidateRoute(TransformClusterValidationContext context)
    {

    }
    public void ValidateCluster(TransformClusterValidationContext context)
    {

    }

    public void Apply(TransformBuilderContext context)
    {
        //inject users info into all requests

        context.AddRequestTransform(async transfromContext =>
        {
            var httpContext = transfromContext.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                //get user info from jwt 
                var userId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? httpContext?.User.FindFirst("sub")?.Value;
                //e
                var email = httpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? httpContext?.User.FindFirst("email")?.Value;
                var roles = string.Join(",", httpContext?.User?.FindAll(ClaimTypes.Role).Select(r => r?.Value));
            }
        });
    }

}
