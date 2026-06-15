using UnityEngine;
using System.Collections;

public class MatsyaStone : MonoBehaviour
{
    public GameObject pressEText;
    public GameObject unlockText_1;
    public GameObject unlockText_2;

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

        unlockText_1.SetActive(true);
        unlockText_2.SetActive(false);

        gameObject.SetActive(false);

        yield return new WaitForSeconds(5f);

        unlockText_1.SetActive(false);
        unlockText_2.SetActive(false);
    }
}