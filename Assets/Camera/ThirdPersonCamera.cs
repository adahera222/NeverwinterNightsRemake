using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    public GameObject character;
    public float zoomSpeed = 5.0f;
    public float rotationSpeed = 180.0f;

    private float distance = 10.0f;
    private Vector3 direction = new Vector3( 0.0f, 1.0f, -1.0f );
    
	// Use this for initialization
	void Start ()
    {
        direction.Normalize();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Camera rotation.
        if ( Input.mousePosition.x == 0 )
        {
            float angle = - rotationSpeed * Time.deltaTime;
            Quaternion rot = Quaternion.AngleAxis( angle, Vector3.up );
            direction = rot * direction;
        }
        else if ( Input.mousePosition.x == Screen.width - 1 )
        {
            float angle = rotationSpeed * Time.deltaTime;
            Quaternion rot = Quaternion.AngleAxis( angle, Vector3.up );
            direction = rot * direction;
        }
        // Camera zooming.
        distance -= zoomSpeed * Input.GetAxis( "Mouse ScrollWheel" );

        transform.position = character.transform.position + direction * distance;
        transform.LookAt( character.transform );
	}
}
