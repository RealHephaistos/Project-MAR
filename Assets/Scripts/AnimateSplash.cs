using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnimateSplash : MonoBehaviour
{
    private Material splashShader;
    [SerializeField] private float fadeSpeed = 0.1f;

    [SerializeField] private float fadeDelay = 3f;

    private float minNoiseScale;
    [SerializeField] private float animationSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        splashShader = GetComponent<DecalProjector>().material;
        splashShader.SetFloat("_Opacity", 1f);
        
        // Assign a random noise scale
        minNoiseScale = UnityEngine.Random.Range(4f, 8f);
        splashShader.SetFloat("_NoiseScale", minNoiseScale);
    }

    // Update is called once per frame
    void Update()
    {
        // Wait for fade delay to be over then fade out
        if(fadeDelay > 0)
        {
            fadeDelay -= Time.deltaTime;
        }
        else if(splashShader.GetFloat("_Opacity") > 0)
        {
            
            splashShader.SetFloat("_Opacity", splashShader.GetFloat("_Opacity") - fadeSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // Animate the shape
        splashShader.SetFloat("_NoiseScale", splashShader.GetFloat("_NoiseScale") + animationSpeed * Time.deltaTime);
    }
}
