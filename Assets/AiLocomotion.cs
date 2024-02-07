using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AiStates
{
    partrol,
    chase,
    idle,
}
public class AiLocomotion : MonoBehaviour
{
    NavMeshAgent agent;
    public AiStates defaultState = AiStates.idle;
    AiStates currentState;
    GameObject spriteObj;

    [Header("Sight Setting")]
    public float sightDistance = 5f;
    public float fov;

    [Header("Line of Sight")]
    [SerializeField] private FieldOfView fieldOfView;

    [Header("Partrol Settings")]
    public List<Transform> partrolPoints = new List<Transform>();
    public float partrolSpeed = 1.0f;
    int pointIndex = 0;

    [Header("Chase Settings")]
    public Transform target;
    public float chaseSpeed = 1.2f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        if (partrolPoints.Count > 0)
        {
            defaultState = AiStates.partrol;
        }

        currentState = defaultState;
        spriteObj = GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    private void Update()
    {
        UpdateRotation();
        switch (currentState)
        {
            case AiStates.partrol:
                PartrolUpdate();
                break;
            case AiStates.chase:
                ChaseUpdate();
                break;
        }
    }

    void PartrolUpdate()
    {
        fieldOfView.SetOrigin(spriteObj.transform.position);
        fieldOfView.SetAimDirection(agent.desiredVelocity.normalized);
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(sightDistance);
        agent.speed = partrolSpeed;
        //Check Sight
        if (SeeTarget())
        {
            currentState = AiStates.chase;
            return;
        }

        //Check if path finished
        if (!agent.hasPath)
        {
            pointIndex = pointIndex == partrolPoints.Count - 1 ? 0 : pointIndex + 1;
        }

        agent.SetDestination(partrolPoints[pointIndex].position);
    }

    void ChaseUpdate()
    {
        agent.speed = chaseSpeed;
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    bool SeeTarget()
    {
        if (!target)
            return false;
        Vector3 dir = (target.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir);
        if (Vector2.Angle(transform.right, dir) > fov/2f)
        {
            return false;
        }

        if (hit.collider != null)
        {
            if (hit.collider.transform == target.transform)
            {
                return true;
            }
        }

        return false;
    }

    void UpdateRotation()
    {
        Vector3 moveDirection = agent.desiredVelocity.normalized;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
