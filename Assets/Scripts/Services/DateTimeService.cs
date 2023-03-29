using System;
using UnityEngine;

namespace Services
{
    public class DateTimeService
    {
        private const string DefaultDate = "01.01.1990 13:00:00";

        public DateTime PreviousDate { get; private set; }
        public DateTime CurrentDatetime => DateTime.Now;

        public void LoadDate(string loadDate)
        {
            Debug.Log(loadDate);
            
            if (string.IsNullOrEmpty(loadDate))
            {
                PreviousDate = DateTime.Parse(DefaultDate);
                return;
            }

            PreviousDate = DateTime.Parse(loadDate);
        }

        public void SaveDate()
        {
            PreviousDate = DateTime.Now;
           
        }
    }
}