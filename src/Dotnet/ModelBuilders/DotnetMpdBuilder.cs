using Bb.DataDeep.Models.Mpd;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DotnetParser
{

    public class DotnetMpdBuilder : DirectoryMpdBuilder<SyntaxTree>
    {

        public DotnetMpdBuilder()
        {
            Add("CsReader", new CsReader(this));
        }

        internal IEnumerable<Library> Parse(string path, string pattern)
        {

            var dir = new DirectoryInfo(path);
            if (dir.Exists)
            {

                HashSet<string> _h = new HashSet<string>();

                var items = ParseAndSearchProjects(dir, pattern).ToList();
                foreach (var item in items)
                    yield return LoadProject(item, pattern);

            }
        }


        private IEnumerable<_lib> ParseAndSearchProjects(DirectoryInfo fieldDirectory, string pattern)
        {

            foreach (var item in fieldDirectory.GetFiles(pattern + "proj", SearchOption.AllDirectories))
            {

                Debug.WriteLine($"current file : {item.FullName}");
                var doc = XDocument.Load(item.FullName);

                string framework = "Unkown";
                string assemblyName = Path.GetFileNameWithoutExtension(item.Name);
                //string rootNamespace = Path.GetFileNameWithoutExtension(item.Name);

                var prop = doc.Descendants("PropertyGroup").FirstOrDefault();
                if (prop != null)
                {
                    framework = prop.Element("TargetFramework")?.Value ?? "Unkown";
                    assemblyName = prop.Element("AssemblyName")?.Value ?? Path.GetFileNameWithoutExtension(item.Name);
                    //rootNamespace = prop.Element("RootNamespace")?.Value ?? Path.GetFileNameWithoutExtension(item.Name);
                }

                var _lib = new _lib()
                {
                    File = item,
                    Location = item.Directory,
                    Name = assemblyName,
                    Label = assemblyName,
                    References = new List<FileInfo>(),
                    Packages = new List<FileInfo>(),
                };

                #region Dependencies

                List<FileInfo> _packages = new List<FileInfo>(20);
                var props = doc.Descendants("ItemGroup");
                foreach (var prop2 in props)
                {
                    var prop3 = prop2.Descendants("PackageReference");
                    if (prop3 != null)
                    {
                        foreach (var prop5 in prop3)
                        {
                            var refProject = prop5.Attribute("Include")?.Value;
                            if (refProject != null)
                            {
                                var path = new FileInfo(Path.Combine(item.Directory.FullName, refProject));
                                _lib.Packages.Add(path);
                            }
                        }
                    }

                    var prop4 = prop2.Descendants("ProjectReference");
                    if (prop4 != null)
                    {
                        foreach (var prop5 in prop4)
                        {
                            var refProject = prop5.Attribute("Include")?.Value;
                            if (refProject != null)
                            {
                                var path = new FileInfo(Path.Combine(item.Directory.FullName, refProject));
                                _lib.References.Add(path);
                            }
                        }
                    }
                }

                #endregion Dependencies

                yield return _lib;

            }

        }


        private Library LoadProject(_lib library, string pattern)
        {

            Debug.WriteLine($"current file : {library.File}");

            var lib = new Library()
            {
                Name = library.Name,
                Label = library.Label,
            };

            var item = library.File;

            foreach (var codeFile in item.Directory.GetFiles(pattern, SearchOption.AllDirectories))
            {
                Debug.WriteLine($"parsing file : {codeFile.FullName}");
                SyntaxTree tree = codeFile.ParseCode();
                this.Resolve("CsReader", tree, lib);
            }

            return lib;



        }

        private class _lib
        {
            public string Name { get; internal set; }
            public string Label { get; internal set; }
            public DirectoryInfo Location { get; internal set; }
            public List<FileInfo> References { get; internal set; }
            public List<FileInfo> Packages { get; internal set; }
            public FileInfo File { get; internal set; }
        }

    }


}
