using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Reactive.Linq;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace Wilhelmic_System.ViewModels
{
    public class DayCell
    {
        public DateStruct Date { get; private set; }
        public ObservableCollection<WilhelmicEventItem> Events { get; } = new ObservableCollection<WilhelmicEventItem>();
        public double CurrentTimeIndicator { get; set; }  // Represents the current time position in the cell

        public DayCell(DateStruct date)
        {
            Date = date;
            UpdateCurrentTimeIndicator();
        }

        public void UpdateCurrentTimeIndicator()
        {
            // ProcessEpoch should be called externally to ensure it's up to date when this method is called
            Mathematics.ProcessEpoch();
            if (IsToday())
            {
                var (MHour, MMinute, _) = Mathematics.ImperialMetricConversion();
                CurrentTimeIndicator = MHour * 10 + (MMinute / 100);  // Convert minutes to a fraction of the 100 units per hour
            }
            else
            {
                CurrentTimeIndicator = -1;  // Stops the time bar from being displayed (invalid if not 'today')
            }
        }

        public bool IsToday()
        {
            Mathematics.ProcessEpoch();
            var today = new DateStruct((int)Mathematics.ntpYear, (int)Mathematics.ntpMonth, (int)Mathematics.ntpDate);
            return Date == today;
        }
    }

    public class DayCellViewModel : ReactiveObject
    {
        public ObservableCollection<DayCell> DayCells { get; } = new ObservableCollection<DayCell>();

        public DayCellViewModel()
        {
            CreateDayCellsForRange(-2, 2);  // Create DayCells for a range around today
        }

        private void CreateDayCellsForRange(int startOffset, int endOffset)
        {
            var today = DateTime.Now.Date;
            for (int i = startOffset; i <= endOffset; i++)
            {
                var day = today.AddDays(i);
                DayCells.Add(new DayCell(new DateStruct(day.Year, day.Month, day.Day)));
            }
        }

        // Call this method regularly or when needed to update the current time indicator
        public void RefreshCurrentTimeIndicators()
        {
            foreach (var cell in DayCells)
            {
                cell.UpdateCurrentTimeIndicator();
            }
            this.RaisePropertyChanged(nameof(DayCells));
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
                events = JsonConvert.DeserializeObject<List<WilhelmicEventItem>>(json);
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
