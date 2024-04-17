using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;


namespace converters
{
    public class TimeToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Define fixed pixel positions for each hour from 1 to 10 on the canvas.
            double[] hourPositions = { 0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300 }; // Example positions

            if (int.TryParse(value.ToString(), out int hour))
            {
                // Ensure hour is within bounds
                if (hour < 0 || hour > 10)
                    return new Thickness(0);

                // Return a new Thickness with the calculated left margin
                return new Thickness(hourPositions[hour], 0, 0, 0);
            }

            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}