/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/UI/CollisionStopAutoScroll.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/UI
 * Created Date: Thursday, September 3rd 2026, 6:43:41 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;

public class CollisionStopAutoScroll : MonoBehaviour
{
    [SerializeField] private AutoScroll _autoScroll;

    // ------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("collision detected");
        if (collider.gameObject.CompareTag("StopAutoScroll"))
        {
            _autoScroll.Stop();
        }
    }
}