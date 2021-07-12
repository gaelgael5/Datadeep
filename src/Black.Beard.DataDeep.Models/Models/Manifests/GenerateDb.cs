using Bb.DataDeep.Models.Mpd;
using Bb.ToSqlite;
using Bb.ToSqlite.CreateTables;
using Bb.ToSqlite.Inserts;
using System.Diagnostics;
using System.IO;

namespace Bb.DataDeep.Models.Manifests
{
    internal class GenerateDb
    {


        public GenerateDb(FileInfo file)
        {
            this._file = file;
            this._builder = Sqlite.ConnectionBuilder(_file.FullName);
        }

        public void Generate(ManifestModel manifestList)
        {

            if (_file.Exists)
                _file.Delete();

            var sqlMpdTable = GenerateMpdTable();


            SqliteTableInsertBuilder i = Sqlite.InsertInto(sqlMpdTable.Schema, sqlMpdTable.Name)
                .InsertColumn(sqlMpdTable)
                .InsertValueColumn(sqlMpdTable)
            //.OnConflict("")
            //    .Where("")
            //        .UpdateSet((new string[] { "col1" }, ""))
            //        .Where("")
            ;

            var dic = i.GetDictionaryParameters();
            var packageId = dic["$packageId"];
            var application = dic["$application"];
            var packageLastUpdateDate = dic["$packageLastUpdateDate"];
            var libraryName = dic["$libraryName"];
            var libraryDescription = dic["$libraryDescription"];
            var libraryLabel = dic["$libraryLabel"];
            var entityName = dic["$entityName"];
            var entityDescription = dic["$entityDescription"];
            var entityLabel = dic["$entityLabel"];
            var entityKind = dic["$entityKind"];
            var entityFamilyName = dic["$entityFamilyName"];
            var fieldName = dic["$fieldName"];
            var fieldDescription = dic["$fieldDescription"];
            var fieldLabel = dic["$fieldLabel"];
            var type = dic["$type"];
            var typeIsArray = dic["$typeIsArray"];

            using (var connection = _builder.AsConnection())
            {
                var result = sqlMpdTable.Execute(connection);
            }


            using (var connection = _builder.AsConnection())
            {
                i.Execute(connection, (batch) =>
                {

                    foreach (var manifest in manifestList.Items)
                        if (manifest.Kind == DocumentKindEnum.Mpd)
                        {

                            var package = Package.Load(manifest, manifestList);

                            if (package != null)
                            {

                                packageId.Value = package.Id ?? string.Empty;
                                application.Value = package.Application ?? string.Empty;
                                packageLastUpdateDate.Value = package.LastUpdateDate;

                                foreach (var library in package.Libraries)
                                {

                                    libraryName.Value = library.Name ?? string.Empty;
                                    libraryDescription.Value = library.Description ?? string.Empty;
                                    libraryLabel.Value = library.Label ?? string.Empty;

                                    foreach (var entity in library.Entities)
                                    {

                                        entityName.Value = entity.Name ?? string.Empty;
                                        entityDescription.Value = entity.Description ?? string.Empty;
                                        entityLabel.Value = entity.Label ?? string.Empty;
                                        entityKind.Value = entity.Kind.ToString() ?? string.Empty;
                                        entityFamilyName.Value = entity.FamilyName ?? string.Empty;

                                        foreach (var field in entity.Attributes)
                                        {
                                            fieldName.Value = field.Name ?? string.Empty;
                                            fieldDescription.Value = entity.Description ?? string.Empty;
                                            fieldLabel.Value = field.Label ?? string.Empty;
                                            type.Value = field.Type.Name ?? string.Empty;
                                            typeIsArray.Value = field.Type.IsList;

                                            batch.Insert();

                                        }
                                    }

                                }
                            }

                        }

                });

            }


        }


        private SqliteCreateTableBuilder GenerateMpdTable()
        {
            var sqlMpdTable = Sqlite.CreateTable("Mpd")
             .IfNotExist()

             .Column("PackageId", SqliteColumnType.TEXT)
             .Column("Application", SqliteColumnType.TEXT)
             .Column("PackageLastUpdateDate", SqliteColumnType.TEXT)

             .Column("LibraryName", SqliteColumnType.TEXT)
             .Column("LibraryDescription", SqliteColumnType.TEXT)
             .Column("LibraryLabel", SqliteColumnType.TEXT)

             .Column("EntityName", SqliteColumnType.TEXT)
             .Column("EntityDescription", SqliteColumnType.TEXT)
             .Column("EntityLabel", SqliteColumnType.TEXT)
             .Column("EntityKind", SqliteColumnType.TEXT)
             .Column("EntityFamilyName", SqliteColumnType.TEXT)

             .Column("FieldName", SqliteColumnType.TEXT)
             .Column("FieldDescription", SqliteColumnType.TEXT)
             .Column("FieldLabel", SqliteColumnType.TEXT)
             .Column("Type", SqliteColumnType.TEXT)
             .Column("TypeIsArray", SqliteColumnType.INTEGER)
           ;

            return sqlMpdTable;

        }



        private readonly FileInfo _file;

        public Microsoft.Data.Sqlite.SqliteConnectionStringBuilder _builder { get; }
    }

}


