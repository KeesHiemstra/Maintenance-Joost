# MaintenanceJournal

I wanted more learn C# with Visual Studio 2017 in 2018. I used ASP.Net. 
I created a log system using the browser.

This experiment uses WPF to use the log system in the same way.

## MainDataGrid

An update of the list is not visible if there a focus on the `MainDataGrid`. 
The `SaveRecord()` forced to the focus outside of `MainDataGrid`.

## Reports

The first I added was setup the reports.

Such a report can help to cleanup the raw data.

Creating the reports learns about LINQ.

### Opened articles

### Coffee usage

### Fallen rain

Is a dynamic object. The basis is not so difficult, but it show in grid was a challenge.

### Got up time

The report shows the minimum, average, and maximum got up times.

The challenge in this report to calculate with the average `TimeSpan`.
It is done with convert the time in `double` TotalSeconds.

```
Avg = TimeSpan.FromSeconds(Math.Round(_journals.Average(x => (double)x.Time.TotalSeconds)))
``` 

# Wish list

- [ ] The first record was saved, but additional records was not saved.
- [ ] The number are right aligned, inclusive the first column. The first column should be left aligned.
- [X] The records are not updated after restore a database.
- [X] Text of 'Event filter' was reset at a `NotifyPropertyChanged`.