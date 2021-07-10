using System.Text;

namespace ObjectToSqlite
{

    /// <summary>
    ///  UNIQUE conflict-clause
    /// </summary>
    public class SqlLiteColumnConstraintUniqueBuilder : SqlLiteColumnConstraintBaseBuilder
    {

        public ConflictClauseEnum? ConflictClause { get; internal set; }

        internal override void Generate(StringBuilder sb)
        {

            sb.Append("UNIQUE ");

            if (ConflictClause.HasValue)
            {
                sb.Append("ON CONFLICT ");
                sb.Append(ConflictClause.ToString().ToUpper() + " ");
            }

        }


    }


}
