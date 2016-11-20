using System.IO;
using UnityEngine;

namespace IESLights
{
    [RequireComponent(typeof(IESToCubemap)), RequireComponent(typeof(IESToSpotlightCookie))]
    public class IESConverter : MonoBehaviour
    {
        public Texture2D IesTexture;
        public int Resolution = 512;
        [Tooltip("Take the natural log of candela values before normalization, to reduce the impact of outliers. This results in a more detailed IES light, without the most luminous region blacking out the rest.")]
        public bool SquashHistogram = true;

        /// <summary>
        /// Converts an IES file to either a point or spot light cookie.
        /// </summary>
        public void ConvertIES(string filePath, string targetPath, bool createSpotlightCookies, out Cubemap pointLightCookie, out Texture2D spotlightCookie, out string targetFilename)
        {
            // Parse the ies data.
            IESData iesData = ParseIES.Parse(filePath, SquashHistogram);
            IesTexture = IESToTexture.ConvertIesData(iesData, Resolution);

            // If spot light cookie creation is enabled, check if the ies data can be projected into a spot light cookie.
            // Only half the sphere may be provided if the ies data is to fit inside a spot light cookie.
            if (createSpotlightCookies && iesData.VerticalType != VerticalType.Full)
            {
                pointLightCookie = null;
                GetComponent<IESToSpotlightCookie>().CreateSpotlightCookie(IesTexture, iesData, Resolution, out spotlightCookie);
            }
            // Create a point light cookie cubemap in all other cases.
            else
            {
                spotlightCookie = null;
                GetComponent<IESToCubemap>().CreateCubemap(IesTexture, iesData, Resolution, out pointLightCookie);
            }

            // Create the target file name and required folders.
            BuildTargetFilename(Path.GetFileNameWithoutExtension(filePath), targetPath, pointLightCookie != null, SquashHistogram, out targetFilename);
        }

        private void BuildTargetFilename(string name, string folderHierarchy, bool isCubemap, bool squashHistogram, out string targetFilePath)
        {
            if (!Directory.Exists(Path.Combine(Application.dataPath, string.Format("IES/Imports/{0}", folderHierarchy))))
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, string.Format("IES/Imports/{0}", folderHierarchy)));
            }

            // If this in an enhanced import, add the [E] prefix.
            targetFilePath = Path.Combine(Path.Combine("Assets/IES/Imports/", folderHierarchy), string.Format("{0}{1}.{2}", squashHistogram ? "[E] " : "", name, isCubemap ? "cubemap" : "asset"));
            if (File.Exists(targetFilePath))
            {
                File.Delete(targetFilePath);
            }
        }
    }
}