using Bb.ToSqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ObjectToSqliteUnitTest
{

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var sqlTable = Sqlite.CreateTable("mytable")
               .IfNotExist()
               .Column("Id", SqliteColumnType.INTEGER, c => c.AsPrimaryKey(ColumnSortWayEnum.ASC, ConflictClauseEnum.Abort, true))
               .Column("Name", SqliteColumnType.TEXT, c => c.Unique(ConflictClauseEnum.Abort))
               ;

            var sqlIndex = Sqlite.CreateIndex("mytable", "indexName", null, true)
                .Column("Name", c => c.Collate(CollationEnum.RTRIM).Order(ColumnSortWayEnum.ASC))
                ;

            var file = new FileInfo(@".\hello.db");
            if (file.Exists)
                file.Delete();

            var builder = Sqlite.ConnectionBuilder(file.FullName);

            using (var connection = builder.AsConnection())
            {
                var result = sqlTable.Execute(connection);
                result = sqlIndex.Execute(connection);
            }

        }


        [TestMethod]
        public void TestMethod2()
        {

            var sqlTable = Sqlite.CreateTable("mytable")
               .IfNotExist()
               .Column<Model1>(
                    c => c.Column(d => d.Id, d => d.AsPrimaryKey()),
                    c => c.Column(d => d.Name, d => d.NotNull(ConflictClauseEnum.Abort))
                )
               ;

            var sqlIndex = Sqlite.CreateIndex("mytable", "indexName", null, true)
                .Column<Model1>(
                    c => c.Column(d => d.Name, d => d.Collate(CollationEnum.RTRIM).Order(ColumnSortWayEnum.ASC))
                )
                ;

            var file = new FileInfo(@".\hello.db");
            if (file.Exists)
                file.Delete();

            var builder = Sqlite.ConnectionBuilder(file.FullName);

            using (var connection = builder.AsConnection())
            {
                var result = sqlTable.Execute(connection);
                result = sqlIndex.Execute(connection);
            }

        }

    }

    public class Model1
    {

        public int Id { get; set; }
        public string Name { get; set; }

    }

}
