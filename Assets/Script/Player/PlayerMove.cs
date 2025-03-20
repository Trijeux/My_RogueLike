// Project : My_RogueLike
// Script by : Nanatchy

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Script.Player
{
   
    public class PlayerMove : MonoBehaviour
    {
        #region Attributs
        
        [SerializeField]private GameObject attackUp;
        [SerializeField]private GameObject attackRightLeft;
        [SerializeField]private GameObject attackDown;
        
        private Rigidbody2D _rb;
        private Animator _animatorPlayer;
        private Vector2 _moveInput;

        private TriggerCollidePlayer _triggerPlayer;
        
        [SerializeField] private float speed = 1;
        
        private bool _attackInput;
        private bool _attackIsGood = true;
        private int _hitCount = 0; 
        private static float RotationY => 180f;
        
        private enum DirectionLook
        {
            Up,
            Down,
            Left,
            Right
        }
        private struct StateValue
        {
            public StateValue(DirectionLook state, float value)
            {
                State = state;
                Value = value;
            }

            public DirectionLook State;
            public float Value;
        }

        private StateValue _state;

        #endregion

        #region Methods

        private void AttackEnd()
        {
            _attackIsGood = true;
        }
        private void AddHitCount()
        {
            _hitCount++;
        }
        
        private void MovePlayer()
        {
            _rb.linearVelocity = new Vector2(_moveInput.x * speed, _moveInput.y * speed);
        }
        
        private void UpdateLookPlayer()
        {
            if (Mathf.Abs(_moveInput.y) >= Mathf.Abs(_moveInput.x))
            {
                _state = _moveInput.y > 0 ? new StateValue(DirectionLook.Up, 0) : new StateValue(DirectionLook.Down, 1);
            }
            else if (Mathf.Abs(_moveInput.x) > 0)
            {
                _state = _moveInput.x > 0 ? new StateValue(DirectionLook.Left, 0.5f) : new StateValue(DirectionLook.Right, 0.5f);
            }
            else
            {
                _state = new StateValue(DirectionLook.Down, 1);
            }
            
            transform.rotation = _state.State switch
            {
                DirectionLook.Right => Quaternion.Euler(0f, RotationY, 0f),
                _ => Quaternion.Euler(0f, 0f, 0f)
            };
        }

        private void UpdateAttackCollider()
        {
            if (!_attackIsGood)
            {
                switch (_state.State)
                {

                    case DirectionLook.Up:
                        attackUp.SetActive(true);
                        attackRightLeft.SetActive(false);
                        attackDown.SetActive(false);
                        break;
                    case DirectionLook.Down:
                        attackUp.SetActive(false);
                        attackRightLeft.SetActive(false);
                        attackDown.SetActive(true);
                        break;
                    case DirectionLook.Left:
                    case DirectionLook.Right:
                        attackUp.SetActive(false);
                        attackRightLeft.SetActive(true);
                        attackDown.SetActive(false);
                        break;
                }
            }
            else
            {
                attackUp.SetActive(false);
                attackRightLeft.SetActive(false);
                attackDown.SetActive(false);
            }
        }

        private void UpdateAnimator()
        {
            if (Mathf.Abs(_moveInput.x) > 0 || Mathf.Abs(_moveInput.y) > 0)
            {
               _animatorPlayer.SetBool("Walk", true);
               _animatorPlayer.SetFloat("State", _state.Value);
            }
            else
            {
                _animatorPlayer.SetBool("Walk", false); 
            }
            
            if (_attackInput && _attackIsGood)
            {
                _attackIsGood = false;
                _animatorPlayer.SetTrigger("Attack");
                _animatorPlayer.SetFloat("State", _state.Value);
            }
        }
        private void OnMove(InputValue value)
        {
            _moveInput = value.Get<Vector2>();
        }
        private void OnAttack(InputValue value)
        {
            _attackInput = value.isPressed;
        }
        
        private void Damage()
        {
            if (_triggerPlayer.IsHitDamage && _hitCount != 2)
            {
                _animatorPlayer.SetBool("Hit", true);
                _animatorPlayer.SetInteger("HitCount", _hitCount);
                _triggerPlayer.Invincibility = true;
            }
            else
            {
                _triggerPlayer.IsHitDamage = false;
                _hitCount = 0;
                _animatorPlayer.SetBool("Hit", false);
                _animatorPlayer.SetInteger("HitCount", _hitCount);
                _triggerPlayer.Invincibility = false;
            }
        }
        
        #endregion

        #region Behaviors

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animatorPlayer = GetComponent<Animator>();
            _triggerPlayer = GetComponentInChildren<TriggerCollidePlayer>();
        }

        private void FixedUpdate()
        {
            MovePlayer();
            UpdateLookPlayer();
            UpdateAnimator();
            UpdateAttackCollider(); 
            Damage();
        }

        #endregion
    }
}
