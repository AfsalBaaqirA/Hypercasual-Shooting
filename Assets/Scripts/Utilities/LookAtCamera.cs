using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private LookAtMode mode;

    private void LateUpdate()
    {
        switch (mode)
        {
            case LookAtMode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case LookAtMode.LookAtInverted:
                transform.LookAt(-Camera.main.transform.position);
                break;
            case LookAtMode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case LookAtMode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}

public enum LookAtMode
{
    LookAt,
    LookAtInverted,
    CameraForward,
    CameraForwardInverted
}
