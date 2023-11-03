using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Yarn.Unity;

public class FileVariableStorage : VariableStorageBehaviour
{
    public override void Clear()
    {
        GlobalVariableStorage.Clear();
    }

    public override bool Contains(string variableName)
    {
        return GlobalVariableStorage.Contains(variableName);
    }

    //[return: TupleElementNames(new[] { "FloatVariables", "StringVariables", "BoolVariables" })]
    public override (Dictionary<string, float> FloatVariables, Dictionary<string, string> StringVariables, Dictionary<string, bool> BoolVariables) GetAllVariables()
    {
        return GlobalVariableStorage.GetAll();
    }

    public override void SetAllVariables(Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools, bool clear = true)
    {
        foreach(string key in floats.Keys)
        {
            SetValue(key, floats[key]);
        }

        foreach(string key in strings.Keys) 
        {  
            SetValue(key, strings[key]); 
        }

        foreach(string key in strings.Keys)
        {
            SetValue(key, floats[key]);
        }
    }

    public override void SetValue(string variableName, string stringValue)
    {
        GlobalVariableStorage.Change(variableName, stringValue);
    }

    public override void SetValue(string variableName, float floatValue)
    {
        GlobalVariableStorage.Change(variableName, floatValue);
    }

    public override void SetValue(string variableName, bool boolValue)
    {
        GlobalVariableStorage.Change(variableName, boolValue);
    }

    public override bool TryGetValue<T>(string variableName, out T result)
    {
        return GlobalVariableStorage.CheckIfExists(variableName, out result);
    }
}
