using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace SQLRefactor
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup ( StartupEventArgs e )
		{
			base.OnStartup ( e );

			// Initialise the objects and set them on the main window.
			var mainWindow = new MainWindow ();
			var manager = new QueryManager ( new Filenames (), new Database ( new SQLRefactor.MainWindow.Progress ( mainWindow.progressBar ) ) );
			Commands.Initialise ( mainWindow, manager );
			mainWindow.DataContext = manager;
			mainWindow.Show ();

			var connectionWindow = new Connection ();
			var connection = new ConnectionManager ();
			connectionWindow.DataContext = connection;

			connectionWindow.Owner = mainWindow;
			connectionWindow.ShowDialog ();
			manager.ConnectionString = connection.ConnectionString;

		}
	}
}
