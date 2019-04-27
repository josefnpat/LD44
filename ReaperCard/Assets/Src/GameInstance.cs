using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Game
{
	public static GameInstance GInstance = null;
}

public class GameInstance : MonoBehaviour
{
	private static string EndSessionScene = "TempReset";

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

    // Update is called once per frame
    void Update()
    {
        
    }

	public void StartSession()
	{
		// Game instance persists for the entire session
		DontDestroyOnLoad(this.gameObject);
		Game.GInstance = this;
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
}
