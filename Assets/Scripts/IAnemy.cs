using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAnemy : MonoBehaviour
{
    enum State
    {
        Patrolling,

        Chasing
    }
    
    State currentState;

    NavMeshAgent enemyAgent;

    Transform playerTransform;

    [SerializeField] Transform patrolAreaCenter;

    [SerializeField] Vector2 patrolAreaSize;

    [SerializeField] float visionRange = 15;
    [SerializeField] float visionAngle = 90;

    void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        switch(currentState)
        {
            case State.Patrolling:
                Patrol();
            break;
            case State.Chasing:
                Chase();
            break;
        }
    }

    void Patrol()
    {
        if(OnRange() == true)
        {
            currentState = State.Chasing;
            Debug.Log("Persiguiendo");
        }
        if(enemyAgent.remainingDistance < 0.5f)
        {
            SetRandomPoint();
        }
    }

    void Chase()
    {
        enemyAgent.destination = playerTransform.position;

        if(OnRange() == false)
        {
            currentState = State.Patrolling;
            Debug.Log("Patrullando");
        }
    }

    void SetRandomPoint()
    {
        float randomX = Random.Range(-patrolAreaSize.x / 2, patrolAreaSize.x / 2);
        float randomZ = Random.Range(-patrolAreaSize.y / 2, patrolAreaSize.y / 2);
        Vector3 randomPoint = new Vector3(randomX, 0f, randomZ) + patrolAreaCenter.position;

        enemyAgent.destination = randomPoint;
    }

    bool OnRange()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector3.Angle(playerTransform.forward, directionToPlayer);

        if(distanceToPlayer <= visionRange && angleToPlayer < visionAngle * 0.5f)
        {
            return true;
        }

        return false;
    }
}
