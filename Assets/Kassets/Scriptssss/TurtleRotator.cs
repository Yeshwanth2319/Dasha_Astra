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

    [Header("Wall Sound")]
    public AudioSource wallAudio;
    public AudioClip wallDownSound;

    private void Awake()
    {
        rends = GetComponentsInChildren<Renderer>();
    }

    public void RotateTurtle()
    {
        transform.Rotate(0f, 90f, 0f);

        if (wallAudio != null && wallDownSound != null)
            wallAudio.PlayOneShot(wallDownSound);
    }

    public void EnableGlow()
    {
        foreach (Renderer r in rends)
        {
            // Loop all materials on each renderer
            Material[] mats = r.materials; // Instance materials for build

            foreach (Material mat in mats)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor(
                    "_EmissionColor",
                    glowColor * glowIntensity
                );
            }
        }

        // Force update emission in build
        DynamicGI.UpdateEnvironment();
    }

    public void DisableGlow()
    {
        foreach (Renderer r in rends)
        {
            Material[] mats = r.materials;

            foreach (Material mat in mats)
            {
                mat.DisableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.black);
            }
        }

        DynamicGI.UpdateEnvironment();
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