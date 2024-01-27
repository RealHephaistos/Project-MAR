using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnimateSplash : MonoBehaviour
{
    private Material splashMaterial;
    [SerializeField] private float fadeSpeed = 0.1f;

    [SerializeField] private float fadeDelay = 3f;

    private float minNoiseScale;
    [SerializeField] private float animationSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        // Unfortunatly we have to copy the material because it's a shared material
        // and we can't use MaterialPropertyBlocks with DecalProjectors yet :(
        splashMaterial = new Material(GetComponent<DecalProjector>().material);
        GetComponent<DecalProjector>().material = splashMaterial;
        splashMaterial.SetFloat("_Opacity", 1f);
        
        // Assign a random noise scale
        minNoiseScale = UnityEngine.Random.Range(4f, 8f);
        splashMaterial.SetFloat("_NoiseScale", minNoiseScale);
    }

    // Update is called once per frame
    void Update()
    {
        // Wait for fade delay to be over then fade out
        if(fadeDelay > 0)
        {
            fadeDelay -= Time.deltaTime;
        }
        else if(splashMaterial.GetFloat("_Opacity") > 0)
        {
            
            splashMaterial.SetFloat("_Opacity", splashMaterial.GetFloat("_Opacity") - fadeSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // Animate the shape
        splashMaterial.SetFloat("_NoiseScale", splashMaterial.GetFloat("_NoiseScale") + animationSpeed * Time.deltaTime);
    }
}
