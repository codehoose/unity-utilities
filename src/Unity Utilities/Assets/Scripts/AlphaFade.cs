using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Alpha blend fade in / out. <seealso cref="TestFadeOut"/> for example usage.
/// </summary>
[RequireComponent(typeof(Image))]
public class AlphaFade : MonoBehaviour
{
    private Sprite _sprite;
    private Texture2D _texture;

    [Header("Alpha Values")]
    [Tooltip("The starting alpha value for the fade")]
    public float alphaStart = 1f;

    [Tooltip("The target (end) alpha value for the fade")]
    public float alphaTarget = 0f;

    [Tooltip("The duration of the fade")]
    public float fadeTime = 0.25f;

    [Header("General")]
    [Tooltip("Gives the cells a hard edge")]
    public bool pixelated;

    [Tooltip("Perform the fade at startup")]
    public bool fadeAtStart;

    [Tooltip("The fade type")]
    public FadeType fadeType = FadeType.TopToBottom;

    [Header("Geometry")]
    [Tooltip("Cell width and height in pixels")]
    public Vector2Int cellSize = new Vector2Int(16, 16);

    int CellCount
    {
        get { return cellSize.x * cellSize.y; }
    }

    void Awake()
    {
        CreateTexture();
        CreateSprite();
    }

    public void DoFade()
    {
        StartCoroutine(FadeOutSelector());
    }

    void Start()
    {
        if (!fadeAtStart)
            return;

        DoFade();
    }

    IEnumerator FadeOutSelector()
    {
        switch (fadeType)
        {
            case FadeType.BottomToTop:
                yield return DoLinearFade(0, cellSize.x, _ => _ < CellCount); ;
                break;
            case FadeType.Random:
                yield return DoRandomFade();
                break;
            case FadeType.TopToBottom:
                yield return DoLinearFade(CellCount - cellSize.x, -cellSize.x, _ => _ >= 0);
                break;
        }
    }

    IEnumerator DoRandomFade()
    {
        var cells = new int[CellCount];
        for (int i = 0; i < CellCount; i++)
            cells[i] = i;

        var randomCells = cells.OrderBy(x => UnityEngine.Random.Range(0, CellCount)).ToArray();
        foreach (var cell in randomCells)
        {
            StartCoroutine(SetPixel(cell, 1, fadeTime));
            yield return null;
        }
    }

    IEnumerator DoLinearFade(int start, int delta, Func<int, bool> test)
    {
        for (int i = start; test(i); i += delta)
        {
            StartCoroutine(SetPixel(i, cellSize.x, fadeTime));
            yield return null;
        }
    }

    IEnumerator SetPixel(int pixelIndex, int rowLength, float duration)
    {
        float time = 0f;
        while (time < 1f)
        {
            var alpha = Mathf.Lerp(alphaStart, alphaTarget, time);
            _texture.SetPixelAlpha(pixelIndex, rowLength, alpha);
            time += Time.deltaTime / duration;
            yield return null;
        }

        _texture.SetPixelAlpha(pixelIndex, rowLength, alphaTarget);
    }

    private void CreateTexture()
    {
        _texture = new Texture2D(cellSize.x, cellSize.y);
        if (pixelated)
        {
            _texture.anisoLevel = 0;
            _texture.filterMode = FilterMode.Point;
        }
        _texture.wrapMode = TextureWrapMode.Clamp;

        var pixels = _texture.GetPixels();
        for (int i = 0; i < CellCount; i++)
        {
            pixels[i] = Color.black;
        }

        _texture.SetPixels(pixels);
        _texture.Apply();
    }

    private void CreateSprite()
    {
        _sprite = Sprite.Create(_texture, new Rect(0, 0, cellSize.x, cellSize.y), Vector2.zero);
        _sprite.name = "Fade Sprite";

        var imageComponent = GetComponent<Image>();
        imageComponent.sprite = _sprite;
    }
}
