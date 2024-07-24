using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class ItemLoader
{
    [MenuItem("Load/All Map")]
    public static void LoadMap()
    {
        string mapSprite = "Assets/G_Asset/Local/Map";
        string storePath = "Assets/G_Asset/Internal/Map";
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { mapSprite });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            string storePathName = $"{storePath}/{texture.name}.asset";
            MapScriptableObject map = AssetDatabase.LoadAssetAtPath<MapScriptableObject>(storePathName);
            if (map == null)
            {
                map = ScriptableObject.CreateInstance<MapScriptableObject>();
                AssetDatabase.CreateAsset(map, storePathName);
            }
            map.mapTexture = texture;
            EditorUtility.SetDirty(map);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        LoadMapPrefab();

        AttachMap();
    }
    public static void AttachMap()
    {
        string storePath = "Assets/G_Asset/Internal/Map";
        string[] guids = AssetDatabase.FindAssets("t:MapScriptableObject", new[] { storePath });
        List<MapScriptableObject> maps = new();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MapScriptableObject map = AssetDatabase.LoadAssetAtPath<MapScriptableObject>(path);
            maps.Add(map);
        }
        MapGenerater mapGenerater = GameObject.FindObjectOfType<MapGenerater>();
        if (mapGenerater != null)
        {
            mapGenerater.SetupMap(maps);
            mapGenerater.LoadMap();
        }
    }
    public static void LoadMapPrefab()
    {
        string ground = "Assets/G_Asset/Internal/Map_Prefabs/ground";
        string soil = "Assets/G_Asset/Internal/Map_Prefabs/soil";
        string[] groundGuids = AssetDatabase.FindAssets("t:GameObject", new[] { ground });
        List<GameObject> grounds = new();
        foreach (string guid in groundGuids)
        {
            string groundGuid = AssetDatabase.GUIDToAssetPath(guid);
            GameObject groundItem = AssetDatabase.LoadAssetAtPath<GameObject>(groundGuid);
            grounds.Add(groundItem);
        }
        GameObject soilPrefabs = null;
        string[] soilGuids = AssetDatabase.FindAssets("t:GameObject", new[] { soil });
        foreach (string guid in soilGuids)
        {
            string groundGuid = AssetDatabase.GUIDToAssetPath(guid);
            soilPrefabs = AssetDatabase.LoadAssetAtPath<GameObject>(groundGuid);
        }
        MapGenerater mapGenerater = GameObject.FindObjectOfType<MapGenerater>();
        if (mapGenerater != null)
        {
            mapGenerater.SetUpPrefabs(grounds);
            mapGenerater.SetUpPrefabs(new() { soilPrefabs }, true);
        }
    }
}
