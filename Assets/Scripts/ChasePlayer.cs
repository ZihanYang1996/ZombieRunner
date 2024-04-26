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

    [SerializeField] float chaseTimeAfterLosesTarget = 5f;
    float squareRadius;
    float squareDistance;
    float squareStoppingDistance;
    [SerializeField] private float turnSpeed = 10f;

    bool isProvoked = false;
    Coroutine keepChasingCoroutine;


    NavMeshAgent agent;
    Animator animator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        squareRadius = Radius * Radius;
        squareStoppingDistance = agent.stoppingDistance * agent.stoppingDistance;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isProvoked)
        {
            EngaeTarget();
        }
        else
        {
            CalculateDistance();
            if (squareDistance <= squareRadius)
            {
                Debug.Log("Provoked");
                isProvoked = true;
            }
        }


        // if (squareDistance <= squareRadius)
        // {
        //     
        //     agent.SetDestination(player.position);
        // }
        // else
        // {
        //     agent.isStopped = true;
        //     // or agent.SetDestination(transform.position);
        // }
    }

    void EngaeTarget()
    {
        CalculateDistance();
        if (squareDistance >= squareStoppingDistance)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isMoving", true);
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }
    }

    private void AttackTarget()
    {
        // Make sure enemy always face the player

        // transform.LookAt(player);  // Also rotate on the x and z axis

        // transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));  // Only rotate on the y-axis

        // More customized way, only rotate on the y-axis
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
        // transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * m_turnSpeed);  // Smooth rotation, not necessary


        // Sets the "isAttacking" parameter in the animator to true.
        // This triggers the attack animation in the game object.
        animator.SetBool("isAttacking", true);
    }

    void ChaseTarget()
    {
        if (squareDistance <= squareRadius)
        {
            // Check if the coroutine is null, if not stop the coroutine
            // Note that the coroutine stopped either by itself or by the StopCoroutine method won't be null
            if (keepChasingCoroutine != null)
            {
                Debug.Log("Restart chasing");
                StopCoroutine(keepChasingCoroutine);
                keepChasingCoroutine = null; // Reset the coroutine to null
            }

            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            if (keepChasingCoroutine == null)
            {
                // get the last known position of the player
                Vector3 lastKnownPosition = player.position;
                keepChasingCoroutine = StartCoroutine(KeepChasing(lastKnownPosition, chaseTimeAfterLosesTarget));
            }
        }
    }

    // Keep moving towards the last known position of the player
    IEnumerator KeepChasing(Vector3 lastKnownPosition, float chaseTimeAfterLosesTarget)
    {
        // Simple but less control over the behavior
        // Debug.Log("Target lost, moving to the last known position");
        // agent.SetDestination(lastKnownPosition);
        // // float timeElapsed = 0;
        // yield return new WaitForSeconds(chaseTimeAfterLosesTarget);
        // Debug.Log("Stopped chasing");
        // agent.isStopped = true;

        // More control over the behavior
        Debug.Log("Target lost, moving to the last known position");
        agent.SetDestination(lastKnownPosition);
        float timeElapsed = 0;
        while (timeElapsed < chaseTimeAfterLosesTarget)
        {
            timeElapsed += Time.deltaTime;
            // Can be used to add some behavior while chasing
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Stopped chasing");
        Debug.Log("No longer provoked");
        agent.isStopped = true;
        isProvoked = false;
        animator.SetBool("isMoving", false);
    }

    void CalculateDistance()
    {
        squareDistance = (player.position - transform.position).sqrMagnitude;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}