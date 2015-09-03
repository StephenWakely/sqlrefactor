using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SQLRefactor
{
	public static class Commands
	{
		private static QueryManager _manager;
		private static MainWindow _window;
		private static IFilenames _filenames;

		public static RoutedUICommand RunQuery = new RoutedUICommand ( "RunQuery", "RunQuery", typeof ( Commands ), new InputGestureCollection { new KeyGesture ( Key.F5, ModifierKeys.None, "Run it" ) } );
		public static RoutedUICommand NextQuery = new RoutedUICommand ( "NextQuery", "NextQuery", typeof ( Commands ) );
		public static RoutedUICommand PreviousQuery = new RoutedUICommand ( "PreviousQuery", "PreviousQuery", typeof ( Commands ) );
		public static RoutedUICommand LoadFile = new RoutedUICommand ( "LoadFile", "LoadFile", typeof ( Commands ) );
		public static RoutedUICommand SaveFile = new RoutedUICommand ( "SaveFile", "SaveFile", typeof ( Commands ) );

		public static void Initialise ( MainWindow window, QueryManager manager )
		{
			_window = window;
			_manager = manager;
			_filenames = new Filenames();

			window.CommandBindings.Add ( new CommandBinding ( RunQuery, RunQueryHandler ) );
			window.CommandBindings.Add ( new CommandBinding ( NextQuery, NextQueryHandler ) );
			window.CommandBindings.Add ( new CommandBinding ( PreviousQuery, PreviousQueryHandler ) );
			window.CommandBindings.Add ( new CommandBinding ( LoadFile, LoadFileHandler ) );
			window.CommandBindings.Add ( new CommandBinding ( SaveFile, SaveFileHandler ) );
		}

		public static void RunQueryHandler ( object sender, ExecutedRoutedEventArgs e )
		{
			_manager.RunQuery ( _window.tabControl.SelectedIndex );
		}

		public static void NextQueryHandler ( object sender, ExecutedRoutedEventArgs e )
		{
			_manager.NextQuery ();
		}

		public static void PreviousQueryHandler ( object sender, ExecutedRoutedEventArgs e )
		{
			_manager.PreviousQuery ();
		}

		public static void LoadFileHandler ( object sender, ExecutedRoutedEventArgs e )
		{
			string filename = _filenames.GetLoadFilename ();
			if ( filename != null )
				_manager.LoadFromFile ( filename );
		}

		public static void SaveFileHandler ( object sender, ExecutedRoutedEventArgs e )
		{
			string filename = _filenames.GetSaveFilename ();
			if ( filename != null )
				_manager.SaveToFile ( filename );
		}

	}
}
