using MaintenanceJournal.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceJournal.ViewModels
{
	public class DefecateViewModel
	{

		#region [ Fields ]

		private readonly MainViewModel VM;
		private DefecateWindow View;

		#endregion

		#region [ Construction ]

		public DefecateViewModel(MainViewModel mainVM)
		{
			VM = mainVM;
		}

		#endregion

		#region [ Properties ]

		List<>

		#endregion

		#region [ Public method ]

		public void ShowReport()
		{
			DefecateWindow view = new DefecateWindow(this)
			{
				Left = VM.View.Left + 100,
				Top = VM.View.Top + 20,
			};
			View = view;

			CollectReport();
			View.Show();
		}

		#endregion

		private void CollectReport()
		{
			throw new NotImplementedException();
		}



	}
}
