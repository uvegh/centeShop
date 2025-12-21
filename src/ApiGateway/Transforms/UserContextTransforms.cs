using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace ApiGateway.Transforms;

public class UserContextTransforms:ITransformProvider
{

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
                var roles = string.Join(",", httpContext.User.FindAll(ClaimTypes.Role).Select(r => r?.Value));

                //add header to requests
                if(!string.IsNullOrEmpty(userId))
                
                    transfromContext.ProxyRequest.Headers.Add("X-User-Id", userId);
                if (!string.IsNullOrEmpty(email))
                    transfromContext.ProxyRequest.Headers.Add("X-User-Email", email);
                if (!string.IsNullOrEmpty(roles))
                    transfromContext.ProxyRequest.Headers.Add("X-User-Roles", roles);
            }
        });
    }

    public void ValidateRoute(TransformRouteValidationContext context)
    {
        throw new NotImplementedException();
    }
}
