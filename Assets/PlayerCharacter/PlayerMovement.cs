using UnityEngine;
using System.Collections;

[RequireComponent( typeof( CharacterController ) )]

public class PlayerMovement : MonoBehaviour
{
    // Collider which we are interested in during casting ray.
    public Collider terrainCollider;

    public float speed = 10.0f;
    public float jumpSpeed = 20.0f;
    public float rotationSpeed = 180.0f;
    public float gravity = 60.0f;

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

        // Move to click.
        if ( Input.GetButtonDown( "Fire1" ) )
        {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit[] hits = Physics.RaycastAll( ray );
            foreach ( RaycastHit hit in hits )
            {
                if ( hit.collider == terrainCollider )
                {
                    clickMovement = true;
                    clickPoint = hit.point;
                    clickPoint.y = 0.0f;
                }
            }
        }

        // Rotation of character.
        Vector3 rotation = Vector3.up;
        rotation *= Input.GetAxis( "Horizontal" ) * rotationSpeed;
        rotation *= Time.deltaTime;
        if ( rotation != Vector3.zero )
        {
            transform.Rotate( rotation );
            // We don't want move to clickPoint anymore.
            clickMovement = false;
        }
        
        // Movement of character.
        // Forward/backward movement.
        Vector3 moveDirection = transform.TransformDirection( Vector3.forward );
        moveDirection *= Input.GetAxis( "Vertical" ) * speed * Time.deltaTime;
        if ( moveDirection != Vector3.zero )
        {
            controller.Move( moveDirection );
            // We don't want move to clickPoint anymore.
            clickMovement = false;
        }

        // There was none of WSAD down.
        if ( clickMovement )
        {
            // Rotate in direction of click point.
            Vector3 relativePos = clickPoint - transform.position;
            // We don't want to take into account Y-difference.
            relativePos.y = 0;
            Quaternion newRotation = Quaternion.LookRotation( relativePos );
            transform.rotation = newRotation;

            // Compute move vector for this frame.
            Vector3 clickMove = transform.TransformDirection( Vector3.forward ) * speed * Time.deltaTime;
            Vector3 toTarget = clickPoint - transform.position;
            toTarget.y = 0.0f;
            if ( clickMove.magnitude > toTarget.magnitude )
            {
                clickMove = toTarget;
                clickMovement = false;
            }
            controller.Move( clickMove );
        }

        // Jump + gravity.
        if ( controller.isGrounded && Input.GetButton( "Jump" ) )
        {
            jumpDirection.y = jumpSpeed;
        }
        if ( !controller.isGrounded )
        {
            jumpDirection.y -= gravity * Time.deltaTime;
        }
        controller.Move( jumpDirection * Time.deltaTime );
    }
}
