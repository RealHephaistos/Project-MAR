using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBombExplosion : MonoBehaviour
{

    [SerializeField] private GameObject splashDecalPrefab;
    private float splashDecalOffset = -0.025f;

    private void OnCollisionEnter(Collision collision)
    {
        // Get the normal of the surface we hit
        Vector3 normal = collision.contacts[0].normal;
        // Spawn a splash decal on the surface of the object we hit
        Vector3 splashLocation = collision.contacts[0].point + splashDecalOffset * normal; // Offset the decal slightly so it doesn't clip into the surface
        GameObject splashDecal = Instantiate(splashDecalPrefab, splashLocation, Quaternion.LookRotation(normal));

        // Destroy the water bomb
        Destroy(this.gameObject);
    }
}
