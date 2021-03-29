using System.Windows.Input;

namespace QuickLog.Commands
{
	public static class MainCommands
	{
		public static readonly RoutedUICommand Exit = new RoutedUICommand
			(
				"E_xit",
				"Exit",
				typeof(MainCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F4, ModifierKeys.Alt),
					new KeyGesture(Key.W, ModifierKeys.Control),
				}
			);

		public static readonly RoutedUICommand Rename = new RoutedUICommand
			(
				"_Rename",
				"Rename",
				typeof(MainCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F12),
				}
			);

		public static readonly RoutedUICommand Save = new RoutedUICommand
			(
				"_Save",
				"Save",
				typeof(MainCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.S, ModifierKeys.Control)
				}
			);

		public static readonly RoutedUICommand Clear = new RoutedUICommand
			(
				"_Clear",
				"Clear",
				typeof(MainCommands),
				new InputGestureCollection() { }
			);

	}
}
