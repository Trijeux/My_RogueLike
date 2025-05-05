// Project : My_RogueLike
// Script by : Nanatchy

using System;
using System.Collections;
using Script.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartUI : MonoBehaviour
{
    #region Attributs
    
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private Animator animatorBook;
    [SerializeField] private GameObject selectStart;
    [SerializeField] private GameObject selectQuit;


    private bool _haveSwitch = false;
    private int _valideIndx = 0;
    
    #endregion

    #region Methods

	

    #endregion

    #region InputSystem
    

    #endregion
    
    #region Behaviors
	
	private void Start()
    {
	    playerMove.playerInput.SwitchCurrentActionMap("UI");
	    StartCoroutine("WaitAndActiveUI");
    }
	
    private void Update()
    {
	    if ((playerMove.InputLeft || playerMove.InputRight) && _valideIndx == 0 && !_haveSwitch)
	    {
		    _haveSwitch = true;
		    _valideIndx = 1;
	    }
	    else if ((playerMove.InputLeft || playerMove.InputRight) && _valideIndx == 1 && !_haveSwitch)
	    {
		    _haveSwitch = true;
		    _valideIndx = 0;
	    }
	    else if (!playerMove.InputLeft && !playerMove.InputRight)
	    {
		    _haveSwitch = false;
	    }
	    if (playerMove.InputValide && _valideIndx == 0)
	    {
		    gameObject.SetActive(false);
		    playerMove.playerInput.SwitchCurrentActionMap("Player");
	    }
	    if (playerMove.InputValide && _valideIndx == 1)
	    {
		    Application.Quit();
	    }

	    switch (_valideIndx)
	    {
		    case 0:
			    selectStart.SetActive(true);
			    selectQuit.SetActive(false);
			    break;
		    case 1:
			    selectStart.SetActive(false);
			    selectQuit.SetActive(true);
			    break;
	    }
    }
    
    private IEnumerator WaitAndActiveUI()
    {
	    yield return new WaitForSeconds(2f);
	    animatorBook.SetBool("Start", true);
	    yield return new WaitForSeconds(1f);
    }
    
	#endregion
}
