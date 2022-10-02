using CommandLine;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsStrongAsFuck
{
    class Program
    {
        public static Worker Worker { get; set; }
        public class Options
        {
            
            [Option('o', "obfuscators", Default = null, Required = true, HelpText = "List of obfuscation methods to use (numeric string 1-9)")]
            public string Obfuscators { get; set; }

            [Option('f', "file", Required = true, HelpText = "Obfuscate a file on disk")]
            public string InFile { get; set; } 

            [Option('h', "help", Required = false, HelpText = "Print this help")]
            public bool Help { get; set; } 
        }

        public static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);

            watch.Stop();

        }

        static void PrintHelp() {
            Console.WriteLine("Usage: asaf.exe -f <path> -o <obfuscators>");
            Console.WriteLine("  Ex: asaf.exe -f .\\test.exe -o 134");
            Console.WriteLine("Available obfuscators: ");
            for (int i = 0; i < Worker.Obfuscations.Count; i++)
            {
                Console.WriteLine(i + 1 + ") " + Worker.Obfuscations[i]);
            }

        }

        static void RunOptions(Options opts)
        {

            if (opts.Help) {
                PrintHelp();
                Environment.Exit(0);
            }

            var Worker = new Worker(opts.InFile);

            Worker.ExecuteObfuscations(opts.Obfuscators);
            Worker.Save();
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (Error err in errs)
            {
                Console.Error.WriteLine(err.ToString());
            }
        }
    }
}
