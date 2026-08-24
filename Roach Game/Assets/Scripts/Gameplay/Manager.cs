/*
 * Filename: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Gameplay/Manager.cs
 * Path: /Users/lindenreid/Documents/GitHub/RoachGame/Roach Game/Assets/Scripts/Gameplay
 * Created Date: Wednesday, August 19th 2026, 2:48:42 pm
 * Author: Travis Reid
 * 
 * Copyright (c) 2026 Studio Tilia
 */

using UnityEngine;

public class Manager : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [Header("Movement")]
    [SerializeField] private Transform _cowerPos;
    [SerializeField] private Renderer _renderer;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    // timeline callback
    public void StartDissolve ()
    {
        Material mat = _renderer.material;
        mat.SetInt("_DoDissolve", 1);
        mat.SetFloat("_DissolveStartTime", Time.time);
    }

    // ------------------------------------------------------------------------
    // timeline callback
    public void TeleportToCowerPos ()
    {
        transform.position = _cowerPos.position;
        transform.rotation = _cowerPos.rotation;
    }

    // ------------------------------------------------------------------------
    // timeline callback
    public void FacePlayer ()
    {
        Transform playerTransform = Player._Instance.transform;

        Vector3 playerPosXZ = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        Vector3 posXZ = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(playerPosXZ - posXZ);
    }
}