#ifndef GLOBAL_VARIABLES_DEFINED
#define GLOBAL_VARIABLES_DEFINED

//Cell Shader 
float4 _Global_CellShaderColor;
float _Global_CellShaderShades;
float _Global_CellShadermin;
float _Global_CellShadermax;

//outline
float4 _Global_OutlineColor;
float _Global_OutlineNormalsTreshold;
float _Global_OutlineColorThreshold;

//vertex offset
float3 _Global_VertexOffsetCurveSettings;

//Dithering Settings
float _Global_DitheringSpread;
float _Global_DitheringResolution;

void GetCellShaderVariables_half(out float4 cellShaderColor, out float cellShaderShades, out float cellShaderMin, out float cellShaderMax)
{
    cellShaderColor = _Global_CellShaderColor;
    cellShaderShades = _Global_CellShaderShades;
    cellShaderMin = _Global_CellShadermin;
    cellShaderMax = _Global_CellShadermax;
}

void GetOutlineVariables_float(out float4 outlineColor, out float outlineNormalsTreshold, out float outlineColorTreshold)
{
    outlineColor = _Global_OutlineColor;
    outlineNormalsTreshold = _Global_OutlineNormalsTreshold;
    outlineColorTreshold = _Global_OutlineColorThreshold;
}

void GetVertexOffsetCurveSettings_half(out float3 vertexOffsetCurveSettings)
{
    vertexOffsetCurveSettings = _Global_VertexOffsetCurveSettings;
}

void GetDitheringSettings_float(out float ditheringSpread, out float ditheringResolution)
{
    ditheringSpread = _Global_DitheringSpread;
    ditheringResolution = _Global_DitheringResolution;
}
#endif 