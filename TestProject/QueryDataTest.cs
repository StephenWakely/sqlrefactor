using SQLRefactor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace TestProject
{


	/// <summary>
	///This is a test class for QueryDataTest and is intended
	///to contain all QueryDataTest Unit Tests
	///</summary>
	[TestClass ()]
	public class QueryDataTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}


		/// <summary>
		///A test for GetDifference_RowCol
		///</summary>
		[TestMethod ()]
		public void GetDifferences_NoDiff ()
		{
			QueryData target = new QueryData ();
			QueryData obj = new QueryData ();

			target.Add ( new object[] { "a", 1, "c" } );
			obj.Add ( new object[] { "a", 1, "c" } );

			target.Add ( new object[] { "x", 2, "z" } );
			obj.Add ( new object[] { "x", 2, "z" } );

			Assert.AreEqual ( 0, target.GetDifferences ( obj ).Count () );
		}

		[TestMethod ()]
		public void GetDifferences_Diff ()
		{
			QueryData target = new QueryData ();
			QueryData obj = new QueryData ();

			target.Add ( new object[] { "a", 1, "c" } );
			obj.Add ( new object[] { "a", 1, "c" } );

			target.Add ( new object[] { "x", 2, "z" } );
			obj.Add ( new object[] { "x", 2, "a" } );

			Assert.AreEqual ( 1, target.GetDifferences ( obj ).Count () );
		}
	}
}
