﻿using MaintenanceJournal.ViewModels;

using System.Windows;

namespace MaintenanceJournal.Views
{
	/// <summary>
	/// Interaction logic for UrineReportWindow.xaml
	/// </summary>
	public partial class UrineReportWindow : Window
	{
		internal UrineReportWindow(UrineViewModel urineViewModel)
		{
			InitializeComponent();
		}
	}
}
