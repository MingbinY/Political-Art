using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winScreen;
    public GameObject loseScreen;

    int targetBench = 0;
    int currentBench = 0;

    public bool startChasing = false;

    public float loseDistance = 0.2f;

    [Header("Cop Settings")]
    public float patrolSpeed = 1.0f;
    public float defaultSpeed = 1.0f;
    public float increaseRate = 1.0f;
    public float maxSpeed = 1.8f;

    private void Awake()
    {
        Instance = this;
        targetBench = FindObjectsOfType<Bench>().Length;
    }

    public void FixedBench()
    {
        currentBench++;

        if (currentBench >= targetBench)
        {
            Win();
            return;
        }
        AudioManager.Instance.FixBenchClip();

        if (currentBench == targetBench - 3)
        {
            MusicManager.Instance.Ending();
        }

        if (currentBench == targetBench - 1) 
        { 
            startChasing = true; 
        }

        if (patrolSpeed <= 1.8f)
            patrolSpeed += increaseRate;
        if (patrolSpeed > 1.8f) patrolSpeed = 1.8f;
    }

    public void Lose()
    {
        // Turn off AI
        AiLocomotion[] aiLocomotions = FindObjectsOfType<AiLocomotion>();
        foreach (AiLocomotion ai in aiLocomotions)
        {
            ai.enabled = false;
            ai.GetComponent<NavMeshAgent>().enabled = false;
        }
        // Disable player control
        FindObjectOfType<PlayerLocomotion>().enabled = false;
        loseScreen.SetActive(true);
        AudioManager.Instance.LoseClip();
    }

    public void Win()
    {
        // Turn off AI
        AiLocomotion[] aiLocomotions = FindObjectsOfType<AiLocomotion>();
        foreach (AiLocomotion ai in aiLocomotions)
        {
            ai.enabled = false;
            ai.GetComponent<NavMeshAgent>().enabled = false;
        }
        // Disable player control
        FindObjectOfType<PlayerLocomotion>().enabled = false;
        winScreen.SetActive(true);
        AudioManager.Instance.WinClip();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
