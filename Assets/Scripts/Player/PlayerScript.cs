using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player script for all various user interactions with it
public class PlayerScript : MonoBehaviour
{
    #region Variables

    #region СharacteristicVariables

    /// <summary>
    /// How much time player will not move after hit condition
    /// </summary>
    public float AfterHitMoveTimeout;
    /// <summary>
    /// How much time player will be invincible after hit condition
    /// </summary>
    public float AfterHitInvincibleTimeout;

    // Player's material component
    private Material _materialComponent;
    // First color of the material
    private Color _baseColor;

    private bool _isInvincible;
    /// <summary>
    /// Is player invincible at the moment
    /// </summary>
    public bool IsInvincible
    {  
        get { return _isInvincible; }
        private set 
        { 
            _isInvincible = value;
            switch(IsInvincible)
            {
                case true:
                    Color newColor = new Color(_baseColor.r, _baseColor.g, _baseColor.b, 0.9f);
                    _materialComponent.SetColor("_Color", newColor);
                    break;
                case false:
                    _materialComponent.SetColor("_Color", _baseColor); 
                    break;
            }
        }
    }

    private int _health;
    /// <summary>
    /// Amount of player's health
    /// </summary>
    public int Health
    {
        get { return _health; }
        private set
        {
            if (value < 0) return;
           _health = value;
            Debug.Log(value);
        }
    }

    #endregion

    #region MovementVariables

    /// <summary>
    /// Should the player move forward
    /// </summary>
    private bool DoesMove { get; set; }

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

    #endregion

    /// <summary>
    /// Player's rigidbody
    /// </summary>
    private Rigidbody _playerRigidbody;

    #endregion

    #region Unity

    // Start is called before the first frame update
    void Start()
    {
        // Initializing variables
        _materialComponent = gameObject.GetComponent<Renderer>().material;
        _baseColor = _materialComponent.color;
        IsInvincible = false;
        _health = 3;
        _turnSpeedMultiplicator = 220;
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

    /// <summary>
    /// Reverses player's availability to move
    /// </summary>
    /// <param name="canMove"></param>
    private void StartMovement()
    {
        DoesMove = true;
    }

    /// <summary>
    /// Reverses player's availability to move
    /// </summary>
    /// <param name="canMove"></param>
    private void DisableInvincibility()
    {
        IsInvincible = false;
    }

    /// <summary>
    /// Sets new player hp
    /// </summary>
    /// <param name="newHp">Points to add, can be negative</param>
    /// <param name="impactType">Impact type</param>
    /// <param name="isInvincibleAfterHit">Will be invincible after hit</param>
    public void AddPointsToPlayerHP(int newHp, ObstacleReactionTypes impactType, bool isInvincibleAfterHit)
    {
        // If player is invincible at the moment ignore collision
        if (IsInvincible) return;
        // Setting new hp
        Health += newHp;
        // If impact pushes player
        AfterHitMove(impactType, isInvincibleAfterHit);
    }

    /// <summary>
    /// Players hit detection, jumps back
    /// </summary>
    private void AfterHitMove(ObstacleReactionTypes impactType, bool isInvincibleAfterHit)
    {
        // Checking if player will be invincible after hit or not
        if (isInvincibleAfterHit)
        {
            IsInvincible = true;
            Invoke(nameof(DisableInvincibility), AfterHitInvincibleTimeout);
        }
        // Jumps back if impact was from push type
        if (impactType == ObstacleReactionTypes.Push)
        {
            // Disabling movement, while our knife jumps back
            DoesMove = false;
            // Giving player control back after some time
            Invoke(nameof(StartMovement), AfterHitMoveTimeout);
            // Jumping back
            _playerRigidbody.AddRelativeForce((Vector3.back + Vector3.up / 3) * ForwardSpeed * 25, ForceMode.Impulse);
        }
    }

    #endregion
}