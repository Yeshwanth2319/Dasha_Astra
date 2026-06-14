using UnityEngine;

public class FishHologram : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public float floatHeight = 0.2f;
    public float floatSpeed = 2f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(
            0,
            rotationSpeed * Time.deltaTime,
            0);

        transform.position =
            startPos +
            Vector3.up *
            Mathf.Sin(Time.time * floatSpeed)
            * floatHeight;
    }
}