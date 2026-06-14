using UnityEngine;
using TMPro;
using System.Collections;

public class KurmaPuzzleManager : MonoBehaviour
{
    [Header("Puzzle")]
    public TurtleRotator[] turtles;

    public GameObject puzzleText;
    public GameObject completedText;

    public Transform centerPoint;

    [Header("Door")]
    public Transform door;
    public float doorLowerHeight = 5f;
    public float doorSpeed = 2f;

    [Header("Door Sound")]
    public AudioSource doorAudio;
    public AudioClip doorOpenSound;

    [Header("Turtle Growth")]
    public float growScale = 2f;
    public float growSpeed = 2f;

    int currentSelection = 0;

    bool puzzleMode = false;
    bool puzzleSolved = false;

    Vector3 doorTarget;

    private void Start()
    {
        // Door will move DOWN when puzzle is solved
        doorTarget = door.position + Vector3.down * doorLowerHeight;
    }

    public void EnterPuzzle()
    {
        puzzleMode = true;
        puzzleText.SetActive(true);

        HighlightSelected();
    }

    void Update()
    {
        if (!puzzleMode || puzzleSolved)
            return;

        // Select next turtle
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentSelection++;

            if (currentSelection >= turtles.Length)
                currentSelection = 0;

            HighlightSelected();
        }

        // Rotate selected turtle
        if (Input.GetKeyDown(KeyCode.R))
        {
            turtles[currentSelection].RotateTurtle();

            CheckPuzzle();
        }

        // Exit puzzle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPuzzle();
        }
    }

    void HighlightSelected()
    {
        for (int i = 0; i < turtles.Length; i++)
        {
            Renderer[] renderers =
                turtles[i].GetComponentsInChildren<Renderer>();

            foreach (Renderer r in renderers)
            {
                if (i == currentSelection)
                    r.material.color = new Color(1f, 0.95f, 0.75f);
                else
                    r.material.color = Color.white;
            }
        }
    }

    void CheckPuzzle()
    {
        bool allCorrect = true;

        foreach (TurtleRotator turtle in turtles)
        {
            Vector3 directionToCenter =
                (centerPoint.position - turtle.transform.position).normalized;

            float angle =
                Vector3.Angle(turtle.transform.forward, directionToCenter);

            if (angle > 20f)
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            PuzzleComplete();
        }
    }

    void PuzzleComplete()
    {
        puzzleSolved = true;

        puzzleText.SetActive(false);

        foreach (TurtleRotator turtle in turtles)
        {
            turtle.EnableGlow();
            StartCoroutine(turtle.Grow());
        }

        IEnumerator ShowCompletedText()
        {
            completedText.SetActive(true);

            yield return new WaitForSeconds(3f);

            completedText.SetActive(false);
        }

        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        // Play door sound
        if (doorAudio != null && doorOpenSound != null)
        {
            doorAudio.PlayOneShot(doorOpenSound);
        }

        // Move door downward slowly
        while (Vector3.Distance(door.position, doorTarget) > 0.05f)
        {
            door.position = Vector3.MoveTowards(
                door.position,
                doorTarget,
                doorSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    IEnumerator GrowTurtles()
    {
        Vector3[] targetScales = new Vector3[turtles.Length];

        for (int i = 0; i < turtles.Length; i++)
        {
            targetScales[i] = turtles[i].transform.localScale * growScale;
        }

        bool growing = true;

        while (growing)
        {
            growing = false;

            for (int i = 0; i < turtles.Length; i++)
            {
                turtles[i].transform.localScale =
                    Vector3.Lerp(
                        turtles[i].transform.localScale,
                        targetScales[i],
                        growSpeed * Time.deltaTime);

                if (Vector3.Distance(
                    turtles[i].transform.localScale,
                    targetScales[i]) > 0.01f)
                {
                    growing = true;
                }
            }

            yield return null;
        }
    }

    public void ExitPuzzle()
    {
        puzzleMode = false;
        puzzleText.SetActive(false);
    }
}