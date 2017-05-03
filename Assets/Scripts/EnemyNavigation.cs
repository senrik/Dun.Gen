using UnityEngine;
using System.Collections;

public class EnemyNavigation : MonoBehaviour {

    public GameObject waypointPrefab;
    public PatrolPath patrolPath;
    public EnemyController controller;
    public float effectiveRange = 2.0f;

    private UnityEngine.AI.NavMeshAgent agent;
    private Transform playerTransform;
    private int currentWaypoint;
	// Use this for initialization
	void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        currentWaypoint = 0;
        agent.autoBraking = false;
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //MoveToNextWaypoint();
	}

    #region Private Methods
    void DrawPath()
    {
        for(int i = 0; i < patrolPath.waypoints.Count; i++)
        {
            if(i + 1 < patrolPath.waypoints.Count)
            {
                Debug.DrawLine(patrolPath.waypoints[i].transform.position, patrolPath.waypoints[i + 1].transform.position);
            }
            else
            {
                Debug.DrawLine(patrolPath.waypoints[i].transform.position, patrolPath.waypoints[0].transform.position);
            }
        }
    }

    void MoveToNextWaypoint()
    {
        //Debug.Log("Incrementing current waypoint.");
        if(currentWaypoint < patrolPath.waypoints.Count)
        {
            //Debug.Log("Setting destination to next waypoint.");
            agent.destination = patrolPath.waypoints[currentWaypoint].transform.position;
        }
        else
        {
            //Debug.Log("Resetting current waypoint.");
            currentWaypoint = 0;
            //Debug.Log("Setting destination to first waypoint.");
            agent.destination = patrolPath.waypoints[currentWaypoint].transform.position;
        }
    }

    void ApproachPlayer()
    {
        // If the player is outside our effect range, move toward the player
        if(Vector3.Distance(transform.position, playerTransform.position) > effectiveRange)
        {
            //agent.Resume();
            //agent.destination = playerTransform.position;
        }
        // The player is within our effective range, stop moving.
        else
        {
            //agent.Stop();
        }
    }
#endregion

    // Update is called once per frame
    public void NaviUpdate () {
        //DrawPath();


        switch (controller.CurrentState)
        {
            case EnemyController.State.Attacking:
                ApproachPlayer();
                break;
            case EnemyController.State.Patrolling:
                if (agent.remainingDistance < 0.5f)
                {
                    currentWaypoint++;
                    //Debug.Log("Reached current waypoint.");
                    //MoveToNextWaypoint();
                }
                break;
            case EnemyController.State.Idling:
                break;
            case EnemyController.State.Dead:
                //agent.Stop();
                break;
        }
        
	}
}
