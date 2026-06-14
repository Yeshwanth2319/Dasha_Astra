using UnityEngine;

public class CubePickup : MonoBehaviour
{
    [Header("References")]
    public Transform holdPoint;
    public GameObject pickupText;

    private GameObject nearbyCube;
    private GameObject heldCube;

    void Start()
    {
        if (pickupText != null)
            pickupText.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldCube == null && nearbyCube != null)
            {
                PickUpCube();
            }
            else if (heldCube != null)
            {
                DropCube();
            }
        }

        // Keep cube stuck to holdPoint every frame
        if (heldCube != null)
        {
            heldCube.transform.position = holdPoint.position;
            heldCube.transform.rotation = holdPoint.rotation;
        }
    }

    void PickUpCube()
    {
        heldCube = nearbyCube;
        nearbyCube = null;

        Rigidbody rb = heldCube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Collider col = heldCube.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        heldCube.transform.SetParent(holdPoint);
        heldCube.transform.localPosition = Vector3.zero;
        heldCube.transform.localRotation = Quaternion.identity;

        if (pickupText != null)
            pickupText.SetActive(false);

        Debug.Log("Picked up: " + heldCube.name);
    }

    void DropCube()
    {
        heldCube.transform.SetParent(null);

        Collider col = heldCube.GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        Rigidbody rb = heldCube.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        Debug.Log("Dropped: " + heldCube.name);
        heldCube = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube") && heldCube == null)
        {
            nearbyCube = other.gameObject;

            if (pickupText != null)
                pickupText.SetActive(true);

            Debug.Log("Near cube: " + nearbyCube.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cube") && other.gameObject == nearbyCube)
        {
            nearbyCube = null;

            if (pickupText != null)
                pickupText.SetActive(false);

            Debug.Log("Left cube: " + other.name);
        }
    }
}