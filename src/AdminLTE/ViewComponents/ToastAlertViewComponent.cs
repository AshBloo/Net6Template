using AdminLTE.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AdminLTE.ViewComponents
{
    public class ToastAlertViewComponent : ViewComponent
    {

        public ToastAlertViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            List<Message> messages;
            if (ViewBag.ToastAlerts == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = new List<Message>(ViewBag.ToastAlerts);
            }
            return View(messages);
        }
    }
}