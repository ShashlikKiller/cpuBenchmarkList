using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace cpuListApp.View.Images
{
    internal class BrandImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string brand)
            {
                switch (brand.ToLower())
                {
                    case "intel":
                        return new BitmapImage(new Uri("../../Images/Sources/intel.png", UriKind.Relative));
                    case "amd":
                        return new BitmapImage(new Uri("../../Images/Sources/amd.png", UriKind.Relative));
                    default:
                        return new BitmapImage(new Uri("../../Images/Sources/amd.png", UriKind.Relative));
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
