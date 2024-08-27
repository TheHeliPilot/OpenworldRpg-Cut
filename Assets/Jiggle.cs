using UnityEngine;

public class Jiggle
{
    private float _velocity, _resistance = .9f;

    public float Value;
    
    public Jiggle(){}

    public Jiggle(float res)
    {
        _resistance = res;
    }
    
    public void Update()
    {
        Value += _velocity;
        _velocity += -Value * .05f; 
        _velocity *= _resistance;
        
    }

    public void AddForce(float force)
    {
        _velocity += force;
    }
}