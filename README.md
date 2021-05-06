# Maintenance Joost

This solution contains applications to maintain the Joost database.
Joost is a logging database.

# Application `MaintenanceJournal`

`MaintenanceJournal` is UI application using `WPF` to support JoostWeb with several reports which are not so development in ASP.Net.



# User articles

For some articles I use the logging system. An example is coffee. How many cups coffee drink I from a jar solution coffee. 
It starts with an entry {Event: Aangebroken, Message: Oploskoffie, pot} and the make entry for every cup {Event: Kop koffie, Message: 1}.

A report show how many cups come from a jar, how many cups coffee drink I and how many days use a jar.

The names followed a certain format in `Message` when I started with the used articles. The chosen format was not very useful. 
I decided to rename the used articles and I used 2 steps.

### Tool `Export-UsedArticles`

The `Export-UsedArticles` cli export the current `Message` for all entries marked `Event` record with `Aangebroken`.

The exported json is a directory list where the `Key` and the `Value` the same.

I used a text editor to 'translate' the `Message` to the a new format.

### Tool `Import-TranslatedUsedArticles`

The `Import-TranslatedUsedArticles` cli reads the json and processes the translated record.

# External logs

I used pen and paper for track the events because direct editing in JoostWeb was not very handy.
Taken over these handwritten notes in JoostWeb took a lot effort.

The 2 application uses `CsvHelper` extension to use csv file create and read the make and processes notes.

### Tool `QuickLog`

The `QuickLog` is a UI application to taken over the handwritten notes. The date is easily to use and the time part is only the numbers, so no format.

It doesn't a lot of time to taken over the notes.

### Tools `Import-QuickLog'

The `Import-QuickLog` cli reads the csv file and processes the stored notes.