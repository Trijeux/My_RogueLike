// Project : My_RogueLike
// Script by : Nanatchy

using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UIElements.Experimental;

namespace Script.Player
{
   
    public class PlayerMove : MonoBehaviour
    {
        #region Attributs

        [FormerlySerializedAs("kill")] [SerializeField] private TextMeshProUGUI killText;
        [SerializeField] private GameObject game;
        [SerializeField] private GameObject gameOver;
        [SerializeField]private GameObject attackUp;
        [SerializeField]private GameObject attackRightLeft;
        [SerializeField]private GameObject attackDown;
        [SerializeField] private int lifeMaxCurrent;
        [SerializeField] private int startLife;
        [SerializeField] private int lifeMax;
        [SerializeField] private int attack;
        [SerializeField] private int numbKill;
        public PlayerInput playerInput;
        
        public bool InputRight { get; private set; }
        
        public bool InputLeft { get; private set; }
        
        public bool InputValide { get; private set; }


        private int _kill = 0;
        public int LifeMaxCurrent => lifeMaxCurrent;
        public int LifeMax => lifeMax;
        public int Life { get; private set; }

        public int Attack => attack;
        
        private Rigidbody2D _rb;
        private Animator _animatorPlayer;
        private Vector2 _moveInput;

        private TriggerCollidePlayer _triggerPlayer;
        
        [SerializeField] private float speed = 1;
        
        private bool _attackInput;
        private bool _attackIsGood = true;
        private int _hitCount = 0; 
        private static float RotationY => 180f;

        public void SetActiveInput(bool value)
        {
            switch (value)
            {
                case true:
                    playerInput.ActivateInput();
                    break;
                case false:
                    playerInput.DeactivateInput();
                    break;
            }
        }
        
        public void AddKill()
        {
            _kill++;
        }
        
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

        public void AddAttack()
        {
            attack++;
        }

        public void AddHeal()
        {
            lifeMaxCurrent += 2;
            if (lifeMaxCurrent > 19*2)
            {
                lifeMaxCurrent = 19 * 2;
            }
        }

        public void Heal()
        {
            Life += 2;
            if (lifeMaxCurrent < Life)
            {
                Life = lifeMaxCurrent;
            }
        }
        
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
            if (Mathf.Abs(_moveInput.x) > 0 || Mathf.Abs(_moveInput.y) > 0)
            {
                if (Mathf.Abs(_moveInput.y) >= Mathf.Abs(_moveInput.x))
                {
                    _state = _moveInput.y > 0 ? new StateValue(DirectionLook.Up, 0) : new StateValue(DirectionLook.Down, 1);
                }
                else if (Mathf.Abs(_moveInput.x) > 0)
                {
                    _state = _moveInput.x > 0 ? new StateValue(DirectionLook.Left, 0.5f) : new StateValue(DirectionLook.Right, 0.5f);
                }
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
        
        private void OnMoveRight(InputValue value)
        {
            InputRight = value.isPressed;
        }
    
        private void OnMoveLeft(InputValue value)
        {
            InputLeft = value.isPressed;
        }
    
        private void OnValide(InputValue value)
        {
            InputValide = value.isPressed;
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

        private void CheckIsAlive()
        {
            if (Life <= 0)
            {
                gameOver.SetActive(true);
                game.SetActive(false);
            }
        }
        
        public void SubLife(int damage)
        {
            Life -= damage;
        } 
        
        #endregion

        #region Behaviors

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animatorPlayer = GetComponent<Animator>();
            _triggerPlayer = GetComponentInChildren<TriggerCollidePlayer>();
            Life = startLife;
        }

        private void FixedUpdate()
        {
            MovePlayer();
            UpdateLookPlayer();
            UpdateAnimator();
            UpdateAttackCollider(); 
            Damage();
            CheckIsAlive();
            if (numbKill <= _kill)
            {
                _kill = 0;
                Heal();
            }
            killText.text = $"{_kill} / {numbKill}";
        }

        #endregion
    }
}
