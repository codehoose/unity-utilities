using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Test script.
[RequireComponent(typeof(Image))]
public class FadeImageRandom : MonoBehaviour {

    Sprite _sprite;
    Texture2D _texture;
    
    IEnumerator Start()
    {
        var component = GetComponent<Image>();
        _texture = new Texture2D(16, 16);
        _texture.anisoLevel = 0;
        _texture.wrapMode = TextureWrapMode.Clamp;

        var colours = _texture.GetPixels();
        for (int i =0; i < colours.Length; i++)
        {
            colours[i] = Color.black;
        }
        _texture.SetPixels(colours);
        _texture.Apply();

        _sprite = Sprite.Create(_texture, new Rect(0, 0, 16, 16), Vector2.zero);
        _sprite.name = "Fade Texture";
        component.sprite = _sprite;

        yield return new WaitForSeconds(3);

        for (int i = 0; i < 256; i += 16)
        //for (int i = 256 - 16; i >= 0; i -= 16)
        {
            StartCoroutine(FadeOnePixel(i));
            yield return null;
        }
    }

    private IEnumerator FadeOnePixel(int i)
    {
        float time = 0f;

        while (time < 1f)
        {
            var pixels = _texture.GetPixels();

            for (int x = i; x < i + 16; x++)
                pixels[x].a = 1 - time;
            _texture.SetPixels(pixels);
            _texture.Apply();

            time += Time.deltaTime;
            yield return null;
        }
    }
}
