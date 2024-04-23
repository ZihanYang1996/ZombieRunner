using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    [SerializeField] Transform player;

    [Tooltip("The square of the distance within which the enemy will chase the player")]
    public float Radius = 10f;
    float squareRadius;
    float squareDistance;

    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        squareRadius = Radius * Radius;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDistance();
        if (squareDistance <= squareRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            // or agent.SetDestination(transform.position);
        }
    }

    void CalculateDistance()
    {
        squareDistance = (player.position - transform.position).sqrMagnitude;
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }


}
