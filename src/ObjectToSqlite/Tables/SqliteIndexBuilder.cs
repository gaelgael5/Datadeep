using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ObjectToSqlite
{

    public class SqliteIndexBuilder : Builder
    {

        public SqliteIndexBuilder(string schema,string tableName, string indexName, bool isUnique)
        {

            this.Schema = schema;
            this.Name = indexName;
            this.TableName = tableName;
            this.Unique = isUnique;

            Columns = new List<SqlLiteColumnIndexBuilder>();

        }

        public string Schema { get; }

        public string Name { get; }

        public string TableName { get; }

        public bool Unique { get; }

        public SqliteIndexBuilder IfNotExist()
        {
            this._ifNotExist = true;
            return this;
        }

        public List<SqlLiteColumnIndexBuilder> Columns { get; }

        public SqliteIndexBuilder Column<T>(params Action<SqliteColumnPropertyBuilder<T>>[] rules)
        {
            var builder = new SqliteColumnPropertyBuilder<T>(this);

            if (rules != null)
                foreach (var rule in rules)
                    rule(builder);

            return this;
        }

        public SqliteIndexBuilder Column(string name, params Action<SqlLiteColumnIndexBuilder>[] rules)
        {

            var col = new SqlLiteColumnIndexBuilder(name)
            {
                
            };

            if (rules != null)
                foreach (var rule in rules)
                    rule(col);

            Columns.Add(col);

            return this;

        }

        internal override void Generate(StringBuilder sb)
        {

            sb.AppendLine(string.Empty);

            sb.Append("CREATE ");
            
            if (this.Unique)
                sb.Append("UNIQUE ");

            sb.Append("INDEX ");

            if (this._ifNotExist)
                sb.Append("IF NOT EXISTS ");

            if (!string.IsNullOrEmpty(Schema))
                sb.Append($"`{Schema}`.");
            sb.Append($"`{Name}` ");

            #region columns

            sb.Append($"ON `{TableName}` ( ");

            string comma = string.Empty;
            foreach (var column in this.Columns)
            {
                
                if (!string.IsNullOrEmpty(comma))
                    sb.AppendLine(comma);

                sb.Append("\t");

                column.Generate(sb);
                comma = ", ";
            }

            sb.AppendLine(")");

            #endregion columns

        }

        private bool _ifNotExist;

        public class SqliteColumnPropertyBuilder<T>
        {

            public SqliteColumnPropertyBuilder(SqliteIndexBuilder sqliteTableBuilder)
            {
                this.sqliteTableBuilder = sqliteTableBuilder;
            }

            public SqliteColumnPropertyBuilder<T> Column(Expression<Func<T, object>> e, params Action<SqlLiteColumnIndexBuilder>[] rules)
            {

                var member = MemberResolverVisitor.Resolve(e);

                SqliteColumnType type = SqliteColumnType.Undefined;
                if (member is PropertyInfo prop)
                    type = prop.PropertyType.ToSqliteType();

                else
                {

                }

                this.sqliteTableBuilder.Column(member.Name, rules);

                return this;

            }

            private class MemberResolverVisitor : ExpressionVisitor
            {

                public static MemberInfo Resolve(Expression e)
                {

                    var visitor = new MemberResolverVisitor();
                    visitor.Visit(e);
                    return visitor._member;
                }

                protected override Expression VisitMember(MemberExpression node)
                {
                    this._member = node.Member;
                    return base.VisitMember(node);
                }

                private MemberInfo _member;

            }

            private SqliteIndexBuilder sqliteTableBuilder;

        }

    }


}
