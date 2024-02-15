using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FogRendererFeature : ScriptableRendererFeature
{
    private class FogPass : ScriptableRenderPass
    {
        private Material _material;
        private int _renderTargetID = Shader.PropertyToID("_Temp");
        private RenderTargetIdentifier _source, _effect;

        public FogPass()
        {
            if (!_material)
            {
                _material = 
                    CoreUtils.CreateEngineMaterial("Custom Post-Processing/" +
                    "Screen Space Fog");
            }

            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void OnCameraSetup(CommandBuffer cmd, 
            ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = 
                renderingData.cameraData.cameraTargetDescriptor;
            _source = renderingData.cameraData.renderer.cameraColorTarget;            
            cmd.GetTemporaryRT(_renderTargetID, descriptor, 
                FilterMode.Bilinear);
            _effect = new RenderTargetIdentifier(_renderTargetID);
        }

        public override void Execute(ScriptableRenderContext context, 
            ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = 
                CommandBufferPool.Get("FogRendererFeature");
            VolumeStack volumes = VolumeManager.instance.stack;
            FogPostProcess fog = 
                volumes.GetComponent<FogPostProcess>();
            
            if (fog.IsActive())
            {
                _material.SetColor("_FogColor", 
                    fog.fogColor.GetValue<Color>());
                _material.SetFloat("_FogDensity", 
                    fog.fogDensity.GetValue<float>());
                _material.SetFloat("_FogOffset", 
                    fog.fogOffset.GetValue<float>());
                _material.SetFloat("_FarFogFactor", 
                    fog.farFogFactor.GetValue<float>());
                
                Blit(commandBuffer, _source, _effect, _material, 0);
                Blit(commandBuffer, _effect, _source);
            }

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_renderTargetID);
        }
    }

    private FogPass _fogPass;

    public override void AddRenderPasses(ScriptableRenderer renderer, 
        ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_fogPass);
    }

    public override void Create()
    {
        _fogPass = new();
    }
}
