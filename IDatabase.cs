using System;
using System.Collections.Generic;
using System.Text;

namespace SQLRefactor
{
	public interface IDatabase
	{
		void SetConnectionString(string connectionString);
		void RunQuery ( string query, QueryResults results );

		int IterationsToRun
		{
			get;
			set;
		}
	}
}
