using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Wilhelmic_System.ViewModels;

namespace Wilhelmic_System.Views
{
    public partial class MonthView : UserControl
    {
        private int displayedMonth;
        private int displayedYear;
        public MonthView()
        {
            InitializeComponent();
            Mathematics.ProcessEpoch();

            // Initialize controls from XAML
            MonthViewGrid = this.FindControl<Grid>("MonthViewGrid");
            PrevMonthButton = this.FindControl<Button>("PrevMonthButton");
            NextMonthButton = this.FindControl<Button>("NextMonthButton");
            EventPanel = this.FindControl<TextBlock>("EventPanel");

            displayedMonth = (int)Mathematics.ntpMonth;
            displayedYear = (int)Mathematics.ntpYear;

            AttachEventHandlers();
            PopulateMonthView();
        }

        private void AttachEventHandlers()
        {
            PrevMonthButton.Click += (s, e) => ChangeMonth(-1);
            NextMonthButton.Click += (s, e) => ChangeMonth(1);
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
                displayedYear--; // Go to previous year
            }
            else if (displayedMonth > 13)
            {
                displayedMonth = 0;
                displayedYear++; // Go to next year
            }

            PopulateMonthView();
        }

        private void DayCell_Click(object? sender, PointerPressedEventArgs e)
        {
            if (sender is TextBlock dayCell)
            {
                string fullDate = dayCell.Tag.ToString();
                EventPanel.Text = $"Events for {fullDate}";
                // Add logic to display events for the given full date
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
