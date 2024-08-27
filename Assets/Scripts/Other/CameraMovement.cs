using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.8F;
    private Vector3 _velocity = Vector3.zero;
    
    void FixedUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));
    
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("yes");
        if (other.gameObject.CompareTag("Cullable")) 
            other.gameObject.GetComponent<FoliageToggler>().Toggle(true);
        
        if(other.GetComponent<ObjectPoolChecker>() != null)
            other.GetComponent<ObjectPoolChecker>().Pool();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cullable"))
            other.gameObject.GetComponent<FoliageToggler>().Toggle(false);
    }
}
