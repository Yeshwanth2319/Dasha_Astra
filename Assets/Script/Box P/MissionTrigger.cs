using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public GameObject pressEText;
    public GameObject missionText;

    private bool playerInside = false;
    private bool missionStarted = false;

    void Start()
    {
        pressEText.SetActive(false);
        missionText.SetActive(false);
    }

    void Update()
    {
        if (playerInside && !missionStarted && Input.GetKeyDown(KeyCode.E))
        {
            missionStarted = true;

            pressEText.SetActive(false);
            missionText.SetActive(true);

            Debug.Log("Mission Started!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !missionStarted)
        {
            playerInside = true;
            pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            pressEText.SetActive(false);
        }
    }
}