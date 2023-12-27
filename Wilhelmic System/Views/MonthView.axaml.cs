using System;
using Avalonia.Controls;
using Wilhelmic_System.ViewModels;

namespace Wilhelmic_System.Views
{
    public partial class MonthView : UserControl
    {
        public MonthView()
        {
            InitializeComponent();
            PopulateMonthView();
        }

        private void PopulateMonthView()
        {
            Mathematics.ProcessEpoch(); // Ensure that the right data is provided
            bool isLeapYear = Mathematics.leapYearInd == 1;
            int currentMonthIndex = (int)Mathematics.ntpMonth;

            // Determine the length of the current month
            int monthLength = GetMonthLength(currentMonthIndex, isLeapYear);

            // Determine the number of weeks in the grid
            int numberOfWeeks = (int)Math.Ceiling((double)monthLength / 7);
            SetupGrid(numberOfWeeks);

            // Populate the grid with day cells
            for (int day = 1; day <= monthLength; day++)
            {
                TextBlock dayCell = new TextBlock { Text = day.ToString() };

                // Set the position of the day cell in the grid
                Grid.SetColumn(dayCell, (day - 1) % 7);
                Grid.SetRow(dayCell, (day - 1) / 7);

                MonthViewGrid.Children.Add(dayCell);
            }
        }

        private int GetMonthLength(int monthIndex, bool isLeapYear)
        {
            // Special handling for the first month
            if (monthIndex == 0)
            {
                return isLeapYear ? 2 : 1;
            }
            // The other months always have 28 days
            else
            {
                return 28;
            }
        }

        private void SetupGrid(int numberOfWeeks)
        {
            MonthViewGrid.ColumnDefinitions.Clear();
            MonthViewGrid.RowDefinitions.Clear();

            for (int i = 0; i < 7; i++) // 7 days in a week
            {
                MonthViewGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < numberOfWeeks; i++) // Number of weeks in the month
            {
                MonthViewGrid.RowDefinitions.Add(new RowDefinition());
            }
        }
    }
}
