namespace TaskManagerPet.Middleware
{
    public class CatchErrorMiddleware
    {

        RequestDelegate _request;
        public CatchErrorMiddleware(RequestDelegate request)
        {
            _request = request; 

        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (Exception e) 
            {
                context.Response.StatusCode = 500;
                Console.WriteLine($"You catch error {e.Message} ");

            }
        }
    }
}
