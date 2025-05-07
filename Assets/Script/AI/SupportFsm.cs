using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Script.Player;
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
        Attach,
        Dead
    }

    private ChaseFriend _chaseFriend;


    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private AudioSource hitAudio;
    [SerializeField] private AudioSource giveChildAudio;
    [SerializeField] private float cooldownGiveChild = 5;
    [SerializeField] private string PlayerAttack;
    [SerializeField] private float cooldownExplotion;
    [SerializeField] private GameObject Explotion;
    [SerializeField] private int maxLife;
    [SerializeField] private int damagePlayer;

    [SerializeField] private CapsuleCollider2D _collider2DTrigger;
    private ActiveChild targetChild;

    private Animator _animator;
    private FsmState _currentState = FsmState.Empty;
    private SupportSteeringBehaviour _motion;
    private float timerGiveChild;
    private bool isHit = false;
    private int hitCount;
    private bool explotionActive = false;
    private float timeExplotion = 0;
    private int _life;
    private bool _isdead;
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
    
    private void destroyKamikaze()
    {
        Destroy(transform.parent.gameObject);
    }
    private void AddCountHit()
    {
        hitCount++;
    }

    private void EndGive()
    {
        giveChildAudio.Play();
        targetChild.Active();
    }

    private void Start()
    {
        var damageenemy = transform.parent.GetComponentInParent<EnemyManager>();
        maxLife = damageenemy.SetHealEnemy(maxLife);
        _motion = GetComponentInParent<SupportSteeringBehaviour>();
        _chaseFriend = GetComponentInParent<ChaseFriend>();
        _animator = GetComponent<Animator>();
        SetState(FsmState.ChaseFriend);
        _life = maxLife;
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
            if (_currentState != FsmState.Attach)
            {
                _collider2DTrigger.enabled = true;
            }
        }

        if (_chaseFriend.KamikazeMod && !explotionActive)
        {
            explotionActive = true;
            _animator.SetBool("WhilExploid",true);
        }

        if (explotionActive)
        {
            if (cooldownExplotion <= timeExplotion)
            {
                timeExplotion = 0;
                _chaseFriend.explotion = true;
                _collider2DTrigger.enabled = false;
                Explotion.SetActive(true);
                _animator.SetBool("WhilExploid",false);
                _animator.SetBool("Explose", true);
            }
            timeExplotion += Time.deltaTime;
        }
    }

    private void CheckTransitions(FsmState state)
    {
        switch (state)
        {
            case FsmState.ChaseFriend:
                if (_chaseFriend.IsGoodDistanceForGrap)
                    SetState(FsmState.Attach);
                if (_isdead)
                {
                    SetState(FsmState.Dead);
                }
                break;
            case FsmState.Attach:
                if (!_chaseFriend.IsGoodDistanceForGrap)
                    SetState(FsmState.ChaseFriend);
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
            case FsmState.ChaseFriend:
                _motion.ChaseFactor = 1;
                break;
            case FsmState.Attach:
                _motion.ChaseFactor = 1;
                _collider2DTrigger.enabled = false;
                timerGiveChild = 0;
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
            case FsmState.ChaseFriend:
                _motion.ChaseFactor = 0;
                break;
            case FsmState.Attach:
                _motion.ChaseFactor = 0;
                _collider2DTrigger.enabled = false;
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
            _collider2DTrigger.enabled = false;
            var player = other.GetComponentInParent<PlayerMove>();
            if (!isHit)
            {
                _life -= player.Attack;
            }
            isHit = true;
            if (_life <= 0)
            {
                player.AddKill();
                _isdead = true;
            }
            hitCount = 0;
        }
    }
}
