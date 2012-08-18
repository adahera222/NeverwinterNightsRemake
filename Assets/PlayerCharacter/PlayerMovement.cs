using UnityEngine;
using System.Collections;

[RequireComponent( typeof( CharacterController ) )]
[RequireComponent( typeof( Animation ) )]

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float runSpeed = 10.0f;
    public float jumpSpeed = 20.0f;
    public float rotationSpeed = 180.0f;
    public float gravity = 60.0f;

    private Vector3 jumpDirection = Vector3.zero;
    private bool isJumping = false;
    private bool clickMovement = false;
    private NavMeshPath path;
    private int actCorner = 0;
    private bool isRunning = true;

    // Use this for initialization
    void Start()
    {
        path = new NavMeshPath();
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
            RaycastHit hit;
            Physics.Raycast( ray, out hit );
            // Generate path to walk for character.
            int passableMask = (1 << NavMesh.GetNavMeshLayerFromName( "Default" ) );
            clickMovement = NavMesh.CalculatePath( transform.position, hit.point, passableMask, path );
            actCorner = 0;
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
            // We don't want move to clickPoint anymore.
            clickMovement = false;
        }

        // There was none of WSAD down.
        if ( clickMovement )
        {
            if ( actCorner < path.corners.Length )
            {
                // Rotate in direction of click point.
                Vector3 relativePos = path.corners[actCorner] - transform.position;
                // We don't want to take into account Y-difference during rotation.
                relativePos.y = 0.0f;
                if ( relativePos != Vector3.zero )
                {
                    Quaternion newRotation = Quaternion.LookRotation( relativePos );
                    transform.rotation = newRotation;
                }

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
                if ( clickMove.magnitude > relativePos.magnitude )
                {
                    clickMove = relativePos;
                    ++actCorner;
                }
                frameMoveDirection += clickMove;
            }
            else
            {
                clickMovement = false;
            }
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
        
        // Move character at last.
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
