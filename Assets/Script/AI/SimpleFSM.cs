using System;
using UnityEngine;

public class SimpleFSM : MonoBehaviour
{
    private enum FsmState
    {
        Empty,
        Chase,
        Flee,
        Attack
    }

    private Chase _chase;

    [SerializeField] private float timerDuration = 5;

    private FsmState _currentState = FsmState.Empty;
    private SteeringBehaviour _motion;
    private float timer;


    private void Start()
    {
        _motion = GetComponent<SteeringBehaviour>();
        _chase = GetComponent<Chase>();
        SetState(FsmState.Chase);
    }

    private void Update()
    {
        CheckTransitions(_currentState);
        OnStateUpdate(_currentState);
    }

    private void CheckTransitions(FsmState state)
    {
        switch (state)
        {
            case FsmState.Chase:
                if (_chase.IsHit)
                    SetState(FsmState.Flee);
                else if (_chase.IsGoodDistanceForAttack)
                    SetState(FsmState.Attack);
                break;
            case FsmState.Flee:
                if (timer > timerDuration)
                    SetState(FsmState.Chase);
                break;
            case FsmState.Attack:
                if (_chase.IsHit)
                    SetState(FsmState.Flee);
                else if (!_chase.IsGoodDistanceForAttack)
                    SetState(FsmState.Chase);
                break;
            case FsmState.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }


    private void OnStateEnter(FsmState state)
    {
        //Debug.Log($"OnEnter : {state}");

        switch (state)
        {
            case FsmState.Chase:
                _motion.ChaseFactor = 1;
                break;
            case FsmState.Flee:
                _motion.FleeFactor = 1;
                timer = 0;
                break;
            case FsmState.Attack:
                _motion.ChaseFactor = 1;
                break;
            case FsmState.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

    }

    private void OnStateExit(FsmState state)
    {
        //Debug.Log($"OnExit : {state}");

        switch (state)
        {
            case FsmState.Chase:
                _motion.ChaseFactor = 0;
                break;
            case FsmState.Flee:
                _motion.FleeFactor = 0;
                break;
            case FsmState.Attack:
                _motion.ChaseFactor = 0;
                break;
            case FsmState.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void OnStateUpdate(FsmState state)
    {
        //Debug.Log($"OnUpdate : {state}");

        switch (state)
        {
            case FsmState.Chase:
                break;
            case FsmState.Flee:
                timer += Time.deltaTime;
                break;
            case FsmState.Attack:
                break;
            case FsmState.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void SetState(FsmState newState)
    {
        if (newState == FsmState.Empty) return;
        if (_currentState != FsmState.Empty) OnStateExit(_currentState);

        _currentState = newState;
        OnStateEnter(_currentState);

    }
}
