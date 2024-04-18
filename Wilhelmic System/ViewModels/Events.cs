﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Reactive.Linq;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia;
using Avalonia.Media;
using System.Security.Cryptography.X509Certificates;
using Avalonia.Data.Converters;
using System.Globalization;

namespace Wilhelmic_System.ViewModels
{
    public class WilhelmicEventItem
    {
        public string Title { get; set; }
        public DateStruct StartTime { get; set; }
        public DateStruct EndTime { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public WilhelmicEventItem(string title, DateStruct startTime, DateStruct endTime, string description = "", string location = "")
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
        private List<WilhelmicEventItem> events; // creates the variable events, as a list variable for storing events data in memory
        private string filePath = "WilhelmicEvents.json"; //defines the location where the events file will be stored
        
        public WilhelmicEvents() // Procedure to call LoadEvents Procedure automatically
        {
            LoadEvents();
        }

        private void LoadEvents()
        {
            if (File.Exists(filePath)) // Decides if the file exists, and to deserialize it if it does
            {
                string json = File.ReadAllText(filePath);
                events = JsonConvert.DeserializeObject<List<WilhelmicEventItem>>(json);
            }
            else
            {
                events = new List<WilhelmicEventItem>(); // if it doesn't exist, it'll spawn a new list and assign it to events variable
            }
        }

        public void SaveEvents()
        {
            // create a file of type string json where the string variable 'events' is saved to the file
            string json = JsonConvert.SerializeObject(events, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void AddEvent(WilhelmicEventItem eventItem)
        {
            events.Add(eventItem);
            SaveEvents(); // call to save the current list with the new changes
        }

        public bool RemoveEvent(WilhelmicEventItem eventItem)
        {
            bool removed = events.Remove(eventItem);
            if (removed)
            {
                SaveEvents(); // call to save the current list with the new changes
            }
            return removed; // return that removed = true
        }

        public List<WilhelmicEventItem> GetEventsOnDate(DateStruct date)
        {
            return events.Where(e => e.StartTime == date).ToList(); // return events of a specified date
        }
        public List<WilhelmicEventItem> GetAllEvents()
        {
            return new List<WilhelmicEventItem>(events); // create a new list of all events and return it
        }

    }
}