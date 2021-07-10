using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectToSqlite
{

    public class SqlLiteColumnIndexBuilder : Builder
    {


        public SqlLiteColumnIndexBuilder(string name)
        {

            // https://www.sqlite.org/syntax/indexed-column.html
            this.Name = name;

        }


        public string Name { get; internal set; }

        public string CollationName { get; internal set; }

        public ColumnSortWayEnum Way { get; internal set; }

        public SqlLiteColumnIndexBuilder Order(ColumnSortWayEnum way)
        {
            this.Way = way;
            return this;
        }

        public SqlLiteColumnIndexBuilder Collate(CollationEnum collationName)
        {
            CollationName = collationName.ToString();
            return this;
        }

        internal override void Generate(StringBuilder sb)
        {

            sb.Append($"`{Name}` ");

            if (!string.IsNullOrEmpty(CollationName))
            {
                sb.Append("COLLATE ");
                sb.Append(CollationName + " ");
            }


            if (Way == ColumnSortWayEnum.ASC)
                sb.Append($"ASC ");
            else
                sb.Append($"DESC ");

        }

    }

    // foreign-key-clause

}
