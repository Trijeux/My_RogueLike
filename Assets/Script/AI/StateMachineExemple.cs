using System;
using UnityEngine;

public class StateMachineExemple : MonoBehaviour
{
	private State _currentState = State.Empty;

	private SteeringBehaviourExemple _motion;
	
	private enum State
	{
		Empty,
		Patrille,
		chase,
		attack,
		fuit
	}
	
	private void SetState(State newState)
	{
		//Test For Glitch
		if (newState == State.Empty)
		{
			return;
		}
		//Fisrt Turn
		if (_currentState != State.Empty)
		{
			OnStateExit(_currentState);
		}
		_currentState = newState;
		OnStateEnter(_currentState);
	}
	
	private void OnStateExit(State state)
	{
		switch (state)
		{
			case State.Patrille:
				_motion.PatrouilleFactor = 0;
				break;
			case State.chase:
				_motion.ChaseFactor = 0;
				break;
			case State.fuit:
				_motion.FleeFactor = 0;
				break;
			case State.attack:
				_motion.AttackFactor = 0;
				_motion.ChaseFactor = 0;
				break;
			case State.Empty:
			default:
				throw new ArgumentOutOfRangeException(nameof(state), state, null);
		}
	}
	
	private void OnStateEnter(State state)
	{
		//Debug.Log($"OnEnter : {state}");

		switch (state)
		{
			case State.Patrille:
				_motion.PatrouilleFactor = 1;
				break;
			case State.chase:
				_motion.ChaseFactor = 1;
				break;
			case State.fuit:
				_motion.FleeFactor = 1;
				break;
			case State.attack:
				_motion.ChaseFactor = 1;
				_motion.AttackFactor = 1;
				break;
			case State.Empty:
			default:
				throw new ArgumentOutOfRangeException(nameof(state), state, null);
		}

	}

	[Header("Patrouille")]
	[SerializeField] private bool NotEnemyInRoom = false;
	[SerializeField] private bool PlayerIsLook = false;
	[Header("Chase")]
	[SerializeField] private bool PlayerIsCaC = false;
	[SerializeField] private bool PlayerIsOutOfLook = false;
	[SerializeField] private bool LowLife = false;
	[Header("Flee")]
	[SerializeField] private bool PlayerIsNotLook = false;
	[Header("Attack")]
	[SerializeField] private bool PlayerIsNotCaC = false;
	[SerializeField] private bool LowLifeTow = false;
	
	private void CheckTransitions(State state)
	{
		switch (state)
		{
			case State.Patrille:
				if (NotEnemyInRoom)
				{
					SetState(State.fuit);
					NotEnemyInRoom = false;
				}
				if (PlayerIsLook)
				{
					SetState(State.chase);
					PlayerIsLook = false;
				}
				break;
			case State.chase:
				if (PlayerIsCaC)
				{
					SetState(State.attack);
					PlayerIsCaC = false;
				}
				if (PlayerIsOutOfLook)
				{
					SetState(State.Patrille);
					PlayerIsOutOfLook = false;
				}
				if (LowLife)
				{
					SetState(State.fuit);
					LowLife = false;
				}
				break;
			case State.fuit:
				if (PlayerIsNotLook)
				{
					SetState(State.Patrille);
					PlayerIsNotLook = false;
				}
				break;
			case State.attack:
				if (PlayerIsNotCaC)
				{
					SetState(State.chase);
					PlayerIsNotCaC = false;
				}
				if (LowLifeTow)
				{
					SetState(State.fuit);
					LowLifeTow = false;
				}
				break;
			case State.Empty:
			default:
				throw new ArgumentOutOfRangeException(nameof(state), state, null);
		}
	}
	
	private void OnStateUpdate(State state)
	{
		//Debug.Log($"OnUpdate : {state}");

		switch (state)
		{
			case State.Patrille:
				break;
			case State.chase:
				break;
			case State.fuit:
				break;
			case State.attack:
				break;
			case State.Empty:
			default:
				throw new ArgumentOutOfRangeException(nameof(state), state, null);
		}
	}
	
	private void Start()
	{
		_motion = GetComponent<SteeringBehaviourExemple>();
        SetState(State.Patrille);
    }
	
    private void Update()
    {
        CheckTransitions(_currentState);
        OnStateUpdate(_currentState);
    }
}
