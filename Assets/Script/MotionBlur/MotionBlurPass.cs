using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MotionBlurPass : ScriptableRenderPass
{
    MotionBlurFeature.Settings settings;
    static RenderTexture prevFrame;
    RTHandle tempHandle;

    public MotionBlurPass(MotionBlurFeature.Settings s)
    {
        settings = s;
        requiresIntermediateTexture = true;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        var desc = renderingData.cameraData.cameraTargetDescriptor;
        desc.depthBufferBits = 0;
        RenderingUtils.ReAllocateIfNeeded(ref tempHandle, desc, name: "_TempBlur");
    }

    [System.Obsolete]
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (settings.material == null) return;

        var cameraData = renderingData.cameraData;
        var desc = cameraData.cameraTargetDescriptor;
        desc.depthBufferBits = 0;

        CommandBuffer cmd = CommandBufferPool.Get("MotionBlurPingPong");

        RTHandle src = cameraData.renderer.cameraColorTargetHandle;

        if (prevFrame == null || prevFrame.width != desc.width || prevFrame.height != desc.height)
        {
            prevFrame?.Release();
            prevFrame = new RenderTexture(desc.width, desc.height, 0, desc.colorFormat);
            Blitter.BlitCameraTexture(cmd, src, tempHandle);
            cmd.CopyTexture(tempHandle, prevFrame);
        }

        settings.material.SetTexture("_PrevTex", prevFrame);
        settings.material.SetFloat("_Alpha", settings.alpha);

        Blitter.BlitCameraTexture(cmd, src, tempHandle, settings.material, 0);
        Blitter.BlitCameraTexture(cmd, tempHandle, src);
        cmd.CopyTexture(tempHandle, prevFrame);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd) { }
}