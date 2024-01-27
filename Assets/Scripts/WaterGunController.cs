using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class WaterGunController : MonoBehaviour
{
    [SerializeField] private float maxWaterTankLevel = 100f;
    private float currentWaterTankLevel;
    [SerializeField] private float waterTankRefillRate;
    [SerializeField] private UIDocument mainUI;
    private ProgressBar waterTankProgressBar;

    [SerializeField] private GameObject baloon;
    private GameObject newBaloon;
    private Rigidbody newBaloonRigidbody;

    private bool isCharging = false;
    [SerializeField] private float minWaterCostMain = 20f;
    [SerializeField] private float chargeRate = 1.0f;
    [SerializeField] private float minForce = 2f;
    [SerializeField] private float maxForce = 10f;
    private float currentForce;

    [SerializeField] private float maxScale = 0.3f;
    private float minScale;
    private float currentScale;

    [SerializeField] private Transform waterGunNozzle;

    void Start()
    {
        currentWaterTankLevel = maxWaterTankLevel;
        waterTankProgressBar = mainUI.rootVisualElement.Q<ProgressBar>("waterTankProgressBar");
        minScale = baloon.transform.localScale.y / 2.0f;
        currentForce = minForce;
    }

    void Update()
    {
        if (isCharging)
        {
            if (currentWaterTankLevel <= 0.0f || !Input.GetMouseButton(0))
            {
                onWaterGunMainRelease();
                Debug.Log("Released");
            }
            else
            {
                // Increase force
                currentForce += chargeRate * Time.deltaTime;
                currentForce = Mathf.Clamp(currentForce, minForce, maxForce);
                if(currentForce < maxForce)
                {
                    // Scale the baloon
                    currentScale = Mathf.Lerp(minScale, maxScale, currentForce / maxForce);
                    newBaloon.transform.localScale = new Vector3(currentScale, currentScale, currentScale);

                    // Lower the water tank level
                    currentWaterTankLevel -= chargeRate * Time.deltaTime;
                    currentWaterTankLevel = Mathf.Clamp(currentWaterTankLevel, 0.0f, maxWaterTankLevel);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && currentWaterTankLevel >= minWaterCostMain)
            {
                onWaterGunMainCharge();
                Debug.Log("Charging");
            }
            else
            {
                // Charge the water tank
                currentWaterTankLevel += waterTankRefillRate * Time.deltaTime;
                currentWaterTankLevel = Mathf.Clamp(currentWaterTankLevel, 0.0f, maxWaterTankLevel);
            }
        }

        // Update the water tank progress bar
        waterTankProgressBar.SetValueWithoutNotify(currentWaterTankLevel);
    }

    public void onWaterGunMainCharge()
    {
        // Instantiate a new baloon
        Vector3 baloonSpawnPosition = waterGunNozzle.position + waterGunNozzle.forward * minScale;
        newBaloon = Instantiate(baloon, baloonSpawnPosition, waterGunNozzle.rotation, waterGunNozzle);
        // Get the baloon's rigidbody
        newBaloonRigidbody = newBaloon.GetComponent<Rigidbody>();
        newBaloonRigidbody.isKinematic = true;
        // Start charging
        isCharging = true;
        currentWaterTankLevel -= minWaterCostMain;
    }

    public void onWaterGunMainRelease()
    {
        // Set the damage component of the baloon
        newBaloon.GetComponent<Damager>().SetDamage(currentForce);

        // Launch the baloon
        Debug.Log("Force: " + currentForce);
        newBaloonRigidbody.isKinematic = false;
        newBaloon.transform.parent = null;
        newBaloonRigidbody.AddForce(currentForce * waterGunNozzle.forward, ForceMode.Impulse);

        // Stop charging
        isCharging = false;
        currentForce = minForce;
    }
}
