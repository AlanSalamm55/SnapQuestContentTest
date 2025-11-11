using UnityEngine;
using DG.Tweening;

public class CaptureEffects : MonoBehaviour
{
    [SerializeField] private NotableObject notable;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material flashMat;
    private Material original;
    private bool isAnimating;

    void Start()
    {
        if (!notable) notable = GetComponent<NotableObject>();
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>();
        original = renderers[0].material;

        notable.onCapture += Flash;
    }

    void Flash()
    {
        if (isAnimating) return; // prevent re-trigger
        isAnimating = true;

        foreach (var r in renderers)
            r.material = flashMat;

        Invoke(nameof(Restore), 0.1f);

        // 10x smaller punch (0.015 instead of 0.15)
        transform.DOPunchScale(Vector3.one * 0.002f, 0.3f)
            .OnComplete(() => isAnimating = false);
    }

    void Restore()
    {
        foreach (var r in renderers)
            r.material = original;
    }
}
