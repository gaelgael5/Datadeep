using Microsoft.Data.Sqlite;
using System.Text;

namespace Bb.ToSqlite.Inserts
{
    public class SqlLiteParameterBuilder : SqlLiteExpressionBuilder
    {

        public SqlLiteParameterBuilder(string name)
            : base()
        {

            if (name.StartsWith("$"))
                name = name.Substring(1);

            this.Name = "$" + name[0].ToString().ToLower() + name.Substring(1);
        }

        public string Name { get; internal set; }

        public SqliteColumnType Type { get; internal set; }

        internal override void Generate(StringBuilder sb)
        {
            sb.Append(Name + " ");

        }

        internal SqliteParameter GetParameter()
        {

            if (this._parameter == null)
                this._parameter = this.Name.ToParameter((SqliteType)(int)this.Type, System.Data.ParameterDirection.Input);

            return this._parameter;

        }


        private SqliteParameter _parameter;

    }

}
