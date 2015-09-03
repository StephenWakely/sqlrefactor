using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace SQLRefactor
{
	public class TextToVisibilityConverter : IValueConverter
	{
		public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return String.Equals(value.ToString() , parameter.ToString(), StringComparison.OrdinalIgnoreCase ) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException ();
		}
	}
}
