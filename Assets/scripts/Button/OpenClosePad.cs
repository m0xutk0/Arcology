using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClosePad : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    [SerializeField] private Animator doorAnimation;

    public void OpenClose()
    {
        isOpen = !isOpen; // Переключаем состояние двери
        if (isOpen)
        {
            doorAnimation.SetBool("IsOpen", true); // Включаем анимацию открытия
        }
        else
        {
            doorAnimation.SetBool("IsOpen", false); // Выключаем анимацию открытия
        }
    }
}
