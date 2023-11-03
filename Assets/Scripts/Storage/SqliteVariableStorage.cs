using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using Yarn.Unity;

public class SqliteVariableStorage : VariableStorageBehaviour
{
    IDbConnection conn = CreateAndOpenDatabase();

    private void OnDestroy()
    {
        this.conn.Close();
    }

    private static IDbConnection CreateAndOpenDatabase()
    {
        // Open a connection to the database.
        Debug.Log("Connected");
        string dbUri = "Data Source=" + Application.dataPath + "/StreamingAssets/GlobalStorage.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        return dbConnection;
    }

    public override bool TryGetValue<T>(string variableName, out T result)
    {
        SqliteParameter param = new SqliteParameter("$value", variableName);

        IDbCommand command = conn.CreateCommand();

        if (typeof(T) == typeof(string))
        {
            command.CommandText = "SELECT * FROM YarnStrings WHERE key = $value";
            command.Parameters.Add(param);
            IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                result = (T)(object)reader.GetString(1);
                return true;
            }
        }
        else if (typeof(T) == typeof(bool))
        {
            command.CommandText = "SELECT * FROM YarnBools WHERE key = $value";
            command.Parameters.Add(param);
            IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                bool value;
                int temp = reader.GetInt32(1);

                if(temp == 0) { value = false; }
                else { value = true; }

                result = (T)(object)value;
                return true;
            }

        }
        else if (typeof(T) == typeof(float))
        {
            command.CommandText = "SELECT * FROM YarnFloats WHERE key = $value";
            command.Parameters.Add(param);
            IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                result = (T)(object)reader.GetFloat(1);
                return true;
            }
        }

        result = default(T);
        return false;
    }

    public override void SetValue(string variableName, string stringValue)
    {
        Debug.Log("Setting " + variableName + " to " + stringValue);
        SqliteParameter param1 = new SqliteParameter("$key", variableName);
        SqliteParameter param2 = new SqliteParameter("$value", stringValue);
        IDbCommand command = conn.CreateCommand();
        command.CommandText = "INSERT OR REPLACE INTO YarnStrings (key, value) VALUES ($key, $value)";
        command.Parameters.Add(param1);
        command.Parameters.Add(param2);
        command.ExecuteNonQuery();
    }

    public override void SetValue(string variableName, float floatValue)
    {
        Debug.Log("Setting " + variableName + " to " + floatValue);

        SqliteParameter param1 = new SqliteParameter("$key", variableName);
        SqliteParameter param2 = new SqliteParameter("$value", floatValue);
        IDbCommand command = conn.CreateCommand();
        command.CommandText = "INSERT OR REPLACE INTO YarnFloats (key, value) VALUES ($key, $value)";
        command.Parameters.Add(param1);
        command.Parameters.Add(param2);
        command.ExecuteNonQuery();
    }

    public override void SetValue(string variableName, bool boolValue)
    {
        Debug.Log("Setting " + variableName + " to " + boolValue);

        SqliteParameter param1 = new SqliteParameter("$key", variableName);
        SqliteParameter param2 = new SqliteParameter("$value", boolValue);
        IDbCommand command = conn.CreateCommand();
        command.CommandText = "INSERT OR REPLACE INTO YarnBools (key, value) VALUES ($key, $value)";
        command.Parameters.Add(param1);
        command.Parameters.Add(param2);
        command.ExecuteNonQuery();
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    public override bool Contains(string variableName)
    {
        SqliteParameter param = new SqliteParameter("$value", variableName);

        IDbCommand command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM YarnFloats WHERE key = $value";
        command.Parameters.Add(param);
        IDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return true;
        }

        command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM YarnStrings WHERE key = $value";
        command.Parameters.Add(param);
        reader = command.ExecuteReader();

        if (reader.Read())
        {
            return true;
        }

        command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM YarnBools WHERE key = $value";
        command.Parameters.Add(param);
        reader = command.ExecuteReader();

        if (reader.Read())
        {
            return true;
        }

        return false;
    }

    public override void SetAllVariables(Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools, bool clear = true)
    {
        throw new System.NotImplementedException();
    }

    public override (Dictionary<string, float> FloatVariables, Dictionary<string, string> StringVariables, Dictionary<string, bool> BoolVariables) GetAllVariables()
    {
        Dictionary<string, float> floats = new Dictionary<string, float>();
        Dictionary<string, string> strings = new Dictionary<string, string>();
        Dictionary<string, bool> bools = new Dictionary<string, bool>();

        IDbCommand command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM YarnFloats";
        IDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            floats.Add(reader.GetString(0), reader.GetFloat(1));
        }

        command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM YarnStrings";
        reader = command.ExecuteReader();

        while (reader.Read())
        {
            strings.Add(reader.GetString(0), reader.GetString(1));
        }

        command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM YarnBools";
        reader = command.ExecuteReader();

        while (reader.Read())
        {
            int value = reader.GetInt32(1);
            if (value == 0)
            {
                bools.Add(reader.GetString(0), false);
            }
            else
            {
                bools.Add(reader.GetString(0), true);
            }
        }

        return (floats, strings, bools);
    }
}
