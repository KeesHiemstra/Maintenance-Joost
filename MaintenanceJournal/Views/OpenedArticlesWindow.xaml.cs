using MaintenanceJournal.ViewModels;

using System.Windows;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for OpenedArticlesWindow.xaml
	/// </summary>
	public partial class OpenedArticlesWindow : Window
	{
		public OpenedArticlesViewModel ArticlesVM { get; private set; }

		public OpenedArticlesWindow(OpenedArticlesViewModel articlesVM)
		{
			InitializeComponent();

			ArticlesVM = articlesVM;
			DataContext = articlesVM;
			ArticleComboBox.Focus();
		}

		private void ArticleComboBox_Selected(object sender, RoutedEventArgs e)
		{
			ArticlesVM.ArticleSelected(sender, e);
		}

		private void ReportDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			ArticlesVM.ArticleDoubleClicked(sender, e);
		}

		private void ReportDataGrid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			ArticlesVM.ArticleKeyUp(sender, e);
		}

		private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Escape)
			{
				Close();
			}
		}

	}
}
