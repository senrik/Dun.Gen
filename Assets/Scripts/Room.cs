using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public List<BoundryTrigger> boundryTriggers;
    public int maxPickups;
    public int maxEnemies;

    public Spawner EnemySpawner;

    public List<GameObject> enemies;

    public Spawner PickupSpawner;

    public List<GameObject> supplies;
    
    public List<Transform> attachPoints;    

    public bool intersected = false;

    private bool checkedIntersections = false;
    private System.Random rand = new System.Random();

    public void Update()
    {
        if (!gameObject.isStatic)
        {
            //Debug.Log("Checking intersections.");
            CheckIntersection();
        }
    }
    /// <summary>
    /// Takes in two ints to populate the room with enemies and pickups
    /// </summary>
    /// <param name="e">The number of enemies.</param>
    /// <param name="p">The number of pickups</param>
    public void PopulateRoom(ref int e, ref int p)
    {
        int tempEnemies = rand.Next(0, maxEnemies);
        int tempPickups = rand.Next(0, maxPickups);

        //Populate the spawn points with pickups and add it to the supplies list.
        for(int i = 0; i < tempPickups; i++)
        {
            if(PickupSpawner != null)
            {
                PickupSpawner.SpawnObjs(supplies);
            }
            
        }

        // Populate Spawn points with enemies and add them to the enemy list.
        for(int j = 0; j < tempEnemies; j++)
        {
            if(EnemySpawner != null)
            {
                EnemySpawner.SpawnObjs(enemies);
            }
            
        }

        e -= tempEnemies;
        p -= tempPickups;
    }

    void CheckIntersection()
    {
        foreach (BoundryTrigger bt in boundryTriggers)
        {
            if (bt.RoomIntersect)
            {
                intersected = true;   
            }
        }
    }

    public bool IsOpen()
    {
        foreach(Transform t in attachPoints)
        {
            if (t)
            {
                if (!t.GetComponent<AttachPoint>().Occupied)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void ResetIntersected()
    {
        intersected = false;
    }

    public bool Intersected
    {
        get { return intersected; }
    }

}
