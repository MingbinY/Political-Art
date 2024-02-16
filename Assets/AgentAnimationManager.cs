using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentAnimationManager : MonoBehaviour
{
    public GameObject rightSprite, leftSprite, upSprite, downSprite;
    GameObject currentSpriteGO;
    NavMeshAgent navmeshAgent;
    Vector2 currentVector = Vector2.zero;

    [SerializeField]
    Animator currentAnimator;

    private void Awake()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        currentSpriteGO = downSprite;
        currentAnimator = currentSpriteGO.GetComponent<Animator>();
    }

    private void Update()
    {
        GetCurrentVector();
        HandleAnimation();
    }

    void GetCurrentVector()
    {
        Vector2 desiredVector = navmeshAgent.desiredVelocity.normalized;

        float dotProduct = Vector2.Dot(Vector2.up, desiredVector);
        Vector2 newVector;
        if (dotProduct > 0.707f) // up
        {
            newVector = Vector2.up;
        } else if (dotProduct > -0.707f) // left or right
        {
            if (Vector2.Dot(Vector2.right, desiredVector) > 0) //right
            {
                newVector = Vector2.right;
            }
            else
            {
                newVector = -Vector2.right;
            }
        }
        else // down
        {
            newVector = -Vector2.up;
        }

        if (newVector != currentVector)
        {
            if (currentSpriteGO != null)
                currentSpriteGO.SetActive(false);
            if (newVector == Vector2.up)
            {
                currentSpriteGO = upSprite;
            }else if (newVector == Vector2.right)
            {
                currentSpriteGO = rightSprite;
            }else if (newVector == -Vector2.up)
            {
                currentSpriteGO = downSprite;
            }else if (newVector == -Vector2.right)
            {
                currentSpriteGO = leftSprite;
            }
            currentSpriteGO.SetActive(true);
            currentVector = newVector;
            currentAnimator = currentSpriteGO.GetComponent<Animator>();
        }
    }

    void HandleAnimation()
    {
        if (currentAnimator)
            currentAnimator.SetFloat("Speed", navmeshAgent.speed);
    }
}
