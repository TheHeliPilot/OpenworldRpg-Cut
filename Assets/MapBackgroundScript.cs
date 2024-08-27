using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBackgroundScript : MonoBehaviour
{
    public Collider2D col;
    
    private Vector2 _targetPos;

    private void Start()
    {
        GetPos();
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, _targetPos) > 5f)
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, _targetPos.x, .01f * Time.fixedDeltaTime),
                Mathf.Lerp(transform.position.y, _targetPos.y, .01f * Time.fixedDeltaTime), 0);
        else GetPos();
    }

    private void GetPos()
    {
        _targetPos = RandomPointInCollider.GetPointInsideCollider(col);
    }
}
