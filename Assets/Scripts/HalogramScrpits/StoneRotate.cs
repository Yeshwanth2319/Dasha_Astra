using UnityEngine;

public class StoneRotate : MonoBehaviour
{
    public float rotationSpeed = 80f;

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}