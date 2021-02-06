using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingObstacleScript : ObstacleScript
{
    /// <summary>
    /// Amount of heal for player
    /// </summary>
    public int Heal;

    /// <summary>
    /// When object is spawned
    /// </summary>
    public void OnEnable()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Overriding hit method
    /// </summary>
    public override void Hit(GameObject hittenBy)
    {
        // Disabling recent object
        gameObject.SetActive(false);
        // Decreassing hp of player
        hittenBy.gameObject.GetComponent<PlayerScript>().AddPointsToPlayerHP(Heal, ObstacleReactionType, false);
    }
}
