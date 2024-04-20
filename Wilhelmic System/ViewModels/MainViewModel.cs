using System;
using System.Reactive.Linq;
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
                string PrintWeekday = CheckForSpecialDays();

                return $"{imperialToMetricTime}, {PrintWeekday}, {PrintMonth()} {Mathematics.ntpYear} ";
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
            return CheckWeekday((Mathematics.ntpDate - 1) % 7);
        }

        public static string CheckWeekday(uint num)
        {
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            return days[num];
        }

        private static string PrintMonth()
        {
            string[] monthArr = { "Nullus", "Luparis", "Nivium", "Vermix", "Rosula", "Floralis", "Fragara", "Cervido", "Sturion", "Frugena", "Venatrix", "Castoris", "Gelida", "Frigora" };

            // Set the month index to the current month in the Mathematics Module
            uint monthIndex = Mathematics.ntpMonth;

            // Debugging Code (Irrelevant to the function of the program, should be removed in final!)
            if (monthIndex >= monthArr.Length)
            {
                throw new InvalidOperationException($"Invalid month value: {monthIndex}");
            }
            // return the value of the month array, after placing monthIndex inside of it
            return monthArr[monthIndex];
        }
    }
}
