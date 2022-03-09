using Coravel.Invocable;
using System;
using System.Threading.Tasks;

namespace AdminLTE.Coravel.Invokable
{
    public class MyFirstInvocable : IInvocable
    {
        public Task Invoke()
        {
            Console.WriteLine("This is my first invocable!");
            return Task.CompletedTask;
        }
    }
}
