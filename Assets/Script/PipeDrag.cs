using NUnit.Framework;
using UnityEngine;

public class PipeDrag : MonoBehaviour
{
    private Camera puzzleCamera;
    private bool isDragging = false;
    private bool isPlaced = false;
    public bool isSelected = false;

    private Slot currentSlot;
    private PuzzleManager puzzleManager;
    private float dragHeight;

    [Header("Sound Effects")]
    public AudioClip pickupSound;
    public AudioClip placeSound;
    public AudioClip removeSound;
    public AudioClip errorSound;

    private AudioSource audioSource;

    void Start()
    {
        puzzleManager = FindObjectOfType<PuzzleManager>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        dragHeight = transform.position.y;
    }

    public void SetCamera(Camera cam)
    {
        puzzleCamera = cam;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    void OnMouseDown()
    {
        if (puzzleCamera == null)
            return;

        isSelected = true;

        if (!isPlaced)
        {
            isDragging = true;
            dragHeight = transform.position.y;

            PlaySound(pickupSound);
        }
    }

    void OnMouseUp()
    {
        if (!isDragging)
            return;

        isDragging = false;

        if (!TryPlacePipe())
        {
            PlaySound(errorSound);
        }
    }

    void Update()
    {
        if (puzzleCamera == null)
            return;

        // Drag pipe
        if (isDragging)
        {
            Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);
            Plane dragPlane = new Plane(Vector3.up, new Vector3(0, dragHeight, 0));

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 worldPoint = ray.GetPoint(enter);

                transform.position = new Vector3(
                    worldPoint.x,
                    dragHeight,
                    worldPoint.z
                );
            }
        }

        // Select pipe


        // Remove from slot with Space
        if (Input.GetKeyDown(KeyCode.Space) && isSelected && currentSlot != null)
        {
            RemoveFromSlot();

            isPlaced = false;
            isDragging = true;

            // Lift slightly above slot
            transform.position += Vector3.up * 0.2f;
            dragHeight = transform.position.y;
        }
    }

    bool TryPlacePipe()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.3f);

        Slot closestSlot = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Slot slot = hit.GetComponent<Slot>();

            if (slot != null && !slot.occupied)
            {
                float distance = Vector3.Distance(
                    transform.position,
                    slot.transform.position
                );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSlot = slot;
                }
            }
        }

        if (closestSlot != null)
        {
            transform.position = closestSlot.transform.position;

            closestSlot.occupied = true;
            currentSlot = closestSlot;
            isPlaced = true;
            isSelected = true;

            PlaySound(placeSound);
            Debug.Log("Pipe placed in closest slot");

            puzzleManager?.CheckPuzzleSolved();
            return true;
        }

        return false;
    }

    void RemoveFromSlot()
    {
        if (currentSlot != null)
        {
            currentSlot.occupied = false;

            Debug.Log("Pipe removed from: " + currentSlot.name);

            currentSlot = null;

            PlaySound(removeSound);
        }
    }


}