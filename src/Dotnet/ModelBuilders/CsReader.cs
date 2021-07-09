using Bb;
using Bb.DataDeep.Models;
using Bb.DataDeep.Models.Mpd;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DotnetParser
{


    internal class CsReader : Reader<SyntaxTree>
    {


        public CsReader(DotnetMpdBuilder parentReader)
        {
            this._parentReader = parentReader;
            this._attributeMapper = new AttributeMapper();
        }

        public override StructureMpdBase Resolve(SyntaxTree element, StructureMpdBase parent)
        {
            CSharpVisitor visitor = new CSharpVisitor(parent, this._attributeMapper);
            visitor.Visit(element.GetRoot());
            return null;
        }

        internal class CSharpVisitor : CSharpSyntaxWalker
        {

            public CSharpVisitor(StructureMpdBase parent, AttributeMapper mapper)
            {
                this._structure = new Stack<StructureMpdBase>();
                this._structure.Push(parent);
                this._buffer = new Queue<List<string>>();
                this._attributeMapper = mapper;
            }


            public override void VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
            {

                base.VisitDocumentationCommentTrivia(node);

            }


            public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
            {



                if (node.Name is IdentifierNameSyntax i)
                    this._currentFamilyname = i.Identifier.ValueText?.Trim() ?? string.Empty;

                else if (node.Name is QualifiedNameSyntax q)
                    this._currentFamilyname = q.ToFullString()?.Trim() ?? string.Empty;

                else
                {
                    LocalDebug.Stop();
                }

                var trivia = GetDocumentation(node.GetLeadingTrivia());
                if (trivia != null)
                {

                }

                base.VisitNamespaceDeclaration(node);

            }



            public override void VisitAttribute(AttributeSyntax node)
            {

                var parent = (Structure)this._structure.Peek();

                var name = node.Name.ToFullString();
                var count = this._buffer.Count;

                base.VisitAttribute(node);

                var arguments = GetItemsFromBuffer(count);

                _attributeMapper.Map(name, arguments, parent);

            }

            public override void VisitAttributeArgument(AttributeArgumentSyntax node)
            {

                string name = string.Empty;
                List<string> _args = new List<string>(2);

                if (node.NameEquals != null)
                    _args.Add(node.NameEquals.Name.Identifier.ValueText);

                if (node.Expression is LiteralExpressionSyntax l)
                {
                    var txt = l.Token.ValueText;
                    _args.Add(txt);
                }
                else if (node.Expression is TypeOfExpressionSyntax t)
                {
                    
                    if (t.Type is GenericNameSyntax g)
                        _args.Add(g.ToFullString());

                    else if (t.Type is IdentifierNameSyntax i)
                        _args.Add(i.ToFullString());

                    else
                    {
                        LocalDebug.Stop();
                    }
                }
                else if (node.Expression is MemberAccessExpressionSyntax m)
                    _args.Add(m.ToFullString());

                else if (node.Expression is IdentifierNameSyntax i)
                    _args.Add(i.ToFullString());

                else if (node.Expression is BinaryExpressionSyntax b)
                    _args.Add(b.ToFullString());

                else
                {
                    LocalDebug.Stop();
                }

                if (_args.Count > 0)
                    this._buffer.Enqueue(_args);

                base.VisitAttributeArgument(node);

            }



            public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
            {
            
                var e = new Entity()
                {
                    FamilyName = this._currentFamilyname,
                    Name = node.Identifier.ValueText,
                    Kind = EntityKindEnum.Enumeration,
                    Label = node.Identifier.ValueText,

                };

                var trivia = GetDocumentation(node.GetLeadingTrivia());
                if (trivia != null)
                    e.Description = trivia.Summary;

                foreach (var item in node.Modifiers)
                    GetMetadatas(item, e);

                var current = this._structure.Peek();
                if (current is Library lib)
                    lib.AddEntity(e);

                else if (current is Entity e2)
                {
                    e.FamilyName = e.FamilyName + "+" + e2.Name;
                    (this._structure.LastOrDefault() as Library).AddEntity(e);
                }
                else
                {
                    LocalDebug.Stop();
                }

                if (node.HasLeadingTrivia)
                {

                }

                this._structure.Push(e);
                base.VisitEnumDeclaration(node);
                this._structure.Pop();

            }

            public override void VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                              
                var e = new Entity()
                {
                    FamilyName = this._currentFamilyname,
                    Name = node.Identifier.ValueText,
                    Kind = EntityKindEnum.Object,
                    Label = node.Identifier.ValueText,

                };

                var trivia = GetDocumentation(node.GetLeadingTrivia());
                if (trivia != null)
                    e.Description = trivia.Summary;

                foreach (var item in node.Modifiers)
                    GetMetadatas(item, e);

                var current = this._structure.Peek();
                if (current is Library lib)
                    lib.AddEntity(e);

                else if (current is Entity e2)
                {
                    e.FamilyName = e.FamilyName + "+" + e2.Name;
                    (this._structure.LastOrDefault() as Library).AddEntity(e);
                }
                else
                {
                    LocalDebug.Stop();
                }

                this._structure.Push(e);
                base.VisitClassDeclaration(node);
                this._structure.Pop();

            }

            public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
            {

                var e = new Entity()
                {
                    FamilyName = this._currentFamilyname,
                    Name = node.Identifier.ValueText,
                    Kind = EntityKindEnum.Contract,
                    Label = node.Identifier.ValueText,

                };

                var trivia = GetDocumentation(node.GetLeadingTrivia());
                if (trivia != null)
                {
                    e.Description = trivia.Summary;
                }

                foreach (var item in node.Modifiers)
                    GetMetadatas(item, e);

                var current = this._structure.Peek();
                if (current is Library lib)
                    lib.AddEntity(e);

                else if (current is Entity e2)
                {
                    e.FamilyName = e.FamilyName + "+" + e2.Name;
                    (this._structure.LastOrDefault() as Library).AddEntity(e);
                }
                else
                {
                    LocalDebug.Stop();
                }

                if (node.HasLeadingTrivia)
                {

                }

                this._structure.Push(e);
                base.VisitInterfaceDeclaration(node);
                this._structure.Pop();

            }

            public override void VisitStructDeclaration(StructDeclarationSyntax node)
            {
                             
                var e = new Entity()
                {
                    FamilyName = this._currentFamilyname,
                    Name = node.Identifier.ValueText,
                    Kind = EntityKindEnum.Object,
                    Label = node.Identifier.ValueText,

                };

                var trivia = GetDocumentation(node.GetLeadingTrivia());
                if (trivia != null)
                {
                    e.Description = trivia.Summary;
                }

                foreach (var item in node.Modifiers)
                    GetMetadatas(item, e);

                var current = this._structure.Peek();
                if (current is Library lib)
                    lib.AddEntity(e);

                else if (current is Entity e2)
                {
                    e.FamilyName = e.FamilyName + "+" + e2.Name;
                    (this._structure.LastOrDefault() as Library).AddEntity(e);
                }
                else
                {
                    LocalDebug.Stop();
                }

                if (node.HasLeadingTrivia)
                {

                }

                this._structure.Push(e);
                base.VisitStructDeclaration(node);
                this._structure.Pop();

            }



            public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
            {

                var name = node.Identifier.ValueText;

                var a = new AttributeField()
                {
                    Name = name,
                    Label = name,
                };

                var trivia = GetDocumentation(node.GetLeadingTrivia());
                if (trivia != null)
                    a.Description = trivia.Summary;

                if (node.EqualsValue != null)
                {

                    if (node.EqualsValue.Value is LiteralExpressionSyntax l)
                        a.AddMetadata(DataDeepConstants.ComponentModel, DataDeepConstants.DefaultValue, l.Token.ValueText);

                    else
                    {
                        LocalDebug.Stop();
                    }

                }
               
                var current = this._structure.Peek();
                if (current is Entity entity)
                    entity.Attributes.Add(a);

                else
                {

                }

                this._structure.Push(a);
                base.VisitEnumMemberDeclaration(node);
                this._structure.Pop();

            }

            public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
            {

                var _ref = node.Type.GetReference();

                var a = new AttributeField()
                {
                    Name = node.Identifier.ValueText,
                    Label = node.Identifier.ValueText,
                };

                var trivia = GetDocumentation(node.GetLeadingTrivia());
                if (trivia != null)
                    a.Description = trivia.Summary;

                foreach (var item in node.Modifiers)
                    GetMetadatas(item, a);

                a.Type = ResolveType(node.Type);


                var current = this._structure.Peek();
                if (current is Entity entity)
                    entity.Attributes.Add(a);

                else
                {
                    LocalDebug.Stop();
                }

                this._structure.Push(a);
                base.VisitPropertyDeclaration(node);
                this._structure.Pop();

            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="triviaList"></param>
            /// <exception cref="Exception" />
            /// <include file='' path='[@name=""]'/>
            /// <permission cref="a" />
            /// <remarks />
            /// <example></example>
            private Documentation GetDocumentation(SyntaxTriviaList triviaList)
            {

                Documentation result = null;

                foreach (var trivia in triviaList)
                {
                    var xml = trivia.GetStructure();
                    if (xml != null)
                    {
                        var datas = xml.ToFullString().Replace("///", "").Trim();
                        if (datas.Contains("<") && datas.Contains(">"))
                        {
                            var parser = new ParsingDocumentation();
                            result = parser.Parse(datas);
                            if (result != null)
                                return result;
                        }
                    }
                }

                return result;

            }
            
            private void GetMetadatas(SyntaxToken item, Structure e)
            {
                switch (item.ValueText)
                {

                    case "public":
                        e.AddMetadata(DataDeepConstants.ComponentModel, DataDeepConstantModifiers.Modifier, DataDeepConstantModifiers.Public);
                        break;

                    case "private":
                        e.AddMetadata(DataDeepConstants.ComponentModel, DataDeepConstantModifiers.Modifier, DataDeepConstantModifiers.Private);
                        break;

                    case "internal":
                        e.AddMetadata(DataDeepConstants.ComponentModel, DataDeepConstantModifiers.Modifier, DataDeepConstantModifiers.Internal);
                        break;

                    case "protected":
                        e.AddMetadata(DataDeepConstants.ComponentModel, DataDeepConstantModifiers.Modifier, DataDeepConstantModifiers.Protected);
                        break;

                    case "static":
                        e.AddMetadata(DataDeepConstants.ComponentModel, DataDeepConstantModifiers.Modifier, DataDeepConstantModifiers.IsStatic);
                        break;

                    case "sealed":
                        e.AddMetadata(DataDeepConstants.ComponentModel, DataDeepConstantModifiers.Modifier, DataDeepConstantModifiers.IsSealed);
                        break;

                    case "abstract":
                        e.AddMetadata(DataDeepConstants.ComponentModel, DataDeepConstantModifiers.Modifier, DataDeepConstantModifiers.IsAbstract);
                        break;

                    case "partial":
                    case "virtual":
                    case "override":
                        break;

                    default:
                        LocalDebug.Stop();
                        break;
                }
            }

            private List<List<string>> GetItemsFromBuffer(int count)
            {
                List<List<string>> _items = new List<List<string>>(this._buffer.Count - count);
                while (this._buffer.Count > count)
                    _items.Add(this._buffer.Dequeue());
                return _items;
            }

            private static TypeReference ResolveType(TypeSyntax node)
            {

                var result = new TypeReference();

                var type = node.ToFullString().Trim();

                if (node.Kind() == SyntaxKind.PredefinedType)
                {
                    if (node is PredefinedTypeSyntax p)
                    {

                        switch (p.Keyword.ValueText)
                        {

                            case "decimal":
                            case "char":
                            case "float":
                            case "byte":
                            case "sbyte":
                            case "object":
                            case "int":
                            case "uint":
                            case "short":
                            case "ushort":
                            case "double":
                            case "long":
                            case "ulong":
                            case "bool":
                            case "string":
                                result.Name = p.Keyword.ValueText;
                                break;

                            default:
                                LocalDebug.Stop();
                                break;
                        }
                    }

                }
                else
                {

                    switch (type)
                    {

                        case "decimal?":
                        case "float?":
                        case "char?":
                        case "byte?":
                        case "sbyte?":
                        case "int?":
                        case "uint?":
                        case "short?":
                        case "ushort?":
                        case "long?":
                        case "ulong?":
                        case "bool?":
                        case "double?":
                        case "string?":
                            result.Name = type;
                            //a.Metadatas.Add(new Bb.DataDeep.Models.Metadata() { Category = dotnetConstants.Contraint, Name = dotnetConstants.Time });
                            break;

                        case "String":
                        case "DateTime":
                            result.Name = type;
                            break;

                        default:
                            //LocalDebug.Stop();
                            result.Name = type;
                            break;
                    }


                }

                if (node.Kind() == SyntaxKind.ArrayType)
                    result.IsList = true;

                return result;

            }
            private Queue<List<string>> _buffer;
            private readonly AttributeMapper _attributeMapper;
            private Stack<StructureMpdBase> _structure;
            private string _currentFamilyname;
        
        }

        private DotnetMpdBuilder _parentReader;
        private readonly AttributeMapper _attributeMapper;
    
    }

}
