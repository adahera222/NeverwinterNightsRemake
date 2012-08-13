using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    public GameObject character;
    public float rotationSpeed = 180.0f;
    private Vector3 cameraOffset = new Vector3( 0.0f, 10.0f, -10.0f );

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ( Input.mousePosition.x == 0 )
        {
            float angle = - rotationSpeed * Time.deltaTime;
            Quaternion rot = Quaternion.AngleAxis( angle, Vector3.up );
            cameraOffset = rot * cameraOffset;
        }
        else if ( Input.mousePosition.x == Screen.width - 1 )
        {
            float angle = rotationSpeed * Time.deltaTime;
            Quaternion rot = Quaternion.AngleAxis( angle, Vector3.up );
            cameraOffset = rot * cameraOffset;
        }
        transform.position = character.transform.position + cameraOffset;
        transform.LookAt( character.transform );
	}
}
