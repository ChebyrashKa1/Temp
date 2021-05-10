using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildWindow : EditorWindow
{
    private static string version  = "0.0.1";
    private static string company  = "DefaultCompany";
    private static string product  = "prod";
    private static Texture2D icon     = null;
    private static UIOrientation orientation;

    [MenuItem("Window/BuildWindow")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BuildWindow));
        Load();
    }

    private void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);

        version = EditorGUILayout.TextField("Version", version);
        company = EditorGUILayout.TextField("Company", company);
        product = EditorGUILayout.TextField("Product", product);

        icon = (Texture2D)EditorGUILayout.ObjectField("Sprite", icon, typeof(Texture2D), allowSceneObjects: true);

        orientation = (UIOrientation)EditorGUILayout.EnumPopup(orientation);

        if (GUILayout.Button("Save"))
        {
            Options();
            Save();
        }
        if (GUILayout.Button("Build"))
        {
            Options();
            Save();
            //build
        }
    }

    public void Options()
    {
        PlayerSettings.defaultInterfaceOrientation = orientation;
        PlayerSettings.bundleVersion = version;
        PlayerSettings.companyName = company;
        PlayerSettings.productName = product;
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] { icon });
        // PlayerSettings.SetPlatformIcons
    }

    private void Save()
    {
        var scriptable          = GetAllInstances<SaveScriptable>();
        scriptable.Company      = company;
        scriptable.Product      = product;
        scriptable.Version      = version;
        scriptable.icon         = icon;
        scriptable.orientation  = orientation;
    }
    private static void Load()
    {
        var scriptable = GetAllInstances<SaveScriptable>();
        company     =   scriptable.Company;
        product     =   scriptable.Product;
        version     =   scriptable.Version;
        icon        =   scriptable.icon;
        orientation =   scriptable.orientation;
    }


    private static T GetAllInstances<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }
}
