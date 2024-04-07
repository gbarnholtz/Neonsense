using UnityEngine;
using UnityEngine.Rendering;

// Might be able to fix the lens flares rendering in the overlay camera

namespace CameraUtils
{
    [RequireComponent(typeof(LensFlareComponentSRP))]
    public class LensFlareController : MonoBehaviour
    {
        [SerializeField] Camera targetCamera;
 
        void OnCameraEnd(ScriptableRenderContext _, Camera currentCamera)
        {
            //if (targetCamera == null || currentCamera == targetCamera) reqLensFlare.enabled = false;
        }
 
        void OnCameraStart(ScriptableRenderContext _, Camera currentCamera)
        {
            //if (targetCamera == null || currentCamera == targetCamera) reqLensFlare.enabled = true;
        }
 
        void Awake()
        {
            RenderPipelineManager.beginCameraRendering += OnCameraStart;
            RenderPipelineManager.endCameraRendering += OnCameraEnd;
        }
        void OnDestroy()
        {
            RenderPipelineManager.beginCameraRendering -= OnCameraStart;
            RenderPipelineManager.endCameraRendering -= OnCameraEnd;
        }
    }