namespace ObjectToSqlite
{

    public enum ConflictClauseEnum
    {
        Roolback,
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
        Undefined,
        INTEGER,
        BLOB,
        TEXT,
        real,

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
