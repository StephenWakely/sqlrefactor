using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRefactor
{
	[Serializable]
	public class QueryData
	{
		private List<object[]> _results;

		/// <summary>
		/// Don't serialize the data results. That could be massive. 
		/// More efficient and probably correct to rebuild this every time.
		/// </summary>
		[System.Xml.Serialization.XmlIgnore]
		public IEnumerable<object[]> Results
		{
			get
			{
				return _results.OrderBy ( o => o.FirstOrDefault ().ToString() );
			}
		}

		public QueryData ()
		{
			_results = new List<object[]> ();
		}

		public void Add ( object[] row )
		{
			_results.Add ( row );
		}

		/// <summary>
		/// Compares the data to another result and returns the rows that differ
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public IEnumerable<Tuple<object[], object[]>> GetDifferences ( QueryData obj )
		{
			if ( obj == null )
				throw new ArgumentNullException ( "obj" );

			var differences = from rows in Results.Zip ( obj.Results, ( a, b ) => Tuple.Create ( a, b ) ) // Zip the rows together
							  where rows.Item1.Zip ( rows.Item2, ( a, b ) => Tuple.Create ( a, b ) ) // Zip the columns of each row together
												.Any ( cols => !cols.Item1.ToString ().Equals ( cols.Item2.ToString () ) ) // And compare the columns
							  select rows; 

			return differences;
		}
	}
}
