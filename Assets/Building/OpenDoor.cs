using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseEnter()
    {
        Debug.Log( "enter" );
        renderer.material.color = new Color( 0.0f, 0.6f, 1.0f, 1.0f );
    }

    public void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
}
