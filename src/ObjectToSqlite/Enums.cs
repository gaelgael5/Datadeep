
namespace Bb.ToSqlite
{

    public enum ConflictClauseEnum
    {
        Rollback,
        Abort,
        Fail,
        Ignore,
        Replace
    }

    public enum ColumnSortWayEnum
    {
        ASC,
        DESC
    }

    public enum SqliteColumnType
    {
        Undefined = 0,
        INTEGER = 1,
        real = 2,
        TEXT = 3,
        BLOB = 4,

    }

    public enum GeneratedEnum
    {
        Stored,
        Virtual,
    }

    public enum CollationEnum
    {
        BINARY,
        RTRIM,
        NOCASE
    }


}
