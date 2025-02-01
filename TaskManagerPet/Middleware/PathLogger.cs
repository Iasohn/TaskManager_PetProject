using Azure.Core;

namespace TaskManagerPet.Middleware
{
    public class PathLogger
    {

        private RequestDelegate _Next;

        public PathLogger(RequestDelegate next)
        {
            _Next = next;
        }
        public async Task InvokeAsync(HttpContext client)
        {
            var Path = client.Request.Path;
            var Method = client.Request.Method;

            Console.WriteLine($"[Request] Method: {Method}, Path: {Path}");

            await _Next(client);
        }

    }
}
