using System.Text;

namespace Bb.ToSqlite.CreateTables
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
