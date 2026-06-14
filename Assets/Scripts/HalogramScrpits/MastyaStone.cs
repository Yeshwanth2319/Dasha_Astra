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

        unlockText.SetActive(true);

        gameObject.SetActive(false);

        yield return new WaitForSeconds(5f);

        unlockText.SetActive(false);
    }
}