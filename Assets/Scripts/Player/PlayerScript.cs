using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player script for all various user interactions with it
public class PlayerScript : MonoBehaviour
{
    #region Variables
    
    /// <summary>
    /// Should the object move forward
    /// </summary>
    public bool DoesMove;

    /// <summary>
    /// Player movement speed in forward direction
    /// </summary>
    public float ForwardSpeed;

    /// <summary>
    /// Player movement speed in left and right direction
    /// </summary>
    public float StrafeSpeed;

    /// <summary>
    /// Player's rigidbody
    /// </summary>
    private Rigidbody _playerRigidbody;

    #endregion

    #region UnityStartUpdate

    // Start is called before the first frame update
    void Start()
    {
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

    #endregion
}