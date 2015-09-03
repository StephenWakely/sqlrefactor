using System;
using System.Collections.Generic;
using System.Text;

namespace SQLRefactor
{
	public class RefactorData
	{
		private Query _masterQuery;

		// Create the list of queries - populated with the initial one.
		private List<Query> _queries = new List<Query> () { new Query () };

		public IDatabase Database
		{
			set
			{
				_masterQuery.Database = value;
			}
		}

		public Query MasterQuery
		{
			get
			{
				return this._masterQuery;
			}
			set
			{
				this._masterQuery = value;
			}
		}

		public List<Query> Queries
		{
			get
			{
				return this._queries;
			}
		}


		public RefactorData ()
		{
			_masterQuery = new Query ( );
		}


	}
}
