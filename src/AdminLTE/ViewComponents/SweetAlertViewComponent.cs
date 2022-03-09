using AdminLTE.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AdminLTE.ViewComponents
{
    public class SweetAlertViewComponent : ViewComponent
    {

        public SweetAlertViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            List<Message> messages;
            if (ViewBag.SweetAlerts == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = new List<Message>(ViewBag.SweetAlerts);
            }
            return View(messages);
        }
    }
}