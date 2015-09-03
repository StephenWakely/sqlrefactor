using SQLRefactor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestProject
{
	
	
	/// <summary>
	///This is a test class for QueryTest and is intended
	///to contain all QueryTest Unit Tests
	///</summary>
	[TestClass ()]
	public class QueryTest
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
		///A test for Query Constructor
		///</summary>
		[TestMethod ()]
		public void QueryConstructorTest ()
		{
			DummyDatabase database = new DummyDatabase ( true, 1 );

			Query target = new Query ( );
			target.Database = database;
			// Should start as null until it has run.
			Assert.AreEqual ( null, target.Results.Success );
			Assert.AreEqual ( 0, target.Results.Time );
		}

		[TestMethod ()]
		public void RunSQL_HasResults_Test ()
		{
			DummyDatabase database = new DummyDatabase ( true, 1 );
			Query target = new Query ( );
			target.Database = database;
			target.Sql = "sql";

			target.RunSql ( );

			Assert.AreEqual ( true, target.Results.Success );
			Assert.AreEqual ( 1, target.Results.Time );
		}

	}
}
