using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    private float damage = 0f;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public float GetDamage()
    {
        return damage;
    }
}
