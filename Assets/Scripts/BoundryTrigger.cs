using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundryTrigger : MonoBehaviour {

    private bool roomIntersect = false;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boundry")
        {
            if (!roomIntersect)
            {
                //Debug.Log("Room intersection.");
                roomIntersect = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Boundry")
        {
            if (!roomIntersect)
            {
                //Debug.Log("Room intersecting.");
                roomIntersect = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Boundry")
        {
            if (roomIntersect)
            {
                //Debug.Log("Intersection resolved.");
                roomIntersect = false;
            }
        }
    }

    public void ResetIntersect()
    {
        roomIntersect = false;
    }

    public bool RoomIntersect
    {
        get { return roomIntersect; }
    }
}
