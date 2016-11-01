using System.Drawing;

namespace adrilight.DesktopDuplication
{
    /// <summary>
    /// Provides image data, cursor data, and image metadata about the retrieved desktop frame.
    /// </summary>
    public class DesktopFrame
    {
        /// <summary>
        /// Gets the bitmap representing the last retrieved desktop frame. This image spans the entire bounds of the specified monitor.
        /// </summary>
        public Bitmap DesktopImage { get; internal set; }

        /// <summary>
        /// Gets a list of the rectangles of pixels in the desktop image that the operating system moved to another location within the same image.
        /// </summary>
        /// <remarks>
        /// To produce a visually accurate copy of the desktop, an application must first process all moved regions before it processes updated regions.
        /// </remarks>
        public MovedRegion[] MovedRegions { get; internal set; }

        /// <summary>
        /// Returns the list of non-overlapping rectangles that indicate the areas of the desktop image that the operating system updated since the last retrieved frame.
        /// </summary>
        /// <remarks>
        /// To produce a visually accurate copy of the desktop, an application must first process all moved regions before it processes updated regions.
        /// </remarks>
        public Rectangle[] UpdatedRegions { get; internal set; }

        /// <summary>
        /// Gets whether the desktop image contains protected content that was already blacked out in the desktop image.
        /// </summary>
        public bool ProtectedContentMaskedOut { get; internal set; }
        
    }
}
