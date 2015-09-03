using System.ComponentModel;
using System.Linq;

namespace SQLRefactor
{
	public class QueryManager : INotifyPropertyChanged
	{
		private IFilenames _filenames;
		private IDatabase _database;
		private RefactorData _data;
		private int _currentQueryIndex = 0;
		private string _currentFilename = "";
		private string _connectionString = "Data Source=localhost;Initial Catalog=*;Integrated Security=True;Connection Timeout=9999";
		private bool _isModified = false;

		public string ConnectionString
		{
			get
			{
				return _connectionString;
			}
			set
			{
				_connectionString = value;
				PropertyChanged ( this, new PropertyChangedEventArgs ( "ConnectionString" ) );
			}
		}

		public string IterationSql
		{
			get
			{
				return this._data.Queries[this._currentQueryIndex].Sql;
			}
			set
			{
				_isModified = true;
				this._data.Queries[this._currentQueryIndex].Sql = value;
			}
		}

		public string MasterSql
		{
			get
			{
				return this._data.MasterQuery.Sql;
			}
			set
			{
				_isModified = true;
				this._data.MasterQuery.Sql = value;
			}
		}

		public Query CurrentQuery
		{
			get
			{
				return this._data.Queries[this._currentQueryIndex];
			}
		}

		public Query MasterQuery
		{
			get
			{
				return this._data.MasterQuery;
			}
		}

		/// <summary>
		/// The currently displayed query.
		/// </summary>
		public int CurrentQueryIndex
		{
			get
			{
				return _currentQueryIndex;
			}
			set
			{
				_currentQueryIndex = value;
				PropertyChanged ( this, new PropertyChangedEventArgs ( "CurrentQuery" ) );
			}
		}

		public bool IsModified
		{
			get
			{
				return _isModified;
			}
			set
			{
				_isModified = value;
				PropertyChanged ( this, new PropertyChangedEventArgs ( "IsModified" ) );
			}
		}

		public QueryManager ( IFilenames filenames, IDatabase database )
		{
			_filenames = filenames;

			_database = database;
			_database.IterationsToRun = 1;

			_data = new RefactorData ();
			_data.Database = this._database;
		}

		public void RunQuery ( int currentTab )
		{
			_database.SetConnectionString ( this.ConnectionString );

			if ( currentTab == 0 )
			{
				runQuery ( _data.MasterQuery );
			}
			else
			{
				runQuery ( _data.Queries.Last () );
			}
		}

		/// <summary>
		/// Move back to the previous query in the list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void PreviousQuery ()
		{
			if ( CurrentQueryIndex > 0 )
				CurrentQueryIndex--;
		}

		/// <summary>
		/// Move forward to the next query in the list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void NextQuery ()
		{
			if ( CurrentQueryIndex < this._data.Queries.Count - 1 )
				CurrentQueryIndex++;
		}


		/// <summary>
		/// Call when the query is modified.
		/// </summary>
		public void TouchQuery ()
		{
			_isModified = true;

			if ( CurrentQuery.Results.Success.HasValue )
			{
				// Current query has been run, create a new query to hold the details.
				_data.Queries.Add ( new Query { Sql = _data.Queries.Last ().Sql } );
				CurrentQueryIndex++;
			}
		}

		/// <summary>
		/// Runs the given query. Displays the result in the given text box (either the master results, or detail results)
		/// </summary>
		/// <param name="queryText"></param>
		/// <param name="results"></param>
		/// <param name="query"></param>
		private void runQuery ( Query query )
		{
			_isModified = true;
			query.Database = this._database;

			BackgroundWorker worker = new BackgroundWorker ();
			worker.DoWork += ( sender, e ) => query.RunSql ();
			worker.RunWorkerCompleted += ( sender, e ) =>
			{
				//_window.btnRun.Content = "Run";
				//this.btnRun.Enabled = true;

				if ( query != _data.MasterQuery )
					this.CurrentQueryIndex = this._data.Queries.Count - 1;

				worker.Dispose ();
			};

			//_window.btnRun.Content = "Running...";
			//this.btnRun.Enabled = false;

			worker.RunWorkerAsync ();
		}

		/// <summary>
		/// Display the details of the master query.
		/// </summary>
		private void ShowMasterQuery ()
		{
			Query query = _data.MasterQuery;
			PropertyChanged ( this, new PropertyChangedEventArgs ( "MasterQuery" ) );
		}

		public void SaveToFile ( string filename )
		{
			_currentFilename = filename;

			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer ( this._data.GetType () );
			using ( System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create ( filename ) )
			{
				serializer.Serialize ( writer, this._data );
			}
			_isModified = false;
		}

		public void LoadFromFile ( string filename )
		{
			_currentFilename = filename;

			using ( System.Xml.XmlReader reader = System.Xml.XmlReader.Create ( filename ) )
			{
				System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer ( typeof ( RefactorData ) );
				this._data = serializer.Deserialize ( reader ) as RefactorData;
			}

			// Wire it all up again.
			this._data.Database = this._database;
			this._data.MasterQuery.Database = this._database;

			CurrentQueryIndex = this._data.Queries.Count - 1;
			ShowMasterQuery ();
			_isModified = false;
		}


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion
	}
}
