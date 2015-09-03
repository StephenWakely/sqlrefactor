using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLRefactor
{
	public class Filenames : IFilenames
	{
		public string GetLoadFilename()
		{
			Microsoft.Win32.OpenFileDialog openDlg = new Microsoft.Win32.OpenFileDialog ();
			openDlg.Filter = "refactor files|*.sqlr";

			if ( openDlg.ShowDialog () ?? false )
				return openDlg.FileName;

			return null;
		}

		public string GetSaveFilename()
		{
			Microsoft.Win32.SaveFileDialog saveDlg = new Microsoft.Win32.SaveFileDialog ();
			saveDlg.DefaultExt = ".sqlr";

			if ( saveDlg.ShowDialog () ?? false )
				return saveDlg.FileName;

			return null;
		}

	}
}
