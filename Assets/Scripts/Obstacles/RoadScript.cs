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
    Road,
    Turn,
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

    /// <summary>
    /// Degrees of the road rotation
    /// </summary>
    //public float RoadTurnDegree
    //{ 
    //    get
    //    {
    //        switch(RoadType)
    //        {
    //            case RoadTypes.TurnLeft:
    //                return -90;
    //            case RoadTypes.TurnRight:
    //                return 90;
    //            default:
    //                return 0;
    //        }
    //    }
    //}
      

    #endregion

    #region Constructor

    // Start is called once
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
