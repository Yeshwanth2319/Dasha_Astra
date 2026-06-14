using System.Collections;
using UnityEngine;

public class TurtleRotator : MonoBehaviour
{
    private Renderer[] rends;

    [Header("Glow Settings")]
    public Color glowColor = Color.green;
    public float glowIntensity = 8f;

    [Header("Growth Settings")]
    public float growthMultiplier = 1.5f;
    public float growthDuration = 1f;

    private void Awake()
    {
        rends = GetComponentsInChildren<Renderer>();
    }

    public void RotateTurtle()
    {
        transform.Rotate(0f, 90f, 0f);
    }

    public void EnableGlow()
    {
        foreach (Renderer r in rends)
        {
            Material mat = r.material;

            mat.EnableKeyword("_EMISSION");

            mat.SetColor(
                "_EmissionColor",
                glowColor * glowIntensity
            );
        }
    }

    public IEnumerator Grow()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = startScale * growthMultiplier;

        float timer = 0f;

        while (timer < growthDuration)
        {
            timer += Time.deltaTime;

            transform.localScale = Vector3.Lerp(
                startScale,
                targetScale,
                timer / growthDuration
            );

            yield return null;
        }

        transform.localScale = targetScale;
    }
}