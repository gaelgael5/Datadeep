using System.Collections.Generic;
using System.Text;

namespace Bb.ToSqlite.Inserts
{
    public class SqliteUpsertClauseBuilder : Builder
    {

        public SqliteUpsertClauseBuilder()
        {
            this._insertedColumn = new List<string>();
        }

        public List<string> Columns => this._insertedColumn;

        internal override void Generate(StringBuilder sb)
        {

            sb.Append("ON CONFLICT ");
            sb.Append("DO ");

            if (_insertedColumn.Count > 0)
            {
                sb.Append(" (");
                _insertedColumn.AppendList(", ", sb, (item, s) => s.Append(item));
                sb.Append($") ");

                if (this.Expression != null)
                {
                    sb.Append("WHERE ");
                    sb.Append(Expression);
                }

                if (this.Do != null)
                {

                    this.Do.Generate(sb);
                    sb.Append(" ");

                }
                else
                    sb.Append("NOTHING ");

            }
        }

        public SqliteUpsertClauseBuilder Where(string expression)
        {
            this.Expression = expression;
            return this;
        }

        private readonly List<string> _insertedColumn;

        public string Expression { get; internal set; }
        
        public SqliteUpsertClauseDoBuilder Do { get; internal set; }

        public SqliteTableInsertBuilder Parent { get; internal set; }

    }

}
