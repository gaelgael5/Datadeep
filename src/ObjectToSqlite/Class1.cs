using Microsoft.Data.Sqlite;
using System;

namespace ObjectToSqlite
{
    public class Class1
    {

        public void Test()
        {

            var sql = Sqlite.CreateTable("mytable")
                
                .IfNotExist()

                .Column("Id", SqliteColumnType.INTEGER, 
                    c => c.AsPrimaryKey(ColumnSortWayEnum.ASC, ConflictClauseEnum.Abort, true),
                    c => c.NotNull(ConflictClauseEnum.Abort),
                    c => c.Unique(ConflictClauseEnum.Abort)
                    )

                ;


            var builder = Sqlite.ConnectionBuilder(@".\hello.db");

            using (var connection = builder.AsConnection())
            {
                var result = sql.Execute(connection);
            }


            //using (var connection = builder.AsConnection())
            //{
            //    connection.Open();
            //    using (var command = connection.CreateCommand())
            //    {
            //        command.CommandText = sql;
            //        // command.Parameters.AddWithValue("$id", id);
            //        using (var reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var name = reader.GetString(0);
            //                Console.WriteLine($"Hello, {name}!");
            //            }
            //        }
            //    }
            //}

        }


    }
}
