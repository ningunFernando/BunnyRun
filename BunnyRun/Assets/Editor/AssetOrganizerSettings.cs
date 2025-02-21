using UnityEngine;

[System.Serializable]
public class FolderMapping
{
    public string prefix;            // Prefijo (ejemplo: M_)
    public string folderPath;        // Carpeta (ejemplo: Assets/Materials)
    public string[] extensions;      // Extensiones (ejemplo: .mat, .prefab)
}

[CreateAssetMenu(fileName = "AssetOrganizerSettings", menuName = "Tools/Asset Organizer Settings")]
public class AssetOrganizerSettings : ScriptableObject
{
    [Header("Mappings")]
    public FolderMapping[] mappings;

    [Header("Opciones Adicionales")]
    public bool organizeParticlePrefabs = true;  // Organizar prefabs con ParticleSystem
    public bool showDetailedLogs = true;         // Mostrar logs detallados
}
