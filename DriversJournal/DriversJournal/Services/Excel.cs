using DriversJournal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace DriversJournal.Services
{
    public class Excel
    {
        /// <summary>
        /// Method for create a Excel arc of an list.
        /// </summary>
        /// <param name="journals">List with journals</param>
        /// <param name="month">Selected month</param>
        /// <param name="year">Selected year</param>
        public static void ListToExcel(List<Journal> journals, string month, string year)
        {
            string debit = "N";
            StringWriter sw = new StringWriter();
            String lt = ";";
            sw.WriteLine("Odometer start" + lt + "Odometer end" + lt + "Start date" + lt + "End date"
                + lt + "From" + lt + "To" + lt + "Travelers" + lt + "ProjectNumber" + lt + "Debit"
                + lt + "KmNo" + lt + "Purpose");
            foreach (Journal item in journals)
            {
                if (item.Debit == 1)
                {
                    debit = "Y";
                }
                else
                {
                    debit = "N";
                }
                sw.WriteLine(string.Format("{0}" + lt + "{1}" + lt + "{2}" + lt + "{3}" + lt + "{4}" + lt
                    + "{5}" + lt + "{6}" + lt + "{7}" + lt + "{8}" + lt + "{9}" + lt + "{10}",
                        item.OdometerStart,
                        item.OdometerEnd,
                        item.StartDate.ToString("yyyy-MM-dd"),
                        item.EndDate.ToString("yyyy-MM-dd"),
                        item.FromDestination,
                        item.ToDestination,
                        item.JournalUser.FirstName + " " + item.JournalUser.LastName + " " + item.Travelers,
                        item.ProjectNumber,
                        debit,
                        item.KmNo,
                        item.Purpose
                    ));
            }
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + year + "_" + month + "_driversjournal.csv");
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
            HttpContext.Current.Response.Write(sw);
            HttpContext.Current.Response.End();
        }
    }
}