/*
 * File: RoachWeapon.cs
 * Created: 28/05/2026, 12:52:38 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class RoachWeapon : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void Use ()
    {
        Player._Instance.Damage();
    }
}