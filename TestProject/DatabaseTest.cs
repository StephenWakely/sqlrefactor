using SQLRefactor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace TestProject
{


	/// <summary>
	///This is a test class for DatabaseTest and is intended
	///to contain all DatabaseTest Unit Tests
	///</summary>
	[TestClass ()]
	public class DatabaseTest
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
		///A test for RunQuery
		///</summary>
		[TestMethod ()]
		public void RunQueryTest ()
		{
			// Dependant on database.. not very useful...
			//DummyProgressBar progressBar = new DummyProgressBar ();

			//Database target = new Database ( progressBar );
			//target.SetConnectionString ( "Data Source=localhost;Initial Catalog=OOk;Integrated Security=True" );
			//string query = "select * from WorkType";
			//QueryResults results = new QueryResults ();
			//target.RunQuery ( query, results );

			//Assert.AreEqual ( true, results.Success, "Query had error " + results.ErrorMessage );
			//Assert.AreEqual ( true, results.Time > 0, "Query should have taken at least some time" );

			//Assert.AreEqual ( 1, progressBar.resetCalled, "Reset should only be called once" );
			//Assert.AreEqual ( 1, progressBar.stepitCalled, "Stepit should be called 10 times" );

		}

	}
}
