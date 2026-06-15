using System.Collections;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [Header("Doors")]
    public Transform leftDoor;
    public Transform rightDoor;

    public float moveDistance = 5f;
    public float moveSpeed = 2f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip doorOpenSound;

    bool opened = false;

    public void OpenDoors()
    {
        if (opened) return;

        opened = true;

        // Play door sound
        if (audioSource != null && doorOpenSound != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }

        StartCoroutine(OpenRoutine());
    }

    IEnumerator OpenRoutine()
    {
        Vector3 leftStart = leftDoor.position;
        Vector3 rightStart = rightDoor.position;

        Vector3 leftTarget = leftStart + Vector3.left * moveDistance;
        Vector3 rightTarget = rightStart + Vector3.right * moveDistance;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;

            leftDoor.position = Vector3.Lerp(leftStart, leftTarget, t);
            rightDoor.position = Vector3.Lerp(rightStart, rightTarget, t);

            yield return null;
        }

        Destroy(leftDoor.gameObject);
        Destroy(rightDoor.gameObject);
    }
}