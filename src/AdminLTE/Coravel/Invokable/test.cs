
using AdminLTE.Data;
using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace AdminLTE.Coravel.Invokable
{
    public class TestInvocable : IInvocable
    {
        private ApplicationDbContext _context;
        public TestInvocable(ApplicationDbContext context)
        {
            this._context = context;
        }

        public Task Invoke()
        {
            throw new System.NotImplementedException();
        }

        //public async Task Invoke()
        //{
        //    await this._context.Application.AddAsync(new Models.Application()
        //    {
        //        Position = "test name"
        //    });

        //    await this._context.SaveChangesAsync();

        //    var models = await this._context.Application.Select(t => t).ToListAsync();

        //    foreach (var model in models)
        //    {
        //        System.Console.WriteLine($"Model Name: {model.Position}, Id: {model.ApplicationId}");
        //    }
        //}
    }
}