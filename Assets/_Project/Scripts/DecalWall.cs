using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using RenderTextureFormat = UnityEngine.RenderTextureFormat;

public class DecalWall : MonoBehaviour
{
    [SerializeField] private Vector2 decalSize = new Vector2(0.2f, 0.2f);
    [SerializeField] private Material blitMaterial;
    
    private RenderTexture renderTexture;
    private RTHandle rtHandle;
    private Material wallMaterial;
    private Renderer wallRenderer;
    
    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    
    private void Start()
    {
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.R8);
            renderTexture.Create();
        }
        
        rtHandle = RTHandles.Alloc(renderTexture);
        
        wallRenderer = GetComponent<Renderer>();
        wallMaterial = wallRenderer.material;

        GraphicsBlitURP.ClearColor(rtHandle, true);
    }

    private void OnDestroy()
    {
        renderTexture?.Release();
    }

    public void OnCollide(BoxCollider collider)
    {
        if (!collider.CompareTag("Bullet")) return;
        
        wallMaterial.SetTexture(BaseMap, renderTexture);
        
        Vector3 hitPoint = collider.ClosestPoint(transform.position);
        Rect uv = GetUVFromHitPoint(hitPoint);
        GraphicsBlitURP.Blit(rtHandle, blitMaterial, uv);
    }
    
    private Rect GetUVFromHitPoint(Vector3 hitPoint)
    {
        Vector2 localPoint = transform.InverseTransformPoint(hitPoint);

        Vector2 sizeWithRatio = decalSize;
        sizeWithRatio.y = decalSize.y * this.transform.localScale.x / this.transform.localScale.y;

        Vector2 uv = localPoint + new Vector2(0.5f, 0.5f) - sizeWithRatio * 0.5f;
        
        int renderTextureWidth = renderTexture.width;
        int renderTextureHeight = renderTexture.height;
        return new Rect
        {
            x = uv.x * renderTextureWidth,
            y = uv.y * renderTextureHeight,
            width = sizeWithRatio.x * renderTextureWidth,
            height = sizeWithRatio.y * renderTextureHeight
        };
    }

}
