using System;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace Wilhelmic_System.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private static IDisposable? timerDisposable;

        public MainViewModel()
        {
            // Set up a timer to refresh every second
            timerDisposable = Observable.Interval(TimeSpan.FromSeconds(1))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => RefreshData());
        }

        private void RefreshData()
        {
            // Update the properties
            this.RaisePropertyChanged(nameof(Greeting));
        }

        public string Greeting
        {
            get
            {
                Mathematics.ProcessEpoch();

                string imperialToMetricTime = Mathematics.ImperialToMetricTime();
                string specialDay = CheckForSpecialDays();

                return $"{imperialToMetricTime}, {specialDay}, {PrintMonth()} {Mathematics.ntpYear} ";
            }
        }

        private static string CheckForSpecialDays()
        {
            if (Mathematics.ntpMonth == 0)
            {
                if (Mathematics.ntpDate == 1) return "New Years";
                if (Mathematics.ntpDate == 2) return "Nullus Two";
            }

            // Return standard weekday if not a special day
            return numToDay((Mathematics.ntpDate - 1) % 7);
        }

        public static string numToDay(uint num)
        {
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            return days[num];
        }

        private static string PrintMonth()
        {
            string[] monthArr = { "Nullus", "Lupus", "Nix", "Mors", "Roseus", "Flos", "Fragium", "Tonitruum", "Rubrum", "Messis", "Venator", "Gelus", "Frigus", "Quercus" };

            // Assuming that ntpMonth is correctly set in Mathematics
            uint monthIndex = Mathematics.ntpMonth;

            // Check if monthIndex is within the valid range
            if (monthIndex >= monthArr.Length)
            {
                throw new InvalidOperationException($"Invalid month value: {monthIndex}");
            }

            return monthArr[monthIndex];
        }
    }
}
