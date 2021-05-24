# Contacts

This WPF application is a simple UI version of the Joost ASP.Net site. It can add a record in 
remote Json file. On the same computer of Joost site, can the records imports.

## Why this application?

I can record on another computer and later import in the central database.

I struggle in previous applications to update the screens. I have now used the binding
in the `.xaml` item and that works better using like call the `ItemsSource` of the 
`DataGrid` or `ComboBox`.

I used the `INotifyPropertyChanged` in every file if possible.

**Part of MainWindow.xaml:**
```c#
<DataGrid
  Grid.Row="2"
  Grid.Column="0"
  Margin="5"
  AutoGenerateColumns="False"
  CanUserAddRows="False"
  CanUserDeleteRows="True"
  IsReadOnly="True"
  ItemsSource="{Binding Contacts}"
  MouseDoubleClick="DataGrid_MouseDoubleClick">
  <DataGrid.Columns>
    <DataGridTextColumn Binding="{Binding DTStart, StringFormat=yyyy-MM-dd HH:mm}" Header="Date/Time" />
    <DataGridTextColumn Binding="{Binding Event}" Header="Event" />
    <DataGridTextColumn Binding="{Binding Message}" Header="Message" />
  </DataGrid.Columns>
</DataGrid>
```

## ExportToJoost method

I used `LINQ` in the `ExportToJoost()` to prevent create duplicates in the database. I couldn't 
the `LogId` because is not yet known. I used the `DTCreation` property to find it.
Unfortunately there was small time different between the database and Json data. I used a frame of 
+/- 2 milliseconds to find the correct record.

**Part of ExportToJoost()**
```c#
//A stored DTCreation can difference of 2 milliseconds
DateTime min = record.DTCreation.Value.AddMilliseconds(-2);
DateTime max = record.DTCreation.Value.AddMilliseconds(2);

List<Journal> search = Db.Journals
	.Where(x => x.DTCreation >= min && x.DTCreation <= max &&
		x.Event == record.Event)
	.ToList();
```


