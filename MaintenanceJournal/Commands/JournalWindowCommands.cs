using System.Windows.Input;

namespace MaintenanceJournal.Commands
{
	public static class JournalWindowCommands
	{
		public static readonly RoutedUICommand Close = new RoutedUICommand
			(
				"_Close",
				"Close",
				typeof(JournalWindowCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.Escape)
				}
			);

		public static readonly RoutedUICommand Delete = new RoutedUICommand
			(
				"_Delete",
				"Delete",
				typeof(JournalWindowCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.Delete, ModifierKeys.Shift)
				}
			);
	}
}
