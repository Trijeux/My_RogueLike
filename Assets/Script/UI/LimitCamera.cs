// Project : My_RogueLike
// Script by : Nanatchy

using System;
using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    #region Attributs

    [SerializeField] private Transform player;

    #endregion

    #region Methods



    #endregion

    #region Behaviors

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -1);
    }

    #endregion
}
