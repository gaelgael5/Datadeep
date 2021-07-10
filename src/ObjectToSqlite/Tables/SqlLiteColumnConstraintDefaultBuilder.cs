using System.Text;

namespace ObjectToSqlite
{
    public class SqlLiteColumnConstraintDefaultBuilder : SqlLiteColumnConstraintBaseBuilder
    {

        public string Expression { get; internal set; }

        public bool IsLiteral { get; internal set; }

        internal override void Generate(StringBuilder sb)
        {

            sb.Append("DEFAULT ");

            if (IsLiteral)
            {
                sb.Append(Expression);
                sb.Append(" ");
            }
            else
            {
                sb.Append("(");
                sb.Append(Expression);
                sb.Append(") ");
            }

        }


    }


}
