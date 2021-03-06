﻿using MaintenanceJournal.ViewModels;

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
	}
}
