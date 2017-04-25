using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public EnemyTrigger trigger;
    public EnemyNavigation navi;
    public EnemyStats stats;

    public enum State
    {
        Patrolling,
        Attacking,
        Idling,
        Dead
    };

    private State currentState;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // ITS ALIVE!!!
		if(currentState != State.Dead)
        {
            if (trigger.Triggered)
            {
                currentState = State.Attacking;
            }

            if(stats.Health <= 0)
            {
                Debug.Log("Enemy killed.");
                currentState = State.Dead;
            }

            //navi.NaviUpdate();
        }
        else
        {
            gameObject.SetActive(false);
        }
	}

    public State CurrentState
    {
        get { return currentState; }
    }
}
