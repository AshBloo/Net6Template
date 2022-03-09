using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdminLTE.Models;
using System.Runtime.CompilerServices;
using System.Text;


namespace AdminLTE.Controllers
{
    public class BaseController : Controller
    {
        internal void AddBreadcrumb(string displayName, string urlPath)
        {
            List<Message> messages;

            if (ViewBag.Breadcrumb == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = ViewBag.Breadcrumb as List<Message>;
            }

            messages.Add(new Message { DisplayName = displayName, URLPath = urlPath });
            ViewBag.Breadcrumb = messages;
        }

        internal void AddPageHeader(string pageHeader = "", string pageDescription = "")
        {
            ViewBag.PageHeader = Tuple.Create(pageHeader, pageDescription);
        }

        internal enum PageAlertType
        {
            Error,
            Info,
            Warning,
            Success,
            Question
        }

        internal void AddPageAlerts(PageAlertType pageAlertType, string description)
        {
            List<Message> messages;

            if (ViewBag.PageAlerts == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = ViewBag.PageAlerts as List<Message>;
            }

            messages.Add(new Message { Type = pageAlertType.ToString().ToLower(), ShortDesc = description });
            ViewBag.PageAlerts = messages;
        }



        internal void AddToastAlerts(PageAlertType pageAlertType, string title, string message)
        {
            List<Message> messages;

            if (ViewBag.ToastAlerts == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = ViewBag.ToastAlerts as List<Message>;
            }

            messages.Add(new Message { Type = pageAlertType.ToString().ToLower(), ShortDesc = message, DisplayName = title });
            ViewBag.ToastAlerts = messages;
        }

        internal void AddToastrAlerts(PageAlertType pageAlertType, string title, string message)
        {
            List<Message> messages;

            if (ViewBag.ToastAlerts == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = ViewBag.ToastAlerts as List<Message>;
            }

            messages.Add(new Message { Type = pageAlertType.ToString().ToLower(), ShortDesc = message, DisplayName = title });
            ViewBag.ToastAlerts = messages;
        }

        //public class Enum
        //{
        //    public enum NotificationType
        //    {
        //        error,
        //        success,
        //        warning,
        //        info,
        //        question
        //    }

        //    public enum SwalType
        //    {
        //        error,
        //        success,
        //        warning,
        //        info,
        //        question
        //    }

        //}

        internal enum DialogType
        {
            Toast,
            Dialog,
            Input,
        
        }

        internal void SweetAlert(DialogType dialogType,string message, string title, string confirmButtonText, PageAlertType pageAlertType, string urlPath = "")
        {

           // var msg = @"Swal.fire({icon: '" + notificationType + "', title: '" + title + "', text: '" + message + "', confirmButtonText: '" + confirmButtonText + "', footer: '" + URLpath + "'})";


            List<Message> messages;

            if (ViewBag.SweetAlerts == null)
            {
                messages = new List<Message>();
            }
            else
            {
                messages = ViewBag.SweetAlerts as List<Message>;
            }

            messages.Add(new Message { DialogType = dialogType.ToString().ToLower(), Type = pageAlertType.ToString().ToLower(), ShortDesc = message, DisplayName = title, URLPath = urlPath, FontAwesomeIcon = confirmButtonText });
            ViewBag.SweetAlerts = messages;
        }
    }
}