using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.XPath;
using UnityEngine;

public static class GlobalVariableStorage
{
    //database
    private static IDbConnection conn;

    //Battlers
    public static List<string> Party = new List<string>();
    public static List<string> CurrentEncounter = new List<string>();

    //Key Elements
    public static Cynthia cynthia;
    public static Asheton asheton;
    public static Bea bea;
    public static Heather heather;
    public static Madison madison;
    public static Logan logan;
    public static Emilia emilia;

    public static void AddToParty(string unit)
	{
        Party.Add(unit);
	}

    public static void RemoveFromParty(string unit)
	{
        Party.Remove(unit);
	}

    public static void CompileEncounter()
	{
        CurrentEncounter.AddRange(Party);

        CurrentEncounter.AddRange(GlobalUtility.CreateEncounter());
	}

    public static IDbConnection CreateAndOpenDatabase()
    {
        // Open a connection to the database.
        string dbUri = "Data Source=" + Application.dataPath + "/StreamingAssets/GlobalStorage.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        return dbConnection;
    }

    private static void SaveUnitToDB(Unit unit)
    {
        CreateAndOpenDatabase();

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

        conn.Close();
    }

    private static Unit PullUnitFromDB(string unitName)
    {
        CreateAndOpenDatabase();

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

        conn.Close();
        return result;
    }

}
