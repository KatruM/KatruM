using BDI3Mobile.Models.Requests;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BDI3Mobile.Helpers
{
    public static class HelperMethods
    {
        #region CollectionToSaveAPIContent
        internal static List<ContentCategory> ContentCategorycollection { get; set; }
        internal static List<LocationResponseModel> LocationModelCollection { get; set; }
        #endregion
        /// <summary>
        /// Validates if the given date is less than the current date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal static DateFormatStructure DateValidation(string date)
        {
            DateTime result;

            if (DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out result) ||
                (DateTime.TryParseExact(date, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)))
            {
                if (DateTime.Compare(result, DateTime.Now) > 0 )
                {
                    return new DateFormatStructure() { result = false };
                }
                return new DateFormatStructure()
                {
                    result = true,
                    time = result,
                };
            }
            return new DateFormatStructure() { result = false };
        }
        /// <summary>
        /// Validates the date; To restrict the future dates, and allow past dates upto (-100 years)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal static DateFormatStructure DateValidationForDOB(string date)
        {
            string dateToComapre = "12/31/1918";
            if (DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out var result) ||
                (DateTime.TryParseExact(date, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)))
            {
                if (DateTime.Compare(result,DateTime.Now) > 0 || (result.Year.ToString().Length < 4) || DateTime.Compare(result, Convert.ToDateTime(dateToComapre)) <= 0)
                {
                    return new DateFormatStructure() { result = false };
                }
                return new DateFormatStructure()
                {
                    result = true,
                    time = result,
                };
            }
            return new DateFormatStructure() { result = false };
        }
        /// <summary>
        /// Validates the date; To allow the future dates upto +100 years, past dates (-100 years)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal static DateFormatStructure DateValidationForEnrollment(string date)
        {
            string dateToComapre = "12/31/1918";
            if (DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out var result) ||
                (DateTime.TryParseExact(date, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)))
            {
                if ((result.Year.ToString().Length < 4) || DateTime.Compare(result, Convert.ToDateTime(dateToComapre)) <= 0)
                {
                    return new DateFormatStructure() { result = false };
                }
                return new DateFormatStructure()
                {
                    result = true,
                    time = result,
                };
            }
            return new DateFormatStructure() { result = false };
        }

        private static int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private static DateTime fromDate;
        private static DateTime toDate;
        private static int year;
        private static int month;
        private static int day;
        /// <summary>
        /// Calculate age difference between two dates.
        /// </summary>
        /// <param name="dob"></param>
        /// <param name="givendate"></param>
        /// <returns></returns>
        internal static int[] CalculateAge(string dob, string givendate)
        {
            int increment = 0;
            int[] duration = new int[3];
            /*DateTime dateTime;
            DateTime.TryParse(dob, out dateTime);
            fromDate = dateTime;
            DateTime dateTime1;
            DateTime.TryParse(givendate, out dateTime1);
            toDate = dateTime1;*/

            var dobSplit = dob.Split('/');
            var givendateSplit = givendate.Split('/');

            try
            {
                fromDate = new DateTime(Convert.ToInt32(dobSplit[2]), Convert.ToInt32(dobSplit[0]), Convert.ToInt32(dobSplit[1]));
                toDate = new DateTime(Convert.ToInt32(givendateSplit[2]), Convert.ToInt32(givendateSplit[0]), Convert.ToInt32(givendateSplit[1]));

                if (fromDate.Day > toDate.Day)
                {
                    increment = monthDay[fromDate.Month - 1];
                }

                if (increment == -1)
                {
                    if (DateTime.IsLeapYear(fromDate.Year))
                    {
                        increment = 29;
                    }
                    else
                    {
                        increment = 28;
                    }
                }

                if (increment != 0)
                {
                    day = (toDate.Day + increment) - fromDate.Day;
                    increment = 1;
                }
                else
                {
                    day = toDate.Day - fromDate.Day;
                }

                if ((fromDate.Month + increment) > toDate.Month)
                {
                    month = (toDate.Month + 12) - (fromDate.Month + increment);
                    increment = 1;
                }
                else
                {
                    month = (toDate.Month) - (fromDate.Month + increment);
                    increment = 0;
                }

                year = toDate.Year - (fromDate.Year + increment);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            string diff = year + "Year(s), " + month + " month(s), " + day + " day(s)";

            duration[0] = year;
            duration[1] = month;
            duration[2] = day;

            return duration;

        }
    }

    public struct DateFormatStructure
    {
        public bool result;
        public DateTime time;
    }
}
