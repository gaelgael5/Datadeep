using Bb.DataDeep.Models.Manifests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataDeepUnitTests
{

    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {


            var summary = ManifestModel.Load(@"D:\Tests_datadeep\datadeep");

            var file = summary.ToSqlite();


        }

        //[TestMethod]
        //public void TestMethod1()
        //{

        //    var sqlTable = Sqlite.CreateTable("mytable")
        //       .IfNotExist()
        //       .Column("Id", SqliteColumnType.INTEGER, c => c.AsPrimaryKey(ColumnSortWayEnum.ASC, ConflictClauseEnum.Abort, true))
        //       .Column("Name", SqliteColumnType.TEXT, c => c.Unique(ConflictClauseEnum.Abort))
        //       ;

        //    var sqlIndex = Sqlite.CreateIndex("mytable", "indexName", null, true)
        //        .Column("Name", c => c.Collate(CollationEnum.RTRIM).Order(ColumnSortWayEnum.ASC))
        //        ;

        //    var file = new FileInfo(@".\hello.db");
        //    if (file.Exists)
        //        file.Delete();

        //    var builder = Sqlite.ConnectionBuilder(file.FullName);

        //    using (var connection = builder.AsConnection())
        //    {
        //        var result = sqlTable.Execute(connection);
        //        result = sqlIndex.Execute(connection);
        //    }

        //}


    }

}
