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

        public class Batch
        {
            private SqliteCommand cmd;

            public Batch(SqliteCommand cmd)
            {
                this.cmd = cmd;
            }

            public void Insert()
            {
                cmd.ExecuteNonQuery();
            }

        }

        public static void Execute(this SqliteTableInsertBuilder self, SqliteConnection cnx, Action<Batch> action)
        {

            switch (cnx.State)
            {

                case System.Data.ConnectionState.Broken:
                    cnx.Open();
                    break;

                case System.Data.ConnectionState.Closed:
                    cnx.Open();
                    break;

                case System.Data.ConnectionState.Connecting:
                    break;

                case System.Data.ConnectionState.Executing:
                    break;

                case System.Data.ConnectionState.Fetching:
                    break;


                case System.Data.ConnectionState.Open:
                default:
                    break;

            }

            using (var transaction = cnx.BeginTransaction())
            using (var cmd = self.GetCommand(cnx))
            {
                var btc = new Batch(cmd);
                action(btc);
                transaction.Commit();

            }

        }

        public static SqliteParameter ToParameter(this string self, SqliteType type, System.Data.ParameterDirection way = System.Data.ParameterDirection.Input)
        {

            string name = self;
            if (!name.StartsWith("$"))
                name = "$" + name;

            return new SqliteParameter()
            {
                ParameterName = name,
                SqliteType = type,
                Direction = way,
            };
        }


        public static SqliteConnection AsConnection(this SqliteConnectionStringBuilder self)
        {
            return new SqliteConnection(self.ConnectionString);
        }

        public static SqliteConnection AsSqliteConnection(this string self)
        {
            return new SqliteConnection(self);
        }

        public static SqliteConnectionStringBuilder ConnectionBuilder(string filename = null)
        {

            var result = new SqliteConnectionStringBuilder();

            if (!string.IsNullOrEmpty(filename))
                result.DataSource = filename;

            return result;

        }

        public static int Execute(this Builder self, SqliteConnection cnx)
        {

            switch (cnx.State)
            {

                case System.Data.ConnectionState.Broken:
                    cnx.Open();
                    break;

                case System.Data.ConnectionState.Closed:
                    cnx.Open();
                    break;


                case System.Data.ConnectionState.Connecting:
                    break;

                case System.Data.ConnectionState.Executing:
                    break;

                case System.Data.ConnectionState.Fetching:
                    break;


                case System.Data.ConnectionState.Open:
                default:
                    break;

            }

            using (var command = cnx.CreateCommand())
            {
                command.CommandText = self.Generate();
                return command.ExecuteNonQuery();
            }

        }

        public static string Generate(this Builder self)
        {
            var sb = new StringBuilder();
            self.Generate(sb);
            return sb.ToString();
        }

        public static SqliteColumnType ToSqliteType(this Type _self)
        {

            if (_self == typeof(bool))
                return SqliteColumnType.INTEGER;

            if (_self == typeof(byte))
                return SqliteColumnType.INTEGER;

            if (_self == typeof(byte[]))
                return SqliteColumnType.BLOB;

            if (_self == typeof(char))
                return SqliteColumnType.TEXT;

            if (_self == typeof(DateTime))
                return SqliteColumnType.TEXT;

            if (_self == typeof(DateTimeOffset))
                return SqliteColumnType.TEXT;

            if (_self == typeof(decimal))
                return SqliteColumnType.TEXT;

            if (_self == typeof(double))
                return SqliteColumnType.real;

            if (_self == typeof(Guid))
                return SqliteColumnType.TEXT;

            if (_self == typeof(Int16))
                return SqliteColumnType.INTEGER;

            if (_self == typeof(Int32))
                return SqliteColumnType.INTEGER;

            if (_self == typeof(Int64))
                return SqliteColumnType.INTEGER;

            if (_self == typeof(UInt16))
                return SqliteColumnType.INTEGER;

            if (_self == typeof(UInt32))
                return SqliteColumnType.INTEGER;

            if (_self == typeof(UInt32))
                return SqliteColumnType.INTEGER;

            if (_self == typeof(TimeSpan))
                return SqliteColumnType.TEXT;

            if (_self == typeof(string))
                return SqliteColumnType.TEXT;

            return SqliteColumnType.Undefined;

        }

        public static void AppendList<T>(this IEnumerable<T> self, string separator, StringBuilder sb, Action<T, StringBuilder> action)
        {

            string s = string.Empty;

            foreach (var item in self)
            {
                sb.Append(s);
                action(item, sb);
                s = separator;
            }

        }

        public static List<string> AsQuoted(this IEnumerable<string> self)
        {
            List<string> result = new List<string>();

            foreach (var name in self)
            {

                if (name.StartsWith("`") && name.EndsWith("`"))
                    result.Add(name);
                else
                    result.Add($"`{name}`");

            }

            return result;
        }

        public static string AsQuoted(this string self)
        {

            if (self.StartsWith("`") && self.EndsWith("`"))
                return self;

            return $"`{self}`";

        }


    }


}
