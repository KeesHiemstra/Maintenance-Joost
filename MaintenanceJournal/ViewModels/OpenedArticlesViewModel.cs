using Joost;

using MaintenanceJournal.Models;
using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace MaintenanceJournal.ViewModels
{
	public class OpenedArticlesViewModel : INotifyPropertyChanged
	{

		#region [ Fields ]

		private readonly MainViewModel VM;
		private OpenedArticlesWindow View;
		private int count;
		private double avg;
		private int min;
		private int max;

		#endregion

		#region [ Properties ]

		public int Count 
		{ 
			get => count;
			set
			{
				if (count != value)
				{
					count = value;
					NotifyPropertyChanged("Count");
				}
			}
		}
		public double Avg 
		{ 
			get => avg;
			set
			{
				if (avg != value)
				{
					avg = value;
					NotifyPropertyChanged("Avg");
				}
			}
		}
		public int Min 
		{ 
			get => min; 
			set
			{
				if (min != value)
				{
					min = value;
					NotifyPropertyChanged("Min");
				}
			}
		}
		public int Max 
		{ 
			get => max;
			set
			{
				if (max != value)
				{
					max = value;
					NotifyPropertyChanged("Max");
				}
			}
		}

		public List<OpenedArticles> Report { get; private set; } = new List<OpenedArticles>();

		#endregion

		#region [ Construction ]

		public OpenedArticlesViewModel(MainViewModel mainVM)
		{
			VM = mainVM;
		}

		#endregion

		#region [ Notification ]

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
			CollectOverviewReport();
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

			CollectDetailedReport(((ComboBox)e.Source).SelectedValue.ToString().SplitArticle().Article);
		}

		private List<Journal> GetArticles(string article)
		{
			//Mix of opened and closes articles
			return VM.Journals
					.Where(x => x.Event == "Aangebroken" || x.Event == "Afgesloten")
					.Where(x => x.Message.SplitArticle().Article == article)
					.OrderBy(x => x.DTStart) //Not Descending
					.Select(x => x)
					.ToList();
		}

		private void CollectOverviewReport()
		{
			View.ReportDataGrid.ItemsSource = null;

			Report = new List<OpenedArticles>();

			foreach (string item in View.ArticleComboBox.ItemsSource)
			{
				List<Journal> articles = GetArticles(item);
				if (articles.Count <= 1) { continue; }

				List<OpenedArticles> report = new List<OpenedArticles>();

				for (int i = 0; i < articles.Count - 1; i++)
				{
					report.Add(new OpenedArticles
					{
						Opened = articles[i].DTStart,
						Days = (int)(articles[i + 1].DTStart.Value.Date - articles[i].DTStart.Value.Date).TotalDays,
					});
				}

				Report.Add(new OpenedArticles()
				{
					Article = item,
					Count = report.Count,
					Avg = report.Average(x => x.Days),
					Min = report.Min(x => x.Days),
					Max = report.Max(x => x.Days),
				});

			}

			#region Show/hide columns

			for (int i = 0; i < 5; i++)
			{
				View.ReportDataGrid.Columns[i].Visibility = Visibility.Visible;
			}
			for (int i = 5; i < 8; i++)
			{
				View.ReportDataGrid.Columns[i].Visibility = Visibility.Collapsed;
			}

			#endregion

			View.ReportDataGrid.ItemsSource = Report;
		}

		private void CollectDetailedReport(string article)
		{
			View.ReportDataGrid.ItemsSource = null;

			List<Journal> articles = GetArticles(article);

			Report = new List<OpenedArticles>();

			for (int i = 0; i < articles.Count; i++)
			{
				if (i + 1 == articles.Count)
				{
					//Hit last record
					if (articles[i].Event == "Aangebroken")
					{
						Report.Insert(0, new OpenedArticles
						{
							Opened = articles[i].DTStart,
							Days = (int)(DateTime.Now.Date - articles[i].DTStart.Value.Date).TotalDays,
							Number = Extensions.SplitArticle(articles[i].Message).Number,
						});
					}
					else
					{
					}
				}
				else
				{
					//More records available
					if (articles[i].Event == "Aangebroken")
					{
						//Opened article has been properly been closed
						if (articles[i + 1].Event == "Afgesloten")
						{
							Report.Insert(0, new OpenedArticles
							{
								Closed = articles[i + 1].DTStart,
								Opened = articles[i].DTStart,
								Days = (int)((articles[i + 1]).DTStart.Value.Date - 
									articles[i].DTStart.Value.Date).TotalDays,
								Number = Extensions.SplitArticle(articles[i + 1].Message).Number,
							});
							continue;
						}
						//Next article has been openend
						else
						{
							Report.Insert(0, new OpenedArticles
							{
								Closed = articles[i + 1].DTStart.Value.Date.AddDays(-1),
								Opened = articles[i].DTStart,
								Days = (int)(articles[i + 1].DTStart.Value.Date.AddDays(-1) - 
									articles[i].DTStart.Value.Date).TotalDays,
								Number = Extensions.SplitArticle(articles[i + 1].Message).Number,
							});
						}
					}
				}
			}

			#region Show/hide columns

			View.OverviewBorder.Visibility = Visibility.Collapsed;
			
			for (int i = 0; i < 5; i++)
			{
				View.ReportDataGrid.Columns[i].Visibility = Visibility.Collapsed;
			}
			for (int i = 5; i < 7; i++)
			{
				View.ReportDataGrid.Columns[i].Visibility = Visibility.Visible;
			}
			//Number column
			View.ReportDataGrid.Columns[7].Visibility = Visibility.Collapsed;
			
			if (Report.Count > 0)
			{
				if (Report[0].Number != "")
				{
					View.ReportDataGrid.Columns[7].Visibility = Visibility.Visible;
				}
			
				if (Report[0].Number == "")
				{
					//Summary
					Count = Report.Count;
					Avg = Report.Average(x => x.Days);
					Min = Report.Min(x => x.Days);
					Max = Report.Max(x => x.Days);
			
					//Show summary
					View.OverviewBorder.Visibility = Visibility.Visible;
				}
			}
			
			#endregion

			//Add just opened article
			//if ((DateTime.Now - articles.First().DTStart.Value).TotalDays > 0)
			//{
			//	Report.Insert(0, new OpenedArticles
			//	{
			//		Opened = null,
			//		Days = (int)(DateTime.Now.Date - articles.First().DTStart.Value.Date).TotalDays,
			//		Number = Extensions.SplitArticle(articles.First().Message).Number,
			//	});
			//}
			//
			View.ReportDataGrid.ItemsSource = Report;
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
