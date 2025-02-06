using com.victorafael.pool;
using UnityEditor;
using UnityEngine;

public static class CreatePooledObjectData
{
    [MenuItem("Assets/Create/Pooled Object Data")]
    public static void Create()
    {
        GameObject selectedPrefab = null;
        if (Selection.activeObject is GameObject g)
        {
            PooledObject p = g.GetComponent<PooledObject>();
            if (p != null)
            {
                selectedPrefab = g;
            }
        }

        PooledObjectData pooledObjectData = ScriptableObject.CreateInstance<PooledObjectData>();

        SerializedObject serializedObject = new SerializedObject(pooledObjectData);
        serializedObject.FindProperty("prefab").objectReferenceValue = selectedPrefab;
        serializedObject.FindProperty("poolSize").intValue = 3;
        serializedObject.FindProperty("incrementSize").intValue = 1;
        serializedObject.ApplyModifiedProperties();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        if (selectedPrefab != null)
        {
            path = AssetDatabase.GetAssetPath(selectedPrefab);
            string assetPath = path.Replace(".prefab", "_PooledObjectData.asset");
            AssetDatabase.CreateAsset(pooledObjectData, assetPath);
        }
        else
        {
            AssetDatabase.CreateAsset(pooledObjectData, AssetDatabase.GenerateUniqueAssetPath($"{path}/NewPooledObjectData.asset"));
        }
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = pooledObjectData;
        // ScriptableObjectUtility.CreateAsset<PooledObjectData>();
    }
}