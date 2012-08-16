using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

    static public string levelName = "Begin Village Scene";

	// Use this for initialization
	void Awake ()
    {
        Application.LoadLevelAdditive( levelName );
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ( !Application.isLoadingLevel )
        {
            Destroy( gameObject );
        }
	}
}
