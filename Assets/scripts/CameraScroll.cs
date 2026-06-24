using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    private Transform CameraTransform; // Ссылка на UI картинку офиса
    [SerializeField] private float maxOffset = 300f;        // На сколько пикселей картинка может смещаться в сторону
    [SerializeField] private float smoothSpeed = 5f;        // Плавность движения камеры

    void Start()
    {
        CameraTransform = GetComponent<Transform>();
    }

    void Update()
    {
        // Получаем позицию мыши в диапазоне от 0 до 1
        float mouseXNormalized = Input.mousePosition.x / Screen.width;

        // Переводим в диапазон от -1 (левый край) до 1 (правый край)
        float targetDirection = (mouseXNormalized * 2f) - 1f;

        // Ограничиваем значения на случай, если мышь вылетела за пределы окна
        targetDirection = Mathf.Clamp(targetDirection, -1f, 1f);

        float targetX = targetDirection * maxOffset;

        // Плавно перемещаем картинку к целевой точке
        Vector3 currentPos = CameraTransform.position;
        currentPos.x = Mathf.Lerp(currentPos.x, targetX, Time.deltaTime * smoothSpeed);
        CameraTransform.position = currentPos;
    }
}
