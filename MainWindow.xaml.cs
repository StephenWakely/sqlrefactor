using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScintillaNET;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Threading;

namespace SQLRefactor
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		/// <summary>
		/// Manage the progress bar.
		/// </summary>
		internal class Progress : IProgressBar
		{
			private System.Windows.Controls.ProgressBar _progressBar;
			public Progress(System.Windows.Controls.ProgressBar progressBar)
			{
				this._progressBar = progressBar;
			}

			#region IProgressBar Members

			public int MaxIterations
			{
				get
				{
					return (int)this._progressBar.Dispatcher.Invoke(DispatcherPriority.Normal,
						new Func<int>(() => Convert.ToInt32(this._progressBar.Maximum)));
				}
				set
				{
					this._progressBar.Dispatcher.Invoke(DispatcherPriority.Normal,
						new Action(() => this._progressBar.Maximum = value));
				}
			}

			public void Reset()
			{
				this._progressBar.Dispatcher.Invoke(DispatcherPriority.Normal,
						new Action(() =>
						{
							this._progressBar.Value = 0;
							this._progressBar.Visibility = Visibility.Visible;
						}));
			}


			public void Finish()
			{
				this._progressBar.Dispatcher.Invoke(DispatcherPriority.Normal,
						new Action(() =>
						{
							this._progressBar.Visibility = Visibility.Hidden;
						}));
			}

			public void StepIt()
			{
				this._progressBar.Dispatcher.Invoke(DispatcherPriority.Normal,
						new Action(() => this._progressBar.Value++));
			}

			#endregion
		}

		public int CurrentQuery { get; set; }

		public string MasterQuery
		{
			get
			{
				return (masterQuery.Child as Scintilla).Text;
			}
			set
			{
				(masterQuery.Child as Scintilla).Text = value;
			}
		}

		public string IterationQuery
		{
			get
			{
				return (iterationQuery.Child as Scintilla).Text;
			}
			set
			{
				(iterationQuery.Child as Scintilla).Text = value;
			}
		}

		private void _SetStyles(Scintilla ed)
		{
			ed.Lexer = Lexer.Sql;
			ed.StyleResetDefault();
			ed.Styles[ScintillaNET.Style.Default].Font = "Consolas";
			ed.Styles[ScintillaNET.Style.Default].Size = 10;
			ed.StyleClearAll();

			ed.Styles[ScintillaNET.Style.Default].ForeColor = System.Drawing.Color.Silver;
			ed.Styles[ScintillaNET.Style.Sql.Comment].ForeColor = System.Drawing.Color.ForestGreen;
			ed.Styles[ScintillaNET.Style.Sql.CommentLine].ForeColor = System.Drawing.Color.ForestGreen;
			ed.Styles[ScintillaNET.Style.Sql.Identifier].ForeColor = System.Drawing.Color.CornflowerBlue;
			ed.Styles[ScintillaNET.Style.Sql.Operator].ForeColor = System.Drawing.Color.Maroon;
			ed.Styles[ScintillaNET.Style.Sql.Word].ForeColor = System.Drawing.Color.DarkOrange;
			ed.Styles[ScintillaNET.Style.Sql.Number].ForeColor = System.Drawing.Color.IndianRed;

			ed.SetKeywords(0, @"absolute action add admin after aggregate 
alias all allocate alter and any are array as asc 
assertion at authorization 
before begin binary bit blob body boolean both breadth by 
call cascade cascaded case cast catalog char character 
check class clob close collate collation column commit 
completion connect connection constraint constraints 
constructor continue corresponding create cross cube current 
current_date current_path current_role current_time current_timestamp 
current_user cursor cycle 
data date day deallocate dec decimal declare default 
deferrable deferred delete depth deref desc describe descriptor 
destroy destructor deterministic dictionary diagnostics disconnect 
distinct domain double drop dynamic 
each else end end-exec equals escape every except 
exception exec execute exists exit external 
false fetch first float for foreign found from free full 
function 
general get global go goto grant group grouping 
having host hour 
identity if ignore immediate in indicator initialize initially 
inner inout input insert int integer intersect interval 
into is isolation iterate 
join 
key 
language large last lateral leading left less level like 
limit local localtime localtimestamp locator 
map match merge minute modifies modify module month 
names national natural nchar nclob new next no none 
not null numeric 
object of off old on only open operation option 
or order ordinality out outer output 
package pad parameter parameters partial path postfix precision prefix 
preorder prepare preserve primary 
prior privileges procedure public 
read reads real recursive ref references referencing relative 
restrict result return returns revoke right 
role rollback rollup routine row rows 
savepoint schema scroll scope search second section select 
sequence session session_user set sets size smallint some| space 
specific specifictype sql sqlexception sqlstate sqlwarning start 
state statement static structure system_user 
table temporary terminate than then time timestamp 
timezone_hour timezone_minute to trailing transaction translation 
treat trigger true 
under union unique unknown 
unnest update usage user using 
value values varchar variable varying view 
when whenever where with without work write 
year 
zone
all alter and any array as asc at authid avg begin between 
binary_integer 
body boolean bulk by char char_base check close cluster collect 
comment commit compress connect constant create current currval 
cursor date day declare decimal default delete desc distinct 
do drop else elsif end exception exclusive execute exists exit 
extends false fetch float for forall from function goto group 
having heap hour if immediate in index indicator insert integer 
interface intersect interval into is isolation java level like 
limited lock long loop max min minus minute mlslabel mod mode 
month natural naturaln new nextval nocopy not nowait null number 
number_base ocirowid of on opaque open operator option or order 
organization others out package partition pctfree pls_integer 
positive positiven pragma prior private procedure public raise 
range raw real record ref release return reverse rollback row 
rowid rownum rowtype savepoint second select separate set share 
smallint space sql sqlcode sqlerrm start stddev subtype successful 
sum synonym sysdate table then time timestamp to trigger true 
type uid union unique update use user validate values varchar 
varchar2 variance view when whenever where while with work write 
year zone");

		}

		public MainWindow()
		{
			InitializeComponent();

			_SetStyles(masterQuery.Child as Scintilla);
			_SetStyles(iterationQuery.Child as Scintilla);

			_SetToolbarImage("SQLRefactor.openHS.png", OpenFileImage);
			_SetToolbarImage("SQLRefactor.saveHS.png", SaveFileImage);

			this.DataContextChanged += new DependencyPropertyChangedEventHandler(MainWindow_DataContextChanged);
		}

		void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue != null)
			{
				(e.OldValue as QueryManager).PropertyChanged -= MainWindow_PropertyChanged;
			}

			(e.NewValue as QueryManager).PropertyChanged += new PropertyChangedEventHandler(MainWindow_PropertyChanged);
		}

		/// <summary>
		/// If the CurrentQuery changes, refresh the differences grid...
		/// TODO, work out how to do this in Binding...
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "CurrentQuery")
			{
				// Now compare to the master
				var differences = (this.DataContext as QueryManager).MasterQuery.Results.Data.GetDifferences((this.DataContext as QueryManager).CurrentQuery.Results.Data);

				if (differences.Count() > 0)
				{
					// Add the columns.
					gridDifferences.Columns.Clear();
					for (int i = 0; i < differences.Max(x => new[] { x.Item1.Count(), x.Item2.Count() }.Max()); i++)
						gridDifferences.Columns.Add(new DataGridTextColumn { Header = "Column " + i.ToString(), Binding = new System.Windows.Data.Binding("[" + i.ToString() + "]") });

					gridDifferences.ItemsSource = this._GetDifferences(differences);
					gridDifferences.Visibility = System.Windows.Visibility.Visible;
				}
				else
				{
					gridDifferences.Visibility = System.Windows.Visibility.Collapsed;
				}
			}
		}

		private IEnumerable<object[]> _GetDifferences(IEnumerable<Tuple<object[], object[]>> differences)
		{
			foreach (var diff in differences)
			{
				yield return diff.Item1; //.Zip ( diff.Item2, (a,b) => a.ToString() + "|" + b.ToString() );
				yield return diff.Item2;
			}
		}

		private void _SetToolbarImage(string filename, Image image)
		{
			System.IO.Stream fileStream = this.GetType().Assembly.GetManifestResourceStream(filename);
			if (fileStream != null)
			{
				PngBitmapDecoder bitmapDecoder = new PngBitmapDecoder(fileStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
				ImageSource imageSource = bitmapDecoder.Frames[0];
				image.Source = imageSource;
			}
		}


		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//this._data = new RefactorData ();

			//CurrentQuery = -1;
			//ShowMasterQuery ();
		}

		private void masterQuery_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.Property.Name == "MasterSql")
				MasterQuery = e.NewValue.ToString();
		}

		private void iterationQuery_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.Property.Name == "CurrentQuery")
				IterationQuery = e.NewValue.ToString();
		}

		private void Iteration_TextChanged(object sender, EventArgs e)
		{
			if (!String.Equals((this.DataContext as QueryManager).IterationSql, IterationQuery))
			{
				(this.DataContext as QueryManager).TouchQuery();
				(this.DataContext as QueryManager).IterationSql = IterationQuery;
			}
		}

		private void Master_TextChanged(object sender, EventArgs e)
		{
			(this.DataContext as QueryManager).MasterSql = MasterQuery;
		}

	}
}
