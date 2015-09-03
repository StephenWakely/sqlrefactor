using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SQLRefactor
{
	public class Database : IDatabase 
	{
		public int IterationsToRun
		{
			get;
			set;
		}

		private string _connectionString;
		private IProgressBar _progressBar;

		#region IDatabase Members

		public Database ( IProgressBar progressBar )
		{
			this._progressBar = progressBar;
			this.IterationsToRun = 1;
		}

		public void SetConnectionString ( string connectionString )
		{
			_connectionString = connectionString;
		}

		public void RunQuery ( string query, QueryResults results )
		{
			if ( results == null )
				throw new ArgumentNullException ( "results" );
			try
			{
				this._progressBar.MaxIterations = IterationsToRun;
				this._progressBar.Reset ();

				long elapsedTime = 0;
				using ( SqlConnection myConnection = new SqlConnection ( this._connectionString ) )
				{
					myConnection.Open ();
					using ( SqlCommand myCommand = new SqlCommand ( query, myConnection ) )
					{
						this._progressBar.Reset ();
						for ( int iteration = 0; iteration < IterationsToRun; iteration++ )
						{
							elapsedTime += RunQueryOnce ( results, myCommand );

							this._progressBar.StepIt ();
						}
					}
				}

				this._progressBar.Finish ();
				results.Time = elapsedTime / IterationsToRun;
				results.Success = true;
				results.ErrorMessage = "";
			}
			catch ( Exception ex )
			{
				results.Time = 0;
				results.Success = false;
				results.ErrorMessage = ex.Message;
			}

		}

		private static long RunQueryOnce ( QueryResults results, SqlCommand myCommand )
		{
			Stopwatch timer = new Stopwatch ();
			timer.Start ();

			SqlDataReader reader = null;
			myCommand.CommandTimeout = Int32.MaxValue;

			using ( reader = myCommand.ExecuteReader () )
			{
				results.RowsReturned = 0;
				while ( reader.Read () )
				{
					object[] rowData = new object[reader.FieldCount];
					reader.GetValues ( rowData );

					results.Data.Add ( rowData );
					results.RowsReturned++;
				}
			}
			timer.Stop ();

			return timer.ElapsedTicks;
		}

		#endregion
	}
}
