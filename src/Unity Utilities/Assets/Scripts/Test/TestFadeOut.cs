using UnityEngine;

public class TestFadeOut : MonoBehaviour
{
    public AlphaFade alphaFade;

    /*
     * In the example scene the fader is set to automatically start when the scene is loaded.
     * This script performs the fade out. 
     */

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // Set the start and end alphas to allow a dissolve to black effect
            alphaFade.alphaStart = 0f;
            alphaFade.alphaTarget = 1f;

            // Perform the fade
            alphaFade.DoFade();
        }
    }
}
