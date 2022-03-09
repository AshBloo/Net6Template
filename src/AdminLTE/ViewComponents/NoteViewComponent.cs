using AdminLTE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using AdminLTE.Common.Extensions;
using AdminLTE.Common;
using AdminLTE.Data;

namespace AdminLTE.ViewComponents
{
    public class NoteViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public NoteViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int id)
        {
            var messages = GetData(id);
            return View(messages);
        }

        private Note GetData(int id)
        {
            Note messages = _context.Note.Find(id);

          
            return messages;
        }
    }
}
