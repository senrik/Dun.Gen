using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject objPrefab;
    public List<Transform> spawnPoints;

    private int spawnNumber;
    /// <summary>
    /// Spawns a prefab GameObject at a random spawn point and returns it.
    /// </summary>
    /// <returns></returns>
    public void SpawnObjs(List<GameObject> collection)
    {
        int point = Random.Range(0, spawnPoints.Count);
        GameObject temp = Instantiate(objPrefab, spawnPoints[point]);
        Debug.Log("Spawned new object at point: "+ point);
        collection.Add(temp);
    }
	
}
