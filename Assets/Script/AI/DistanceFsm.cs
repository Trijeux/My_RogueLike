using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Script.Player;
using UnityEngine;
using UnityEngine.Serialization;

public class DistanceFsm : MonoBehaviour
{
    private enum FsmState
    {
        Empty,
        Chase,
        Flee,
        Attack,
        Dead
    }

    private Chase _chase;

    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private AudioSource hitAudio;
    [SerializeField] private float cooldownAttack = 5;
    [SerializeField] private float timerDurationFeel = 5;
    [SerializeField] private string PlayerAttack;
    [SerializeField] private float distanceFeel;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private int maxLife;
    [SerializeField] private int damagePlayer;
    
    
    [SerializeField] private CapsuleCollider2D _collider2DTrigger;
    [SerializeField]private CapsuleCollider2D _collider2D;
    [SerializeField] private ActiveChild _activeChild;
    [SerializeField] private GameObject _Child;
    
    private Transform target;

    private Animator _animator;
    private FsmState _currentState = FsmState.Empty;
    private DistanceSteeringBehaviour _motion;
    private float timerAttack;
    private float timerFeel;
    private bool isHit = false;
    private int hitCount;
    private float distanceToTarget;
    private ContactFilter2D _contactFilter2D;
    private List<Collider2D> _colliders = new List<Collider2D>();
    private bool canNotAttack;
    private Vector3 scale;
    private int _life;
    private bool _isdead = false;
    [SerializeField] private AIPath _path;
    
    private void Death()
    {
        Destroy(gameObject.transform.parent.gameObject);
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

    private void Shoot()
    {
        Vector2 direction = (target.position - firePoint.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Instantiate(arrow, firePoint.transform.position, Quaternion.Euler(0, 0, angle));
    }

    private void Start()
    {
        var damageenemy = transform.parent.GetComponentInParent<EnemyManager>();
        maxLife = damageenemy.SetHealEnemy(maxLife);
        _motion = GetComponentInParent<DistanceSteeringBehaviour>();
        _chase = GetComponentInParent<Chase>();
        _animator = GetComponent<Animator>();
        target = _chase.Target.GetComponentInParent<Transform>();
        SetState(FsmState.Chase);
        _contactFilter2D.SetLayerMask(_layerMask);
        scale = transform.parent.localScale;
        _life = maxLife;
    }

    private void Update()
    {
        DetectionWallForAttack();
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

        if (_chase.Target.position.x > transform.parent.position.x)
        {
            transform.parent.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
        else
        {
            transform.parent.localScale = new Vector3(scale.x, scale.y, scale.z);
        }
    }

    private void DetectionWallForAttack()
    {

        distanceToTarget = Vector3.Distance(transform.position, target.position);
        _colliders.Clear();
        Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 100, _contactFilter2D, _colliders);
        Collider2D goodObject = _colliders.FirstOrDefault(c => c.CompareTag("Player"));
        if (goodObject != null)
        {
            Vector2 goodObjectDistance = goodObject.bounds.center - transform.position;
            List<RaycastHit2D> hits = new List<RaycastHit2D>();
            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), goodObjectDistance, _contactFilter2D, hits, 100) > 0)
            {
                if (hits[0].collider == goodObject)
                {
                    canNotAttack = false;
                }
                else
                {
                    canNotAttack = true;
                }
            }
        }
    }

    private void CheckTransitions(FsmState state)
    {
        switch (state)
        {
            case FsmState.Chase:
                if (isHit || distanceToTarget < distanceFeel)
                    SetState(FsmState.Flee);
                else if (_chase.IsGoodDistanceForAttack && !canNotAttack)
                    SetState(FsmState.Attack);
                if (_isdead)
                {
                    SetState(FsmState.Dead);
                }
                break;
            case FsmState.Flee:
                if (canNotAttack || timerFeel > timerDurationFeel)
                    SetState(FsmState.Chase);
                if (_isdead)
                {
                    SetState(FsmState.Dead);
                }
                break;
            case FsmState.Attack:
                if (isHit || distanceToTarget < distanceFeel && !canNotAttack)
                    SetState(FsmState.Flee);
                else if (!_chase.IsGoodDistanceForAttack)
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
        switch (state)
        {
            case FsmState.Chase:
                _motion.ChaseFactor = 0;
                break;
            case FsmState.Flee:
                _collider2D.enabled = true;
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
            var player = other.GetComponentInParent<PlayerMove>();
            if (!isHit)
            {
                _life -= player.Attack;
            }
            isHit = true;
            if (_life <= 0 && !_Child.activeSelf)
            {
                player.AddKill();
                _isdead = true;
            }
            hitCount = 0;
        }
    }
}
