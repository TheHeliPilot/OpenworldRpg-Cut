using UnityEngine;
using Random = UnityEngine.Random;

public class CubeForceScript : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(Random.Range(300,700), 0, 0);
        GetComponent<Rigidbody>().AddTorque(Random.Range(10,50), Random.Range(10,50), Random.Range(10,50));
    }
}
