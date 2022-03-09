using AdminLTE.Data;
using AdminLTE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IUrlHelperFactory factory;
        public NotificationViewComponent(ApplicationDbContext context, IUrlHelperFactory factory)
        {
            _context = context;
            this.factory = factory;
        }

        public IViewComponentResult Invoke(string filter)
        {
            var tasks = GetData(filter);
            ViewData["NotificationTypeId"] = new SelectList(_context.NotificationType, "NotificationTypeId", "TypeName");
            ViewData["NotificationStatusId"] = new SelectList(_context.NotificationStatus, "NotificationStatusId", "StatusName");
            ViewData["NotificationOutcomeId"] = new SelectList(_context.NotificationOutcome, "NotificationOutcomeId", "StatusName");

            IUrlHelper helper = factory.GetUrlHelper(ViewContext);
            helper.Action(); // returns url to the current controller action


            return View(tasks);
        }

        private List<Notification> GetData(string filter)
        {
            //var messages = new List<Tasks>();

            //var tasks = _context.Tasks.Include(t => t.E).Include(t => t.TaskType);

            IQueryable<Notification> queryResult = _context.Notification.Where(x => x.Completed == false && x.IsRead == false && x.AssignedTo == filter).Include(t => t.Application).Include(t => t.NotificationType).Include(t => t.NotificationStatus).Include(t => t.NotificationOutcome).AsQueryable();


            var msg = new List<Notification>();
            if (queryResult != null)
            {


                foreach (var item in queryResult)
                {

                    int candidateid = _context.Application.Where(a => a.ApplicationId == item.ApplicationId).FirstOrDefault().CandidateId ?? 0;

                    var candidate = _context.Candidate.Where(x => x.CandidateId == candidateid).FirstOrDefault();


                    msg.Add(new Notification
                    {
                        Completed = item.Completed,
                        CompletedDate = item.CompletedDate,
                        CompletedId = item.CompletedId,
                        CreatedById = item.CreatedById,
                        DueDate = item.DueDate,
                        DueTime = item.DueTime,
                        Application = item.Application,
                        Extended = item.Extended,
                        FontAwesomeIcon = item.FontAwesomeIcon,
                        Required = item.Required,
                        NotificationStatusId = item.NotificationStatusId,
                        Created = item.Created,
                        NotificationTypeId = item.NotificationTypeId,
                        NotificationType = item.NotificationType,
                        NotificationOutcomeId = item.NotificationOutcomeId,
                        Subject = item.Subject,
                        Title = item.Title,
                        Details = item.Details,
                        NotificationId = item.NotificationId,
                        IsRead = item.IsRead,
                        AssignedTo = item.AssignedTo,
                        CandidateName = candidate.Forename + " " + candidate.Surname,
                    });
                }


            }
            return msg;
        }

    }
}
