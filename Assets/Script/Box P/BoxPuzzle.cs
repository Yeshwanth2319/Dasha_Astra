using UnityEngine;

public class PuzzleBox : MonoBehaviour
{
    public int boxNumber;

    public MatchPuzzleManager manager;

    public GameObject Etext;

    bool playerNear = false;

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            manager.CheckBox(boxNumber);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            Etext.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            Etext.SetActive(false);
        }
    }
}