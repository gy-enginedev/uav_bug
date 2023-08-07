using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UAVBugTest : MonoBehaviour
{
    // Start is called before the first frame update
    public RawImage m_UIImage;

    ComputeShader shader;    
    RenderTexture m_ResultTexture;
    ComputeBuffer m_ConditinalBuffer;
    int m_Kernel;
    void Start()
    {
        RenderTextureDescriptor desc_ = new RenderTextureDescriptor()
        {
            width             = 1024,
            height            = 1024,
            depthBufferBits   = 0,
            dimension         = TextureDimension.Tex2D,
            colorFormat       = RenderTextureFormat.ARGB32,
            useMipMap         = false,
            autoGenerateMips  = false,
            bindMS            = false,
            enableRandomWrite = true,
            volumeDepth       = 1,
            msaaSamples       = 1
        };
        m_ResultTexture = new RenderTexture(desc_);
        m_UIImage.texture = m_ResultTexture;

        int[] data = new int[] { 1 };
        m_ConditinalBuffer = new ComputeBuffer(1, sizeof(int));
        m_ConditinalBuffer.SetData(data);
        shader = Resources.Load<ComputeShader>("UAVConditional");
        m_Kernel = shader.FindKernel("UVABugTest");
        shader.SetTexture(m_Kernel, "ResultTexture_RW", m_ResultTexture);
        shader.SetBuffer(m_Kernel, "Conditinal_RW", m_ConditinalBuffer);
       
    }

    // Update is called once per frame
    void Update()
    {
        shader.Dispatch(m_Kernel, 1000000 / 64, 1, 1 );
    }

    void OnDestroy()
    {
        m_ResultTexture.Release();
        m_ConditinalBuffer.Dispose();
    }
}
