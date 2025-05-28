using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace MaintenanceJournal.ViewModels
{
	public class FallenRainViewModel
	{

		#region [ Fields ]

		private readonly MainViewModel VM;
		private FallenRainWindow View;

		//Overwrite the Region number style
		private NumberStyles style = NumberStyles.Integer | NumberStyles.AllowDecimalPoint;
		private CultureInfo provider = new CultureInfo("en-US", false);

		public readonly string[] Names = new string[] { "Total",
				"January",
				"February",
				"March",
				"April",
				"May",
				"June",
				"July",
				"August",
				"September",
				"October",
				"November",
				"December"
				};

		#endregion

		#region [ Properties ]

		public DataSet Data { get; set; } = new DataSet("Data");
		private DataView PivotView { get; set; }

		#endregion

		#region [ Construction ]

		public FallenRainViewModel(MainViewModel mainVM)
		{
			VM = mainVM;
		}

		#endregion

		#region [ Public methods ]

		public void ShowReport()
		{
			FallenRainWindow view = new FallenRainWindow(this)
			{
				Left = VM.View.Left + 100,
				Top = VM.View.Top + 20,
			};
			View = view;
			View.DataContext = this;

			CreateTable();
			View.Show();
		}

		#endregion

		private void CreateTable()
		{
			if (Data.Tables.Count > 0) { Data.Tables.Clear(); }

			DataTable pivot = new DataTable("Pivot");
			pivot.Columns.Add("Month", typeof(string));
			pivot.Columns[0].AllowDBNull = false;

			TotalYearFallenRain(pivot);

			PivotView = new DataView(Data.Tables["Pivot"]);
			View.FallenRainDataGrid.ItemsSource = PivotView;
		}

		private void TotalYearFallenRain(DataTable pivot)
		{
			var years = VM.Journals
				.Where(x => x.Event == "Regen")
				.Where(x => x.DTStart >= new DateTime(2018, 01, 01))
				.GroupBy(
					x => x.DTStart.Value.Year,
					x => decimal.Parse(x.Message, style, provider),
					(Year, TotalRain) => new
					{
						Key = Year,
						Rain = TotalRain.Sum(x => x)
					}
					)
				.OrderByDescending(x => x.Key);

			//Create pivot header
			int count = 0;
			foreach (var year in years)
			{
				pivot.Columns.Add(year.Key.ToString(), typeof(decimal));
				count++;
				pivot.Columns[count].AllowDBNull = true;
			}
			Data.Tables.Add(pivot);

			//Fill the first row (total)
			DataRow row = null;
			row = Data.Tables[0].NewRow();
			row["Month"] = Names[0];

			List<decimal> rainYears = new List<decimal>();
			count = 0;
			foreach (var year in years)
			{
				count++;
				row[count] = Math.Round(year.Rain, 0);
				rainYears.Add(year.Key);
			}

			Data.Tables[0].Rows.Add(row);


			TotalMonthFallenRain(rainYears);
		}

		private void TotalMonthFallenRain(List<decimal> rainYears)
		{
			var months = VM.Journals
				.Where(x => x.Event == "Regen")
				.Where(x => x.DTStart >= new DateTime(2018, 01, 01))
				.GroupBy(
					x => (Month: x.DTStart.Value.Month, Year: x.DTStart.Value.Year),
					x => decimal.Parse(x.Message, style, provider),
					(Date, rain) => new
					{
						Key = Date,
						MonthTotals = rain.Sum(x => x)
					}
					)
				.OrderBy(x => x.Key.Month)
				.ThenByDescending(x => x.Key.Year);

			int dispRow = 0;
			int dispCol = 0;
			DataRow row = null;
			foreach (var month in months)
			{
				if (month.Key.Month != dispRow)
				{
					if (row != null) { Data.Tables[0].Rows.Add(row); }

					dispRow++;
					row = null;
					row = Data.Tables[0].NewRow();
					row["Month"] = Names[dispRow];
					dispCol = 1;
				}

				if (dispCol == 1 && month.Key.Year != rainYears[0])
				{
					row[dispCol] = DBNull.Value;
					dispCol++;
					row[dispCol] = Math.Round(month.MonthTotals, 0);
				}
				else
				{
					row[dispCol] = Math.Round(month.MonthTotals, 0);
				}

				dispCol++;
			}
			if (row != null) { Data.Tables[0].Rows.Add(row); }
		}

	}
}
