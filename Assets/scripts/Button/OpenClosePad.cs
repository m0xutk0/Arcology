using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClosePad : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    [SerializeField] private Animator PadAnimation;
    [SerializeField] private RectTransform OpenClosePadButton;
    [SerializeField] private CameraScroll cameraScroll;
    [SerializeField] private AudioSource audioSource;

    public void OpenClose()
    {
        isOpen = !isOpen; // Переключаем состояние двери
        if (isOpen)
        {
            cameraScroll.enabled = false; // Отключаем скролл камеры при открытии
            PadAnimation.SetBool("IsOpen", true); // Включаем анимацию открытия
            OpenClosePadButton.localScale = new Vector3(1, -1, 1); // Устанавливаем масштаб для открытия
        }
        else
        {
            cameraScroll.enabled = true; // Включаем скролл камеры при закрытии
            PadAnimation.SetBool("IsOpen", false); // Выключаем анимацию открытия
            OpenClosePadButton.localScale = new Vector3(1, 1, 1); // Устанавливаем масштаб для закрытия
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
