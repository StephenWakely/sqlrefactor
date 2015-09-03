using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace SQLRefactor
{
	[Serializable]
	public class QueryResults : INotifyPropertyChanged
	{
		private bool? _success;
		private long _time;
		private int _rowsReturned;
		private string _errorMessage;

		public bool? Success
		{
			get { return _success; }
			set
			{
				_success = value;
				PropertyChanged ( this, new PropertyChangedEventArgs ( "Success" ) );
			}
		}

		public long Time
		{
			get { return _time; }
			set
			{
				_time = value;
				PropertyChanged ( this, new PropertyChangedEventArgs ( "Time" ) );
			}
		}

		public int RowsReturned
		{
			get { return _rowsReturned; }
			set
			{
				_rowsReturned = value;
				PropertyChanged ( this, new PropertyChangedEventArgs ( "RowsReturned" ) );
			}
		}

		public string ErrorMessage
		{
			get { return _errorMessage; }
			set
			{
				_errorMessage = value;
				PropertyChanged ( this, new PropertyChangedEventArgs ( "ErrorMessage" ) );
			}
		}

		public QueryData Data
		{
			get;
			set;
		}

		public QueryResults ()
		{
			this.Data = new QueryData ();
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion

	}
}
