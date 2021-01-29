using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player script for all various user interactions with it
public class PlayerScript : MonoBehaviour
{
    #region Variables
    
    /// <summary>
    /// Should the player move forward
    /// </summary>
    public bool DoesMove { get; set; }

    /// <summary>
    /// Player movement speed in forward direction
    /// </summary>
    public float ForwardSpeed;

    /// <summary>
    /// Player movement speed in left and right direction
    /// </summary>
    public float StrafeSpeed;

    /// <summary>
    /// Player turning speed multiplicator
    /// </summary>
    private float _turnSpeedMultiplicator;

    /// <summary>
    /// Position of the point to which player should rotate
    /// </summary>
    private Vector3 _pointPosition;

    /// <summary>
    /// Point to which player should rotate
    /// </summary>
    private Quaternion _turnToPoint;

    /// <summary>
    /// Player's rigidbody
    /// </summary>
    private Rigidbody _playerRigidbody;

    #endregion

    #region UnityStartUpdate

    // Start is called before the first frame update
    void Start()
    {
        // Initializing variables
        _turnSpeedMultiplicator = 210;
        _turnToPoint = transform.rotation;
        DoesMove = true;
        // Getting components
        _playerRigidbody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMoveControl(Input.GetAxis("Horizontal")); 
    }

    #endregion

    #region Methods

    /// <summary>
    /// Player movement control
    /// </summary>
    /// <param name="horizontalInput">Player horizontal input</param>
    private void PlayerMoveControl(float horizontalInput)
    {
        // We check if the object should rotate
        // This helps us then we need to take turn
        if (transform.rotation != _turnToPoint)
        {
            // Closer we are to target point, the faster the rotation will execute
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, _turnToPoint,
                ForwardSpeed * (_turnSpeedMultiplicator / Vector3.Distance(transform.position, _pointPosition)) * Time.deltaTime);
            // We are blocking user strafe while, he is turning
            horizontalInput = 0;
        }

        // We check if the object should move.
        // It will come in handy, when we reach the finish or wait 
        // for the user to press "Start" or "Next level"
        if ( DoesMove == true )
        {
            // Forward movement
            _playerRigidbody.AddRelativeForce(Vector3.forward * ForwardSpeed, ForceMode.Impulse);
            // Allows us to move left and right
            _playerRigidbody.AddRelativeForce(Vector3.right * horizontalInput * StrafeSpeed, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Sets new point for our player to rotate to
    /// </summary>
    /// <param name="pointPos">Point to which player will move</param> 
    /// <param name="pointQuat">Point to which player will rotate</param>
    public void SetTurnPoint(Vector3 pointPos, Quaternion pointQuat)
    {
        _pointPosition = pointPos;
        _turnToPoint = pointQuat;
    }

    #endregion
}