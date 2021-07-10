using System.Text;

namespace ObjectToSqlite
{
    public class SqlLiteColumnConstraintCollateBuilder : SqlLiteColumnConstraintBaseBuilder
    {

        public string CollationName { get; internal set; }

        internal override void Generate(StringBuilder sb)
        {

            sb.Append("COLLATE ");
            sb.Append(CollationName);
            
        }

    }


}
