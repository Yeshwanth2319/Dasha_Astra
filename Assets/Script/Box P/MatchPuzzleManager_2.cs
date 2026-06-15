using UnityEngine;
using TMPro;
using System.Collections;

public class MatchPuzzleManager : MonoBehaviour
{
    [Header("Puzzle")]
    public int currentStep = 1;
    private bool puzzleSolved = false;

    [Header("Wall")]
    public Transform wall;
    public float wallDownDistance = 5f;
    public float wallSpeed = 2f;

    [Header("UI")]
    public TextMeshProUGUI progressText;

    [Header("Line")]
    public LineRenderer lineRenderer;
    public Transform[] boxPoints; // Assign 9 points in Inspector

    [Header("Audio")]
    public AudioSource wallAudio;
    public AudioClip wallDownSound;

    private Vector3 wallTarget;

    void Start()
    {
        progressText.text = "Selected: 0/9";

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;

            // Cyan line color
            lineRenderer.startColor = Color.cyan;
            lineRenderer.endColor = Color.cyan;
        }

        if (wall != null)
        {
            wallTarget = wall.position + Vector3.down * wallDownDistance;
        }
    }

    public void CheckBox(int boxNumber)
    {
        if (puzzleSolved)
            return;

        if (boxNumber == currentStep)
        {
            currentStep++;

            progressText.text = "Selected: " + (currentStep - 1) + "/9";

            DrawLine();
        }
        else
        {
            currentStep = 1;

            progressText.text = "Wrong Order! 0/9";

            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 0;
            }
        }

        if (currentStep > 9)
        {
            puzzleSolved = true;

            progressText.text = "Puzzle Complete!";

            StartCoroutine(LowerWall());
            StartCoroutine(HideProgressText());
        }
    }

    void DrawLine()
    {
        int pointsToShow = currentStep - 1;

        if (lineRenderer == null)
            return;

        lineRenderer.positionCount = pointsToShow;

        for (int i = 0; i < pointsToShow; i++)
        {
            lineRenderer.SetPosition(i, boxPoints[i].position);
        }
    }

    IEnumerator LowerWall()
    {
        // Play wall sound
        if (wallAudio != null && wallDownSound != null)
        {
            wallAudio.PlayOneShot(wallDownSound);
        }

        while (Vector3.Distance(wall.position, wallTarget) > 0.05f)
        {
            wall.position = Vector3.MoveTowards(
                wall.position,
                wallTarget,
                wallSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    IEnumerator HideProgressText()
    {
        yield return new WaitForSeconds(5f);

        progressText.gameObject.SetActive(false);
    }
}