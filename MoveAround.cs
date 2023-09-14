using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveAround : MonoBehaviour
{
    public Vector3 offset;
    private Vector3 _pos;
    
    private float _speed = 0f;
    void Start()
    {
        _speed = 0.02f + Random.value * 0.02f;
        _pos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = _pos + offset * ((float)Math.Sin(_speed * Time.time));
    }
}
