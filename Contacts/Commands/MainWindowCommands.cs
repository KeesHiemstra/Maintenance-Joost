using System.Windows.Input;

namespace Contacts.Commands
{
	public static class MainWindowCommands
	{
		public static readonly RoutedUICommand Exit = new RoutedUICommand
			(
				"E_xit",
				"Exit",
				typeof(MainWindowCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F4, ModifierKeys.Alt),
					new KeyGesture(Key.W, ModifierKeys.Control)
				}
			);

		public static readonly RoutedUICommand NewContact = new RoutedUICommand
			(
				"_New contact",
				"NewContact",
				typeof(MainWindowCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.N, ModifierKeys.Control)
				}
			);

	}
}
