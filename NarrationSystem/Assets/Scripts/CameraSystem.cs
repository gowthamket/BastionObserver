using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _zoomCam;
    [SerializeField] bool _isActive;
    [SerializeField] bool _isPPActive;

    public void ZoomInCamera() {
        StartCoroutine(IZoomInCameraRoutine());
    }

    IEnumerator IZoomInCameraRoutine()
    {
        if (_isActive) {
            _zoomCam.Priority = 100;
            yield return new WaitForSeconds(2.5f);
            _zoomCam.Priority = 0;
        }
    }
}
