# New window

Using a new window has several general steps. In this documentation I use NewWindow as example.
The `New` part in the name can be used for other pieces.

A new window uses a `NewViewModel.cs`, `NewWindow.xaml`, and `NewWindow.xaml.cs`.

## Step 1: Create `NewWindow`

Start to create `NewWindow` (`Window (WPF)`) file in the `Views` folder.

It used to setup references in `NewViewModel`.

## Step 2: Create `NewViewModel`

Start to create `NewViewModel` (`Class`) file in the `ViewModels` folder.

Make the class public accessible.

Add the fields `VM` and `View`, the link to the central collection (`MainViewModel`).

```
private readonly MainViewModel VM;
private NewWindow View;
```

Add the construction.

```
public NewViewModel(MainViewModel mainViewModel)
{
	VM = mainViewModel;
}
```

Add the general `ShowReport()` method

```
public void ShowReport()
{
	NewWindow view = new NewWindow(this)
	{
		Left = VM.View.Left + 100,
		Top = VM.View.Top + 20,
	};
	View = view;
	View.DataContext = this;

	//Collect the window data
	View.Show();
}
```

## Step 3: Edit 'NewWindow.xaml.cs' file

Edit the construction and add the 'NewViewModel' as parameter.

```
public GotUpTimeWindow(NewViewModel newViewModel)
```

## Step 4: Add menu entry

Edit the `MainWindowCommands.cs` file in the folder `Commands`.

Add a `RoutedUICommand`

```
public static readonly RoutedUICommand ReportNew = new RoutedUICommand
	(
		"_New",
		"ReportNew",
		typeof(MainWindowCommands),
		new InputGestureCollection() { }
	);
```

Edit the 'MainWindows.xaml' file in the folder `Views`.

Add a `CommandBinding` in the section `Window.CommandBindings`

```
<CommandBinding
	CanExecute="ReportNewCommand_CanExecute"
	Command="cmd:MainWindowCommands.ReportNew"
	Executed="ReportNewCommand_Executed" />

```

Add a `MenuItem` in the section `Menu`.

```
<MenuItem Command="cmd:MainWindowCommands.ReportNew" />
```

Edit the `MainWindow.xaml.cs` file in `Views` folder.

Add the `ReportNewCommand_CanExecute` and `ReportNewCommand_Executed` methods.

```
private void ReportNewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
{
	e.CanExecute = true;
}

private void ReportNewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
{
	VM.ReportNew();
}
```

Edit the `MainViewModel`.

Add the `ReportNew` method.

```
internal void ReportNew()
{
	NewViewModel report = new NewViewModel(this);
	report.ShowReport();
}
```

## Step 5: Finalize the report part
