using UnityEngine;

public class PipeRotate : MonoBehaviour
{
    [Header("Sound Effects")]
    public AudioClip rotateSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            transform.Rotate(0f, 0f, 90f);

            if (rotateSound != null && audioSource != null)
                audioSource.PlayOneShot(rotateSound);
        }
    }
}