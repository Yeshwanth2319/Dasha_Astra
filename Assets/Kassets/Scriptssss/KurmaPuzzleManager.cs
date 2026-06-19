using System.Collections;
using UnityEngine;

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

    [Header("Highlight Settings")]
    public Color selectedColor = new Color(1f, 0.95f, 0.75f, 1f);
    [Range(0f, 1f)] public float opacity = 1f;

    [Header("State")]
    public bool puzzleSolved = false;

    int currentSelection = 0;
    bool puzzleMode = false;

    Vector3 doorTarget;

    void Start()
    {
        doorTarget = door.position + Vector3.down * doorLowerHeight;
    }

    public void EnterPuzzle()
    {
        if (puzzleSolved) return;

        puzzleMode = true;
        puzzleText.SetActive(true);
        HighlightSelected();
    }

    void Update()
    {
        if (!puzzleMode || puzzleSolved)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            currentSelection = (currentSelection + 1) % turtles.Length;
            HighlightSelected();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            turtles[currentSelection].RotateTurtle();
            CheckPuzzle();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPuzzle();
        }
    }

    void HighlightSelected()
    {
        for (int i = 0; i < turtles.Length; i++)
        {
            Renderer[] renderers = turtles[i].GetComponentsInChildren<Renderer>();

            foreach (Renderer r in renderers)
            {
                Color c = (i == currentSelection) ? selectedColor : Color.white;
                c.a = opacity;
                r.material.color = c;
            }
        }
    }

    void CheckPuzzle()
    {
        bool allCorrect = true;

        foreach (TurtleRotator turtle in turtles)
        {
            Vector3 dir = centerPoint.position - turtle.transform.position;
            dir.y = 0; // ignore height

            Vector3 forward = turtle.transform.forward;
            forward.y = 0;

            float dot = Vector3.Dot(forward.normalized, dir.normalized);

            // more stable than angle
            if (dot < 0.7f)   // adjust sensitivity
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            Debug.Log("PUZZLE SOLVED!");
            PuzzleComplete();
        }
    }

    void PuzzleComplete()
    {
        puzzleSolved = true;
        puzzleMode = false;

        puzzleText.SetActive(false);

        if (completedText != null)
            StartCoroutine(ShowCompleted());

        foreach (TurtleRotator turtle in turtles)
        {
            turtle.EnableGlow();
            StartCoroutine(turtle.Grow());
        }

        StartCoroutine(OpenDoor());
    }

    IEnumerator ShowCompleted()
    {
        completedText.SetActive(true);
        yield return new WaitForSeconds(3f);
        completedText.SetActive(false);
    }

    IEnumerator OpenDoor()
    {
        if (doorAudio != null && doorOpenSound != null)
            doorAudio.PlayOneShot(doorOpenSound);

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

    public void ExitPuzzle()
    {
        puzzleMode = false;

        if (puzzleText != null)
            puzzleText.SetActive(false);
    }
}