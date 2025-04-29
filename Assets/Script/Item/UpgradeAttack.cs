// Project : My_RogueLike
// Script by : Nanatchy

using System;
using Script.Player;
using UnityEngine;

public class UpgradeAttack : MonoBehaviour
{
    #region Attributs

    [SerializeField] private GameObject itemUpHeal;

    #endregion

    #region Methods



    #endregion

    #region Behaviors
	
    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (other.CompareTag("Player"))
	    {
		    var player = other.GetComponent<PlayerMove>();
		    player.AddAttack();
		    Destroy(itemUpHeal);
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
