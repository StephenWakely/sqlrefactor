using System;
using System.Collections.Generic;
using System.Text;

namespace SQLRefactor
{
	public interface IQueryResults
	{
		bool Success 
		{
			get;
			set;
		}

		long Time
		{
			get;
			set;
		}

		int RowsReturned
		{
			get;
			set;
		}

		string ErrorMessage
		{
			get;
			set;
		}
	}
}
