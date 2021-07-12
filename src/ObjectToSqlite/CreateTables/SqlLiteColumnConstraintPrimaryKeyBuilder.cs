using System.Text;

namespace Bb.ToSqlite.CreateTables
{
    /// <summary>
    /// PRIMARY KEY (ASC|DESC) conflict-clause AUTOINCREMENT
    /// 
    /// </summary>
    public class SqlLiteColumnConstraintPrimaryKeyBuilder : SqlLiteColumnConstraintBaseBuilder
    {

        public ColumnSortWayEnum Way { get; internal set; }

        public bool AutoIncrement { get; internal set; }

        public ConflictClauseEnum? ConflictClause { get; internal set; }

        internal override void Generate(StringBuilder sb)
        {

            sb.Append($"PRIMARY KEY ");

            if (Way == ColumnSortWayEnum.ASC)
                sb.Append($"ASC ");
            else
                sb.Append($"DESC ");

            if (ConflictClause.HasValue)
            {
                sb.Append("ON CONFLICT ");
                sb.Append(ConflictClause.ToString().ToUpper() + " ");
            }

            if (AutoIncrement)
                sb.Append("AUTOINCREMENT ");

        }

    }


}
