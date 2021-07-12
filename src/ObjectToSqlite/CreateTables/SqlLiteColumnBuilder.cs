using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bb.ToSqlite.CreateTables
{
    public class SqlLiteColumnBuilder : Builder
    {

        public SqlLiteColumnBuilder()
        {
            /// https://www.sqlite.org/syntax/column-constraint.html
            this._constraints = new List<SqlLiteColumnConstraintBaseBuilder>();
        }


        public string Name { get; internal set; }


        public SqliteColumnType Type { get; internal set; }

        public SqlLiteColumnBuilder Length(int length)
        {
            if (Type != SqliteColumnType.TEXT)
                throw new Exception("Length is strictly reserved for TEXT type");

            this._int1 = length;

            return this;
        }

        public SqlLiteColumnBuilder Scale(int scale)
        {
            if (Type != SqliteColumnType.INTEGER)
                throw new Exception("scale is strictly reserved for INTEGER type");

            this._int1 = scale;

            return this;
        }

        public SqlLiteColumnBuilder Scale(int scale, int precision)
        {
            if (Type != SqliteColumnType.real)
                throw new Exception("method is strictly reserved for real type");

            this._int1 = scale;
            this._int2 = precision;

            return this;
        }

        public SqlLiteColumnConstraintPrimaryKeyBuilder AsPrimaryKey(ColumnSortWayEnum way = ColumnSortWayEnum.ASC, ConflictClauseEnum? conflictClause = null, bool autoIncrement = false)
        {

            var item = this._constraints.OfType<SqlLiteColumnConstraintPrimaryKeyBuilder>().FirstOrDefault();
            if (item == null)
            {

                var constraint = new SqlLiteColumnConstraintPrimaryKeyBuilder()
                {
                    Name = $"{Name}_PK",
                    Way = way,
                    ConflictClause = conflictClause,
                    AutoIncrement = autoIncrement,
                };

                this._constraints.Add(constraint);

                item = constraint;

            }

            return item;

        }

        public SqlLiteColumnConstraintNotNullBuilder NotNull(ConflictClauseEnum? conflictClause)
        {

            var item = this._constraints.OfType<SqlLiteColumnConstraintNotNullBuilder>().FirstOrDefault();
            if (item == null)
            {

                var constraint = new SqlLiteColumnConstraintNotNullBuilder()
                {
                    Name = $"{Name}_Required",
                    ConflictClause = conflictClause,
                };

                this._constraints.Add(constraint);

                item = constraint;

            }

            return item;

        }

        public SqlLiteColumnConstraintUniqueBuilder Unique(ConflictClauseEnum? conflictClause)
        {

            var item = this._constraints.OfType<SqlLiteColumnConstraintUniqueBuilder>().FirstOrDefault();
            if (item == null)
            {

                var constraint = new SqlLiteColumnConstraintUniqueBuilder()
                {
                    Name = $"{Name}_Unique",
                    ConflictClause = conflictClause,
                };

                this._constraints.Add(constraint);

                item = constraint;

            }

            return item;

        }

        public SqlLiteColumnConstraintCheckBuilder Check(string expression)
        {

            var item = this._constraints.OfType<SqlLiteColumnConstraintCheckBuilder>().FirstOrDefault(c => c.Expression == expression);
            if (item == null)
            {

                var constraint = new SqlLiteColumnConstraintCheckBuilder()
                {
                    Name = $"{Name}_Check_" + Guid.NewGuid().ToString("A"),
                    Expression = expression
                };

                item = constraint;

            }

            return item;

        }

        public SqlLiteColumnConstraintDefaultBuilder Default(string expression, bool isLiteral)
        {

            var item = this._constraints.OfType<SqlLiteColumnConstraintDefaultBuilder>().FirstOrDefault();
            if (item == null)
            {

                var constraint = new SqlLiteColumnConstraintDefaultBuilder()
                {
                    Name = $"{Name}_Default_" + Guid.NewGuid().ToString("A"),
                    Expression = expression,
                    IsLiteral = isLiteral,
                };

                this._constraints.Add(constraint);

                item = constraint;

            }

            return item;

        }

        public SqlLiteColumnConstraintCollateBuilder Collate(CollationEnum collationName)
        {

            var item = this._constraints.OfType<SqlLiteColumnConstraintCollateBuilder>().FirstOrDefault();
            if (item == null)
            {

                var constraint = new SqlLiteColumnConstraintCollateBuilder()
                {
                    Name = $"{Name}_Check_" + Guid.NewGuid().ToString("A"),
                    CollationName = collationName.ToString().ToUpper()
                };

                this._constraints.Add(constraint);

                item = constraint;

            }

            return item;

        }

        public SqlLiteColumnConstraintGeneratedBuilder Generated(string expression, GeneratedEnum? mode)
        {

            var item = this._constraints.OfType<SqlLiteColumnConstraintGeneratedBuilder>().FirstOrDefault();
            if (item == null)
            {

                var constraint = new SqlLiteColumnConstraintGeneratedBuilder()
                {
                    Name = $"{Name}_Generated",
                    Expression = expression,
                    Mode = mode
                };

                this._constraints.Add(constraint);

                item = constraint;

            }

            return item;

        }

        internal override void Generate(StringBuilder sb)
        {

            sb.Append($"`{Name}` ");

            switch (this.Type)
            {

                case SqliteColumnType.INTEGER:
                    sb.Append($"INTEGER ");
                    if (this._int1 != null)
                        sb.Append($"({this._int1.Value}) ");
                    break;

                case SqliteColumnType.BLOB:
                    sb.Append($"BLOB ");

                    break;

                case SqliteColumnType.TEXT:
                    sb.Append($"TEXT ");
                    if (this._int1 != null)
                        sb.Append($"({this._int1.Value}) ");

                    break;

                case SqliteColumnType.real:
                    sb.Append($"REAL ");
                    if (this._int1 != null)
                    {
                        sb.Append($"({this._int1.Value}");
                        if (this._int2 != null)
                        {
                            sb.Append($", {this._int1.Value}");
                        }
                        sb.Append(") ");
                    }
                    break;

                case SqliteColumnType.Undefined:
                default:
                    break;
            }

            foreach (var item in _constraints)
            {
                item.Generate(sb);
            }

        }

        private List<SqlLiteColumnConstraintBaseBuilder> _constraints;
        private int? _int1;
        private int? _int2;

    }

    // foreign-key-clause

}
