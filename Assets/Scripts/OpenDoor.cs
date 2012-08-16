using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {

    public string levelName;
    public string loadingLevelName = "Loading Scene";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        LoadLevel.levelName = levelName;
        Application.LoadLevel( loadingLevelName );
    }

    void OnMouseEnter()
    {
        renderer.material.color = new Color( 0.0f, 0.6f, 1.0f, 1.0f );
    }

    void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
}
