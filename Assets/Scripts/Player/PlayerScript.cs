using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Player speed
    /// </summary>
    public float Speed;
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
        _playerRigidbody.AddForce(Vector3.right * horizontalInput * Speed, ForceMode.Impulse);
    }

    #endregion
}