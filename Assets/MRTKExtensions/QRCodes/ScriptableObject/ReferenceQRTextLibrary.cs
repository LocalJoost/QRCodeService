using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new ReferenceQRTextLibrary", menuName = "ScriptableObjects/new ReferenceQRTextLibrary", order = 1)]
public class ReferenceQRTextLibrary : ScriptableObject
{
    public List<QRContentPrefab> QRContentPrefabs;
}

[Serializable]
public class QRContentPrefab
{
    public string QRText;
    public GameObject Prefab;
}
