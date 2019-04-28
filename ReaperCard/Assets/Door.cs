using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Quaternion desiredRotation = Quaternion.Euler(0, 0, 0);

    Quaternion doorClosedQuat = Quaternion.Euler(0, 0, 0);
    Quaternion doorOpenQuat = Quaternion.Euler(0, 118, 0);

    // Update is called once per frame
    void Update()
    {
        if(!transform.rotation.Equals(desiredRotation))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime);
        }
    }

    public void OpenDoor(bool bOpen)
    {
        if(bOpen)
        {
            desiredRotation = doorOpenQuat;
        }
        else
        {
            desiredRotation = doorClosedQuat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        OpenDoor(false);
    }
}
