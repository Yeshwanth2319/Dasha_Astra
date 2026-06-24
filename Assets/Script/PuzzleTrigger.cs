using UnityEngine;
using TMPro;

public class PuzzleTrigger : MonoBehaviour
{
    [Header("Cameras")]
    public Camera playerCamera;
    public Camera puzzleCamera;

    [Header("Player")]
    public MonoBehaviour playerMovement;
    public ThirdPersonCamera thirdPersonCamera;

    [Header("UI")]
    public TextMeshProUGUI pressEText;

    [Header("Pipe detection")]
    public LayerMask pipeLayerMask;
    private bool playerInside = false;
    private bool puzzleOpen = false;
    PipeDrag pipeDrag;
    void Start()
    {
        puzzleCamera.enabled = false;
        playerCamera.enabled = true;

        if (pressEText != null)
            pressEText.gameObject.SetActive(false);

        // Assign puzzle camera to all pipes in the scene
        AssignCameraToPipes();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (pressEText != null)
                pressEText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (pressEText != null)
                pressEText.gameObject.SetActive(false);

            if (puzzleOpen)
                ClosePuzzle();
        }
    }

    void Update()
    {
        if (playerInside && !puzzleOpen && Input.GetKeyDown(KeyCode.E))
            OpenPuzzle();

        // Close puzzle with ESC
        if (puzzleOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle();
        }

    
    Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, pipeLayerMask))
        {
            if (pipeDrag==null)
            {
                pipeDrag = hit.transform.GetComponent<PipeDrag>();
                pipeDrag.isSelected = true;
            }
           
            
        }
        else
        {
            if (pipeDrag != null)
            {
                pipeDrag.isSelected = false;
                pipeDrag = null;
            }
        }
        
    }

    void AssignCameraToPipes()
    {
        // Find ALL pipes in the scene since there's no puzzle board parent
        PipeDrag[] pipes = FindObjectsOfType<PipeDrag>();
        Debug.Log($"Found {pipes.Length} pipes.");
        foreach (PipeDrag pipe in pipes)
            pipe.SetCamera(puzzleCamera);
    }

    void OpenPuzzle()
    {
        puzzleOpen = true;

        if (playerMovement != null) playerMovement.enabled = false;
        if (thirdPersonCamera != null) thirdPersonCamera.enabled = false;

        playerCamera.enabled = false;
        puzzleCamera.enabled = true;

        if (pressEText != null)
            pressEText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void BackButton()
    {
        ClosePuzzle();
    }
    public void ClosePuzzle()
    {
        puzzleOpen = false;

        if (playerMovement != null) playerMovement.enabled = true;
        if (thirdPersonCamera != null) thirdPersonCamera.enabled = true;

        // Close puzzle with ESC
       

        playerCamera.enabled = true;
        puzzleCamera.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerInside && pressEText != null)
            pressEText.gameObject.SetActive(true);
    }
}