using System.Diagnostics;

namespace Interface.Middlewares
{
    public class ProfillingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ProfillingMiddleware> logger;

        public ProfillingMiddleware(RequestDelegate next, ILogger<ProfillingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await next.Invoke(context);

            stopwatch.Stop();
            logger.LogInformation($"Request from: {context.Request.Path} took {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
