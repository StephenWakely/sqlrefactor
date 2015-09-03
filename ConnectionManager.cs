using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace SQLRefactor
{
	public class ConnectionManager : INotifyPropertyChanged
	{
		private string _server;
		private Lazy<List<String>> _servers = new Lazy<List<String>> ( () =>
		{
			List<String> servers = new List<String> ();
			servers.Add ( "(local)" );
			foreach ( DataRow server in SqlDataSourceEnumerator.Instance.GetDataSources ().Rows )
				servers.Add ( server["ServerName"].ToString () + "\\" + server["InstanceName"].ToString () );

			return servers;
		} );

		public List<String> Servers
		{
			get
			{
				return _servers.Value;
			}
		}

		public List<String> Databases { get; set; }
		public String Server
		{
			get { return _server; }
			set
			{
				_server = value;
				UpdateDatabases ();

				this.PropertyChanged ( this, new PropertyChangedEventArgs ( "Server" ) );
			}
		}

		public String Database { get; set; }
		public bool UseIntegratedSecurity { get; set; }
		public String UserName { get; set; }
		public String Password { get; set; }

		public String ConnectionString
		{
			get
			{
				if ( UseIntegratedSecurity )
					return String.Format ( "Data Source={0};Initial Catalog={1};Integrated Security=True;Connection Timeout=9999", Server, Database );
				else
					return String.Format ( "Data Source={0};Initial Catalog={1};uid={2};pwd={3};Connection Timeout=9999", Server, Database, UserName, Password );
			}
		}

		public ConnectionManager ()
		{
			Databases = new List<String> ();
			Server = "(local)";
		}

		/// <summary>
		/// Update the list of available databases.
		/// </summary>
		private void UpdateDatabases ()
		{
			Databases.Clear ();
			try
			{
				//String strConn = String.Format ( "server={0};uid={1};pwd={2}", _server );
				String strConn = String.Format ( "server={0};Integrated Security=True", _server );
				using ( SqlConnection sqlConn = new SqlConnection ( strConn ) )
				{
					sqlConn.Open ();
					DataTable tblDatabases = sqlConn.GetSchema ( "Databases" );
					sqlConn.Close ();

					foreach ( DataRow row in tblDatabases.Rows )
					{
						Databases.Add ( row["database_name"].ToString () );
					}
				}
			}
			finally
			{
				this.PropertyChanged ( this, new PropertyChangedEventArgs ( "Databases" ) );
			}
		}


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion
	}

}
