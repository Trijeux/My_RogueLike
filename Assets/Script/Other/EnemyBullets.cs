using System;
using UnityEngine;

public class EnemyBullets : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D _rigidbody2D;
    private GameObject _character;
    private Vector2 _direction;
    private bool _hitPlayer = false;
    private float timerDestroy = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _character = GameObject.FindGameObjectWithTag("Player");
        
        _direction =  _character.transform.position - transform.position;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _direction * speed;
    }

    private void Update()
    {
        if (_hitPlayer)
        {
            if (0.1f < timerDestroy)
            {
                Destroy(gameObject);
            }   
            timerDestroy += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Player"))
        {
            _hitPlayer = true;
        }
    }
}
