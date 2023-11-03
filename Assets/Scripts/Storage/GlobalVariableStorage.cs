using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using UnityEngine;

public static class GlobalVariableStorage
{
    //Flags for Yarn
    private static Dictionary<string, string> labels = new Dictionary<string, string>();
    private static Dictionary<string, bool> flags = new Dictionary<string, bool>();
    private static Dictionary<string, float> counters = new Dictionary<string, float>();
    private static string stringPath = Application.dataPath + "/MyTest.txt";

    //Inventory
    public static List<Item> items = new List<Item>();

    public static void Clear()
    {
        labels.Clear();
        flags.Clear();
        counters.Clear();
    }

    public static bool Contains(string key)
    {
        bool contains = false;

        if(labels.ContainsKey(key) || flags.ContainsKey(key) || counters.ContainsKey(key))
        {
            contains = true;
        }

        return contains;
    }

    public static (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>) GetAll()
    {
        return (counters, labels, flags);
    }

    public static void Add<T>(string key, T value)
    {
		if (typeof(T) == typeof(string))
		{
            labels.Add(key, (string)(object)value);
		}
		else if (typeof(T) == typeof(bool))
		{
			flags.Add(key, (bool)(object)value);
		}
		else if (typeof(T) == typeof(float))
		{
			counters.Add(key, (float)(object)value);
		}
	}

	public static void Change<T>(string key, T value)
	{
		if (typeof(T) == typeof(string))
		{
			labels[key] = (string)(object)value;
		}
		else if (typeof(T) == typeof(bool))
		{
			flags[key] = (bool)(object)value;
		}
		else if (typeof(T) == typeof(float))
		{
            counters[key] = (float)(object)value;
		}
	}

	public static void Trigger(string key)
    {
        if (!flags[key])
        {
            flags[key] = true;
        }
        else
        {
            Debug.Log("Flag is already triggered");
        }
    }

    public static bool CheckIfExists<T>(string key, out T result)
    {   
        if(typeof(T) == typeof(string))
        {
            if (labels.ContainsKey(key))
            {
                result = (T)(object)labels[key];
                return true;
            }
        }
        else if(typeof(T) == typeof(bool))
        {
            if(flags.ContainsKey(key))
            {
                result = (T)(object)flags[key]; 
                return true;
            }
        }
        else if( typeof(T) == typeof(float))
        {
            if(counters.ContainsKey(key))
            {
                result = (T)(object)counters[key];
                return true;
            }
        }

        result = default(T);
        return false;

    }

    public static void PopulateFlags()
    {
        flags.Clear();

        Debug.Log(stringPath);

        if (!File.Exists(stringPath))
        {
            File.Create(stringPath);
        }

        using(StreamReader sr = new StreamReader(stringPath))
        {
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                try
                {
                    bool state;
                    string[] pieces = line.Split(',');

                    if (pieces[1] == "false")
                    {
                        state = false;
                    }
                    else if (pieces[1] == "true")
                    {
                        state = true;
                    }
                    else
                    {
                        throw new System.Exception();
                    }

                    flags.Add(pieces[0], state);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }

            sr.Close();
        }
    }

    public static void WriteFlagsToFile()
    {
        Debug.Log(stringPath);

        if (!File.Exists(stringPath))
        {
            File.Create(stringPath);
        }

        using (StreamWriter sw = new StreamWriter(stringPath))
        {
            foreach(string flag in flags.Keys)
            {
                string state;

                if (flags[flag])
                {
                    state = "true";
                }
                else
                {
                    state = "false";
                }
                sw.WriteLine(flag + "," +  state);
            }
            sw.Close();
        }

    }
}
