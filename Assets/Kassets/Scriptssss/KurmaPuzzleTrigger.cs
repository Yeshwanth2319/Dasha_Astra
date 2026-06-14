using UnityEngine;

public class KurmaPuzzleTrigger : MonoBehaviour
{
    public GameObject interactText;
    public KurmaPuzzleManager puzzleManager;

    bool playerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            interactText.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            interactText.SetActive(false);
            puzzleManager.EnterPuzzle();
        }
    }
}