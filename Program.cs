using System;

namespace SequentialMessages
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Application();
            app.RunAsync().GetAwaiter().GetResult();
        }
    }
}
