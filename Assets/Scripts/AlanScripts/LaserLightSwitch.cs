using UnityEngine;

public class LaserLightSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] toShow;
    [SerializeField] private Laser laser;
    [SerializeField] private AudioSource audioSource;
    void Awake()
    {
        // hide them at start 
        foreach (GameObject object_ in toShow) if (object_) object_.SetActive(false);
    }

    void Start() { laser.OnLaserHitTarget += Show; }

    void Show()
    {
        foreach (var go in toShow)
        {
            if (go)
            {
                go.SetActive(true);
            }
        }
        audioSource.Play();

    }

}
