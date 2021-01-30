using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObstacleScript : ObstacleScript
{
    /// <summary>
    /// Damage, which object will cause player
    /// </summary>
    public int Damage;

    /// <summary>
    /// Overriding hit method
    /// </summary>
    public override void Hit(Collision hittenBy)
    {
        // Decreassing hp of player
        hittenBy.gameObject.GetComponent<PlayerScript>().AddPointsToPlayerHP(-Damage, ObstacleReactionType);
    }
}
