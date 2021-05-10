using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/saveSO")]
public class SaveScriptable : ScriptableObject
{
    public string Company;
    public string Product;
    public string Version;
    public Texture2D icon;
    public UnityEditor.UIOrientation orientation;
}