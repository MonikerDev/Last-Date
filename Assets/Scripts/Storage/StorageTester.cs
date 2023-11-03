using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class StorageTester : MonoBehaviour
{
    IDbConnection conn;

    // Start is called before the first frame update
    void Start()
    {
        conn = CreateAndOpenDatabase();

        Cynthia cynthia = new Cynthia(10, 10, 10, 10, 10, 10, 10, 10, 10, 10);

        SaveUnitToDB(cynthia);

        Unit c = PullUnitFromDB("Cynthia");

        Debug.Log(c.charName);
    }

    private IDbConnection CreateAndOpenDatabase()
    {
        // Open a connection to the database.
        string dbUri = "Data Source=" + Application.dataPath + "/StreamingAssets/GlobalStorage.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        return dbConnection;
    }

    private void SaveUnitToDB(Unit unit)
    {
        SqliteParameter param1 = new SqliteParameter("$charName", unit.charName);
        SqliteParameter param2 = new SqliteParameter("$bsAtk", unit.bsAtk);
        SqliteParameter param3 = new SqliteParameter("$bsDef", unit.bsDef);
        SqliteParameter param4 = new SqliteParameter("$bsSpd", unit.bsSpd);
        SqliteParameter param5 = new SqliteParameter("$bsHp", unit.bsHp);
        SqliteParameter param6 = new SqliteParameter("$bsStm", unit.bsStm);
        SqliteParameter param7 = new SqliteParameter("$currAtk", unit.currAtk);
        SqliteParameter param8 = new SqliteParameter("$currDef", unit.currDef);
        SqliteParameter param9 = new SqliteParameter("$currSpd", unit.currSpd);
        SqliteParameter param10 = new SqliteParameter("$currHp", unit.currHp);
        SqliteParameter param11 = new SqliteParameter("$currStm", unit.currStm);

        IDbCommand command = conn.CreateCommand();
        command.CommandText = "INSERT OR REPLACE INTO Characters (charName, bsAtk, bsDef, bsSpd, bsHp, bsStm, currAtk, currDef, currSpd, currHp, currStm) VALUES ($charName, $bsAtk, $bsDef, $bsSpd, $bsHp, $bsStm, $currAtk, $currDef, $currSpd, $currhp, $currStm)";
        command.Parameters.Add(param1);
        command.Parameters.Add(param2);
        command.Parameters.Add(param3);
        command.Parameters.Add(param4);
        command.Parameters.Add(param5);
        command.Parameters.Add(param6);
        command.Parameters.Add(param7);
        command.Parameters.Add(param8);
        command.Parameters.Add(param9);
        command.Parameters.Add(param10);
        command.Parameters.Add(param11);
        command.ExecuteNonQuery();
    }

    private Unit PullUnitFromDB(string unitName)
    {
        Unit result = null;

        SqliteParameter param = new SqliteParameter("$unitName", unitName);

        IDbCommand command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM Characters WHERE charName = $unitName";
        command.Parameters.Add(param);

        IDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            int bsAtk = reader.GetInt32(1);
            int bsDef = reader.GetInt32(2);
            int bsSpd = reader.GetInt32(3);
            int bsHp = reader.GetInt32(4);
            int bsStm = reader.GetInt32(5);
            int currAtk = reader.GetInt32(6);
            int currDef = reader.GetInt32(6);
            int currSpd = reader.GetInt32(6);
            int currHp = reader.GetInt32(6);
            int currStm = reader.GetInt32(6);

            switch (reader.GetString(0))
            {
                case "Cynthia":
                    
                    result = new Cynthia(bsAtk, bsDef, bsSpd, bsHp, bsStm, currAtk, currDef, currSpd, currHp, currStm);
                    break;
            }
        }

        return result;
    }
}
