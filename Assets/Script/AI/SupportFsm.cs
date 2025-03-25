using System;
using UnityEngine;

public class SupportFsm : MonoBehaviour
{
    private enum FsmState
    {
        Empty,
        Walk,
        Flee,
        Give
    }

    [SerializeField] private float timerDuration = 5;

    private FsmState _currentState = FsmState.Empty;
    private SupportSteeringBehaviour _motion;
    private float timer;


    private void Start()
    {
        _motion = GetComponent<SupportSteeringBehaviour>();
        SetState(FsmState.Walk);
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
            case FsmState.Walk:
                if (true /*Hit ? || Joueur Trop proche pendent x second*/)
                    SetState(FsmState.Flee);
                else if (false /*Good distance ?*/)
                    SetState(FsmState.Give);
                break;
            case FsmState.Flee:
                if (timer > timerDuration /*|| bad distance ?*/)
                    SetState(FsmState.Walk);
                else if(true /*Good attack distance ?*/)
                    SetState(FsmState.Give);
                break;
            case FsmState.Give:
                if (true  /*Hit ? || Joueur Trop proche pendent x second*/)
                    SetState(FsmState.Flee);
                else if (false /*Bad distance ?*/)
                    SetState(FsmState.Walk);
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
            case FsmState.Walk:
                _motion.WalkFactor = 1;
                break;
            case FsmState.Flee:
                _motion.FleeFactor = 1;
                timer = 0;
                break;
            case FsmState.Give:
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
            case FsmState.Walk:
                _motion.WalkFactor = 0;
                break;
            case FsmState.Flee:
                _motion.FleeFactor = 0;
                break;
            case FsmState.Give:
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
            case FsmState.Walk:
                break;
            case FsmState.Flee:
                timer += Time.deltaTime;
                break;
            case FsmState.Give:
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
