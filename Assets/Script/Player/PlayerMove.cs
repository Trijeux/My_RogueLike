// Project : My_RogueLike
// Script by : Nanatchy

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
    public class PlayerMove : MonoBehaviour
    {
        #region Attributs

        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        [SerializeField] private float speed = 1;

        #endregion

        #region Methods

        private void OnMove(InputValue value)
        {
            _moveInput = value.Get<Vector2>();
        }
        
        #endregion

        #region Behaviors

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = new Vector2(_moveInput.x * speed, _moveInput.y * speed);
        }

        #endregion
    }
}
