// Project : My_RogueLike
// Script by : Nanatchy

using System;
using Script.Player;
using UnityEngine;

public class Healing : MonoBehaviour
{
    #region Attributs

    #endregion

    #region Methods



    #endregion

    #region Behaviors

    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (other.CompareTag("Player"))
	    {
		    var player = other.GetComponent<PlayerMove>();
		    player.Heal();
		    Destroy(gameObject);
	    }
    }

    private void Start()
    {
        
    }
	
    private void Update()
    {
        
    }
	
	#endregion
}
