using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CacFsm : MonoBehaviour
{
    private enum FsmState
    {
        Empty,
        Chase,
        Flee,
        Attack
    }

    private Chase _chase;

    [SerializeField] private float timerDurationFeel = 5;
    [SerializeField] private float cooldownAttack = 5;
    [SerializeField] private GameObject _gameObjectAttack;
    [SerializeField] private string PlayerAttack;
    
    private CapsuleCollider2D _collider2DTrigger;
    [SerializeField]private CapsuleCollider2D _collider2D;
    private Animator _animator;
    private FsmState _currentState = FsmState.Empty;
    private CacSteeringBehaviour _motion;
    private float timerFeel;
    private float timerAttack;
    private bool haveAttacked = false;
    private bool isHit = false;
    private int hitCount;


    private void AddCountHit()
    {
        hitCount++;
    }
    private void EndAttack()
    {
        _gameObjectAttack.SetActive(false);
        haveAttacked = true;
    }

    private void Start()
    {
        _motion = GetComponent<CacSteeringBehaviour>();
        _chase = GetComponent<Chase>();
        _animator = GetComponent<Animator>();
        _collider2DTrigger = GetComponent<CapsuleCollider2D>();
        SetState(FsmState.Chase);
    }

    private void Update()
    {
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
            case FsmState.Chase:
                if (isHit)
                    SetState(FsmState.Flee);
                else if (_chase.IsGoodDistanceForAttack)
                    SetState(FsmState.Attack);
                break;
            case FsmState.Flee:
                if (timerFeel > timerDurationFeel)
                    SetState(FsmState.Chase);
                break;
            case FsmState.Attack:
                if (isHit)
                    SetState(FsmState.Flee);
                else if (!_chase.IsGoodDistanceForAttack && haveAttacked)
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
                _collider2D.enabled = false;
                timerFeel = 0;
                break;
            case FsmState.Attack:
                _motion.ChaseFactor = 1;
                timerAttack = 0;
                haveAttacked = false;
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
                _collider2D.enabled = true;
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
                timerFeel += Time.deltaTime;
                break;
            case FsmState.Attack:
                timerAttack += Time.deltaTime;
                if (timerAttack > cooldownAttack)
                {
                    timerAttack = 0;
                    _animator.SetTrigger("Attack");
                    _gameObjectAttack.SetActive(true);
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
            _gameObjectAttack.SetActive(false);
            isHit = true;
            hitCount = 0;
        }
    }
}
