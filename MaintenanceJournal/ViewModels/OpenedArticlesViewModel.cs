using Joost;

using MaintenanceJournal.Models;
using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

		/// <summary>
		/// Show the unique articles to select.
		/// </summary>
		private void CollectArticles()
		{
			List<string> articles = VM.Journals
				.Where(x => x.Event == "Aangebroken")
				.Select(x => x.Message.Trim())
				.Distinct()
				.OrderBy(x => x)
				.ToList();

			for (int i = 0; i < articles.Count; i++)
			{
				articles[i] = Extensions.SplitArticle(articles[i]).Article;
			}

			View.ArticleComboBox.ItemsSource = articles
				.Distinct()
				.OrderBy(x => x)
				.ToList();
		}

		internal void ArticleSelected(object sender, RoutedEventArgs e)
		{
			if (sender == null) { return; }

			CollectReportAsync(((ComboBox)e.Source).SelectedValue.ToString().ToLower().SplitArticle().Article);
		}

		private void CollectReportAsync(string article)
		{
			View.ReportDataGrid.ItemsSource = null;

			List<Journal> articles = VM.Journals
				.Where(x => x.Event == "Aangebroken")
				.Where(x => x.Message.ToLower().SplitArticle().Article == article)
				.OrderByDescending(x => x.DTStart)
				.Select(x => x)
				.ToList();

			Report = new List<OpenedArticles>();

			if ((DateTime.Now - articles.First().DTStart.Value).TotalDays > 0)
			{
				Report.Add(new OpenedArticles
				{
					Opened = null,
					Days = (int)(DateTime.Now - articles.First().DTStart.Value.Date).TotalDays,
					Number = Extensions.SplitArticle(articles.First().Message).Number,
				});
			}

			for (int i = 0; i < articles.Count - 1; i++)
			{
				Report.Add(new OpenedArticles
				{
					Opened = articles[i].DTStart,
					Days = (int)(articles[i].DTStart.Value.Date - articles[i + 1].DTStart.Value.Date).TotalDays,
					Number = Extensions.SplitArticle(articles[i + 1].Message).Number,
				});
			}

			View.ReportDataGrid.ItemsSource = Report;
			if (Report[0].Number == "")
			{
				View.ReportDataGrid.Columns[2].Visibility = Visibility.Collapsed;
			}
			else
			{
				View.ReportDataGrid.Columns[2].Visibility = Visibility.Visible;
			}
		}

	}

	public static class Extensions
	{
		public static (string Article, string Number) SplitArticle(this string input)
		{
			string number = "";
			Regex regex = new Regex(@".(?<Number>\[.+\])");

			Match match = regex.Match(input);
			string article = regex.Replace(input, "");

			if (match.Value != "")
			{
				regex = new Regex(@"\[|\]");
				number = regex.Replace(match.Value, "").Trim();
			}

			return (article, number);
		}

	}
}
