using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace SQLRefactor
{
	/// <summary>
	/// Stores the details about a particular run of the query.
	/// </summary>
	public class Query : INotifyPropertyChanged
	{
		private IDatabase _database;
		private QueryResults _results;
		private string _sql;

		/// <summary>
		/// The results of running this query
		/// </summary>
		public QueryResults Results
		{
			get
			{
				return _results;
			}
			set
			{
				this._results = value;
				PropertyChanged ( this, new PropertyChangedEventArgs ( "Results" ) );
			}
		}

		/// <summary>
		/// The query text
		/// </summary>
		public string Sql
		{
			get
			{
				return _sql;
			}
			set
			{
				this._sql = value;
			}
		}

		public IDatabase Database
		{
			set
			{
				this._database = value;
			}
		}

		public Query ()
		{
			this._results = new QueryResults ();
		}

		/// <summary>
		/// Run the given sql. Store the results in our QueryResults member.
		/// </summary>
		/// <param name="sql"></param>
		public void RunSql ( )
		{
			this._database.RunQuery ( this._sql, this.Results );
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion

	}
}
