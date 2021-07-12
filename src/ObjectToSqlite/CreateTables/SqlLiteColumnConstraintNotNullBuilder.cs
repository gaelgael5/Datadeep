using System.Text;

namespace Bb.ToSqlite.CreateTables
{

    /// <summary>
    /// NOT NULL conflict-clause
    /// </summary>
    public class SqlLiteColumnConstraintNotNullBuilder : SqlLiteColumnConstraintBaseBuilder
    {

        public ConflictClauseEnum? ConflictClause { get; internal set; }

        internal override void Generate(StringBuilder sb)
        {

            sb.Append("NOT NULL ");

            if (ConflictClause.HasValue)
            {
                sb.Append("ON CONFLICT ");
                sb.Append(ConflictClause.ToString().ToUpper() + " ");
            }

        }


    }


}
