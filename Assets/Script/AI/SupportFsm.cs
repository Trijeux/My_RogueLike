using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SupportFsm : MonoBehaviour
{
    private enum FsmState
    {
        Empty,
        ChaseFriend,
        Attach
    }

    private ChaseFriend _chaseFriend;

    [SerializeField] private float cooldownGiveChild = 5;
    [SerializeField] private string PlayerAttack;

    private CapsuleCollider2D _collider2DTrigger;

    private ActiveChild targetChild;

    private Animator _animator;
    private FsmState _currentState = FsmState.Empty;
    private SupportSteeringBehaviour _motion;
    private float timerGiveChild;
    private bool isHit = false;
    private int hitCount;

    private void AddCountHit()
    {
        hitCount++;
    }

    private void EndGive()
    {
        targetChild.Active();
    }

    private void Start()
    {
        _motion = GetComponentInParent<SupportSteeringBehaviour>();
        _chaseFriend = GetComponentInParent<ChaseFriend>();
        _animator = GetComponent<Animator>();
        _collider2DTrigger = GetComponent<CapsuleCollider2D>();
        SetState(FsmState.ChaseFriend);
    }

    private void Update()
    {
        if (_chaseFriend.Target != null)
        {
            targetChild = _chaseFriend.Target.GetComponentInParent<ActiveChild>();
            targetChild.supportIsHere = true;
        }
        
        CheckTransitions(_currentState);
        OnStateUpdate(_currentState);
        if (isHit && hitCount != 2)
        {
            _animator.SetBool("Hit", true);
            _animator.SetInteger("HitCount", hitCount);
            _collider2DTrigger.enabled = false;
        }
        else
        {
            isHit = false;
            hitCount = 0;
            _animator.SetBool("Hit", false);
            _animator.SetInteger("HitCount", hitCount);
            _collider2DTrigger.enabled = true;
        }
    }

    private void CheckTransitions(FsmState state)
    {
        switch (state)
        {
            case FsmState.ChaseFriend:
                if (_chaseFriend.IsGoodDistanceForGrap)
                    SetState(FsmState.Attach);
                break;
            case FsmState.Attach:
                if (!_chaseFriend.IsGoodDistanceForGrap)
                    SetState(FsmState.ChaseFriend);
                break;
            case FsmState.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }


    private void OnStateEnter(FsmState state)
    {
        switch (state)
        {
            case FsmState.ChaseFriend:
                _motion.ChaseFactor = 1;
                break;
            case FsmState.Attach:
                _motion.ChaseFactor = 1;
                timerGiveChild = 0;
                break;
            case FsmState.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

    }

    private void OnStateExit(FsmState state)
    {
        switch (state)
        {
            case FsmState.ChaseFriend:
                _motion.ChaseFactor = 0;
                break;
            case FsmState.Attach:
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
            case FsmState.ChaseFriend:
                break;
            case FsmState.Attach:
                timerGiveChild += Time.deltaTime;
                if (timerGiveChild > cooldownGiveChild)
                {
                    timerGiveChild = 0;
                    if (_chaseFriend.Target != null)
                    {
                        if (!targetChild.Child.activeSelf)
                        {
                            _animator.SetTrigger("GiveChild");
                        }
                    }
                }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PlayerAttack))
        {
            isHit = true;
            hitCount = 0;
        }
    }
}
