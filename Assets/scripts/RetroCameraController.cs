using UnityEngine;

[ExecuteAlways] // Чтобы изменения были видны прямо в редакторе без запуска игры
[RequireComponent(typeof(Camera))]
public class RetroCameraController : MonoBehaviour
{
    [Header("Material Reference")]
    [SerializeField] private Material retroMaterial;

    [Header("Retro Settings")]
    [Range(1, 8)] public int redBits = 5;
    [Range(1, 8)] public int greenBits = 6;
    [Range(1, 8)] public int blueBits = 5;

    [Header("Resolution (Pixelation)")]
    public bool enablePixelation = true;
    public int targetWidth = 320;
    public int targetHeight = 180;

    // Кэшируем ID свойств для оптимизации производительности
    private static readonly int RedBitsID = Shader.PropertyToID("_RedBits");
    private static readonly int GreenBitsID = Shader.PropertyToID("_GreenBits");
    private static readonly int BlueBitsID = Shader.PropertyToID("_BlueBits");
    private static readonly int PixelationActiveID = Shader.PropertyToID("_PixelationActive");
    private static readonly int PixelSizeXID = Shader.PropertyToID("_PixelSizeX");
    private static readonly int PixelSizeYID = Shader.PropertyToID("_PixelSizeY");

    void Update()
    {
        UpdateShaderProperties();
    }

    void OnValidate()
    {
        // Вызывается при изменении ползунков в инспекторе
        UpdateShaderProperties();
    }

    private void UpdateShaderProperties()
    {
        if (retroMaterial == null) return;

        // Передаем параметры в шейдер
        retroMaterial.SetFloat(RedBitsID, redBits);
        retroMaterial.SetFloat(GreenBitsID, greenBits);
        retroMaterial.SetFloat(BlueBitsID, blueBits);
        
        retroMaterial.SetFloat(PixelationActiveID, enablePixelation ? 1.0f : 0.0f);
        retroMaterial.SetFloat(PixelSizeXID, Mathf.Max(1, targetWidth));
        retroMaterial.SetFloat(PixelSizeYID, Mathf.Max(1, targetHeight));
    }
}