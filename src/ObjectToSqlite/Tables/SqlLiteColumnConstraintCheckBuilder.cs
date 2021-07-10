using System.Text;

namespace ObjectToSqlite
{
    public class SqlLiteColumnConstraintCheckBuilder : SqlLiteColumnConstraintBaseBuilder
    {

        public string Expression { get; internal set; }

        internal override void Generate(StringBuilder sb)
        {

            sb.Append("CHECK (");
            sb.Append(Expression);
            sb.Append(") ");


        }


    }


}
