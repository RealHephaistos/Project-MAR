using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OnImpact : MonoBehaviour
{
    // The strength of the graffito, the higher the value the harder it is to remove
    [SerializeField] private float graffitiDifficultyMultiplier = 0.1f;

    private BoxCollider boxCollider;

    private Material graffitiMaterial;

    private GameplayLoopMainScene gameplayLoopMainSceneScript;

    // Start is called before the first frame update    
    void Start()
    {
        // Unfortunatly we have to copy the material because it's a shared material
        // and we can't use MaterialPropertyBlocks with DecalProjectors yet :(
        DecalProjector decalProjector = GetComponent<DecalProjector>();
        graffitiMaterial = new Material(decalProjector.material);
        decalProjector.material = graffitiMaterial;

        boxCollider = GetComponent<BoxCollider>();
       

        // Set the dimensions of the box collider to match the size of the decal projector
        boxCollider.size = decalProjector.size;

        gameplayLoopMainSceneScript = GameObject.Find("Terrain").gameObject.GetComponent<GameplayLoopMainScene>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get the damager component from the object that collided with the decal projector
        Damager damager = other.gameObject.GetComponent<Damager>();

        // Reduce opacity of the decal proportionally to the scale of the water bomb
        graffitiMaterial.SetFloat("_Opacity", graffitiMaterial.GetFloat("_Opacity") - graffitiDifficultyMultiplier * damager.GetDamage());

        if (graffitiMaterial.GetFloat("_Opacity") <= 0)
        {
            // Destroy the decal projector if it's opacity is 0 or less
            Destroy(this.gameObject);
            gameplayLoopMainSceneScript.GraffitiCleaned();
        }

    }
}
