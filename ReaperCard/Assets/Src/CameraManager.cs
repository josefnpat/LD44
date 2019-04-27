using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject ItemCamera;
    public GameObject DialogueCamera;

    private CinemachineVirtualCamera[] CameraArray = new CinemachineVirtualCamera[EActorState.GetNames(typeof(EActorState)).Length];

    private Actor PlayerActor;

    public void setPlayerActor(Actor player)
    {
        PlayerActor = player;
        Transform playerTransform = player.GetTransform();

        CinemachineVirtualCamera vcam;
 
        vcam = MainCamera.GetComponent<CinemachineVirtualCamera>();
        if (vcam)
        {
            CameraArray[(int)EActorState.Walking] = vcam;
        }

        vcam = ItemCamera.GetComponent<CinemachineVirtualCamera>();
        if (vcam)
        {
            CameraArray[(int)EActorState.ReceivingItem] = vcam;
        }

        vcam = DialogueCamera.GetComponent<CinemachineVirtualCamera>();
        if (vcam)
        {
            CameraArray[(int)EActorState.InConversation] = vcam;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Game.GInstance.SetCameraManager(this);
    }

    void SwitchToCamera(EActorState state)
    {

    }
}
