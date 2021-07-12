using Bb.ToSqlite.CreateTables;
using Bb.ToSqlite.Inserts;
using Microsoft.Data.Sqlite;
using System;
using System.Text;

namespace Bb.ToSqlite
{

    public static partial class Sqlite
    {


        public static SqliteCreateTableBuilder CreateTable(string name, string schema = null, bool isTemporary = false)
        {
            return new SqliteCreateTableBuilder(schema, name, isTemporary);
        }

        public static SqliteCreateIndexBuilder CreateIndex(string targetTableName, string name, string schema = null, bool isUnique = false)
        {
            return new SqliteCreateIndexBuilder(schema, targetTableName, name, isUnique);
        }

 
    }


}
