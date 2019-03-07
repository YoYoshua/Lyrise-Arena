using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _AppStart : MonoBehaviour
{
    public GameObject HexPrefab;
    public GameObject HexHoverPrefab;

    private void Awake()
    {
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        //Here you can initialize all scene managers

        /* Prefab Manager */
        PrefabManager.HexPrefab = HexPrefab;
    }
}
