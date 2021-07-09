using Bb;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotnetParser
{

    public class ParsingDocumentation
    {


        public Documentation Parse(string payload)
        {

            Documentation result = null;

            HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(payload);

            // ParseErrors is an ArrayList containing any errors from the Load statement
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required
            }
            else
            {

                if (htmlDoc.DocumentNode != null)
                {

                    result = new Documentation()
                    {
                        Summary = GetSummary(htmlDoc),
                        Params = GetParams(htmlDoc),
                        Returns = GetReturns(htmlDoc),
                        SeeAlsos = GetSeeAlso(htmlDoc),
                        Remarks = GetRemarks(htmlDoc),
                        Permissions = Getpermissions(htmlDoc),
                        Exceptions = Getexceptions(htmlDoc),
                    };

                }
            }

            return result;

        }

        private static string GetSummary(HtmlDocument htmlDoc)
        {

            HtmlAgilityPack.HtmlNode summaryNode = htmlDoc.DocumentNode.SelectSingleNode("//summary");
            if (summaryNode != null)
                return summaryNode.InnerHtml?.Trim();

            return string.Empty;

        }

        private static string GetRemarks(HtmlDocument htmlDoc)
        {

            List<DocumentationItem> result = new List<DocumentationItem>();

            var nodes = htmlDoc.DocumentNode.SelectSingleNode("//remarks");
            if (nodes != null)
                return nodes.InnerHtml;

            return null;

        }

        private static List<DocumentationItem> GetSeeAlso(HtmlDocument htmlDoc)
        {

            List<DocumentationItem> result = new List<DocumentationItem>();

            var nodes = htmlDoc.DocumentNode.SelectNodes("//seealso");
            if (nodes != null)
                foreach (var item in nodes)
                    result.Add(GetDocumentationItem(item, "cref"));

            return result;

        }

        private static string GetReturns(HtmlDocument htmlDoc)
        {

            HtmlAgilityPack.HtmlNode returnsNode = htmlDoc.DocumentNode.SelectSingleNode("//returns");
            if (returnsNode != null)
                return returnsNode.InnerText?.Trim();

            return string.Empty;

        }

        private static List<DocumentationItem> GetParams(HtmlDocument htmlDoc)
        {
            List<DocumentationItem> result = new List<DocumentationItem>();

            var nodes = htmlDoc.DocumentNode.SelectNodes("//param");
            if (nodes != null)
                foreach (var item in nodes)
                    result.Add(GetDocumentationItem(item, "name"));

            return result;

        }

        private static List<DocumentationItem> Getpermissions(HtmlDocument htmlDoc)
        {
            List<DocumentationItem> result = new List<DocumentationItem>();

            var nodes = htmlDoc.DocumentNode.SelectNodes("//permissions");
            if (nodes != null)
                foreach (var item in nodes)
                    result.Add(GetDocumentationItem(item, "cref"));

            return result;

        }

        private static List<DocumentationItem> Getexceptions(HtmlDocument htmlDoc)
        {
            List<DocumentationItem> result = new List<DocumentationItem>();

            var nodes = htmlDoc.DocumentNode.SelectNodes("//exceptions");
            if (nodes != null)
                foreach (var item in nodes)
                    result.Add(GetDocumentationItem(item, "cref"));

            return result;

        }

        private static DocumentationItem GetDocumentationItem(HtmlNode node, string key)
        {
            var i = new DocumentationItem()
            {
                KeyName = key,
                KeyValue = node.Attributes.Contains(key)
                    ? node.Attributes[key].Value
                    : string.Empty,
                content = node.InnerHtml,
            };

            return i;
        }

    }

   
}
