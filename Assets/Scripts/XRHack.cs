using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// Forces RoomScale XR Mode - known bug in Unity defaults back to stationary in various circumstances when loading
public class XRHack : MonoBehaviour
{
    private bool _isRoomScale = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetXRModeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator SetXRModeRoutine() {
        for (int i = 0; i < 10; i++)
        {
            yield return true;
            _isRoomScale =  XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
        }
    }
}
