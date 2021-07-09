using Bb;
using Bb.CommandLines.Outs;
using Bb.DataDeep.Models.Mpd;
using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using DotnetParser.Commands;
using System;
using System.IO;

namespace DotnetParser
{

    public partial class Program
    {

        public static int ExitCode { get; private set; }

        public static void Main(params string[] args)
        {

            CommandLineApplication app = null;
            try
            {

                app = new CommandLineApplication()
                    .Initialize()
                    .CommandExport()
                ;

                int result = app.Execute(args);

                Output.Flush();

                Environment.ExitCode = Program.ExitCode = result;

            }
            catch (System.FormatException e2)
            {
                FormatException(app, e2);
            }
           
            catch (CommandParsingException e)
            {

                Output.WriteLineError(e.Message);
                Output.WriteLineError(e.StackTrace);
                Output.Flush();

                if (e.HResult > 0)
                    Environment.ExitCode = Program.ExitCode = e.HResult;

                app.ShowHelp();

                Environment.ExitCode = Program.ExitCode = 1;

            }
            catch (Exception e)
            {

                Output.WriteLineError(e.Message);
                Output.WriteLineError(e.StackTrace);
                Output.Flush();

                if (e.HResult > 0)
                    Environment.ExitCode = Program.ExitCode = e.HResult;

                Environment.ExitCode = Program.ExitCode = 1;

            }

        }

        private static void FormatException(CommandLineApplication app, FormatException e2)
        {
            Output.WriteLineError(e2.Message);
            Output.Flush();
            app.ShowHelp();
            Environment.ExitCode = Program.ExitCode = 2;
        }

    }

}
