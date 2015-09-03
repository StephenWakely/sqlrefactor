using System;
namespace SQLRefactor
{
	public interface IFilenames
	{
		string GetLoadFilename ();
		string GetSaveFilename ();
	}
}
