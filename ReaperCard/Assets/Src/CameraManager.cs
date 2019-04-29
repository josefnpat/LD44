using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
  [SerializeField]
  public GameObject FocusObject; // Additional actor to focus on when the player is conversing with them

  [SerializeField]
  public GameObject WalkingCamera;
  [SerializeField]
  public GameObject ItemCamera;
  [SerializeField]
  public GameObject DialogueCamera;

  private CinemachineVirtualCamera[] CameraArray = new CinemachineVirtualCamera[EActorState.GetNames(typeof(EActorState)).Length];
  public void Awake()
  {
    CameraArray[(int)EActorState.Walking] = WalkingCamera.GetComponent<CinemachineVirtualCamera>();
    CameraArray[(int)EActorState.ReceivingItem] = ItemCamera.GetComponent<CinemachineVirtualCamera>();
    CameraArray[(int)EActorState.InConversation] = DialogueCamera.GetComponent<CinemachineVirtualCamera>();
  }

  public void onStartDialog(GameObject player, GameObject npc)
  {
    if (npc) {
      FocusObject.GetComponent<CinemachineTargetGroup>().m_Targets[1].target = npc.transform;
    }

    var DialogCamera = CameraArray[(int)EActorState.InConversation];
    DialogCamera.Priority = 15;

    //DialogCamera.m_LookAt = FocusObject.transform;
  }

  public void onEndDialog()
  {
    var DialogCamera = CameraArray[(int)EActorState.InConversation];
    DialogCamera.Priority = 9;
  }

  // void Update()
  // {
    // EActorState newState = PlayerActor.GetState();
    // if (newState != activeCamera)
    // {
       // Switch camera to the new one
    // }
  // }
}
