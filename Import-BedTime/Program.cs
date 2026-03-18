using CHi.Extensions;

using CsvHelper;
using CsvHelper.Configuration;

using Joost;

using QuickLog;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import_BedTime
{
  internal class Program
  {
    private static string Database = "Joost_Dev";
    private static readonly string InputPath = "%OneDrive%\\Data\\BedTime.csv";
    private static readonly CsvConfiguration Config =
      new CsvConfiguration(CultureInfo.CurrentUICulture)
      {
        Delimiter = "\t",
      };

    public static List<QLog> Logs { get; set; } = new List<QLog>();
    public static JournalDbContext Db { get; private set; }

    static void Main(string[] args)
    {
      Console.WriteLine("Import-BedTime\n");
      if (Environment.MachineName == "ROMMELDIJK")
      {
        Database = "Joost";
      }

      ImportQuickLog();

      Console.Write("\nPress any key...");
      Console.ReadKey();
    }

    private static void ImportQuickLog()
    {
      if (!File.Exists(InputPath.TranslatePath()))
      {
        Console.WriteLine($"The file '{InputPath}' doesn't exist");
        return;
      }

      //Read the all records of csv file
      using (var reader = new StreamReader(InputPath.TranslatePath()))
      using (var cvs = new CsvReader(reader, Config))
      {
        var logs = cvs.GetRecords<QLog>();
        Logs = new List<QLog>(logs);
      }

      Console.WriteLine($"Read {Logs.Count} records");
      if (Logs.Count == 0) { return; }

      ProcessCsv();
    }

    private static void ProcessCsv()
    {
      string connection = $"Trusted_Connection=True;Data Source=(Local);" +
        $"Database={Database};MultipleActiveResultSets=true";

      int added = 0;
      int changed = 0;
      int skipped = 0;
      int excepts = 0;
      int missed = 0;
      //Open the database
      using (Db = new JournalDbContext(connection))
      {
        //Process each record in csv file
        foreach (QLog record in Logs)
        {
          //Search gotup time for the date of the bedtime time
          DateTime dt1 = record.Time.Date;
          DateTime dt2 = record.Time.Date.AddDays(1);
          List<Journal> search = Db.Journals
            .Where(x => x.Event == "Opgestaan" &&
              x.DTStart >= dt1 && x.DTStart < dt2)
            .ToList();

          if (search.Count == 0)
          {
            missed++;
            Console.WriteLine($"Missed {record.Time}");
          }
          else if (search.Count > 1)
          {
            excepts++;
            Console.WriteLine($"Multiple records found for {record.Time}");
          }
          else
          {
            DateTime FoundDateTime = search.First().DTStart.Value;
            if (record.Time < FoundDateTime)
            {
              //BedTime after midnight, need to add one day
              record.Time = record.Time.AddHours(24);
            }
            TimeSpan timeSpan = record.Time - FoundDateTime;
            string message = $"Dag tijd: {timeSpan.ToString(@"hh\:mm")}";

            List<Journal> found = Db.Journals
              .Where(x => x.DTStart == record.Time &&
                     x.Event == "Naar bed")
              .ToList();

            if (found.Count == 0)
            {
              Journal journal = new Journal()
              {
                DTStart = record.Time,
                DTCreation = record.Time,
                Event = "Naar bed",
                Message = message,
              };
              Db.Journals.Add(journal);
              Db.SaveChanges();

              added++;
            }
            else
            {
              if (found.Count == 1)
              {
                if (found[0].Message != message)
                {
                  found[0].Message = message;
                  Db.SaveChanges();

                  changed++;
                }
                else
                {
                  skipped++;
                }
              }
              else
              {
                excepts++;
              }
            }
          }
        }
      }

      Console.WriteLine($"Missed {missed} records");
      Console.WriteLine($"Added {added} records");
      Console.WriteLine($"Changed {changed} records");
      Console.WriteLine($"Skipped {skipped} records");
      Console.WriteLine($"{excepts} lines caused to excepts");
    }

  }
}
