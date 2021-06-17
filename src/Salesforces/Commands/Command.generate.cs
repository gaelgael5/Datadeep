using Bb;
using Bb.CommandLines;
using Bb.CommandLines.Validators;
using Bb.DataDeep.Models.Mpd;
using Microsoft.Extensions.CommandLineUtils;
using Salesforces;
using System;
using System.IO;
using System.Linq;

namespace Salesforce.Commands
{

    /*
     
    // generate 
    */

    /// <summary>
    /// 
    /// </summary>
    public static partial class Command
    {

        public static CommandLineApplication CommandExport(this CommandLineApplication app)
        {

            // json template 

            var cmd = app.Command("generate", config =>
            {

                config.Description = "generate a model from salesforce description folder";
                config.HelpOption(HelpFlag);

                var validator = new GroupArgument(config);

                var pathSource = validator.Argument("<source folder path>", "source folder path where the salesforce structure is stored"
                    , ValidatorExtension.EvaluateRequired
                    , ValidatorExtension.EvaluateDirectoryPathIsValid
                    );

                var argApplicationName = validator.Argument("<applicationName>", "application name"
                    , ValidatorExtension.EvaluateDirectoryPathIsValid
                    , ValidatorExtension.EvaluateRequired
                    );

                var pathTarget = validator.Argument("<target folder path>", "target folder path"
                    , ValidatorExtension.EvaluateRequired
                    , ValidatorExtension.EvaluateDirectoryPathIsValid
                    );


                var argName = validator.Option("--name", "Name of the package");
                var argLabel = validator.Option("--label", "label of the package");
                var argDescription = validator.Option("--description", "description of the package");
                var argVersion = validator.Option("--version", "Version of the package");
                var argSummary = validator.OptionNoValue("--summary", "generate or regenerate summary");


                config.OnExecute(() =>
                {

                    if (!validator.Evaluate(out int errorNum))
                        return errorNum;

                    // load config
                    var sourceDir = new DirectoryInfo(pathSource.Value.TrimPath());
                    var targetDir = new DirectoryInfo(pathTarget.Value.TrimPath());

                    Package package = new Package()
                    {
                        LastUpdateDate = DateTime.Now,
                        Application = argApplicationName.Value,
                    };

                    if (argName.HasValue())
                        package.Name = argName.Value();

                    if (argLabel.HasValue())
                        package.Label = argLabel.Value();

                    if (argDescription.HasValue())
                        package.Description = argDescription.Value();

                    if (argVersion.HasValue())
                        package.Version = new Version(argVersion.Value());

                    package.Id = Crc32.Calculate(package.Name + package.Version).ToString();


                    SalesforceMpdBuilder builder = new SalesforceMpdBuilder();
                    var libs = builder.Parse(sourceDir.FullName).ToArray();
                    foreach (var lib in libs)
                        package.AddLib(lib);

                    package.Save(targetDir.FullName);

                    if (argVersion.HasValue())
                    {
                        var i = Bb.DataDeep.Models.Manifests.ManifestModel.Create(targetDir.FullName);
                        i.Save(targetDir.FullName);
                    }

                    return 0;

                });

            });

            return app;

        }

    }

}
