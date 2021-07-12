using Microsoft.Data.Sqlite;
using System.Text;

namespace Bb.ToSqlite.Inserts
{

    public class SqlLiteLiteralBuilder : SqlLiteExpressionBuilder
    {

        public SqlLiteLiteralBuilder(object value)
            : base()
        {
            this.Value = value;
        }

        public object Value { get; internal set; }

        public SqliteColumnType Type { get; internal set; }

        internal override void Generate(StringBuilder sb)
        {
            if (Value is string s)
                sb.Append($"'{s}'");
            else
                sb.Append(Value);
        }

    }


    //public SqliteTableInsertBuilder()
    //{
    //    using (var con = new SqliteConnection("Data Source=MyDatabase.sqlite;Version=3;"))
    //    using (var cmd = new SqliteCommand("", con))
    //    {


    //        using (var transaction = con.BeginTransaction())
    //        {
    //            for (var i = 0; i < 1000000; i++)
    //            {
    //                cmd.CommandText = "insert into Student(FirstName,LastName) values ('John','Doe')";
    //                cmd.ExecuteNonQuery();
    //            }
    //            transaction.Commit();
    //        }
    //    }
    //}

}
