//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ObjectToSqlite
//{

//    public class SqliteForeignTableConstraintBuilder : Builder
//    {

//        public SqliteForeignTableConstraintBuilder(string schema, string tableName, string name)
//        {

//            this.Schema = schema;
//            this.Name = name;
//            this.TableName = tableName;

//            Columns = new List<SqlLiteColumnIndexBuilder>();

//        }

//        public string Schema { get; }

//        public string TableName { get; }

//        public string Name { get; }

//        public List<SqlLiteColumnIndexBuilder> Columns { get; }

//        public SqliteForeignTableConstraintBuilder Column(string name, params Action<SqlLiteColumnIndexBuilder>[] rules)
//        {

//            var col = new SqlLiteColumnIndexBuilder(name)
//            {
                
//            };

//            if (rules != null)
//                foreach (var rule in rules)
//                    rule(col);

//            Columns.Add(col);

//            return this;

//        }

//        internal override void Generate(StringBuilder sb)
//        {

//            sb.AppendLine(string.Empty);

//            sb.Append($"ALTER TABLE ");
          
//            if (!string.IsNullOrEmpty(Schema))
//                sb.Append($"`{Schema}`.");
//            sb.Append($"`{TableName}` ");

//            sb.Append("INDEX ");

//            #region columns

//            sb.Append($"ON `{TableName}` ( ");

//            string comma = string.Empty;
//            foreach (var column in this.Columns)
//            {
                
//                if (!string.IsNullOrEmpty(comma))
//                    sb.AppendLine(comma);

//                sb.Append("\t");

//                column.Generate(sb);
//                comma = ", ";
//            }

//            sb.AppendLine(")");

//            #endregion columns

//        }

      
//    }


//}
