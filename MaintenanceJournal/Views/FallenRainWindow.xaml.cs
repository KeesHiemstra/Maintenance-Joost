﻿using MaintenanceJournal.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for FallenRainWindow.xaml
	/// </summary>
	public partial class FallenRainWindow : Window
	{
		public FallenRainWindow(FallenRainViewModel fallenRainVM)
		{
			InitializeComponent();
		}

		private void FallenRainDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
		{
			for (int i = 1; i < FallenRainDataGrid.Columns.Count; i++)
			{
				FallenRainDataGrid.Columns[i].MinWidth = 50;
			}
		}
	}
}