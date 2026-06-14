using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorOpener doorOpener;

    public GameObject pressEText;
    public GameObject solvePuzzleText;

    bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (GameProgress.Instance.AllPuzzlesSolved())
            {
                pressEText.SetActive(true);
            }
            else
            {
                solvePuzzleText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            pressEText.SetActive(false);
            solvePuzzleText.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInside &&
            Input.GetKeyDown(KeyCode.E) &&
            GameProgress.Instance.AllPuzzlesSolved())
        {
            pressEText.SetActive(false);
            solvePuzzleText.SetActive(false);

            doorOpener.OpenDoors();

            // Disable this trigger so it won't show text again
            gameObject.SetActive(false);
        }
    }
}