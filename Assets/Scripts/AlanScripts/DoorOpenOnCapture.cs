using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class DoorOpenOnCapture : MonoBehaviour
{
    [SerializeField] NotableObject target;          // the notable to listen to
    [SerializeField] Transform door;                // defaults to this transform
    [SerializeField] Vector3 openOffset = new Vector3(0, 3, 0);
    [SerializeField] float openTime = 0.4f;

    [Header("Cinemachine Shake (optional)")]
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] CinemachineVirtualCamera vcam2;
    [SerializeField] float shakeAmp = 1.2f;
    [SerializeField] float shakeFreq = 2f;
    [SerializeField] float shakeTime = 0.25f;

    bool opened;

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

        // shake camera (if assigned & has noise)
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
