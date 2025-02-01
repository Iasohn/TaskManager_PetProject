using System.Diagnostics;

namespace TaskManagerPet.Middleware
{
    public class TimerM
    {
        RequestDelegate _request;
        public TimerM(RequestDelegate request)
        {
            _request = request;
        }

        public async Task Invoke(HttpContext client)
        {
            Stopwatch Start = new Stopwatch();
            Start.Start();

            await _request(client);

            Start.Stop();

            Console.WriteLine($"Time: {Start.ElapsedMilliseconds}ms");
        }


    }
}
