using System.Collections.Generic;

namespace IESLights
{
    public class IESData
    {
        /// <summary>
        /// The angles at which candela measurements were taken, from the center of the photometric sphere to the hull. Each vertical slice uses these same angles.
        /// </summary>
        public List<float> VerticalAngles { get; set; }
        /// <summary>
        /// The angles around the polar axis at which vertical slices were measured.
        /// </summary>
        public List<float> HorizontalAngles { get; set; }
        /// <summary>
        /// Nested list of all candela values - the inner list are the actual candela measurements done in a vertical slice, the outer list groups them per vertical slice for each horizontal angle.
        /// </summary>
        public List<List<float>> CandelaValues { get; set; }
        /// <summary>
        /// The normalized candela values. These values can be the natural log of the actual values, depending on whether or not enhanced mode was enabled.
        /// </summary>
        public List<List<float>> NormalizedValues { get; set; }

        public PhotometricType PhotometricType { get; set; }
        /// <summary>
        /// Convenient way of checking what range of vertical angles is provided.
        /// </summary>
        public VerticalType VerticalType { get; set; }
        /// <summary>
        /// Convenient way of checking what range of horizontal angles is provided.
        /// </summary>
        public HorizontalType HorizontalType { get; set; }

        /// <summary>
        /// The field of view of the spot light. -1 if the light can't be represented as a spot light.
        /// </summary>
        public float HalfSpotlightFov { get; set; }
    }

    /// <summary>
    /// Three photometric types are defined in the IES standard, but in reality only C is ever used.
    /// </summary>
    public enum PhotometricType
    {
        TypeC = 1,
        typeB = 2,
        TypeA = 3
    }

    /// <summary>
    /// An IES file can either specify the full 180 degree range of angles, or only 90, in which case the top half of the photometric sphere stay black.
    /// </summary>
    public enum VerticalType
    {
        Full,
        Bottom,
        Top
    }

    /// <summary>
    /// An IES file can either specify the full 360 range of horizontal angles, have regular symmetry, quadrant symmetry, or specify no horizontal angles at all, in which case a single vertical slice is 
    /// lathed around the polar axis.
    /// </summary>
    public enum HorizontalType
    {
        Full,
        Half,
        Quadrant,
        None
    }
}
