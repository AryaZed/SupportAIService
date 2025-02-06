namespace SupportAI.API.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        public TenantMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            var user = context.User;
            if (user.Identity.IsAuthenticated)
            {
                var tenantIdClaim = user.FindFirst("TenantId");
                if (tenantIdClaim != null)
                {
                    context.Items["TenantId"] = tenantIdClaim.Value;
                }
            }
            await _next(context);
        }
    }
}
