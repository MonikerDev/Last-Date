using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Overworld/Waypoint")]

public class Waypoint : ScriptableObject
{
    public string locationName;
    public Vector3 location;
    public string scene;
}
