using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

/// <summary>
/// Road types
/// </summary>
public enum RoadTypes
{
    Start,
    Straight,
    TurnLeft,
    TurnRight,
    Finish
}

// Road script is used for working with our path and roads.
// It allows procedural generator to connect these objects and
// generate level for us.

// To create new road, you need to take unity prefab "BaseStraightRoad"
// It is acceptable to change the length of the "RoadSettings" prefab ('z' axis), however changing 
// others axis can cause troubles in level generator.
// After that you can start to fill "ObstaclesList" prefab with child elements you wish to put on the road.
// Finally, you need to save it as a prefab in any directory,
// then throw it in the roads pool in the game manager script.

// This system allows developer easily add new types of the roads with various obstacles.
public class RoadScript : MonoBehaviour
{
    #region Variables 

    /// <summary>
    /// Road type
    /// </summary>
    public RoadTypes RoadType;

    /// <summary>
    /// Position of joint for next road to connect to
    /// </summary>
    public Vector3 JointPosition 
    { 
        get
        {
           return transform.Find("RoadSettings" + "/" + "Joint").transform.position;
        }
    }

    /// <summary>
    /// Position of joint for next road to connect to
    /// </summary>
    public Quaternion JointRotation
    {
        get
        {
            return transform.Find("RoadSettings" + "/" + "Joint").transform.rotation;
        }
    }  

    #endregion

    #region Unity

    // Working with triggers 
    private void OnTriggerEnter(Collider other)
    {
        // If player enters trigger do specific action
        if (other.gameObject.tag == "Player")
        {
            // Setting new point of rotation for our player
            other.GetComponent<PlayerScript>().SetTurnPoint(JointPosition, JointRotation);
        }
    }

    #endregion
}
