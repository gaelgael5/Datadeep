using System.Collections.Generic;
using System.Text;

namespace Bb.ToSqlite.Inserts
{
    public class SqliteUpsertClauseDoBuilder : Builder
    {

        public SqliteUpsertClauseDoBuilder()
        {
            this._insertedColumn = new List<(string[], string)>();
        }

        public SqliteTableInsertBuilder Where(string expression)
        {
            this.Expression = expression;
            return this.Parent.Parent;
        }

        internal override void Generate(StringBuilder sb)
        {

            sb.Append("UPDATE SET ");

            this._insertedColumn.AppendList(", ", sb, (a, b) =>
            {

                if (a.Item1.Length == 1)
                {
                    sb.Append(a.Item1[0].AsQuoted());
                    sb.Append(" ");
                }
                else
                {
                    sb.Append("(");
                    a.Item1.AppendList(", ", sb, (c, d) => d.Append(c.AsQuoted()));
                    sb.Append(") ");
                }

                sb.Append("= ");
                sb.Append(a.Item2.AsQuoted());

                sb.Append(" ");


            });

            if(this.Expression != null)
                {
                sb.Append("WHERE ");
                sb.Append(Expression);
            }

        }

        public List<(string[], string)> Columns => this._insertedColumn;

        public string Expression { get; private set; }
        public SqliteUpsertClauseBuilder Parent { get; internal set; }

        private readonly List<(string[], string)> _insertedColumn;

    }

}

