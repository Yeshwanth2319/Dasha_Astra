using UnityEngine;
using System.Collections;

public class RhythmPuzzleManager : MonoBehaviour
{
    public static RhythmPuzzleManager instance;

    [Header("Puzzle")]
    public int totalSlots = 5;

    [Header("Wall")]
    public GameObject wall;
    public float wallDownDistance = 5f;
    public float wallSpeed = 2f;

    [Header("Effects")]
    public AudioClip solveSound;

    private int solvedSlots = 0;
    private AudioSource audioSource;

    private Vector3 wallTarget;

    void Awake()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (wall != null)
        {
            wallTarget = wall.transform.position +
                         Vector3.down * wallDownDistance;
        }
    }

    public void SlotSolved()
    {
        solvedSlots++;

        Debug.Log("Solved: " + solvedSlots + " / " + totalSlots);

        if (solvedSlots >= totalSlots)
        {
            PuzzleCompleted();
        }
    }

    void PuzzleCompleted()
    {
        Debug.Log("CHAMBER 2 COMPLETE!");

        // Play sound
        if (solveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(solveSound);
        }

        // Lower wall
        if (wall != null)
        {
            StartCoroutine(LowerWall());
        }
        else
        {
            Debug.LogError("Wall not assigned in RhythmPuzzleManager!");
        }
    }

    IEnumerator LowerWall()
    {
        while (Vector3.Distance(
            wall.transform.position,
            wallTarget) > 0.05f)
        {
            wall.transform.position = Vector3.MoveTowards(
                wall.transform.position,
                wallTarget,
                wallSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}