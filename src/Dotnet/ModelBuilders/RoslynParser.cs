using Bb;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using System.Text;

namespace DotnetParser
{
    internal static class RoslynParser
    {

        public static Microsoft.CodeAnalysis.SyntaxTree ParseCode(this FileInfo file)
        {
            var content = file.FullName.LoadContentFromFile();
            CSharpParseOptions options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp6);
            var stringText = Microsoft.CodeAnalysis.Text.SourceText.From(content, Encoding.UTF8);
            var tree = SyntaxFactory.ParseSyntaxTree(stringText, options, file.FullName);
            return tree;
        }


    }


}
