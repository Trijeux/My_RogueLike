// Project : My_RogueLike
// Script by : Nanatchy

using System;
using UnityEngine;

public class SteeringBehaviourExemple : MonoBehaviour
{
	private float chaseFactor = 0f;
	private float fleeFactor = 0f;
	private float patrouilleFactor = 0f;
	private float attackFactor = 0f;
	
	public float ChaseFactor
	{
		get => chaseFactor;
		set => chaseFactor = value;
	}

	public float AttackFactor
	{
		get => attackFactor;
		set => attackFactor = value;
	}
	
	public float FleeFactor
	{
		get => fleeFactor;
		set => fleeFactor = value;
	}
	
	public float PatrouilleFactor
	{
		get => patrouilleFactor;
		set => patrouilleFactor = value;
	}
	
	private void Start()
    {
        
    }
	
    private void Update()
    {
	    if (chaseFactor > 0)
	    {
		    Debug.Log("Chase");
		    if (attackFactor > 0)
		    {
			    Debug.Log("attack");
		    }
	    }
	    if (fleeFactor > 0)
	    {
		    Debug.Log("Flee");
	    }
	    if (patrouilleFactor > 0)
	    {
		    Debug.Log("Patrouille");
	    }
    }
}
