using Microsoft.Data.Sqlite;
using System;
using System.Text;

namespace ObjectToSqlite
{

    public static class Sqlite
    {


        public static SqliteConnection AsConnection(this SqliteConnectionStringBuilder self)
        {
            return new SqliteConnection(self.ConnectionString);
        }

        public static SqliteConnection  AsSqliteConnection(this string self)
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

        public static SqliteTableBuilder CreateTable(string name, string schema = null, bool isTemporary = false)
        {
            return new SqliteTableBuilder(schema, name, isTemporary);
        }

        public static SqliteIndexBuilder CreateIndex(string targetTableName, string name, string schema = null, bool isUnique = false)
        {
            return new SqliteIndexBuilder(schema, targetTableName, name, isUnique);
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

            if (_self == typeof(bool[]))
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

    }


}
