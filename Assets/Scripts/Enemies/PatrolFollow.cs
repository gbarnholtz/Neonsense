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

    NavMeshAgent navMeshAgent;
    
    void OnDrawGizmosSelected()
    {
        // Display the chase radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToChasePlayer);
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        ToWaypoint();
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
        if (Vector3.Distance(agent.transform.position, target.transform.position) < distanceToChasePlayer)
        {
            agent.SetDestination(target.transform.position);
            ToWaypoint();
        }

        else
        {
            if (Vector3.Distance(agent.transform.position, waypointSelected.transform.position) >= 2)
            {
                // pursue 'waypointSelected'
                //agent.SetDestination(waypointSelected.transform.position);
            }

            else
            {
                // if the distance is too small, find a new 'waypointSelected'
                ToWaypoint();
            }
        }
    }
}
