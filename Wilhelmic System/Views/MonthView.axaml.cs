using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using Wilhelmic_System.ViewModels;

namespace Wilhelmic_System.Views
{
    public partial class MonthView : UserControl
    {
        private int displayedMonth;
        private int displayedYear;
        private string displayedMonthName;
        public WilhelmicEvents WilhelmicEvents;
        private WilhelmicEventItem selectedEvent;

        public MonthView()
        {
            InitializeComponent();
            Mathematics.ProcessEpoch();
            WilhelmicEvents = new WilhelmicEvents();
            this.FindControl<Button>("RemoveEventButton").Click += RemoveButton_Click;
            this.FindControl<Button>("AddEventButton").Click += AddEventButton_Click;
            // Initialize the controls from the axaml
            MonthViewGrid = this.FindControl<Grid>("MonthViewGrid");
            PrevMonthButton = this.FindControl<Button>("PrevMonthButton");
            NextMonthButton = this.FindControl<Button>("NextMonthButton");
            EventPanel = this.FindControl<StackPanel>("EventPanel");

            displayedMonth = (int)Mathematics.ntpMonth;
            displayedYear = (int)Mathematics.ntpYear;
            displayedMonthName = GetMonthName(displayedMonth); // update displayedMonthName

            MonthOffSetButtons(); // call the month offset buttons
            PopulateMonthView();
        }

        private string GetMonthName(int monthIndex) // set parameter so that displayedMonth can be passed in.
        {
            // array is copied from the MainViewModel, more months are displayed further on (the line is very long)
            string[] monthArr = { "Nullus (0)", "Luparis (1)", "Nivium (2)", "Vermix (3)", "Rosula (4)", "Floralis (5)", "Fragara (6)", "Cervido (7)", "Sturion (8)", "Frugena (9)", "Venatrix (10)", "Castoris (11)", "Gelida (12)", "Frigora (13)" };
            return monthArr[monthIndex]; // return the string value from the array and pass it back out of the function
        }

        private void MonthOffSetButtons()
        {
            PrevMonthButton.Click += (s, e) => ChangeMonth(-1); //decrease month by passing -1 as arg
            NextMonthButton.Click += (s, e) => ChangeMonth(1); //increase month by passing 1 as arg
        }

        private string FormatFullDate(int day, int month, int year)
        {
            return $"{day:D2}/{month:D2}/{year}";
        }

        private void PopulateMonthView()
        {
            bool isLeapYear = ((displayedYear % 4 == 0 && displayedYear % 100 != 0) || displayedYear % 400 == 0);
            int monthLength = GetMonthLength(displayedMonth, isLeapYear);
            int numberOfWeeks = (int)Math.Ceiling((double)monthLength / 7);
            SetupGrid(numberOfWeeks);

            MonthViewGrid.Children.Clear(); // Clear existing cells

            for (int day = 1; day <= monthLength; day++)
            {
                string fullDateString = FormatFullDate(day, displayedMonth, displayedYear);

                TextBlock dayCell = new TextBlock
                {
                    Text = day.ToString(),
                    Margin = new Thickness(5),
                    Tag = fullDateString // Store the full date string in the Tag property
                };
                dayCell.PointerPressed += DayCell_Click;

                if (day == Mathematics.ntpDate && displayedMonth == Mathematics.ntpMonth && displayedYear == Mathematics.ntpYear)
                {
                    dayCell.Background = new SolidColorBrush(Colors.LightBlue);
                }

                TextBlock monthYearHeader = this.FindControl<TextBlock>("MonthYearHeader"); // create a header above the columns and rows
                monthYearHeader.Text = $"{displayedMonthName} {displayedYear}"; // assign a string to show month and year in the header


                Grid.SetColumn(dayCell, (day - 1) % 7);
                Grid.SetRow(dayCell, (day - 1) / 7);

                MonthViewGrid.Children.Add(dayCell);
            }
        }

        private void ChangeMonth(int offset)
        {
            displayedMonth += offset;

            if (displayedMonth < 0)
            {
                displayedMonth = 13;
                displayedYear--; // Decrement year
            }
            else if (displayedMonth > 13)
            {
                displayedMonth = 0;
                displayedYear++; // Increment year
            }

            // update the displayed month name as well as the displayed month cells
            displayedMonthName = GetMonthName(displayedMonth);
            PopulateMonthView();
        }

        private DateStruct CreateDateStruct(int year, int month, int day)
        {
            return new DateStruct(year, month, day);
        }


        private void DayCell_Click(object? sender, PointerPressedEventArgs e)
        {
            if (sender is TextBlock dayCell)
            {
                int day = int.Parse(dayCell.Text); // Set the day variable to the selected day cell

                currentlyDisplayedDate = CreateDateStruct(displayedYear, displayedMonth, day);
                RefreshEventsDisplay(currentlyDisplayedDate); // save the currently displayed date
            }
        }

        private DateStruct currentlyDisplayedDate; // Spawn a date struct for displayed date

        private void RefreshEventsDisplay(DateStruct currentDate)
        {
            var events = WilhelmicEvents.GetEventsOnDate(currentDate); // Get the updated list of events for the current date
            EventPanel.Children.Clear(); // Clear the existing contents of the EventPanel
            EventPanel.Children.Add(new TextBlock { Text = $"Events for {currentDate.Day}/{currentDate.Month}/{currentDate.Year}:\n", FontWeight = FontWeight.Bold });
            FormatEventsForDisplay(events, EventPanel); // Pass events and the EventPanel for modification
        }

        private void FormatEventsForDisplay(List<WilhelmicEventItem> events, Panel eventPanel)
        {
            if (events == null || events.Count == 0)
            {
                eventPanel.Children.Add(new TextBlock { Text = "No events scheduled." }); // If there are no events, return this string
                return;
            }

            foreach (var eventItem in events) // Loop the process for each EventItem in events for a specific day.
            {
                var eventToggleButton = new ToggleButton // spawn event objects as Togglebuttons
                {
                    Content = $"{eventItem.StartTime.Day}/{eventItem.StartTime.Month}/{eventItem.StartTime.Year} - {eventItem.Title}: {eventItem.Description} at {eventItem.Location}",
                    Margin = new Thickness(5),
                    Tag = eventItem
                };
                eventToggleButton.Checked += EventToggleButton_Checked; // if event is clicked, toggle button state to true
                eventToggleButton.Unchecked += EventToggleButton_Unchecked; // if event is clicked again, toggle button state to false
                eventPanel.Children.Add(eventToggleButton); // Populate the EventPanel
            }
        }

        private void EventToggleButton_Checked(object? sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggleButton && toggleButton.Tag is WilhelmicEventItem eventItem)
            {
                selectedEvent = eventItem;  // set the selected event
                UpdateToggleButtonState(toggleButton, true); // set the button state to selected
            }
        }

        private void EventToggleButton_Unchecked(object? sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggleButton)
            {
                selectedEvent = null;  // clear the selected event
                UpdateToggleButtonState(toggleButton, false); // set the button state to unselected
            }
        }

        private void UpdateToggleButtonState(ToggleButton clickedToggleButton, bool isChecked)
        {
            if (isChecked)
            {
                // for each of the toggle buttons in the eventpanel
                foreach (ToggleButton tb in EventPanel.Children.OfType<ToggleButton>())
                {
                    if (tb != clickedToggleButton)
                        tb.IsChecked = false;  // ensure only one event/togglebutton can be selected at a time
                }
            }
        }

        private async void RemoveButton_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new EventRemovalDialog(); // Spawn Event Removal Dialog
            var window = this.FindAncestorOfType<Window>(); // Retrieve the parent window of user control
            if (window != null) // if window is not null
            {
                await dialog.ShowDialog(window);  // Await until the dialog displays
                if (dialog.DialogResult)
                {
                    WilhelmicEvents.RemoveEvent(selectedEvent); // Proceed with removing the selected event
                    selectedEvent = null; // set the selected event to null (disselect the event)
                    RefreshEventsDisplay(currentlyDisplayedDate);
                }
            }
        }

        private async void AddEventButton_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new AddEventDialog(currentlyDisplayedDate); // Spawn Event Creation Window
            var window = this.FindAncestorOfType<Window>(); // Retrieve the parent window of user control
            if (window != null) // if the window is not null
            {
                await dialog.ShowDialog(window); // Await until the window displays
                if (dialog.NewEvent != null) // if new event is not null
                {
                    WilhelmicEvents.AddEvent(dialog.NewEvent); // create the new event by passing the window's event into WilhelmicEvents
                    RefreshEventsDisplay(currentlyDisplayedDate);  // Refresh the view
                }
            }
        }

        private int GetMonthLength(int monthIndex, bool isLeapYear)
        {
            if (monthIndex == 0)
                return isLeapYear ? 2 : 1;
            else
                return 28;
        }

        private void SetupGrid(int numberOfWeeks)
        {
            MonthViewGrid.ColumnDefinitions.Clear();
            MonthViewGrid.RowDefinitions.Clear();

            for (int i = 0; i < 7; i++)
            {
                MonthViewGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < numberOfWeeks; i++)
            {
                MonthViewGrid.RowDefinitions.Add(new RowDefinition());
            }
        }
    }
}