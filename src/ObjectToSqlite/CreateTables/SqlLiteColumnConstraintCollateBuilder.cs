using System.Text;

namespace Bb.ToSqlite.CreateTables
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
