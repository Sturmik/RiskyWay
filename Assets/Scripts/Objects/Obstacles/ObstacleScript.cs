using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Obstacles types
/// </summary>
public enum ObstacleReactionTypes
{
    // Pushes our player backwards
    Push,             
    // Doesn't push our player backwards
    NoPush,
}

/// <summary>
/// Interface defines the fact, that the object can be hit
/// </summary>
public interface IHittable
{
    /// <summary>
    /// Reaction of the object to being hitten
    /// </summary>
    void Hit(Collision hittenBy);
}

/// <summary>
/// Base abstract class for all obstacles created
/// </summary>
public abstract class ObstacleScript : MonoBehaviour, IHittable
{
    #region Variables

    public ObstacleReactionTypes ObstacleReactionType;

    #endregion

    #region Unity

    /// <summary>
    /// Collision check
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // If it was the player, who hit the object
        if (collision.gameObject.tag == "Player")
        {
            // Do this
            Hit(collision);
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Virtual method for override
    /// </summary>
    public virtual void Hit(Collision hittenBy)
    {
        // Action here
    }

    #endregion
}
