using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 *    GameInstance
 *  Contains runtime references and critical game framework behaviour.
 *
 *  Important functions:
 *        PlayerActor GetPlayerActor()
 *        PlayerController GetPlayerController()
 *
 *
 */

public static class Game
{
    public static GameInstance GInstance = null;
}

public class GameInstance : MonoBehaviour
{
    private static string EndSessionScene = "TempReset";

    [Header("Prefab Config")]
    [SerializeField]
    public GameObject PlayerPrefab;
    [SerializeField]
    public GameObject ControllerPrefab;
    [SerializeField]
    public GameObject CameraPrefab;
    //[Space(20)]

    private Actor PlayerActor;
    private PlayerController Controller;
    private CameraManager Cameras;

    public GameState gameState = new GameState();

    public Vector3 StartLocation;

    public EFacingDirection StartDirection
    {
        get;

        set;
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Check to see if we're already started a session (this allows every room to have an object for designers to start from any room)
        if(Game.GInstance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Initializing Game...", this);

            StartSession();

            Debug.Log("Game initialized successfully.", this);
        }
    }

    // Called when the scene is loaded
    void OnSceneLoaded(Scene InScene, LoadSceneMode Mode)
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerActor(Actor player)
    {
        if(Controller)
        {
            Controller.PlayerActor = player;
        }
        if(Cameras)
        {
            Cameras.setPlayerActor(player);
        }
        PlayerActor = player;
    }

    public Actor GetPlayerActor()
    {
        return PlayerActor;
    }

    public void SetPlayerController(PlayerController pc)
    {
        if (PlayerActor)
        {
            pc.PlayerActor = PlayerActor;
        }
        Controller = pc;
    }

    public PlayerController GetPlayerController()
    {
        return Controller;
    }

    public void SetCameraManager(CameraManager cm)
    {
        if (PlayerActor)
        {
            cm.setPlayerActor(PlayerActor);
        }
        Cameras = cm;
    }

    public CameraManager GetCameraManager()
    {
        return Cameras;
    }

    public void SpawnPlayer()
    {
        Quaternion StartRot = Quaternion.Euler(0f, 0f, 0f);

        GameObject Obj1 = Instantiate(ControllerPrefab) as GameObject;
        GameObject Obj2 = Instantiate(CameraPrefab) as GameObject;
        GameObject Obj3 = Instantiate(PlayerPrefab, StartLocation, StartRot) as GameObject;
  
        //Controller = Obj2.gameObject.GetComponent<PlayerController>();
        //PlayerActor = Obj.GetComponent<Actor>();// as Actor;

        //Controller.PlayerActor = PlayerActor;
    }

    public void SpawnInventoryItems() {

    }

    public void StartSession()
    {
        // Game instance persists for the entire session
        DontDestroyOnLoad(this.gameObject);
        Game.GInstance = this;

        // Bind events
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Spawn intial player for now
        SpawnPlayer();
    }

    public void EndSession()
    {
        // Safe to destroy this on load now
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
        Game.GInstance = null;
        SceneManager.LoadScene(EndSessionScene);
        //move to title scene?
    }

    public void ChangeScene(string SceneName)
    {
        Debug.Log(string.Format("Loading scene {0}", SceneName), this);
        SceneManager.LoadScene(SceneName);
    }
}
