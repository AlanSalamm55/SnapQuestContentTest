using System;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] int maxBounces = 5;
    [SerializeField] float maxDistance = 300f;
    [SerializeField] Transform startPoint;
    [SerializeField] bool reflectOnlyMirror = false;

    [SerializeField] string finalTag = "Receiver";
    public event Action OnLaserHitTarget;
    private bool calledOnce = false;
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
        lr.positionCount = maxBounces + 1;
        lr.SetPosition(0, position);

        for (int i = 0; i < maxBounces; i++)
        {
            if (Physics.Raycast(position, direction, out RaycastHit hit, maxDistance))
            {
                lr.SetPosition(i + 1, hit.point);

                // check for special hit
                if (hit.transform.CompareTag(finalTag))
                {
                    if (calledOnce) return;

                    OnLaserHitTarget?.Invoke(); // trigger event
                    calledOnce = true;
                    break;
                }

                if (reflectOnlyMirror && !hit.transform.CompareTag("Mirror"))
                {
                    for (int j = i + 2; j <= maxBounces; j++)
                        lr.SetPosition(j, hit.point);
                    break;
                }

                // reflect
                position = hit.point;
                direction = Vector3.Reflect(direction, hit.normal);
            }
            else
            {
                Vector3 end = position + direction * maxDistance;
                lr.SetPosition(i + 1, end);
                for (int j = i + 2; j <= maxBounces; j++)
                    lr.SetPosition(j, end);
                break;
            }
        }
    }
}
