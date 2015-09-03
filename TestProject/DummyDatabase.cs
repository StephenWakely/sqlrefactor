using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRefactor;

namespace TestProject
{
	public class DummyDatabase : IDatabase
	{
		bool _shouldSucceed;
		int _time;

		public DummyDatabase ( bool shouldSucceed, int time )
		{
			this._shouldSucceed = shouldSucceed;
			this._time = time;
		}

		#region IDatabase Members

		public void SetConnectionString ( string connectionString )
		{
			
		}

		public void RunQuery ( string query, QueryResults results )
		{
			results.Success = _shouldSucceed;
			results.Time = _time;
		}

		public int IterationsToRun
		{
			get
			{
				return 10;
			}
			set
			{
				
			}
		}

		#endregion
	}
}
