
using System;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenuForRenderPipeline(
    "Custom/Fog Post-Processing",
    typeof(UniversalRenderPipeline))]
public class FogPostProcess : VolumeComponent,
    IPostProcessComponent
{
    public ColorParameter fogColor = new(Color.white); 
    public ClampedFloatParameter fogDensity = new(0.5f, 0.0f, 1.0f);
    public ClampedFloatParameter fogOffset = new(0.0f, 0.0f, 100.0f);
    public ClampedFloatParameter farFogFactor = new(0.8f, 0.0f, 1.0f);

    public bool IsActive() => true;
    public bool IsTileCompatible() => true;
}