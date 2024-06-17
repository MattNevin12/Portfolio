using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayTerrainHeight : MonoBehaviour
{
    private float offset;

    private void Start()
    {
        offset = transform.position.y;   
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = (Terrain.activeTerrain.SampleHeight(transform.position)) + offset;
        transform.position = pos;
    }
}
