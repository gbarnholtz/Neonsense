using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PatrolFollow : MonoBehaviour
{

    GameObject[] waypoint;
    int rand;
    GameObject waypointSelected;
    UnityEngine.AI.NavMeshAgent agent;
    private Transform target;
    [SerializeField] private int distanceToChasePlayer;
    [SerializeField] private int stoppingDistance;

    NavMeshAgent navMeshAgent;
    
    void OnDrawGizmosSelected()
    {
        // Display the chase radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToChasePlayer);
    }

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //ToWaypoint();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void ToWaypoint()
    {
        waypoint = GameObject.FindGameObjectsWithTag("waypoint");
        rand = Random.Range(0, waypoint.Length);
        waypointSelected = waypoint[rand];
    }

    // Update is called once per frame
    void Update()
    {
        /* If player is within range, chase player */
        if (ChasePlayer())
        {
            /* If stopping distance is not reached, chase player */
            if (!StoppingDistanceReached())
            {
                agent.SetDestination(target.transform.position);
                //ToWaypoint();
            } else
            {
                /* Else set destination to current position */
                agent.SetDestination(transform.position);
            }
        }
    }

    bool ChasePlayer()
    {
        return Vector3.Distance(agent.transform.position, target.transform.position) < distanceToChasePlayer;
    }

    bool StoppingDistanceReached()
    {
        return Vector3.Distance(agent.transform.position, target.transform.position) < stoppingDistance;
    }
}
