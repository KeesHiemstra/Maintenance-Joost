using Joost;

using MaintenanceJournal.Models;
using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MaintenanceJournal.ViewModels
{
	public class OpenedArticlesViewModel
	{

		#region [ Fields ]

		private readonly MainViewModel VM;
		private OpenedArticlesWindow View;

		#endregion

		#region [ Properties ]

		public List<OpenedArticles> Report { get; private set; } = new List<OpenedArticles>();

		#endregion

		#region [ Construction ]

		public OpenedArticlesViewModel(MainViewModel mainVM)
		{
			VM = mainVM;
		}

		#endregion

		internal void ArticleSelected(object sender, RoutedEventArgs e)
		{
			if (sender == null)	{ return;	}

			CollectReportAsync(((ComboBox)e.Source).SelectedValue.ToString().ToLower());
		}

		public void ShowReport()
		{
			OpenedArticlesWindow view = new OpenedArticlesWindow(this)
			{
				Left = VM.View.Left + 100,
				Top = VM.View.Top + 20,
			};
			View = view;

			CollectArticles();
			View.Show();
		}

		private void CollectReportAsync(string article)
		{
			View.ReportDataGrid.ItemsSource = null;

			List<Journal> articles = VM.Journals
				.Where(x => x.Event == "Aangebroken")
				.Where(x => x.Message.ToLower() == article)
				.OrderByDescending(x => x.DTStart)
				.Select(x => x)
				.ToList();

			Report = new List<OpenedArticles>();
			
			if ((DateTime.Now - articles.First().DTStart.Value).TotalDays > 2)
			{
				Report.Add(new OpenedArticles
				{
					Opened = null,
					Days = (int)(DateTime.Now - articles.First().DTStart.Value.Date).TotalDays
				});
			}

			for (int i = 0; i < articles.Count - 1; i++)
			{
				Report.Add(new OpenedArticles
				{
					Opened = articles[i].DTStart,
					Days = (int)(articles[i].DTStart.Value.Date - articles[i + 1].DTStart.Value.Date).TotalDays
				});
			}

			View.ReportDataGrid.ItemsSource = Report;
		}

		private void CollectArticles()
		{
			View.ArticleComboBox.ItemsSource = VM.Journals
				.Where(x => x.Event == "Aangebroken")
				.Select(x => x.Message.Trim())
				.Distinct()
				.OrderBy(x => x)
				.ToList();
		}
	}
}
