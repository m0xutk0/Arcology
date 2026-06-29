using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CacheClearEvent : BaseEvent
{
    public float cacheFillRate = 0.07f;
    public float clearDuration = 3f;
    [SerializeField] private Image cacheImage; // Ссылка на UI-изображение, которое будет заполняться

    [Header("Tablet Glitch Settings")]
    [SerializeField] private GameObject glitchFaceUI;
    [SerializeField] private GameObject navigationUI;
    [SerializeField] private AudioSource tabletAudioSource;
    [SerializeField] private Animator PadAnimator;
    [SerializeField] private AudioClip laughClip;
    [SerializeField] private GameManager GameManager;
    private float faceDuration = 2.0f;

    private bool isClearing = false;
    private float clearTimer = 0f;

    public override void FixAction()
    {
        if (isActive && !isClearing)
        {
            isClearing = true;
            clearTimer = clearDuration;
        }
    }

    public override void StartEvent()
    {
        base.StartEvent();
        isClearing = false;
    }

    public override void Tick(float deltaTime, float speedMultiplier)
    {
        if (!isActive) return;

        difficulty += cacheFillRate * speedMultiplier * deltaTime;

        cacheImage.color = new Color(cacheImage.color.r, cacheImage.color.g, cacheImage.color.b, difficulty); // Обновляем прозрачность изображения в зависимости от сложности

        if (isClearing)
        {
            clearTimer -= deltaTime;
            if (clearTimer <= 0f && difficulty < 1f)
            {
                WinEvent();
                cacheImage.color = new Color(cacheImage.color.r, cacheImage.color.g, cacheImage.color.b, 0f);
            } 
        }

        if (difficulty >= 1f) LoseEvent();

        difficulty = Mathf.Clamp01(difficulty);
    }

protected override void LoseEvent()
{
    difficulty = 0f;
    cacheImage.color = new Color(cacheImage.color.r, cacheImage.color.g, cacheImage.color.b, 0f);
    
    // Запускаем корутину ЭТОГО скрипта, но на базом движке МЕНЕДЖЕРА, который не выключится
    if (GameManager.Instance != null)
    {
        GameManager.Instance.StartCoroutine(TriggerTabletGlitchRoutine());
    }

    base.LoseEvent(); 
}

    public IEnumerator TriggerTabletGlitchRoutine()
    {
        // 1. Включаем искаженное лицо
        if (glitchFaceUI != null) glitchFaceUI.SetActive(true);
        // 2. Воспроизводим звук из динамика
        if (tabletAudioSource != null && laughClip != null)
        {
            tabletAudioSource.PlayOneShot(laughClip);
        }
        
        faceDuration = Random.Range(10f, 15f); // Случайная длительность показа лица
        // Ждем, пока игрок созерцает лицо
        yield return new WaitForSeconds(faceDuration);
        
        // 2. Убираем лицо
        if (glitchFaceUI != null) glitchFaceUI.SetActive(false);
    }
}