using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClosePad : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    [SerializeField] private GameObject ImageButton;
    [SerializeField] private GameObject Pad;

    public void OpenClose()
    {
        isOpen = !isOpen; // Переключаем состояние двери
        if (isOpen)
        {
            Vector3 newScale = new Vector3(ImageButton.transform.localScale.x, -1.0f, ImageButton.transform.localScale.z);
            ImageButton.transform.localScale = newScale;
            Pad.SetActive(true); // Показываем Pad
        }
        else
        {
            Vector3 newScale = new Vector3(ImageButton.transform.localScale.x, 1.0f, ImageButton.transform.localScale.z);
            ImageButton.transform.localScale = newScale;
            Pad.SetActive(false); // Скрываем Pad
        }
    }
}
