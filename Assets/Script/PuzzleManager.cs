using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Slot[] slots;
    public GameObject puzzleSolvedPopup;
    public int puzzleID;

    private bool solved = false;
    private void Update()
    {
       
    }
    public void CheckPuzzleSolved()
    {
        if (solved) return;

        foreach (Slot slot in slots)
            if (!slot.occupied) return;

        solved = true;

        if (puzzleSolvedPopup != null)
            puzzleSolvedPopup.SetActive(true);

        Debug.Log("Puzzle Solved!");
        Invoke(nameof(ClosePuzzle), 2f);
    }

    void ClosePuzzle()
    {
        if (puzzleSolvedPopup != null)
            puzzleSolvedPopup.SetActive(false);

        if (puzzleID == 1)
            GameProgress.Instance.puzzle1Solved = true;
        else if (puzzleID == 2)
            GameProgress.Instance.puzzle2Solved = true;
    }
}