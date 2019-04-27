using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 *	GameInstance
 *  Contains runtime references and critical game framework behaviour.
 * 
 *  Important functions:
 *		PlayerActor GetPlayerActor()
 *		PlayerController GetPlayerController()
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
	public GameObject PlayerPrefab;
	public GameObject ControllerPrefab;
	//[Space(20)]

	private Actor PlayerActor;
	private PlayerController Controller;


    // Start is called before the first frame update
    void Start()
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

	Actor GetPlayerActor()
	{
		return PlayerActor;
	}

	PlayerController GetPlayerController()
	{
		return Controller;
	}

	public void SpawnPlayer()
	{
		GameObject Obj = Instantiate(ControllerPrefab);
		PlayerActor = Obj.GetComponent<Actor>();

		Quaternion StartRot = Quaternion.Euler(0f, 0f, 0f);

		Obj = Instantiate(PlayerPrefab, StartLocation, StartRot);
		Controller = Obj.GetComponent<PlayerController>();
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

	public Vector3 StartLocation
	{
		get;

		set;
	}

	public EFacingDirection StartDirection
	{
		get;

		set;
	}
}
