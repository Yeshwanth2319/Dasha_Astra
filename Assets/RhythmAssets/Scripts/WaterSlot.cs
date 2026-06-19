using UnityEngine;

public class WaterSlot : MonoBehaviour
{
    private bool solved = false;       // Permanently solved by blessed cube
    private bool occupied = false;     // Temporarily occupied by normal cube

    public bool IsSolved()
    {
        return solved; // Only blessed cube makes it permanently solved
    }

    public bool IsOccupied()
    {
        return solved || occupied;
    }

    public void TrySolve(DivineCube cube, GameObject cubeObj)
    {
        if (solved) return;

        if (cube.isBlessedCube)
        {
            // Blessed cube — permanently solve slot
            solved = true;
            occupied = true;

            Renderer rend = cubeObj.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.EnableKeyword("_EMISSION");
                rend.material.SetColor(
                    "_EmissionColor",
                    Color.cyan * 5f
                );
            }

            Debug.Log("Slot SOLVED by blessed cube: " + gameObject.name);
            RhythmPuzzleManager.instance.SlotSolved();
        }
        else
        {
            // Normal cube — just occupy slot temporarily
            occupied = true;

            Renderer rend = cubeObj.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.EnableKeyword("_EMISSION");
                rend.material.SetColor(
                    "_EmissionColor",
                    Color.yellow * 2f // Yellow glow for normal cube
                );
            }

            Debug.Log("Slot occupied by normal cube: " + gameObject.name);
        }
    }

    void SetEmission(GameObject obj, Color emissionColor)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend == null) return;

        // Loop all materials on the object
        Material[] mats = rend.materials;

        foreach (Material mat in mats)
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", emissionColor);
        }

        DynamicGI.UpdateEnvironment();
    }
    // Called when normal cube is removed from slot
    public void FreeSlot()
    {
        if (solved) return; // Blessed cube slot cannot be freed

        occupied = false;

        Debug.Log("Slot freed: " + gameObject.name);
    }

    void OnDrawGizmos()
    {
        // Green = free, Yellow = normal cube, Red = blessed/solved
        if (solved)
            Gizmos.color = Color.red;
        else if (occupied)
            Gizmos.color = Color.yellow;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.6f);
    }
}