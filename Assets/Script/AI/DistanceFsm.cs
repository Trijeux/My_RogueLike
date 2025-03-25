using System;
using UnityEngine;

public class DistanceFsm : MonoBehaviour
{
    private enum FsmState
    {
        Empty,
        Teleport,
        Flee,
        Attack
    }

    [SerializeField] private float timerDuration = 5;

    private FsmState _currentState = FsmState.Empty;
    private DistanceSteeringBehaviour _motion;
    private float timer;


    private void Start()
    {
        _motion = GetComponent<DistanceSteeringBehaviour>();
        SetState(FsmState.Teleport);
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
            case FsmState.Teleport:
                if (true /*Hit ? || Joueur Trop proche pendent x second*/)
                    SetState(FsmState.Flee);
                else if (false /*Good attack distance ?*/)
                    SetState(FsmState.Attack);
                break;
            case FsmState.Flee:
                if (timer > timerDuration /*|| bad attack distance ?*/)
                    SetState(FsmState.Teleport);
                else if(true /*Good attack distance ?*/)
                    SetState(FsmState.Attack);
                break;
            case FsmState.Attack:
                if (true  /*Hit ? || Joueur Trop proche pendent x second*/)
                    SetState(FsmState.Flee);
                else if (false /*Bad attack distance ?*/)
                    SetState(FsmState.Teleport);
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
            case FsmState.Teleport:
                _motion.TeleportFactor = 1;
                break;
            case FsmState.Flee:
                _motion.FleeFactor = 1;
                timer = 0;
                break;
            case FsmState.Attack:
                _motion.TeleportFactor = 1;
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
            case FsmState.Teleport:
                _motion.TeleportFactor = 0;
                break;
            case FsmState.Flee:
                _motion.FleeFactor = 0;
                break;
            case FsmState.Attack:
                _motion.TeleportFactor = 0;
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
            case FsmState.Teleport:
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
