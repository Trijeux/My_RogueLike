using System;
using Pathfinding;
using Script.Player;
using UnityEngine;
using UnityEngine.Serialization;

public class CacFsm : MonoBehaviour
{
    private enum FsmState
    {
        Empty,
        Chase,
        Flee,
        Attack,
        Dead,
    }
    private Chase _chase;

    [SerializeField] private float timerDurationFeel = 5;
    [SerializeField] private float cooldownAttack = 5;
    [SerializeField] private GameObject _gameObjectAttack;
    [SerializeField] private string PlayerAttack;
    [SerializeField] private int maxLife;
    [SerializeField] private AudioSource hitAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private CapsuleCollider2D _collider2DTrigger;
    [SerializeField]private CapsuleCollider2D _collider2D;
    [SerializeField] private ActiveChild _activeChild;
    [SerializeField] private GameObject _Child;
    private Animator _animator;
    private FsmState _currentState = FsmState.Empty;
    private CacSteeringBehaviour _motion;
    private float timerFeel;
    private float timerAttack;
    private bool haveAttacked = false;
    private bool isHit = false;
    private int hitCount;
    private int _life;
    private bool _isdead = false;
    [SerializeField] private AIPath _path;

    private void Death()
    {
        Destroy(gameObject);
    }
    
    private void PlayAttack()
    {
        attackAudio.Play();
    }

    private void PlayHit()
    {
        hitAudio.Play();
    }
    
    private void AddCountHit()
    {
        hitCount++;
    }

    private void StartAttack()
    {
        _gameObjectAttack.SetActive(true);
    }
    
    private void EndAttack()
    {
        _gameObjectAttack.SetActive(false);
        haveAttacked = true;
    }

    private void Start()
    {
        var damageenemy = GetComponentInParent<EnemyManager>();
        maxLife = damageenemy.SetHealEnemy(maxLife);
        _motion = GetComponent<CacSteeringBehaviour>();
        _chase = GetComponent<Chase>();
        _animator = GetComponent<Animator>();
        SetState(FsmState.Chase);
        _life = maxLife;
    }

    private void Update()
    {
        CheckTransitions(_currentState);
        OnStateUpdate(_currentState);
        if (isHit && hitCount != 2)
        {
            if (_Child.activeSelf)
            {
                _activeChild.Deactive();
            }
            _animator.SetBool("Hit", true);
            _animator.SetInteger("HitCount", hitCount);
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
                if (_isdead)
                {
                    SetState(FsmState.Dead);
                }
                break;
            case FsmState.Flee:
                if (timerFeel > timerDurationFeel)
                    SetState(FsmState.Chase);
                if (_isdead)
                {
                    SetState(FsmState.Dead);
                }
                break;
            case FsmState.Attack:
                if (isHit)
                    SetState(FsmState.Flee);
                else if (!_chase.IsGoodDistanceForAttack && haveAttacked)
                    SetState(FsmState.Chase);
                if (_isdead)
                {
                    SetState(FsmState.Dead);
                }
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
            case FsmState.Dead:
                _path.destination = transform.position;
                _animator.SetBool("Death", true);
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
            _collider2DTrigger.enabled = false;
            _gameObjectAttack.SetActive(false);
            var player = other.GetComponentInParent<PlayerMove>();
            if (!isHit)
            {
                _life -= player.Attack;
            }
            isHit = true;
            if (_life <= 0 && !_Child.activeSelf)
            {
                _isdead = true;
                player.AddKill();
            }
            hitCount = 0;
        }
    }
}
