using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


[ExecuteAlways]
public class CinemachinePreview : MonoBehaviour
{
    private void Start()
    {
        if(vc!=null)
        vc.m_Lens.FarClipPlane = 5000;
    }

    public CinemachineVirtualCamera vc;
#if UNITY_EDITOR
    void Update()
    {
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            vc.m_Lens.FarClipPlane = 5000;
            this.enabled = false;
        }
        else if(StaticProperties.DepthPreview)
        {
            if (vc!=null && transform.localPosition.z != -vc.m_Lens.FarClipPlane)
            {
                vc.m_Lens.FarClipPlane = -transform.localPosition.z;
            }
        }
        else
        {
            if (vc.m_Lens.FarClipPlane != 5000)
            {
                vc.m_Lens.FarClipPlane = 5000;
            }
        }
    }
#endif

}
