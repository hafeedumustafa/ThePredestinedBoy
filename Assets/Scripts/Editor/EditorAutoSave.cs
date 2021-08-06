using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditorAutoSave
{
    static EditorAutoSave() {
        EditorApplication.playModeStateChanged += SaveOnPlay;
    }

    private static void SaveOnPlay(PlayModeStateChange state) {
        if(state == PlayModeStateChange.ExitingEditMode) {
            Debug.Log("autosaving scene...");
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
            Debug.Log("autosaved scene!");
        }
    }
}