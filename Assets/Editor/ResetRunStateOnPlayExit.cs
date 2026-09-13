using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Play mode writes run progress straight into these assets, and it kept getting committed.
// Resetting them when you return to edit mode keeps the files at their defaults on disk.
[InitializeOnLoad]
public static class ResetRunStateOnPlayExit
{
    static ResetRunStateOnPlayExit()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state != PlayModeStateChange.EnteredEditMode) return;

            foreach (CharacterStats a in Load<CharacterStats>()) { a.ResetStats(); EditorUtility.SetDirty(a); }
            foreach (LoadoutState a in Load<LoadoutState>()) { a.ResetLoadout(); EditorUtility.SetDirty(a); }
            foreach (SceneState a in Load<SceneState>()) { a.ResetStates(); EditorUtility.SetDirty(a); }
            foreach (EnemySystem a in Load<EnemySystem>()) { a.ResetEnemies(); EditorUtility.SetDirty(a); }

            AssetDatabase.SaveAssets();
        };
    }

    private static IEnumerable<T> Load<T>() where T : Object
    {
        foreach (string guid in AssetDatabase.FindAssets("t:" + typeof(T).Name))
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
            if (asset != null) yield return asset;
        }
    }
}
