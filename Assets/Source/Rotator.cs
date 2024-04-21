using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    [SerializeField] private float angularVelocity;

    void Update()
    {
        transform.Rotate(axis, angularVelocity * Time.deltaTime);
    }
}
