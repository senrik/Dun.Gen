using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPoint : MonoBehaviour {

    private bool occupied;

    public List<GameObject> doorwayPanels;

    public void OpenDoor()
    {
        for(int i = 0; i < doorwayPanels.Count; i++)
        {
            if (doorwayPanels[i])
            {
                if (doorwayPanels[i].activeSelf)
                {
                    doorwayPanels[i].SetActive(false);
                }
            }
        }
    }

    public bool Occupied
    {
        get { return occupied; }
        set { occupied = value; }
    }
}
