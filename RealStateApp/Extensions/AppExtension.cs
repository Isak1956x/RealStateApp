namespace RealStateApp.Presentation.API.Extensions
{
    public static class AppExtension
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app, IEndpointRouteBuilder endpointRouteBuilder)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var versionDescriptions = endpointRouteBuilder.DescribeApiVersions();
                if (versionDescriptions != null && versionDescriptions.Any())
                {
                    
                    foreach (var apiVersion in versionDescriptions)
                    {
                        var url = $"/swagger/{apiVersion.GroupName}/swagger.json";
                        var name = "RealState API " + apiVersion.GroupName.ToUpperInvariant;
                        c.SwaggerEndpoint(url, name);
                    }
                }
            });
        }
    }
}
