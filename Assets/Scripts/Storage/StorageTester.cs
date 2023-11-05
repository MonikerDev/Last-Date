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
        conn = GlobalVariableStorage.CreateAndOpenDatabase();
    }

}