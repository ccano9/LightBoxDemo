using System;
using UnityEngine;

namespace IESLights
{
    public class IESToSpotlightCookie : MonoBehaviour
    {
        private Material _material;

        void OnDestroy()
        {
            // Clean up.
#if UNITY_EDITOR
            DestroyImmediate(_material);
#else
            Destroy(_material);
#endif
        }

        public void CreateSpotlightCookie(Texture2D iesTexture, IESData iesData, int resolution, out Texture2D cookie)
        {
            // Init the material.
            if(_material == null)
            {
                _material = new Material(Shader.Find("Hidden/IES/IESToSpotlightCookie"));
            }

            CalculateAndSetSpotHeight(iesData);
            SetShaderKeywords(iesData);

            cookie = CreateTexture(iesTexture, resolution);
        }

        private Texture2D CreateTexture(Texture2D iesTexture, int resolution)
        {
            RenderTexture renderTarget = RenderTexture.GetTemporary(resolution, resolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            renderTarget.DiscardContents();
            RenderTexture.active = renderTarget;
            Graphics.Blit(iesTexture, _material);
            Texture2D cookieTexture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);
            cookieTexture.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);

            // Clean up.
            RenderTexture.active = null;
            renderTarget.Release();
            return cookieTexture;
        }

        /// <summary>
        /// To make optimal usage of the spot light, the ies cookie is filled to fit the field of view of the spot.
        /// </summary>
        private void CalculateAndSetSpotHeight(IESData iesData)
        {
            // The spot height is defined by the max spot angle over the radius of the uv plane (0.5).
            float spotHeight = 0.5f / Mathf.Tan(iesData.HalfSpotlightFov * Mathf.Deg2Rad);
            _material.SetFloat("_SpotHeight", spotHeight);
        }

        private void SetShaderKeywords(IESData iesData)
        {
            // Set the appropriate keyword for whether or not the top half of the sphere is used.
            if(iesData.VerticalType == VerticalType.Top)
            {
                _material.EnableKeyword("TOP_VERTICAL");
            }
            else
            {
                _material.DisableKeyword("TOP_VERTICAL");
            }

            // Also set the approrpiate keyword for horizontal symmetry.
            if (iesData.HorizontalType == HorizontalType.None)
            {
                _material.DisableKeyword("QUAD_HORIZONTAL");
                _material.DisableKeyword("HALF_HORIZONTAL");
                _material.DisableKeyword("FULL_HORIZONTAL");
            }
            else if (iesData.HorizontalType == HorizontalType.Quadrant)
            {
                _material.EnableKeyword("QUAD_HORIZONTAL");
                _material.DisableKeyword("HALF_HORIZONTAL");
                _material.DisableKeyword("FULL_HORIZONTAL");
            }
            else if (iesData.HorizontalType == HorizontalType.Half)
            {
                _material.DisableKeyword("QUAD_HORIZONTAL");
                _material.EnableKeyword("HALF_HORIZONTAL");
                _material.DisableKeyword("FULL_HORIZONTAL");
            }
            else if (iesData.HorizontalType == HorizontalType.Full)
            {
                _material.DisableKeyword("QUAD_HORIZONTAL");
                _material.DisableKeyword("HALF_HORIZONTAL");
                _material.EnableKeyword("FULL_HORIZONTAL");
            }
        }
    }
}