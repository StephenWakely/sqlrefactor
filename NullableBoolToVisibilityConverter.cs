using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace SQLRefactor
{
	public class NullableBoolToVisibilityConverter : IValueConverter
	{
		public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( bool.Equals ( value, parameter ) )
				return Visibility.Visible;
			else
				return Visibility.Collapsed;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException ();
		}
	}
}
