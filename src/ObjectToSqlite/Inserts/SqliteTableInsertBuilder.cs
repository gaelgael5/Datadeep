using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.ToSqlite.Inserts
{


    public class SqliteTableInsertBuilder : Builder
    {


        public SqliteTableInsertBuilder()
        {
            this._items = new List<SqlLiteExpressionBuilder>();
            this._insertedColumn = new List<string>();
        }


        public string SchemaName { get; internal set; }

        public string TableName { get; internal set; }

        public string Mode { get; internal set; }

        public ConflictClauseEnum? Policy { get; internal set; }

        public SqliteUpsertClauseBuilder UpsertClause { get; internal set; }

        public SqliteTableInsertBuilder Insert(params string[] names)
        {
            this._insertedColumn.AddRange(names.AsQuoted());
            return this;
        }

        public SqliteTableInsertBuilder Values(params SqlLiteExpressionBuilder[] items)
        {
            this._items.AddRange(items);
            return this;
        }

        public SqliteCommand GetCommand(SqliteConnection cnx)
        {

            var result = cnx.CreateCommand();
            result.CommandText = this.Generate();
            
            var parameters = GetDictionaryParameters();
            foreach (var parameter in parameters)
                result.Parameters.Add(parameter.Value);

            return result;

        }

        public SqliteParameter[] GetListParameters()
        {
            if (_parameters2 == null)
                GetDictionaryParameters();
            return _parameters2;
        }

        public Dictionary<string, SqliteParameter> GetDictionaryParameters()
        {
            if (_parameters == null)
            {
                _parameters = new Dictionary<string, SqliteParameter>();
                var _parameters3 = new List<SqliteParameter>(_items.Count);
                foreach (SqlLiteExpressionBuilder e in _items)
                    if (e is SqlLiteParameterBuilder p)
                    {
                        var parameter = p.GetParameter();
                        _parameters.Add(parameter.ParameterName, parameter);
                        _parameters3.Add(parameter);
                    }
                _parameters2 = _parameters3.ToArray();
            }

            return _parameters;

        }

        internal override void Generate(StringBuilder sb)
        {

            string comma = string.Empty;

            sb.Append(this.Mode);

            if (Policy.HasValue)
                sb.Append(" OR " + Policy.Value.ToString().ToUpper());

            sb.Append(" INTO ");

            if (!string.IsNullOrEmpty(SchemaName))
                sb.Append($"`{SchemaName}`.");
            sb.Append($"`{TableName}`");

            if (_insertedColumn.Count > 0)
            {
                sb.Append(" (");
                _insertedColumn.AppendList(", ", sb, (item, s) => s.Append(item));
                sb.Append($") ");
            }

            sb.Append($"VALUES (");
            _items.AppendList(", ", sb, (item, s) => item.Generate(s));
            sb.Append(")");

            if (UpsertClause != null)
            {
                sb.Append(" ");
                UpsertClause.Generate(sb);
            }

            sb.Append(";");

        }

        private Dictionary<string, SqliteParameter> _parameters;
        private SqliteParameter[] _parameters2;
        private readonly List<SqlLiteExpressionBuilder> _items;
        private readonly List<string> _insertedColumn;

    }


    //internal class Test
    //{
    //    public Test()
    //    {

    //        SqliteTableInsertBuilder i = Sqlite.InsertInto("mytable")
    //            .Insert("Id", "Name")
    //            .Values("@id", "@name")

    //            //.OnConflict("")
    //            //    .Where("")
    //            //        .UpdateSet((new string[] { "col1" }, ""))
    //            //        .Where("")

    //            ;

    //        using (var cnx = new SqliteConnection(""))
    //        {

    //            var parameterId = i.GetDictionaryParameters()["@id"];
    //            var parameterName = i.GetDictionaryParameters()["@name"];

    //            i.Execute(cnx, (action) =>
    //            {


    //            });
    //        }


    //    }

    //}

}

