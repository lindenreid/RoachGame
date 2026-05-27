/*
 * File: RoachWeapon.cs
 * Created: 26/05/2026, 8:57:36 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class RoachWeapon : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        Roach roach;
        if(roach = other.gameObject.GetComponent<Roach>())
        {
            roach.Hit();
        }
    }
}