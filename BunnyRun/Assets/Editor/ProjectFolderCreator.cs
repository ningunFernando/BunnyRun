using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProjectFolderCreator : EditorWindow
{

    [MenuItem("Tools/Create Project Folder")]
    public static void CreateProjectFolders()
    {

        string[] folders = {
            "Animations",
            "Audios",
            "Materials",
            "Models",
            "Prefabs",
            "Levels",
            "Scripts",
            "Shaders",
            "Textures",
            "VFX"
        };

        foreach (string folder in folders)
        {
            if (!AssetDatabase.IsValidFolder("Assets/" + folder))
            {
                AssetDatabase.CreateFolder("Assets", folder);
                Debug.Log($"Carpeta creada: {folder}");
            }
            else
            {
                Debug.Log($"La carpeta {folder} ya existia");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Carpetas creadas exitosamente");

    }

}
