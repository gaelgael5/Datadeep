using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Bb.ToSqlite.CreateTables
{


    public class SqliteCreateTableBuilder : Builder
    {

        public SqliteCreateTableBuilder(string schema, string tableName, bool isTemporary)
        {
            this.Schema = schema;
            this.Name = tableName;
            this.IsTemporary = isTemporary;
            Columns = new List<SqlLiteColumnBuilder>();
        }

        public string Schema { get; }

        public string Name { get; }

        public bool IsTemporary { get; }

        public SqliteCreateTableBuilder IfNotExist()
        {
            this._ifNotExist = true;
            return this;
        }

        public SqliteCreateTableBuilder WithoutRowId()
        {
            this._withoutRowId = true;
            return this;
        }

        public List<SqlLiteColumnBuilder> Columns { get; }

        public SqliteCreateTableBuilder Column<T>(params Action<SqliteColumnPropertyBuilder<T>>[] rules)
        {
            var builder = new SqliteColumnPropertyBuilder<T>(this);

            if (rules != null)
                foreach (var rule in rules)
                    rule(builder);

            return this;
        }

        public SqliteCreateTableBuilder Column(string name, SqliteColumnType type, params Action<SqlLiteColumnBuilder>[] rules)
        {

            var col = new SqlLiteColumnBuilder()
            {
                Name = name,
                Type = type
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

            if (this.IsTemporary)
                sb.Append("TEMPORARY ");

            sb.Append("TABLE ");

            if (this._ifNotExist)
                sb.Append("IF NOT EXISTS ");

            if (!string.IsNullOrEmpty(Schema))
                sb.Append($"`{Schema}`.");
            sb.Append($"`{Name}` ");


            #region columns

            sb.AppendLine("( ");

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

            if (this._withoutRowId)
                sb.Append("WITHOUT ROWID ");

        }

        private bool _ifNotExist;
        private bool _withoutRowId;

        public class SqliteColumnPropertyBuilder<T>
        {

            public SqliteColumnPropertyBuilder(SqliteCreateTableBuilder sqliteTableBuilder)
            {
                this.sqliteTableBuilder = sqliteTableBuilder;
            }

            public SqliteColumnPropertyBuilder<T> Column(Expression<Func<T, object>> e, params Action<SqlLiteColumnBuilder>[] rules)
            {

                var member = MemberResolverVisitor.Resolve(e);

                SqliteColumnType type = SqliteColumnType.Undefined;
                if (member is PropertyInfo prop)
                    type = prop.PropertyType.ToSqliteType();

                else
                {

                }

                this.sqliteTableBuilder.Column(member.Name, type, rules);

                return this;

            }

            private SqliteCreateTableBuilder sqliteTableBuilder;

        }

    }


}
