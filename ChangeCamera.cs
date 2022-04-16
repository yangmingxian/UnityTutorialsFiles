using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using UnityEngine.Rendering;


public class ChangeCamera : MonoBehaviour
{
    [SerializeField] Transform[] headParts;
    public ShadowCastingMode showShadowCastingMode = (ShadowCastingMode)ShadowCastingMode.On;
    public ShadowCastingMode hideShadowCastingMode = (ShadowCastingMode)ShadowCastingMode.ShadowsOnly;

    [SerializeField] GameObject thirdRoot;
    [SerializeField] GameObject firstRoot;

    ThirdPersonController thirdPersonController;
    [SerializeField] CinemachineVirtualCamera firstPersonCam;
    [SerializeField] CinemachineVirtualCamera thirdPersonCam;
    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
    }
    private void OnEnable()
    {
        CameraSwitcher.Register(thirdPersonCam);
        CameraSwitcher.Register(firstPersonCam);
        CameraSwitcher.SwitchCamera(thirdPersonCam);
        thirdPersonController.CinemachineCameraTarget = thirdRoot;

    }
    private void OnDisable()
    {
        CameraSwitcher.Unregister(thirdPersonCam);
        CameraSwitcher.Unregister(firstPersonCam);
    }
    public void HideHeadPart()
    {
        foreach (Transform part in headParts)
        {
            Transform[] parts = part.GetComponentsInChildren<Transform>();
            // wait 2s
            foreach (Transform p in parts)
            {
                if (p.gameObject.GetComponent<SkinnedMeshRenderer>())
                {
                    p.gameObject.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = hideShadowCastingMode;

                }
            }
        }
    }
    public void UnhideHeadPart()
    {
        foreach (Transform part in headParts)
        {
            Transform[] parts = part.GetComponentsInChildren<Transform>();
            // wait 2s
            foreach (Transform p in parts)
            {
                if (p.gameObject.GetComponent<SkinnedMeshRenderer>())
                {
                    p.gameObject.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = showShadowCastingMode;


                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (CameraSwitcher.IsActiveCamera(thirdPersonCam))
            {
                print("firstPersonCam now");
                thirdPersonController.cameraType = 1;
                thirdPersonController.CinemachineCameraTarget = firstRoot;
                HideHeadPart();
                CameraSwitcher.SwitchCamera(firstPersonCam);
            }
            else if (CameraSwitcher.IsActiveCamera(firstPersonCam))
            {
                print("thirdPersonCam now");
                thirdPersonController.cameraType = 3;
                thirdPersonController.CinemachineCameraTarget = thirdRoot;
                UnhideHeadPart();
                CameraSwitcher.SwitchCamera(thirdPersonCam);
            }

        }

    }
}
