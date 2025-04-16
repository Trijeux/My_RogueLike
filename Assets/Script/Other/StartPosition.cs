// Project : My_RogueLike
// Script by : Nanatchy

using System;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    #region Attributs

    private Transform _player;

    private bool _startPosForPlayer = false;
    
    #endregion

    #region Methods
    
    #endregion

    #region Behaviors
    
	private void Start()
    {
        
    }
	
    private void Update()
    {
	    if (!_startPosForPlayer)
	    {
		    var targetList = GameObject.FindGameObjectWithTag("Player");
		    
		    if (targetList != null)
		    {
			    _player = targetList.GetComponent<Transform>();

			    if (_player != null)
			    {
				    _player.position = transform.position;
				    
				    _startPosForPlayer = true;
			    }
		    }
	    }
    }
	
	#endregion
}
