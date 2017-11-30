using System;
using UnityEditor;
using UnityEngine;

public class MyWindow : EditorWindow
{
    SerializedObject winObj;
    public Texture2D map;
    SerializedProperty propMap;

    public GameObject begin;
    SerializedProperty propBegin;

    public GameObject prefab;
    SerializedProperty propPrefab;

    private bool dropDown = false;
    string size = "0";
    private bool dropDownElement = false;


    public ColorToPrefaf[] colorMappings;
    SerializedProperty propColorMappings;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(MyWindow));
    }

    public void OnEnable()
    {
        map = null;

        winObj = new SerializedObject(this);
        propMap = winObj.FindProperty("map");
        propPrefab = winObj.FindProperty("prefab");
        propBegin = winObj.FindProperty("begin");
        propColorMappings = winObj.FindProperty("colorMappings");

    }

    public void OnGUI()
    {
        int intSize;
        GUILayout.Label("Level Generator", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(propMap, new GUIContent("Map"));
        EditorGUILayout.PropertyField(propBegin, new GUIContent("Begin Point"));

        EditorGUILayout.PropertyField(propColorMappings, new GUIContent("colorMapping"), true);

        if (GUILayout.Button("Générer"))
        {
            GenerateLevel();
        }

        if (GUILayout.Button("Clear Level"))
        {
            ClearLevel();
        }

        winObj.ApplyModifiedProperties();
    }

    void GenerateLevel()
    {

        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);
        if (pixelColor.a == 0)
        {
            //transparence balec
            return;
        }

        foreach (ColorToPrefaf colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Vector2 position = new Vector2(x, y);
                Instantiate(colorMapping.prefab, position, Quaternion.identity, begin.transform);
            }
        }
    }

    void ClearLevel()
    {
        while(begin.transform.childCount != 0)
        {
            DestroyImmediate(begin.transform.GetChild(0).gameObject);
        }

    }
}