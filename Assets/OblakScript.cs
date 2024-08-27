using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OblakScript : MonoBehaviour
{
    public Sprite[] sprites;
    public Collider2D col;
    
    private Vector2 _targetPos;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        GetPos();
        transform.position = _targetPos;
        GetPos();
    }

    private void FixedUpdate()
    {
        if(Vector2.Distance(transform.position,Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 1f)
            GetComponent<Rigidbody2D>().AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * (-2000f * Time.fixedDeltaTime));

        if (Vector2.Distance(transform.position, _targetPos) > .2f)
            GetComponent<Rigidbody2D>().AddForce((_targetPos - (Vector2)transform.position).normalized * (10f * Time.fixedDeltaTime));
        else GetPos();
    }

    private void GetPos()
    {
        _targetPos = RandomPointInCollider.GetPointInsideCollider(col);
    }
}
