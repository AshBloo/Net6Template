using AdminLTE.Common.Extensions;
using AdminLTE.Data;
using AdminLTE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.CustomExtensions;
using static Microsoft.EntityFrameworkCore.CustomExtensions.ExpressionBuilder;

namespace AdminLTE.Common
{

    public static class GlobalHelper
    {
        public static string GetUserId(this IPrincipal principal)
        {

            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return "";
            }
            else
            {
                return claim.Value;
            }

        }
        public static string GetUserName(this IPrincipal principal )
        {
            
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.GivenName);
            var claim2 = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.Surname);
            if (claim == null)
            {
                return "";
            }
            else
            {
                return claim.Value + " " + claim2.Value;
            }

        }

        public static string GetUserEmail(this IPrincipal principal)
        {

            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.Email);
            if (claim == null)
            {
                return "";
            }
            else
            {
                return claim.Value;
            }

        }
        private static readonly int[,] _addOffset =
            {
              // 0  1  2  3  4
                {0, 1, 2, 3, 4}, // Su  0
                {0, 1, 2, 3, 4}, // M   1
                {0, 1, 2, 3, 6}, // Tu  2
                {0, 1, 4, 5, 6}, // W   3
                {0, 1, 4, 5, 6}, // Th  4
                {0, 3, 4, 5, 6}, // F   5
                {0, 2, 3, 4, 5}, // Sa  6
            };

        public static DateTime AddWeekdays(this DateTime date, int weekdays)
        {
            int extraDays = weekdays % 5;
            int addDays = weekdays >= 0
                ? (weekdays / 5) * 7 + _addOffset[(int)date.DayOfWeek, extraDays]
                : (weekdays / 5) * 7 - _addOffset[6 - (int)date.DayOfWeek, -extraDays];
            return date.AddDays(addDays);
        }
        static readonly int[,] _diffOffset =
{
  // Su M  Tu W  Th F  Sa
    {0, 1, 2, 3, 4, 5, 5}, // Su
    {4, 0, 1, 2, 3, 4, 4}, // M 
    {3, 4, 0, 1, 2, 3, 3}, // Tu
    {2, 3, 4, 0, 1, 2, 2}, // W 
    {1, 2, 3, 4, 0, 1, 1}, // Th
    {0, 1, 2, 3, 4, 0, 0}, // F 
    {0, 1, 2, 3, 4, 5, 0}, // Sa
};

        public static int GetWeekdaysDiff(this DateTime dtStart, DateTime dtEnd)
        {
            int daysDiff = (int)(dtEnd - dtStart).TotalDays;
            return daysDiff >= 0
                ? 5 * (daysDiff / 7) + _diffOffset[(int)dtStart.DayOfWeek, (int)dtEnd.DayOfWeek]
                : 5 * (daysDiff / 7) - _diffOffset[6 - (int)dtStart.DayOfWeek, 6 - (int)dtEnd.DayOfWeek];
        }

    }
}

