using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalWalkScript : MonoBehaviour
{
    public float speed;
    public Collider2D bounds;
    public Sprite[] sprites;
    public GameObject fire;
    
    private Rigidbody2D _rb;
    private bool _isMoving;
    private bool _canMove = true;
    private Vector2 _target;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_isMoving)
        {
            int counter = 0;
            do
            {
                if (counter > 100)
                    break;
                _target = RandomPointInCollider.GetPointInsideCollider(bounds);
                counter++;
            } while (Vector2.Distance(transform.position, _target) > 4f);

            _isMoving = true;
        }

        if (Vector2.Distance(transform.position, _target) < .05)
        {
            _canMove = false;
            Invoke(nameof(Move), Random.Range(0f, 5f));
        }

        GetComponent<SpriteRenderer>().sprite = transform.position.x < _target.x ? sprites[0] : sprites[1];
    }

    public void BURN()
    {
        fire.SetActive(true);
        speed *= 5;
        Destroy(gameObject, Random.Range(3f, 10f));
    }

    private void FixedUpdate()
    {
        if (_canMove)
            _rb.AddForce((_target - (Vector2)transform.position).normalized * (speed * Time.fixedDeltaTime));
    }

    private void Move()
    {
        _canMove = true;
        _isMoving = false;
    }
}
