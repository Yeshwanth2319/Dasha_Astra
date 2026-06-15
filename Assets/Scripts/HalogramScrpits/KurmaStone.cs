using UnityEngine;
using System.Collections;

public class KurmaStone : MonoBehaviour
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

        unlockText.SetActive(true);

        // Wait 3 seconds
        yield return new WaitForSeconds(3f);

        // Hide completed text
        unlockText.SetActive(false);

        // Now remove the stone
        gameObject.SetActive(false);
    }
}