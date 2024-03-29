using UnityEngine;

/*
 * This script continuously loops through the color spectrum and applies the resulting color with a specified saturation to the ambient light every frame.
 * The colorChangeSpeed variable controls the speed of color change, while the saturation variable controls the saturation level of the colors.
 */

public class ColorSpectrum : MonoBehaviour
{
    // Speed of color change
    [SerializeField] private float colorChangeSpeed = 0.1f;
    
    // Saturation of the color
    [SerializeField] private float saturation = 0.8f;
    
    // Brightness (value) of the color
    [SerializeField] private float brightness = 0.5f;

    // Starting color
    private float hue = 0f;

    void Update()
    {
        // Increment the hue value based on time and speed
        hue += colorChangeSpeed * Time.deltaTime;

        // Wrap the hue value to keep it within the range [0, 1]
        if (hue > 1f)
            hue -= 1f;

        // Convert the hue value to RGB color with specified saturation and brightness
        Color newColor = Color.HSVToRGB(hue, saturation, brightness);

        // Apply the new color to the ambient light
        RenderSettings.ambientLight = newColor;
    }
}