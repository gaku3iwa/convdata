using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Console;

namespace convdata
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Count() != 1 || (args.Count() == 1 && !File.Exists(args[0])))
      {
        Process.GetCurrentProcess().MainModule.FileName.Usage();
      }
      else
      {
        string full = Path.GetFullPath(args[0]);
        full.Read().Convert().Write(full + "T");
      }
      return;
    }
  }

  public static partial class Extensible
  {

    public static void Usage(this string Value)
    {
      ForegroundColor = ConsoleColor.DarkGreen;
      WriteLine($"");
      WriteLine($" This program reads binary data and converts text.");
      WriteLine($" Applicable only if the structure of the binary file is a 16-bit signed integer.");
      WriteLine($"");
      WriteLine($" How to....");
      WriteLine($"");
      WriteLine($" On the command line, write the executable file name,");
      WriteLine($" and then write the read file as an argument.Press return key");
      WriteLine($"   {Path.GetFileName(Value)} <filename>");
      WriteLine($"");
      WriteLine($" The output file name is the file name with T added to the data file name.");
      WriteLine($" It is saved in the same folder.");
      WriteLine($"   ex. 87654321.21 -> 87654321.21T");
      ResetColor();
    }

    public static List<short> Read(this string Value)
    {
      List<short> result = new List<short>();

      using (FileStream fs = File.Open(Value, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader br = new BinaryReader(fs))
        {
          for (int i = 0; i < (fs.Length / sizeof(short)); ++i)
          {
            short num = br.ReadInt16();
            result.Add(num);
          }
        }
      }
      return result;
    }

    public static List<Tuple<decimal, decimal>> Convert(this List<short> Value)
    {
      List<Tuple<decimal, decimal>> result = new List<Tuple<decimal, decimal>>();
      decimal n = 0M;

      foreach (var x in Value)
      {
        result.Add(new Tuple<decimal, decimal>(n, x * 0.03125M));
        n += 0.01M;
      }
      return result;
    }

    public static void Write(this List<Tuple<decimal, decimal>> Value, string FileName)
    {
      using (StreamWriter sw = new StreamWriter(FileName))
      {
        foreach (var x in Value)
        {
          sw.WriteLine($"{x.Item1,8:f2},{x.Item2,12:f5}");
        }
      }
    }

  } //  class Extensible

}
