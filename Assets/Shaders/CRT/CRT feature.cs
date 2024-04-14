using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CRTfeature : ScriptableRendererFeature
{
    enum TextureQuality
    {
        Default,
        High,
        Low
    }
    [SerializeField] TextureQuality textureQuality;
    [SerializeField] Shader pixelShader;

    
    [Space(10)]
    [Header("ScanLines")]
    [SerializeField][Range(0,1)] float intensity = 0.5f;
    [SerializeField] [Range(0, 1)] float intensityRandomness = 0;
    [SerializeField] [Min(1)] int lineSize = 1;
    [SerializeField] float scrollSpeed = 0;
    [SerializeField] [Min(0)] float lineRandomOffset = 0;

    [Space(10)]
    [Header("Noise")]
    [SerializeField] [Min(1)] float NoiseScale = 500;
    [SerializeField] float NoiseIntensity = 0;
    [SerializeField] Vector2 LineCurveNoiseScale = new Vector2(1,1);
    [SerializeField] float LineCurveNoiseIntensity = 1;

    [Space(10)]
    [Header("Sidebars")]
    [SerializeField] float DistortionIntensity = 1;
    [SerializeField] [Min(0)] float Vignette_width = 0;

    [Space(10)]
    [Header("Colors")]
    [SerializeField] float ChromaticAberrationMiddle = 0;
    [SerializeField] float ChromaticAberrationEnd = 0;

    class CRTPass : ScriptableRenderPass
    {
        TextureQuality textureQuality;
        Material mat;

        ProfilingSampler _profilingSampler = new ProfilingSampler("CRT");

        int temporaryBufferID = Shader.PropertyToID("_CRTTemporaryBuffer");
        RenderTargetIdentifier tempBuffer;

        public CRTPass(TextureQuality quality, Material pixelMat)
        {
            textureQuality = quality;
            mat = pixelMat;

            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.enableRandomWrite = true;
            if (textureQuality == TextureQuality.Default) descriptor.colorFormat = RenderTextureFormat.RGB111110Float;
            else if (textureQuality == TextureQuality.High) descriptor.colorFormat = RenderTextureFormat.ARGB64;
            else descriptor.colorFormat = RenderTextureFormat.ARGB32;
            descriptor.msaaSamples = 1;
            descriptor.depthStencilFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.None;

            cmd.GetTemporaryRT(temporaryBufferID, descriptor, FilterMode.Point);
            tempBuffer = new RenderTargetIdentifier(temporaryBufferID);

        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var camera = renderingData.cameraData.camera;
            if (camera.cameraType != CameraType.Game) return;

            CommandBuffer cmd = CommandBufferPool.Get("CRTPass");

            using (new ProfilingScope(cmd, _profilingSampler))
            {
                cmd.Blit(renderingData.cameraData.renderer.cameraColorTargetHandle, tempBuffer, mat);
                cmd.Blit(tempBuffer, renderingData.cameraData.renderer.cameraColorTargetHandle);
            }


            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }
    }

    

    CRTPass pass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }

    public override void Create()
    {
        Material pixelMat = new Material(pixelShader);
        pixelMat.SetFloat("_Intensity", intensity);
        pixelMat.SetFloat("_LineSize", lineSize);
        pixelMat.SetFloat("_IntensityRandomness", intensityRandomness);
        pixelMat.SetFloat("_ScrollSpeed", scrollSpeed);
        pixelMat.SetFloat("_NoiseScale", NoiseScale);
        pixelMat.SetFloat("_NoiseIntensity", NoiseIntensity);
        pixelMat.SetFloat("_LineRandomOffset", lineRandomOffset);
        pixelMat.SetVector("_LineCurveNoiseScale", LineCurveNoiseScale);
        pixelMat.SetFloat("_LineCurveNoiseIntensity", LineCurveNoiseIntensity);
        pixelMat.SetFloat("_Distortion_Intensity", DistortionIntensity);
        pixelMat.SetFloat("_Vignette_width", Vignette_width);
        pixelMat.SetFloat("_ChromaticAberrationMiddle", ChromaticAberrationMiddle);
        pixelMat.SetFloat("_ChromaticAberrationEnd", ChromaticAberrationEnd);

        pass = new CRTPass(textureQuality, pixelMat);
    }
}
