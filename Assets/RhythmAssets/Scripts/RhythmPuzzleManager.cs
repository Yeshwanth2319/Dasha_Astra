using UnityEngine;
using System.Collections;

public class RhythmPuzzleManager : MonoBehaviour
{
    public static RhythmPuzzleManager instance;

    public int totalSlots = 5;

    [Header("Wall Settings")]
    public GameObject wall;
    public float moveDistance = 5f;   // How far the wall moves down
    public float moveSpeed = 2f;      // Movement speed

    [Header("Effects")]
    public AudioClip solveSound;

    private int solvedSlots = 0;
    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void SlotSolved()
    {
        solvedSlots++;

        Debug.Log($"Solved: {solvedSlots} / {totalSlots}");

        if (solvedSlots >= totalSlots)
            PuzzleCompleted();
    }

    void PuzzleCompleted()
    {
        Debug.Log("CHAMBER 2 COMPLETE!");

        if (wall != null)
        {
            StartCoroutine(MoveWallDown());
        }
        else
        {
            Debug.LogError("Wall not assigned in RhythmPuzzleManager!");
        }
    }

    IEnumerator MoveWallDown()
    {
        // Play sound
        if (solveSound != null && audioSource != null)
            audioSource.PlayOneShot(solveSound);

        Vector3 startPos = wall.transform.position;
        Vector3 targetPos = startPos + Vector3.down * moveDistance;

        while (Vector3.Distance(wall.transform.position, targetPos) > 0.01f)
        {
            wall.transform.position = Vector3.MoveTowards(
                wall.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        wall.transform.position = targetPos;

        // Optional: disable wall after moving
        wall.SetActive(false);

        Debug.Log("Wall moved down!");
    }
}