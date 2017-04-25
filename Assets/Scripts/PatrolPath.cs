using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Holds a collection of waypoints that an enemy agent will move through.
/// </summary>
[Serializable]
public class PatrolPath {

    public List<GameObject> waypoints;

    public PatrolPath()
    {
        waypoints = new List<GameObject>();
    }
}
