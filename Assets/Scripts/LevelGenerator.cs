using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour {

    public Camera mainCam;
    public float length, width, buildStep;
    public List<GameObject> level;
    public List<GameObject> roomPrefabs;
    private bool levelBuilt = false;
    private bool building = false;
    private Vector3 camPosition;
    private float camSmoothing;
    private int regress = 1;
    private int enemyPool;
    private int pickupPool;
    private NavMeshSurface navMesh;
	// Use this for initialization
	void Start () {
        navMesh = GetComponent<NavMeshSurface>();
        if (GameObject.FindGameObjectWithTag("GameController"))
        {
            StartBuild(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().LevelSize);
        }
        else
        {
            StartBuild((int)length);
        }
        camSmoothing = 1.5f;
        regress = 1;
        enemyPool = 200;
        pickupPool = 20;
    }
	
    public void StartBuild(int size)
    {
        length = size;
        Debug.Log("Length of level: " + length);
        building = true;
        StartCoroutine(BuildLevel());
    }

    IEnumerator BuildLevel()
    {
        Debug.Log("Building new level.");
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < length; i++)
        {
            // Select a random prefabricated room
            //Debug.Log("Generating new room.");
            GameObject tempRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)]);
            Transform point = null;
            // There are other rooms in the collection
            if (level.Count > 0)
            {
                if (level[level.Count - regress].GetComponent<Room>().IsOpen())
                {
                    //Debug.Log("The last room on the level has open attach points.");
                }
                else
                {
                    while (!level[level.Count - regress].GetComponent<Room>().IsOpen())
                    {
                        regress++;
                    }
                    //Debug.Log("The last room on the level does not have open attach points.");
                }
                // Transform of the attach point
                point = level[level.Count - regress].GetComponent<Room>().attachPoints[Random.Range(0, level[level.Count - regress].GetComponent<Room>().attachPoints.Count)].transform;

                // Point selected is occupied, try again
                while (point.GetComponent<AttachPoint>().Occupied)
                {
                    // Prevents from selecting itself again
                    if (point != level[level.Count - regress].GetComponent<Room>().attachPoints[Random.Range(0, level[level.Count - regress].GetComponent<Room>().attachPoints.Count)].transform)
                    {
                        point = level[level.Count - regress].GetComponent<Room>().attachPoints[Random.Range(0, level[level.Count - regress].GetComponent<Room>().attachPoints.Count)].transform;
                    }

                }
                // Mark the point as occupied

                // Move the new room to an attach point of the previous room
                //Debug.Log("Placing the room at the attach point.");
                tempRoom.transform.position = point.position;
                //yield return new WaitForSeconds(0.5f);
                // Rotate the room so its forward vector is the point's rotation
                //Debug.Log("Rotating the room to match the attach point's rotation.");
                tempRoom.transform.rotation = point.rotation;
                //yield return new WaitForSeconds(0.5f);
                // Offset the room to line up the attach points
                //Debug.Log("Offsetting the new room.");
                OffsetRoom(tempRoom);

            }

            yield return new WaitForSeconds(buildStep);

            if (tempRoom.GetComponent<Room>().Intersected)
            {
                //Debug.Log("New room is intersecting.");
                // Marking this point as occupied means no new rooms will be place here.
                point.GetComponent<AttachPoint>().Occupied = true;
                Destroy(tempRoom);
                tempRoom = null;
                i--;
            }
            else
            {
                //Debug.Log("No intersection.");
            }
            // Add new room to the level
            if (tempRoom)
            {
                if (point)
                {
                    point.GetComponent<AttachPoint>().Occupied = true;
                    point.GetComponent<AttachPoint>().OpenDoor();
                }
                tempRoom.isStatic = true;
                level.Add(tempRoom);
                if (regress > 1)
                {
                    regress = 1;
                }
            }
        }
        GenNevMesh();
        Debug.Log("Populating new level.");
        for (int i = 0; i < level.Count; i++)
        {
            level[i].GetComponent<Room>().PopulateRoom(ref enemyPool, ref pickupPool);
        }
        levelBuilt = true;
        building = false;
        Debug.Log("Level built.");
        yield return null;
    }

    /// <summary>
    /// r is the new room being added to the level.
    /// </summary>
    /// <param name="r"></param>
    void OffsetRoom(GameObject r)
    {
        for(int i = 0; i < r.GetComponent<Room>().attachPoints.Count; i++)
        {
            if(r.GetComponent<Room>().attachPoints[i].localPosition.z < 0)
            {
                Transform tempPoint = r.GetComponent<Room>().attachPoints[i].transform;

                // Move the room along its forward vector by the z value of the offset point
                r.transform.position += r.transform.forward * Mathf.Abs(tempPoint.localPosition.z);
                // Move the room along its x vector by the x value of the offset point;
                r.transform.position -= r.transform.right * tempPoint.localPosition.x;
                // Mark the point as occupied
                //Debug.Log("Offset Vector: (" + tempPoint.localPosition.x + ", 0, " + Mathf.Abs(tempPoint.localPosition.z) + ")");
                tempPoint.GetComponent<AttachPoint>().Occupied = true;
                tempPoint.GetComponent<AttachPoint>().OpenDoor();
            }
        }
    }

    void GenNevMesh()
    {
        navMesh.Bake();
    }
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        if(level.Count > 0)
        {
            if (mainCam)
            {
                camPosition.x = level[level.Count - 1].transform.position.x;
                camPosition.y = mainCam.transform.position.y;
                camPosition.z = level[level.Count - 1].transform.position.z;
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPosition, Time.deltaTime * camSmoothing);
            }
        }

        if (Input.GetKeyUp(KeyCode.Backslash))
        {
            if (!building)
            {
                mainCam.transform.SetPositionAndRotation(new Vector3(0, 200, 0), mainCam.transform.rotation);
                if (level.Count > 0)
                {
                    for (int i = 0; i < level.Count; i++)
                    {
                        Destroy(level[i]);
                    }
                    level.Clear();
                }
                StartBuild((int)Random.Range(10, 30));
            }
            
        }
#endif
    }

    public bool LevelBuilt
    {
        get { return levelBuilt; }
    }

    public bool Building
    {
        get { return true; }
    }
}
