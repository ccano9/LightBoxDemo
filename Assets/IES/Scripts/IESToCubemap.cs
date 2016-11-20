using UnityEngine;

namespace IESLights
{
    public class IESToCubemap : MonoBehaviour
    {
        private Material _iesMaterial;

        public void CreateCubemap(Texture2D iesTexture, IESData iesData, int resolution, out Cubemap cubemap)
        {
            if (_iesMaterial == null)
            {
                _iesMaterial = GetComponent<Renderer>().sharedMaterial;
            }
            _iesMaterial.mainTexture = iesTexture;

            SetShaderKeywords(iesData, _iesMaterial);

            CreateCubemap(resolution, out cubemap);

            // Clean up.
            _iesMaterial.mainTexture = null;
            DestroyImmediate(iesTexture);
        }

        private void SetShaderKeywords(IESData iesData, Material iesMaterial)
        {
            // Fill either the entire sphere or only the bottom of top half, depending on the vertical angles described in the file.
            if (iesData.VerticalType == VerticalType.Bottom)
            {
                iesMaterial.EnableKeyword("BOTTOM_VERTICAL");
                iesMaterial.DisableKeyword("TOP_VERTICAL");
                iesMaterial.DisableKeyword("FULL_VERTICAL");
            }
            else if(iesData.VerticalType == VerticalType.Top)
            {
                iesMaterial.EnableKeyword("TOP_VERTICAL");
                iesMaterial.DisableKeyword("BOTTOM_VERTICAL");
                iesMaterial.DisableKeyword("FULL_VERTICAL");
            }
            else
            {
                iesMaterial.DisableKeyword("TOP_VERTICAL");
                iesMaterial.DisableKeyword("BOTTOM_VERTICAL");
                iesMaterial.EnableKeyword("FULL_VERTICAL");
            }

            // Also set the approrpiate keyword for horizontal symmetry.
            if (iesData.HorizontalType == HorizontalType.None)
            {
                iesMaterial.DisableKeyword("QUAD_HORIZONTAL");
                iesMaterial.DisableKeyword("HALF_HORIZONTAL");
                iesMaterial.DisableKeyword("FULL_HORIZONTAL");
            }
            else if (iesData.HorizontalType == HorizontalType.Quadrant)
            {
                iesMaterial.EnableKeyword("QUAD_HORIZONTAL");
                iesMaterial.DisableKeyword("HALF_HORIZONTAL");
                iesMaterial.DisableKeyword("FULL_HORIZONTAL");
            }
            else if (iesData.HorizontalType == HorizontalType.Half)
            {
                iesMaterial.DisableKeyword("QUAD_HORIZONTAL");
                iesMaterial.EnableKeyword("HALF_HORIZONTAL");
                iesMaterial.DisableKeyword("FULL_HORIZONTAL");
            }
            else if (iesData.HorizontalType == HorizontalType.Full)
            {
                iesMaterial.DisableKeyword("QUAD_HORIZONTAL");
                iesMaterial.DisableKeyword("HALF_HORIZONTAL");
                iesMaterial.EnableKeyword("FULL_HORIZONTAL");
            }
        }

        private void CreateCubemap(int resolution, out Cubemap cubemap)
        {
            cubemap = new Cubemap(resolution, TextureFormat.ARGB32, false); // Mip maps in cookies can lead to artefacts - they are disabled to be sure. https://en.wikibooks.org/wiki/Cg_Programming/Unity/Cookies
            GetComponent<Camera>().RenderToCubemap(cubemap);
        }
    }
}