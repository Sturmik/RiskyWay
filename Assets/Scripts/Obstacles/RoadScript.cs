using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Road script is used for working with our path and roads.
// It allows procedural generator to connect these objects and
// generate level for us.
public class RoadScript : MonoBehaviour
{
    #region Variables 

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

    #region Constructor

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Methods



    #endregion
}
