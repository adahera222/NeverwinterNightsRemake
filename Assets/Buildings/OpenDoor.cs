using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {

    public string levelName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        Application.LoadLevel( levelName );
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
