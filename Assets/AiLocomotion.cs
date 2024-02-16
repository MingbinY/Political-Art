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
    public LayerMask sightLayer;
    public LayerMask fovObstacleLayer;
    NavMeshAgent agent;
    public AiStates defaultState = AiStates.idle;
    [SerializeField] AiStates currentState;
    GameObject spriteObj;

    [Header("Sight Setting")]
    public float sightDistance = 5f;
    public float fov;
    bool seeTarget = false;

    [Header("Line of Sight")]
    public FieldOfView fovPrefab;
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
        agent.angularSpeed = 0;
        target = FindObjectOfType<PlayerLocomotion>().transform;
        if (partrolPoints.Count > 0)
        {
            defaultState = AiStates.partrol;
        }

        fieldOfView = Instantiate(fovPrefab).GetComponent<FieldOfView>();
        fieldOfView.layerMask = fovObstacleLayer;
        currentState = defaultState;
        spriteObj = GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    private void Update()
    {
        //UpdateRotation();
        UpdateViewCone();
        if (!seeTarget)
            seeTarget = SeeTarget();
        switch (currentState)
        {
            case AiStates.partrol:
                PartrolUpdate();
                break;
            case AiStates.chase:
                ChaseUpdate();
                break;
            case AiStates.idle:
                IdleUpdate();
                break;
        }
    }

    void UpdateViewCone()
    {
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(agent.desiredVelocity.normalized);
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(sightDistance);
    }

    void IdleUpdate()
    {
        if (SeeTarget())
        {
            currentState = AiStates.chase;
        }
    }

    void PartrolUpdate()
    {
        agent.speed = partrolSpeed;
        //Check Sight
        if (seeTarget)
        {
            currentState = AiStates.chase;
            AudioManager.Instance.CopSeenClip();
            MusicManager.Instance.Chasing();
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
            Debug.Log((target.position - transform.position).magnitude);
            if ((target.position - transform.position).magnitude < GameManager.Instance.loseDistance)
            {
                GameManager.Instance.Lose();
                return;
            }
            agent.SetDestination(target.position);
        }
    }

    bool SeeTarget()
    {
        if (Vector3.Distance(GetPosition(), target.position) < sightDistance)
        {
            // Player inside viewDistance
            Vector3 dirToPlayer = (target.position - GetPosition()).normalized;
            if (Vector3.Angle(agent.desiredVelocity.normalized, dirToPlayer) < fov / 2f)
            {
                // Player inside Field of View
                RaycastHit2D raycastHit2D = Physics2D.Raycast(GetPosition(), dirToPlayer, sightDistance, sightLayer);
                if (raycastHit2D.collider != null)
                {
                    // Hit something
                    Debug.Log(raycastHit2D.collider.gameObject);
                    if (raycastHit2D.collider.gameObject.GetComponent<PlayerLocomotion>() != null)
                    {
                        // Hit Player
                        return true;
                    }
                    else
                    {
                        // Hit something else
                    }
                }
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
    public Vector3 GetPosition()
     {
         return transform.position;
      }
}
