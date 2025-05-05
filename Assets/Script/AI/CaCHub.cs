// Project : My_RogueLike
// Script by : Nanatchy

using System;
using UnityEngine;

public class CaCHub : MonoBehaviour
{
    #region Attributs

    [SerializeField] private GameObject attack;

    #endregion

    #region Methods

    private void attackOn()
    {
	    attack.SetActive(true);
    }

    private void attackOff()
    {
	    attack.SetActive(false);
    }
    
    #endregion

    #region Behaviors
	
	private void Start()
    {
        
    }
	
    private void Update()
    {
        
    }
	
	#endregion
}
