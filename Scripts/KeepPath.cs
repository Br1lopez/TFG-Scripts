using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class KeepPath : MonoBehaviour
{
    CinemachineSmoothPath path;
    public static CinemachineSmoothPath.Waypoint[] _TPwaypoints;

    // Start is called before the first frame update
    void Start()
    {
        path = this.gameObject.GetComponent<CinemachineSmoothPath>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        path.m_Waypoints = _TPwaypoints;
        print(_TPwaypoints.Length);
    }
}
