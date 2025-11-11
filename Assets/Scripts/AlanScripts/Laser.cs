using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] int maxBounces = 5;
    [SerializeField] float maxDistance = 300f;
    [SerializeField] Transform startPoint;
    [SerializeField] bool reflectOnlyMirror = false;
    [SerializeField] LayerMask hitMask = ~0; // optional: choose what the laser can hit

    LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        CastLaser(startPoint.position, startPoint.forward);
    }

    void CastLaser(Vector3 position, Vector3 direction)
    {
        // ensure enough slots: start + maxBounces points
        lr.positionCount = maxBounces + 1;
        lr.SetPosition(0, position);

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(position, direction, out RaycastHit hit, maxDistance, hitMask))
            {
                lr.SetPosition(i + 1, hit.point);

                // stop if not mirror and we're mirror-only
                if (reflectOnlyMirror && !hit.transform.CompareTag("Mirror"))
                {
                    // fill remaining positions with last hit point
                    for (int j = i + 2; j <= maxBounces; j++)
                        lr.SetPosition(j, hit.point);
                    break;
                }

                // reflect and continue
                position = hit.point;
                direction = Vector3.Reflect(direction, hit.normal);
            }
            else
            {
                // no hit: draw to max distance and fill the rest
                Vector3 end = position + direction * maxDistance;
                lr.SetPosition(i + 1, end);
                for (int j = i + 2; j <= maxBounces; j++)
                    lr.SetPosition(j, end);
                break;
            }
        }
    }
}
