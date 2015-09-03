using System;
using System.Collections.Generic;
using System.Text;

namespace SQLRefactor
{
	public interface IProgressBar
	{
		int MaxIterations
		{
			get;
			set;
		}

		void Reset ();
		void Finish ();
		void StepIt ();
	}
}
