using UnityEngine;
using DG.Tweening;

public class CaptureEffects : MonoBehaviour
{
    [SerializeField] private NotableObject notable;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material flashMat;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private float punchStrength = 0.015f;

    private Material[][] originalMats;
    private bool isAnimating;

    void Start()
    {
        if (!notable) notable = GetComponent<NotableObject>();
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>();

        // store all original materials
        originalMats = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
            originalMats[i] = renderers[i].materials;

        notable.onCapture += Flash;
    }

    void Flash()
    {
        if (isAnimating) return;
        isAnimating = true;

        foreach (var r in renderers)
        {
            var mats = new Material[r.materials.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = flashMat;
            r.materials = mats;
        }

        Invoke(nameof(Restore), flashDuration);

        transform.DOPunchScale(Vector3.one * punchStrength, 0.3f)
            .OnComplete(() => isAnimating = false);
    }

    void Restore()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].materials = originalMats[i];
    }
}
