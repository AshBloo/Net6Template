using AdminLTE.Data;
using Coravel.Pro.Features.Lenses.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AdminLTE.Coravel.Lenses
{
    public class CompletedTaks : ILense
    {
        private ApplicationDbContext _db;

        public CompletedTaks(ApplicationDbContext db)
        {
            this._db = db;
        }

        // This is the text displayed on the menu.
        public string Name => "Completed Tasks";

        IQueryable<object> ILense.Select(string filter)
        {
            throw new System.NotImplementedException();
        }

        // This returns the projection for Coravel Pro to display.
        //public IQueryable<object> Select(string filter) =>
        //    this._db.TaskLog.Include(t => t.Task)
        //        .Select(i => i)
        //        .Where(i =>
        //            filter == null
        //                ? true
        //                : i.Completed != null && i.Task.Title.Contains(filter)
        //        //       : i.Title.Contains(filter)
        //        )
        //        .OrderByDescending(i => i.Completed);
    }
}
