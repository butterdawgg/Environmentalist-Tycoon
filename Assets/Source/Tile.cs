using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Tile : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] float surfaceLevel;
    [SerializeField] bool isSuitableForBuilding;

    [SerializeField] private Transform model;

    public float SurfaceLevel { get { return Mathf.Clamp(surfaceLevel, 0f, 1f); } }
    public bool IsSuitableForBuilding { get { return isSuitableForBuilding; } }

    private void Awake()
    {
        model.rotation = Quaternion.Euler(0f, 90f * Random.Range(0, 4), 0f);
    }
}