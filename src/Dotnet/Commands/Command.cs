
using Microsoft.Extensions.CommandLineUtils;
using System;


namespace DotnetParser.Commands
{

    public static partial class Command
    {

        static Command()
        {

            /// Command._access = "('" + string.Join("','", Enum.GetNames(typeof(AccessModuleEnum))) + "')";

        }


        /// <summary>
        /// Initializes the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns></returns>
        public static CommandLineApplication Initialize(this CommandLineApplication app)
        {

            //Helper.Load();

            AnsiConsole.GetError(true);

            app.HelpOption(HelpFlag);
            app.VersionOption(VersionFlag, Bb.Constants.ShortVersion, Bb.Constants.LongVersion);

            app.Name = Bb.Constants.Name;
            app.Description = Bb.Constants.ProgramHelpDescription;
            app.ExtendedHelpText = Bb.Constants.ExtendedHelpText;

            return app;

        }

       

        // public static BbClientHttp Client => new BbClientHttp(new Uri(Helper.Parameters.ServerUrl));

        public static object Result { get; internal set; }

        public const string HelpFlag = "-? |-h |--help";
        public const string VersionFlag = "-v |--version";
        public static string _access;


    }


}
