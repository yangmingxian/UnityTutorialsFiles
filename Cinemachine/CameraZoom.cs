using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
// using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{

    [SerializeField] private CinemachineFreeLook freeLookCamera;

    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomAcceleration = 2f;
    [SerializeField] private float zoomInnerRange = 3f;
    [SerializeField] private float zoomOutterRange = 15f;

    private float currentMiddleRigRadius;
    private float newMiddleRigRadius;

    [SerializeField] private float zoomYAxis = 0f;
    // private PlayerControls inputActions;
    private void Start()
    {
        // inputActions = new PlayerControls();
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        currentMiddleRigRadius = freeLookCamera.m_Orbits[1].m_Radius;
        // zoomInnerRange = Mathf.Min(freeLookCamera.m_Orbits[0].m_Radius, freeLookCamera.m_Orbits[2].m_Radius);
    }

    private void FixedUpdate()
    {
        // zoomYAxis = inputActions.Camera.Zoom.ReadValue<float>();
        // inputActions.Camera.Zoom.performed += inputActions => zoomYAxis = inputActions.ReadValue<float>();
        zoomYAxis = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
        adjustCameraZoomIndex(zoomYAxis);
        UpdateZoomLevel();
    }

    private void UpdateZoomLevel()
    {
        if (currentMiddleRigRadius == newMiddleRigRadius)
            return;

        currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius, zoomAcceleration * Time.deltaTime);
        currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRange, zoomOutterRange);

        freeLookCamera.m_Orbits[1].m_Radius = currentMiddleRigRadius;
        freeLookCamera.m_Orbits[0].m_Height = freeLookCamera.m_Orbits[1].m_Radius;
        freeLookCamera.m_Orbits[2].m_Height = -freeLookCamera.m_Orbits[1].m_Radius;
    }

    public void adjustCameraZoomIndex(float zoomYAxis)
    {
        if (zoomYAxis == 0) { return; }
        else if (zoomYAxis < 0)
        {
            newMiddleRigRadius = currentMiddleRigRadius + zoomSpeed;
        }
        else if (zoomYAxis > 0)
        {
            newMiddleRigRadius = currentMiddleRigRadius - zoomSpeed;
        }
    }

}
