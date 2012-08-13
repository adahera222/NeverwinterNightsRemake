using UnityEngine;
using System.Collections;

[RequireComponent( typeof( CharacterController ) )]

public class PlayerMovement : MonoBehaviour
{

    float speed = 10.0f;
    float jumpSpeed = 20.0f;
    float rotationSpeed = 180.0f;
    float gravity = 60.0f;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 jumpDirection = Vector3.zero;
    private Vector3 clickPoint = Vector3.zero;
    private bool clickMovement = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();

        // Rotation of character.
        Vector3 rotation = Vector3.up;
        rotation *= Input.GetAxis( "Horizontal" ) * rotationSpeed;
        rotation *= Time.deltaTime;
        transform.Rotate( rotation );
        
        // Movement of character.
        // Forward/backward movement.
        moveDirection = transform.TransformDirection( Vector3.forward );
        moveDirection *= Input.GetAxis( "Vertical" ) * speed * Time.deltaTime;
        // Jump + gravity.
        if ( controller.isGrounded && Input.GetButton( "Jump" ) )
        {
            jumpDirection.y = jumpSpeed;
        }
        if ( !controller.isGrounded )
        {
            jumpDirection.y -= gravity * Time.deltaTime;
        }

        // Move to click.
        if ( Input.GetButtonDown( "Fire1" ) )
        {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            int terrainMask = 1 << 8;
            if ( Physics.Raycast( ray, out hit, Mathf.Infinity, terrainMask ) )
            {
                clickMovement = true;
                clickPoint = hit.point;
            }
        }
        Vector3 wholeMove = Vector3.zero;

        // If WSAD was clicked, we don't want the ball go to clickPoint anymore.
        if ( rotation != Vector3.zero )
        {
            clickMovement = false;
        }

        if ( moveDirection != Vector3.zero )
        {
            wholeMove = moveDirection;
            clickMovement = false;
        }

        if ( clickMovement )
        {
            // Rotate in direction of click point.
            Vector3 relativePos = clickPoint - transform.position;
            // We don't want to take into account Y-difference.
            relativePos.y = 0;
            Quaternion newRotation = Quaternion.LookRotation( relativePos );
            transform.rotation = newRotation;

            // Compute move vector for this frame.
            wholeMove = transform.TransformDirection( Vector3.forward ) * speed * Time.deltaTime;
            Vector3 toTarget = clickPoint - transform.position;
            if ( wholeMove.magnitude > distanceXY( clickPoint, transform.position ) )
            {
                wholeMove = toTarget;
                clickMovement = false;
            }
        }

        wholeMove += jumpDirection * Time.deltaTime;
        controller.Move( wholeMove );
    }

    private float distanceXY( Vector3 a, Vector3 b )
    {
        a.y = 0;
        b.y = 0;
        return ( b - a ).magnitude;
    }
}
