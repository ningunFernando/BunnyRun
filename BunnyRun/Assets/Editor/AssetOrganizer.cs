using UnityEditor;
using UnityEngine;
using System.IO;

public class GlobalAssetOrganizer : EditorWindow
{
    private static AssetOrganizerSettings settings;

    [MenuItem("Tools/Organize Assets by Settings")]
    public static void OrganizeAllAssets()
    {

        settings = Resources.Load<AssetOrganizerSettings>("AssetOrganizerSettings");

        if (settings == null)
        {
            Debug.LogError("❌ No se encontró 'AssetOrganizerSettings'. Crea uno en Resources.");
            return;
        }

        string[] allAssets = AssetDatabase.FindAssets("");
        int totalMoved = 0;

        foreach (string assetGUID in allAssets)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string fileExtension = Path.GetExtension(assetPath).ToLower();

            if (AssetDatabase.IsValidFolder(assetPath) || assetPath.Contains("/Editor/"))
                continue;

            bool moved = false;

            
            foreach (var mapping in settings.mappings)
            {
                if (fileName.StartsWith(mapping.prefix) &&
                    (mapping.extensions.Length == 0 || System.Array.Exists(mapping.extensions, ext => ext == fileExtension)))
                {
                    totalMoved += MoveAsset(assetPath, mapping.folderPath);
                    moved = true;
                    break;
                }
            }


            if (!moved && settings.organizeParticlePrefabs && fileExtension == ".prefab" && ContainsParticleSystem(assetPath))
            {
                totalMoved += MoveAsset(assetPath, "Assets/Particles");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log($"✅ Organización Finalizada: Archivos movidos: {totalMoved}");
    }

    private static int MoveAsset(string assetPath, string targetFolder)
    {
        if (!AssetDatabase.IsValidFolder(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        string fileName = Path.GetFileName(assetPath);
        string targetPath = Path.Combine(targetFolder, fileName);

        string result = AssetDatabase.MoveAsset(assetPath, targetPath);
        if (string.IsNullOrEmpty(result))
        {
            if (settings.showDetailedLogs)
                Debug.Log($"✅ Activo Movido: {assetPath} ➡️ {targetPath}");
            return 1;
        }
        else
        {
            Debug.LogWarning($"⚠️ Error al mover activo {assetPath}: {result}");
            return 0;
        }
    }

    private static bool ContainsParticleSystem(string assetPath)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        if (prefab != null && prefab.GetComponentInChildren<ParticleSystem>(true) != null)
        {
            if (settings.showDetailedLogs)
                Debug.Log($"💨 Sistema de partículas detectado: {assetPath}");
            return true;
        }
        return false;
    }
}
