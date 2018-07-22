using UnityEngine;

/// <summary>
/// Extension methods for texture 2D. It's called 'ColorArrayExtensions' in the video, but that's
/// just bad naming on my part, sorry.
/// </summary>
public static class ColorArrayExtensions
{
    /// <summary>
    /// Set the alpha colour of a pixel in a texture array.
    /// </summary>
    /// <param name="texture">Texture.</param>
    /// <param name="index">Pixel index.</param>
    /// <param name="rowLength">The number of pixels adjacent to set the alpha. 1 for a single cell.</param>
    /// <param name="alpha">Alpha value.</param>
    public static void SetPixelAlpha(this Texture2D texture, int index, int rowLength, float alpha)
    {
        var pixels = texture.GetPixels();

        for (int i = index; i < index + rowLength; i++)
            pixels[i].a = alpha;

        texture.SetPixels(pixels);
        texture.Apply();
    }
}
