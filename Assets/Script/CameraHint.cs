using UnityEngine;
using TMPro;
using System.Collections;

public class CameraHint : MonoBehaviour
{
    public GameObject hintUI;
    private bool hasUsedV = false;

    void Start()
    {
        StartCoroutine(ShowHintRoutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            hasUsedV = true;
            hintUI.SetActive(false);
        }
    }

    IEnumerator ShowHintRoutine()
    {
        while (!hasUsedV)
        {
            yield return new WaitForSeconds(60f);

            if (!hasUsedV)
            {
                hintUI.SetActive(true);

                yield return new WaitForSeconds(5f);

                hintUI.SetActive(false);
            }
        }
    }
}