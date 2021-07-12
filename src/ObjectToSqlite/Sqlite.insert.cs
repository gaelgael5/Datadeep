using Bb.ToSqlite.CreateTables;
using Bb.ToSqlite.Inserts;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.ToSqlite
{

    public static partial class Sqlite
    {


        public static SqliteTableInsertBuilder InsertValueColumn(this SqliteTableInsertBuilder self, SqliteCreateTableBuilder table)
        {

            foreach (var column in table.Columns)
                self.Values(new SqlLiteParameterBuilder(column.Name) { Type = column.Type });

            return self;

        }

        public static SqliteTableInsertBuilder InsertColumn(this SqliteTableInsertBuilder self, SqliteCreateTableBuilder table)
        {

            foreach (var column in table.Columns)
                self.Insert(column.Name);

            return self;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="columnToUpdate">columns to set</param>
        /// <returns></returns>
        public static SqliteUpsertClauseDoBuilder UpdateSet(this SqliteUpsertClauseBuilder self, params (string[], string)[] columnToUpdate)
        {
            var r = new SqliteUpsertClauseDoBuilder();
            r.Columns.AddRange(columnToUpdate);
            self.Do = r;
            r.Parent = self;
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="indexedColumns">conflicted column managed</param>
        /// <returns></returns>
        public static SqliteUpsertClauseBuilder OnConflict(this SqliteTableInsertBuilder self, params string[] indexedColumns)
        {
            var r = new SqliteUpsertClauseBuilder();
            r.Columns.AddRange(indexedColumns.AsQuoted());
            self.UpsertClause = r;
            r.Parent = self;
            return r;
        }

        public static SqliteTableInsertBuilder Replace(string name, ConflictClauseEnum? policy = null)
        {
            return new SqliteTableInsertBuilder()
            {
                SchemaName = null,
                TableName = name,
                Mode = "REPLACE",
            };
        }

        public static SqliteTableInsertBuilder InsertInto(string name, ConflictClauseEnum? policy = null)
        {
            return new SqliteTableInsertBuilder()
            {
                SchemaName = null,
                TableName = name,
                Mode = "INSERT",
                Policy = policy
            };
        }

        public static SqliteTableInsertBuilder InsertInto(string schema, string name, ConflictClauseEnum? policy = null)
        {
            return new SqliteTableInsertBuilder()
            {
                SchemaName = schema,
                TableName = name,
                Mode = "INSERT",
                Policy = policy
            };

        }


    }


}
