using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimationManager : MonoBehaviour
{
    public GameObject rightSprite, leftSprite, upSprite, downSprite;
    GameObject currentSpriteGO;
    Rigidbody2D rb;
    public Vector2 currentVector = Vector2.zero;
    Animator currentAnimator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpriteGO = downSprite;
        currentVector = Vector2.down;
        currentAnimator = currentSpriteGO.GetComponent<Animator>();
    }

    private void Update()
    {
        GetCurrentVector();
        HandleAnimation();
    }

    void GetCurrentVector()
    {
        if (currentVector.x == currentVector.y) return;
        GameObject newDir = currentSpriteGO;
        if (currentVector.x == 0) // Up Down
        {
            if (currentVector.y > 0)
            {
                newDir = upSprite;
            }
            else
            {
                newDir = downSprite;
            }
        }
        else // Left Right
        {
            if (currentVector.x > 0)
            {
                newDir = rightSprite;
            }
            else
            {
                newDir = leftSprite;
            }
        }

        if (newDir != currentSpriteGO)
        {
            currentSpriteGO.SetActive(false);
            newDir.SetActive(true);
            currentSpriteGO=newDir;
            currentAnimator = currentSpriteGO.GetComponent<Animator>();
        }
    }

    void HandleAnimation()
    {
        currentAnimator.SetFloat("Speed", Mathf.Abs(currentVector.magnitude));
    }
}
