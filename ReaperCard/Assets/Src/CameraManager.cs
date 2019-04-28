using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
  [SerializeField]
  public GameObject Player;
  private Actor PlayerActor;
  public GameObject FocusObject; // Additional actor to focus on when the player is conversing with them

  [SerializeField]
  public GameObject WalkingCamera;
  [SerializeField]
  public GameObject ItemCamera;
  [SerializeField]
  public GameObject DialogueCamera;

  private CinemachineVirtualCamera[] CameraArray = new CinemachineVirtualCamera[EActorState.GetNames(typeof(EActorState)).Length];
  private EActorState activeCamera = EActorState.Walking;

  public void Start()
  {
    PlayerActor = Player.GetComponent<Actor>();
    activeCamera = PlayerActor.GetState();
    
    CinemachineVirtualCamera vcam;
    vcam = WalkingCamera.GetComponent<CinemachineVirtualCamera>();
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

  void Update()
  {
    EActorState newState = PlayerActor.GetState();
    if (newState != activeCamera)
    {
      //Switch camera to the new one
    }
  }
}
