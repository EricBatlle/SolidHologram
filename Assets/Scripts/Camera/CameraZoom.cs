using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : VCManager {

    [SerializeField] protected Cinemachine.CinemachineVirtualCamera vcMain;

    public override void CameraStartAction()
    {
        base.CameraStartAction();
        vc.Follow = vcMain.Follow;
    }
    public override void CameraEndAction()
    {
        base.CameraEndAction();
        vc.Follow = null;
    }

}
