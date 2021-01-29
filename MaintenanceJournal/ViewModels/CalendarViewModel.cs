using Joost;

using MaintenanceJournal.Models;
using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MaintenanceJournal.ViewModels
{
	public class CalendarViewModel : INotifyPropertyChanged
	{

		#region [ Fields ]

		private readonly MainViewModel VM;
		private CalendarWindow View;
		private readonly ObservableCollection<Journal> CalendarJournals;

		#endregion

		#region [ Properties ]

		public List<string> Events = new List<string>();
		public ObservableCollection<Calendar> Calendars { get; set; } =
			new ObservableCollection<Calendar>();

		public int CalendarCount { get => Calendars.Count; }

		#endregion

		#region [ Construction ]

		public CalendarViewModel(MainViewModel mainVM, ObservableCollection<Journal> journals)
		{
			VM = mainVM;
			CalendarJournals = journals;
			Events = CalendarJournals
				.Select(x => x.Event)
				.Distinct()
				.ToList();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region [ Public methods ]

		public void ShowReport()
		{
			CalendarWindow view = new CalendarWindow(this)
			{
				Left = VM.View.Left + 100,
				Top = VM.View.Top + 20,
			};
			View = view;

			View.DataContext = this;
			View.FilterEvent1ComboBox.ItemsSource = Events;
			View.FilterEvent2ComboBox.ItemsSource = Events;

			if (CalendarJournals.Count < 500) { CollectCalendar(); }

			View.Show();
		}

		#endregion

		internal void ApplyFilter() 
		{
			//View.CalendarDataGrid.ItemsSource = null;

			CollectCalendar();

			//View.CalendarDataGrid.ItemsSource = Filtered;
		}

		private void CollectCalendar()
		{
			DateTime fromDate, toDate, monday, sunday;
			
			View.CalendarDataGrid.ItemsSource = null;
			Calendars.Clear();

			if (View.FilterFromDatePicker.SelectedDate == null)
			{
				fromDate = CalendarJournals
					.Select(x => x.DTStart)
					.Min(x => x)
					.Value.Date;
			}
			else
			{
				fromDate = View.FilterFromDatePicker.SelectedDate.Value.Date;
			}
			fromDate = CalcMondayDate(fromDate);

			if (View.FilterToDatePicker.SelectedDate == null)
			{
				toDate = CalendarJournals
					.Select(x => x.DTStart)
					.Max(x => x)
					.Value;
			}
			else
			{
				toDate = View.FilterToDatePicker.SelectedDate.Value;
			}
			toDate = toDate.Date.AddDays(1).AddSeconds(-1);

			monday = CalcMondayDate(toDate);
			while (monday >= fromDate)
			{
				sunday = monday.AddDays(7).AddSeconds(-1);
				Calendar calendar = new Calendar(monday, DateInfo.DateTimeWeekNumber(monday));
				List<Journal> week = CalendarJournals
					.Where(x => x.DTStart >= monday && x.DTStart <= sunday)
					.ToList();

				CollectWeek(monday, week, calendar);
				Calendars.Add(calendar);

				monday = monday.AddDays(-7);
			}

			View.CalendarDataGrid.ItemsSource = Calendars;
		}

		private void CollectWeek(DateTime monday, List<Journal> week, Calendar calendar)
		{
			if (week.Count == 0) { return; }

			for (int i = 0; i < 7; i++)
			{
				List<Journal> day = week
					.Where(x => x.DTStart >= monday.AddDays(i) && x.DTStart < monday.AddDays(i + 1))
					.OrderBy(x => x.Event)
					.ThenByDescending(x => x.DTStart)
					.ToList();

				if (day.Count == 0) { continue; }

				int? dayNumber = (int)day
					.Select(x => x.DTStart.Value)
					.First().DayOfWeek;

				if (dayNumber != null)
				{
					string text = string.Empty;
					foreach (Journal item in day)
					{
						if (!string.IsNullOrEmpty(text)) { text += "\n"; }
						text += item.Message;
					}

					switch (dayNumber)
					{
						case 0: //Sunday
							calendar.Su = text;
							break;
						case 1: //Monday
							calendar.Mo = text;
							break;
						case 2: //Tuesday
							calendar.Tu = text;
							break;
						case 3: //Wednesday
							calendar.We = text;
							break;
						case 4: //Thursday
							calendar.Th = text;
							break;
						case 5: //Friday
							calendar.Fr = text;
							break;
						case 6: //Saturday
							calendar.Sa = text;
							break;
						default:
							break;
					}
				}
			}
		}

		private DateTime CalcMondayDate(DateTime date)
		{
			DateTime mondayDate = date.Date;
			int daysOfWeek = (int)mondayDate.DayOfWeek - 1;
			if (daysOfWeek < 0) { daysOfWeek = 6; }

			return mondayDate.AddDays(-daysOfWeek);
		}

	}
}
