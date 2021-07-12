using System;

namespace Bb.ToSqlite.Inserts
{

    public abstract class SqlLiteExpressionBuilder : Builder
    {

        public SqlLiteExpressionBuilder()
        {

        }

        public static implicit operator SqlLiteExpressionBuilder(string self)
        {

            if (self.StartsWith("$"))
                return new SqlLiteParameterBuilder(self) { Type = SqliteColumnType.Undefined };

            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.TEXT };

        }

        public static implicit operator SqlLiteExpressionBuilder(bool self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.INTEGER };
        }

        public static implicit operator SqlLiteExpressionBuilder(byte self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.INTEGER };
        }

        public static implicit operator SqlLiteExpressionBuilder(byte[] self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.BLOB };
        }

        public static implicit operator SqlLiteExpressionBuilder(char self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.TEXT };
        }

        public static implicit operator SqlLiteExpressionBuilder(DateTime self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.TEXT };
        }

        public static implicit operator SqlLiteExpressionBuilder(DateTimeOffset self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.TEXT };
        }

        public static implicit operator SqlLiteExpressionBuilder(decimal self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.TEXT };
        }

        public static implicit operator SqlLiteExpressionBuilder(double self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.real };
        }

        public static implicit operator SqlLiteExpressionBuilder(Guid self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.TEXT };
        }

        public static implicit operator SqlLiteExpressionBuilder(UInt16 self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.INTEGER };
        }

        public static implicit operator SqlLiteExpressionBuilder(Int16 self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.INTEGER };
        }

        public static implicit operator SqlLiteExpressionBuilder(uint self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.INTEGER };
        }

        public static implicit operator SqlLiteExpressionBuilder(int self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.INTEGER };
        }

        public static implicit operator SqlLiteExpressionBuilder(UInt64 self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.INTEGER };
        }

        public static implicit operator SqlLiteExpressionBuilder(Int64 self)
        {
            return new SqlLiteLiteralBuilder(self){Type = SqliteColumnType.INTEGER};
        }

        public static implicit operator SqlLiteExpressionBuilder(TimeSpan self)
        {
            return new SqlLiteLiteralBuilder(self) { Type = SqliteColumnType.TEXT };
        }

    }

}
