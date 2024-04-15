using System;
using System.Collections.Generic;

namespace Wilhelmic_System.ViewModels
{
    public class DayCell
    {
        public DateTime Date { get; private set; }
        public List<WilhelmicEvents> Events { get; private set; } = new List<WilhelmicEvents>();

        public DayCell()
        {
            UpdateDate();
        }

       // public void AddEvent(WilhelmicEventItem eventItem)
       // {
       //     WihelmicEvents.Add(eventItem);
       // }

        public void UpdateDate()
        {
            Mathematics.ProcessEpoch();
            this.Date = new DateTime((int)Mathematics.ntpYear, (int)Mathematics.ntpMonth, (int)Mathematics.ntpDate);
            AddEvent(WilhelmicEventItem.TestEvent);
        }

        public double UpdateTimeBarPos()
        {
            Mathematics.ProcessEpoch();
            var (MHour, MMinute, MSecond) = Mathematics.ImperialMetricConversion();
            int totalMetricSeconds = MHour * 10000 + MMinute * 100 + MSecond;
            double DayConstant = 100000;
            return (double)totalMetricSeconds / DayConstant;
        }
    }

    public class WilhelmicEventItem
    {
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public WilhelmicEventItem(string title, DateTime startTime, DateTime endTime, string description = "", string location = "")
        {
            Title = title;
            StartTime = startTime;
            EndTime = endTime;
            Description = description;
            Location = location;
        }

        public static readonly WilhelmicEventItem TestEvent = new EventItem(title: "test", startTime: new DateTime(2024, 4, 20, 4, 0, 0), endTime: new DateTime(2024, 4, 20, 5, 0, 0), description: "lol", location: "lol");
    }
}
