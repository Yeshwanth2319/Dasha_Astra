using UnityEngine;
using TMPro;

public class DivineCube : MonoBehaviour
{
    [Header("Settings")]
    public bool isBlessedCube = false;

    [Header("References")]
    public Transform holdPoint;

    [Header("UI Text")]
    public GameObject pickupText;  // "Press E to Pickup"
    public GameObject holdText;    // "Press F to Fit  |  Press G to Drop"

    private bool isHeld = false;
    private bool isPlaced = false;
    private bool playerNearby = false;
    private Rigidbody rb;
    private Collider physicsCollider;
    private Collider triggerCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Collider[] cols = GetComponents<Collider>();
        foreach (Collider col in cols)
        {
            if (col.isTrigger)
                triggerCollider = col;
            else
                physicsCollider = col;
        }

        HideAllText();
    }

    void HideAllText()
    {
        if (pickupText != null) pickupText.SetActive(false);
        if (holdText != null) holdText.SetActive(false);
    }

    void Update()
    {
        // E — Pickup when player nearby and cube not held
        if (playerNearby && !isHeld)
        {
            if (isPlaced && isBlessedCube)
            {
                // Blessed cube locked
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                PickUp();
            }
        }

        if (isHeld)
        {
            // Lock to holdPoint every frame
            transform.position = holdPoint.position;
            transform.rotation = holdPoint.rotation;

            // Make sure holdText stays visible while held
            if (holdText != null && !holdText.activeSelf)
                holdText.SetActive(true);

            // G — Drop
            if (Input.GetKeyDown(KeyCode.G))
                Drop();

            // F — Place in slot
            if (Input.GetKeyDown(KeyCode.F))
                TryPlace();
        }
    }

    // ------------------- PICKUP -------------------

    void PickUp()
    {
        if (isPlaced)
        {
            WaterSlot currentSlot = GetCurrentSlot();
            if (currentSlot != null)
                currentSlot.FreeSlot();
        }

        isHeld = true;
        isPlaced = false;
        playerNearby = false; // Will trigger OnTriggerExit but we handle below

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
        }

        if (physicsCollider != null)
            physicsCollider.enabled = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Show holdText — AFTER parenting so OnTriggerExit doesnt hide it
        if (pickupText != null) pickupText.SetActive(false);
        if (holdText != null) holdText.SetActive(true);

        Debug.Log("Picked up: " + gameObject.name);
    }

    // ------------------- DROP -------------------

    void Drop()
    {
        isHeld = false;

        transform.SetParent(null);

        if (physicsCollider != null)
            physicsCollider.enabled = true;

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        // Hide hold text show pickup text
        if (holdText != null) holdText.SetActive(false);
        if (pickupText != null) pickupText.SetActive(true);

        Debug.Log("Dropped: " + gameObject.name);
    }

    // ------------------- PLACE -------------------

    void TryPlace()
    {
        WaterSlot[] allSlots = FindObjectsByType<WaterSlot>(FindObjectsSortMode.None);
        WaterSlot nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (WaterSlot slot in allSlots)
        {
            if (slot.IsSolved()) continue;

            float dist = Vector3.Distance(
                holdPoint.position,
                slot.transform.position
            );

            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = slot;
            }
        }

        if (nearest != null && nearestDist <= 3f)
        {
            isHeld = false;
            isPlaced = true;

            transform.SetParent(null);
            transform.position = nearest.transform.position;
            transform.rotation = nearest.transform.rotation;

            if (physicsCollider != null)
                physicsCollider.enabled = true;

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            nearest.TrySolve(this, gameObject);

            HideAllText();

            Debug.Log("Placed: " + gameObject.name);
        }
        else
        {
            Debug.Log("No slot nearby — get closer!");
        }
    }

    // ------------------- GET CURRENT SLOT -------------------

    WaterSlot GetCurrentSlot()
    {
        WaterSlot[] allSlots = FindObjectsByType<WaterSlot>(FindObjectsSortMode.None);

        foreach (WaterSlot slot in allSlots)
        {
            float dist = Vector3.Distance(
                transform.position,
                slot.transform.position
            );

            if (dist < 0.5f)
                return slot;
        }

        return null;
    }

    // ------------------- PLAYER DETECTION -------------------

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            bool canPickup = !isHeld && !(isPlaced && isBlessedCube);

            if (canPickup)
            {
                if (pickupText != null) pickupText.SetActive(true);
                if (holdText != null) holdText.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            // Only hide text if cube is NOT held
            if (!isHeld)
                HideAllText();

            // If held keep holdText visible
            if (isHeld)
            {
                if (pickupText != null) pickupText.SetActive(false);
                if (holdText != null) holdText.SetActive(true);
            }
        }
    }
}