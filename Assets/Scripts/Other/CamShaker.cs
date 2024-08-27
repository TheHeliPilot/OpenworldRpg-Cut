using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CamShaker : MonoBehaviour
{
    public float d;
    public float m;
    public void StartShake(float duration, float magnitude)
    {
        d = duration;
        m = magnitude/10;
        StartCoroutine(nameof(Shake));
    }
    
    private IEnumerator Shake()
    {
        Vector3 originalPose = new Vector3(0, 0, 0);
        float elapsed = 0;

        while (elapsed < d)
        {
            float xOffset = Random.Range(-.5f, 5f) * m;
            float yOffset = Random.Range(-.5f, 5f) * m;
            transform.localPosition = new Vector3(xOffset, yOffset, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPose;
    }
}
