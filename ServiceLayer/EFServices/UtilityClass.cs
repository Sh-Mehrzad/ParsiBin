using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ServiceLayer.EFServices
{
    public static class UtilityClass
    {

        public static string getCurrentDate()
        {
            PersianCalendar pc = new PersianCalendar();
            string month = pc.GetMonth(DateTime.Now).ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string day = pc.GetDayOfMonth(DateTime.Now).ToString();
            if (day.Length == 1)
            {
                day = "0" + day;
            }
            string currentdate = pc.GetYear(DateTime.Now).ToString() + "-" + month + "-" + day;
            return currentdate;
        }

        public static string getCurrentDate(DateTime DT)
        {
            PersianCalendar pc = new PersianCalendar();
            string month = pc.GetMonth(DT).ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string day = pc.GetDayOfMonth(DateTime.Now).ToString();
            if (day.Length == 1)
            {
                day = "0" + day;
            }
            string currentdate = pc.GetYear(DateTime.Now).ToString() + "-" + month + "-" + day;
            return currentdate;
        }

        public static string GetTextDate(DateTime DT)
        {
            PersianCalendar pc = new PersianCalendar();
            string dayName = pc.GetDayOfWeek(DT).ToString();
            switch (dayName)
            {
                case "1":
                    dayName = "دوشنبه";
                    break;
                case "2":
                    dayName = "سه شنبه";
                    break;
                case "3":
                    dayName = "چهارشنبه";
                    break;
                case "4":
                    dayName = "پنجشنبه";
                    break;
                case "5":
                    dayName = "جمعه";
                    break;
                case "6":
                    dayName = "شنبه";
                    break;
                case "7":
                    dayName = "یکشنبه";
                    break;
            }
            string day = pc.GetDayOfMonth(DateTime.Now).ToString();
            if (day.Length == 1)
            {
                day = "0" + day;
            }
            string month = pc.GetMonth(DT).ToString();
            switch (month)
            {
                case "1":
                    month = "فروردین";
                    break;
                case "2":
                    month = "اردیبهشت";
                    break;
                case "3":
                    month = "خرداد";
                    break;
                case "4":
                    month = "تیر";
                    break;
                case "5":
                    month = "مرداد";
                    break;
                case "6":
                    month = "شهریور";
                    break;
                case "7":
                    month = "مهر";
                    break;
                case "8":
                    month = "آبان";
                    break;
                case "9":
                    month = "آذر";
                    break;
                case "10":
                    month = "دی";
                    break;
                case "11":
                    month = "بهمن";
                    break;
                case "12":
                    month = "اسفند";
                    break;
            }
            string currentdate = dayName + " " + day + " " + month;
            return currentdate;
        }
    }
}