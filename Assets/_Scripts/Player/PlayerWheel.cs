using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWheel : MonoBehaviour
{
    private float _speed;
    private float _rotation;

    private void Start()
    {
        _rotation = Random.Range(0f, 360f);  
    }

    public void SpinWheel(float rotation)
    {
        _rotation += rotation;
        transform.rotation = Quaternion.Euler(transform.position.x, transform.position.y, _rotation);
    }
}
