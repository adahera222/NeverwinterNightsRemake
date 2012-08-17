using UnityEngine;
using System.Collections;

[RequireComponent( typeof( CharacterController ) )]
[RequireComponent( typeof( Animation ) )]

public class PlayerMovement : MonoBehaviour
{
    // Collider which we are interested in during casting ray.
    public Collider terrainCollider;

    public float walkSpeed = 3.0f;
    public float runSpeed = 10.0f;
    public float jumpSpeed = 20.0f;
    public float rotationSpeed = 180.0f;
    public float gravity = 60.0f;

    private Vector3 jumpDirection = Vector3.zero;
    private bool isJumping = false;
    private Vector3 clickPoint = Vector3.zero;
    private bool clickMovement = false;
    private bool isRunning = true;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 frameMoveDirection = Vector3.zero;

        CharacterController controller = GetComponent<CharacterController>();
        Animation animation = GetComponent<Animation>();
        float vertAxis = Input.GetAxis( "Vertical" );
        float horizAxis = Input.GetAxis( "Horizontal" );

        // Run/Walk switch.
        if ( Input.GetButtonDown( "RunWalk Switch" ) )
        {
            isRunning = !isRunning;
        }

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
        if ( horizAxis != 0.0f )
        {
            Vector3 rotation = new Vector3( 0.0f, horizAxis, 0.0f );
            rotation *= rotationSpeed * Time.deltaTime;
            transform.Rotate( rotation );
            // We don't want move to clickPoint anymore.
            clickMovement = false;
        }
        
        // Movement of character.
        // Forward/backward movement.
        if ( vertAxis != 0.0f )
        {
            Vector3 moveDirection = transform.TransformDirection( Vector3.forward );
            moveDirection *= vertAxis * Time.deltaTime;
            if ( isRunning )
            {
                moveDirection *= runSpeed;
            }
            else
            {
                moveDirection *= walkSpeed;
            }
            frameMoveDirection += moveDirection;
            //controller.Move( moveDirection );
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
            Vector3 clickMove = transform.TransformDirection( Vector3.forward ) * Time.deltaTime;
            if ( isRunning )
            {
                clickMove *= runSpeed;
            }
            else
            {
                clickMove *= walkSpeed;
            }
            Vector3 toTarget = clickPoint - transform.position;
            toTarget.y = 0.0f;
            if ( clickMove.magnitude > toTarget.magnitude )
            {
                clickMove = toTarget;
                clickMovement = false;
            }
            frameMoveDirection += clickMove;
            //controller.Move( clickMove );
        }

        // Jump + gravity.
        if ( isJumping && controller.isGrounded )
        {
            isJumping = false;
        }
        if ( !isJumping && Input.GetButton( "Jump" ) )
        {
            isJumping = true;
            jumpDirection.y = jumpSpeed;
        }
        if ( !controller.isGrounded )
        {
            jumpDirection.y -= gravity * Time.deltaTime;
        }
        frameMoveDirection += jumpDirection * Time.deltaTime;
        //controller.Move( jumpDirection * Time.deltaTime );
        controller.Move( frameMoveDirection );

        // Plays animation.
        if ( vertAxis != 0.0f || horizAxis != 0.0f || clickMovement )
        {
            if ( isRunning )
            {
                animation.Play( "run" );
            }
            else
            {
                animation.Play( "walk" );
            }
        }
        else
        {
            animation.Play( "idle" );
        }
    }
}
