using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Wilhelmic_System.ViewModels;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

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

        public void AddEvent(WilhelmicEventItem eventItem)
        {
            WihelmicEvents.Add(eventItem);
        }

        public void UpdateDate()
        {
            Mathematics.ProcessEpoch();
            this.Date = new DateTime((int)Mathematics.ntpYear, (int)Mathematics.ntpMonth, (int)Mathematics.ntpDate);;
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

    public class DayCellViewModel : ReactiveObject
    {
        private DayCell _dayCell;

        public DayCell DayCell
        {
            get => _dayCell;
            set => this.RaiseAndSetIfChanged(ref _dayCell, value);
        }

        public DayCellViewModel()
        {
            _dayCell = new DayCell();
        }

        public void RefreshDayCell()
        {
            DayCell.UpdateDate();
            this.RaisePropertyChanged(nameof(DayCell)); // Notify UI to update
        }
    }



    // Below is everything to do with events

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
    }
    public class WilhelmicEvents
    {
        private List<WilhelmicEventItem> events;
        private string filePath = "WilhelmicEvents.json";
        
        public WilhelmicEvents()
        {
            LoadEvents();
        }

        private void LoadEvents()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                events = JsonConvert.DeserializeObject<list<WilhelmicEventItem>>(json);
            }
            else
            {
                events = new List<WilhelmicEventItem>();
            }
        }

        public void SaveEvents()
        {
            string json = JsonConvert.SerializeObject(events, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void AddEvent(WilhelmicEventItem eventItem)
        {
            events.Add(eventItem);
            SaveEvents();
        }

        public bool RemoveEvent(WilhelmicEventItem eventItem)
        {
            bool removed = events.Remove(eventItem);
            if (removed)
            {
                SaveEvents();
            }
            return removed;
        }

        public List<WilhelmicEventItem> GetEventsOnDate(DateTime date)
        {
            return events.Where(e => e.StartTime.Date == date.Date).ToList();
        }

        public List<WilhelmicEventItem> GetAllEvents()
        {
            return new List<WilhelmicEventItem>(events);
        }

    }
}
