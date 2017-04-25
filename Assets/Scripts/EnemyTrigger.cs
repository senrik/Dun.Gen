using UnityEngine;
using System.Collections;

public class EnemyTrigger : MonoBehaviour
{

    private bool triggered;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Enemy Triggered.");
            triggered = true;
        }
    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if(triggered)
            {
                Debug.Log("Enemy Untriggered.");
                triggered = false;
            }
        }

    }

    public bool Triggered
    {
        get { return triggered; }
    }
}
