using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class DoorOpenOnCapture : MonoBehaviour
{
    [SerializeField] private NotableObject target;
    [SerializeField] private Transform door;
    [SerializeField] private Vector3 openOffset = new Vector3(0, 3, 0);
    [SerializeField] private float openTime = 0.4f;


    [Header("Cinemachine Shake)")]
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private CinemachineVirtualCamera vcam2;
    [SerializeField] private float shakeAmp = 1.2f;
    [SerializeField] private float shakeFreq = 2f;
    [SerializeField] private float shakeTime = 0.25f;

    private bool opened;


    [Header("Sound")]
    [SerializeField] private AudioSource openSound;


    void Start()
    {
        if (!door) door = transform;
        if (target) target.onCapture += Open;
    }


    void Open()
    {
        if (opened) return;
        opened = true;
        Debug.Log("jj");

        // open door
        door.DOLocalMove(door.localPosition + openOffset, openTime).SetEase(Ease.OutCubic);
        openSound.Play();

        if (vcam)
        {
            var noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            var noise2 = vcam2.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise)
            {
                noise.m_AmplitudeGain = shakeAmp;
                noise.m_FrequencyGain = shakeFreq;
                Invoke(nameof(StopShake), shakeTime);
            }
            if (noise2)
            {
                noise2.m_AmplitudeGain = shakeAmp;
                noise2.m_FrequencyGain = shakeFreq;
                Invoke(nameof(StopShake), shakeTime);
            }
        }
    }

    void StopShake()
    {
        var n1 = vcam ? vcam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>() : null;
        if (n1)
        {
            n1.m_AmplitudeGain = 0f;
            n1.m_FrequencyGain = 0f;
        }

        var n2 = vcam2 ? vcam2.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>() : null;
        if (n2)
        {
            n2.m_AmplitudeGain = 0f;
            n2.m_FrequencyGain = 0f;
        }
    }
}
