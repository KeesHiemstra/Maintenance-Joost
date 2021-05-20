using Contacts.ViewModels;

using System.Windows;
using System.Windows.Controls;

namespace Contacts.Views
{
	/// <summary>
	/// Interaction logic for EditContactWindow.xaml
	/// </summary>
	public partial class EditContactWindow : Window
	{
		#region [ Fields ]

		private readonly EditContactViewModel ContactVM;

		#endregion

		#region [ Construction ]

		public EditContactWindow(EditContactViewModel contactVM)
		{
			InitializeComponent();

			ContactVM = contactVM;
			DataContext = contactVM;

			if (string.IsNullOrEmpty(ContactVM.Contact.Message))
			{
				Title = "New contact";
			}
		}

		#endregion

		#region Buttons

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			ContactVM.SaveContact();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			ContactVM.CancelContact();
		}

		#endregion

		#region Handle TimeTextBox

		/// <summary>
		/// Changes in the TimeTextBox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimeTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			//Prevent action if is not focused
			if (!((TextBox)e.Source).IsFocused) { return; }

			//Prevent errors
			string text = ((TextBox)e.Source).Text;
			if (string.IsNullOrEmpty(text)) { return; }

			//Keep the caret position
			int caretIndex = ((TextBox)e.Source).CaretIndex;

			//Prevent longer time text
			if (text.Length > 4)
			{
				((TextBox)e.Source).Text = text.Substring(0, 4);
				((TextBox)e.Source).CaretIndex = caretIndex;
				return;
			}

			//Allow only numbers
			foreach (char check in text)
			{
				if (check < '0' || check > '9')
				{
					((TextBox)e.Source).Text = text.Replace(check.ToString(), "");
					((TextBox)e.Source).CaretIndex = caretIndex;
					return;
				}
			}
		}
		
		/// <summary>
		/// Select all the text.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimeTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			((TextBox)e.Source).SelectAll();
		}

		/// <summary>
		/// Format the time.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TimeTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			string text = ((TextBox)e.Source).Text;
			//Prevent errors
			if (string.IsNullOrEmpty(text)) { return; }
			//Prevent adding a colon if it already exist
			if (text.Contains(":")) { return; }

			//Add zeros and a colon
			text = text.PadLeft(4, '0');
			((TextBox)e.Source).Text = text.Insert(text.Length - 2, ":");
		}

		#endregion

		#region Handle MessageTextBox

		/// <summary>
		/// Prevent to save empty message.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			//Prevent action if is not focused
			if (!((TextBox)e.Source).IsFocused) { return; }

			//Prevent errors and save empty message
			string text = ((TextBox)e.Source).Text;
			if (string.IsNullOrEmpty(text))
			{
				SaveButton.IsEnabled = false;
				return; 
			}

			SaveButton.IsEnabled = true;
		}

		#endregion

	}
}
