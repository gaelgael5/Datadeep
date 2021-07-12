using System.Text;

namespace Bb.ToSqlite.CreateTables
{

    public class SqlLiteColumnConstraintGeneratedBuilder : SqlLiteColumnConstraintBaseBuilder
    {

        public string Expression { get; internal set; }

        public GeneratedEnum? Mode { get; set; }

        internal override void Generate(StringBuilder sb)
        {

            sb.Append("GENERATED ALWAYS AS (");
            sb.Append(Expression);
            sb.Append(") ");

            if (Mode.HasValue)
                sb.Append( Mode.ToString().ToUpper() + " ");

        }

    }


}
