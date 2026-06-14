using UnityEngine;

public class RhythmPuzzleManager : MonoBehaviour
{
    public static RhythmPuzzleManager instance;

    public int totalSlots = 5;
    public GameObject wall; // Assign your Wall GameObject in Inspector

    [Header("Effects")]
    public AudioClip solveSound;   // Optional sound when wall disappears
     // Optional particle effect

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

        // Disappear the wall
        if (wall != null)
        {
            wall.SetActive(false);
            Debug.Log("Wall removed!");
        }
        else
        {
            Debug.LogError("Wall not assigned in RhythmPuzzleManager!");
        }

        // Play sound if assigned
        if (solveSound != null && audioSource != null)
            audioSource.PlayOneShot(solveSound);

        
    }
}