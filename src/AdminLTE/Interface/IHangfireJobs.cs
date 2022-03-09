using AdminLTE.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AdminLTE.Services
{
    //INTERFACE
        public interface IPrintJob
        {
            void Print();
        }
    //TASK METHOD
        public class PrintJob : IPrintJob
        {
            public void Print()
            {
                Console.WriteLine($"Hanfire recurring job!");
            }
        }

    //INTERFACE
    public interface IUpdateTasksJob
    {
        void UpdateAllTaskLog(ApplicationDbContext _context);
    }
    //TASK METHOD
    public class UpdateTasksJob : IUpdateTasksJob
    {

        public void UpdateAllTaskLog(ApplicationDbContext _context)
        {
     

            Console.WriteLine($"Hanfire recurring job! TASK UPDATES");

            // var tasklist = _context.TaskList;
            var tasklog = _context.TaskLog.ToList();
            var taskrules = _context.TaskRules.AsQueryable();
            //var candidate = _context.Candidate;
            string userid = "BACKGROUND JOB";  

            foreach (var item in tasklog) //Loops through every task
            {
                var tr = taskrules.Where(r => r.TaskId == item.TaskId).ToList(); //Select single task and all associated rules

                //for (int i = 0; i < tr.Count(); i++) //Builds query string based on rules
                //{

                if (tr.Count() > 0)
                {
                    string sqlstringstart = "N'";
                    string sqlstringend = "'";
                    string[] criteria = new String[tr.Count()];
                    string sqlop = "";
                    int count = 0;

                    foreach (var i in tr)
                    {


                        if (i.Criteria == "BLANK")
                        {
                            sqlstringstart = "";
                            sqlstringend = "";
                            criteria[count] = "NULL";
                        }
                        else
                        {
                            criteria[count] = i.Criteria;
                        }

                        if (i.IsOr == true)
                        {
                            sqlop = "OR";
                        }

                        if (i.IsAnd == true)
                        {
                            sqlop = "AND";
                        }

                        count++;

                    }


                    string tableName = tr[0].SourceTable.ToString();





                    string msgsql = "";

                    if (tr.Count() == 3)
                    {
                        msgsql = @"SELECT *
                                    FROM [HomecareRecruitment].[dbo].[" + tableName + "]" +
                                    "WHERE (" + tr[0].SourceColumn.ToString() + " " + tr[0].Operator.ToString() + " " + sqlstringstart + criteria[0] + sqlstringend + ")" +
                                   sqlop + " (" + tr[1].SourceColumn.ToString() + " " + tr[1].Operator.ToString() + " " + sqlstringstart + criteria[1] + sqlstringend + ")" +
                                   sqlop + " (" + tr[2].SourceColumn.ToString() + " " + tr[2].Operator.ToString() + " " + sqlstringstart + criteria[2] + sqlstringend + ")";
                    }
                    else if (tr.Count() == 2)
                    {
                        msgsql = @"SELECT *
                                    FROM [HomecareRecruitment].[dbo].[" + tableName + "]" +
                                    "WHERE (" + tr[0].SourceColumn.ToString() + " " + tr[0].Operator.ToString() + " " + sqlstringstart + criteria[0] + sqlstringend + ")" +
                                   sqlop + " (" + tr[1].SourceColumn.ToString() + " " + tr[1].Operator.ToString() + " " + sqlstringstart + criteria[1] + sqlstringend + ")";
                    }
                    else if (tr.Count() == 1)
                    {
                        msgsql = @"SELECT *
                                    FROM [HomecareRecruitment].[dbo].[" + tableName + "]" +
                                  "WHERE (" + tr[0].SourceColumn.ToString() + " " + tr[0].Operator.ToString() + " " + sqlstringstart + criteria[0] + sqlstringend + ")";

                    }
                    else
                    {

                    }


                    //IF STATEMENT FOR TABLE SELECTION TASK UPDATE

                    if (tableName == "Candidate")
                    {
                        var tableselect = _context.Candidate.FromSqlRaw(msgsql.ToString()).ToList();

                        if (tableselect.Count() > 0)
                        {
                            var task = tasklog.Where(t => t.TaskId == item.TaskId).FirstOrDefault();
                            if (task.Completed == null)
                            {
                                task.Completed = DateTime.UtcNow;
                                task.CompletedBy = userid;
                                _context.Update(task);
                            }
                        }
                        else
                        {
                            var task = tasklog.Where(t => t.TaskId == item.TaskId).FirstOrDefault();
                            if (task.Completed != null)
                            {
                                task.Completed = null;
                                task.CompletedBy = null;
                                _context.Update(task);
                            }

                        }

                        _context.SaveChanges();
                    }

                    //IF STATEMENT FOR TABLE SELECTION TASK UPDATE

                    if (tableName == "Application")
                    {
                        var tableselect = _context.Application.FromSqlRaw(msgsql.ToString()).ToList();
                        if (tableselect.Count() > 0)
                        {
                            var task = tasklog.Where(t => t.TaskId == item.TaskId).FirstOrDefault();
                            if (task.Completed == null)
                            {
                                task.Completed = DateTime.UtcNow;
                                task.CompletedBy = userid;
                                _context.Update(task);
                            }
                        }
                        else
                        {
                            var task = tasklog.Where(t => t.TaskId == item.TaskId).FirstOrDefault();
                            if (task.Completed != null)
                            {
                                task.Completed = null;
                                task.CompletedBy = null;
                                _context.Update(task);
                            }

                        }

                        _context.SaveChanges();
                    }

                }



            }


            //return new UpdateTasksJob();

        }

    }






}
