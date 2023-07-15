using System;
using System.Globalization;
using Xamarin.Forms;

namespace FitApp.Infrastructure.Converters
{
    public class TimerColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isWorking = (bool)value;
            Color green = new Color(229, 249, 219);
            Color blue = new Color(175, 211, 226);
            return isWorking ? Color.LightGreen : Color.LightBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
