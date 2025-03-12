using UnityEngine;

[CreateAssetMenu(fileName = "ShaderGlobalSetting", menuName="Scriptable Objects/ ShaderGlobalSetting")]

public class ShaderGlobalSetting : ScriptableObject
{
    [Header("Outline")]
    [SerializeField] private Color outlineColor = Color.red;
    [SerializeField] private float normalsTreshold = 1f;
    [SerializeField] private float colorThreshold = 1f;
    [Header("Vertex Offset")]
    [SerializeField] private Vector3 curveSettings = new Vector3(0.003f, 0.0f, 4.0f); 
    [Header("Dithering")]
    [SerializeField] private float Spread = 10f;
    [SerializeField] private float Resolution = 1f;
    [Header("Cell Shader")]
    [SerializeField] private Color cellShaderColor = Color.red;
    [SerializeField] private float Shades = 0.25f;
    [SerializeField] private float min = 0.26f;
    [SerializeField] private float max = 1.94f;


    private void OnValidate() 
    {
        //Cell Shader
        Shader.SetGlobalColor("_Global_CellShaderColor", cellShaderColor);    
        Shader.SetGlobalFloat("_Global_CellShaderShades", Shades);
        Shader.SetGlobalFloat("_Global_CellShadermin", min);
        Shader.SetGlobalFloat("_Global_CellShadermax", max);
        //Outline
        Shader.SetGlobalColor("_Global_OutlineColor", outlineColor);
        Shader.SetGlobalFloat("_Global_OutlineNormalsTreshold", normalsTreshold);
        Shader.SetGlobalFloat("_Global_OutlineColorThreshold", colorThreshold);
        //Vertex Offset
        Shader.SetGlobalVector("_Global_VertexOffsetCurveSettings", curveSettings);
        //Dithering
        Shader.SetGlobalFloat("_Global_DitheringSpread", Spread);
        Shader.SetGlobalFloat("_Global_DitheringResolution", Resolution);
    }
}