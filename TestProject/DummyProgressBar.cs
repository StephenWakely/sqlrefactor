using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLRefactor;

namespace TestProject
{
	public class DummyProgressBar : IProgressBar
	{
		public int resetCalled = 0;
		public int stepitCalled = 0;
		#region IProgressBar Members

		public int MaxIterations
		{
			get;
			set;
		}

		public void Reset ()
		{
			resetCalled++;
		}

		public void StepIt ()
		{
			stepitCalled++;
		}

		public void Finish ()
		{ 
		}

		#endregion
	}
}
