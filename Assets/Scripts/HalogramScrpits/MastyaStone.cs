using UnityEngine;
using System.Collections;

public class MatsyaStone : MonoBehaviour
{
    public GameObject pressEText;
    public GameObject unlockText;

    bool playerNear = false;
    bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            pressEText.SetActive(false);
        }
    }

    void Update()
    {
        if (playerNear &&
            !collected &&
            Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(CollectStone());
        }
    }

    IEnumerator CollectStone()
    {
        collected = true;

        pressEText.SetActive(false);

        // Show unlock text
        unlockText.SetActive(true);

        // Keep it visible for 3 seconds
        yield return new WaitForSeconds(3f);

        unlockText.SetActive(false);

        // Disable the stone
        gameObject.SetActive(false);
    }
}