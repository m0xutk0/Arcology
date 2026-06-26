using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    private Transform CameraTransform; 

    [Header("Настройки движения")]
    [SerializeField] private float maxOffset = 300f;        // Максимальное смещение от начальной точки
    [SerializeField] private float scrollSpeed = 400f;      // Скорость движения камеры при зажатии
    [SerializeField] private float smoothSpeed = 5f;        // Плавность остановки/доводки

    [Header("Зоны нажатия (в процентах от экрана)")]
    [Range(0f, 0.5f)]
    [SerializeField] private float edgeZoneSize = 0.3f;     // 0.3 означает 30% экрана слева и 30% справа

    private float targetX;
    private float startX;

    void Start()
    {
        CameraTransform = GetComponent<Transform>();
        
        // Запоминаем начальную позицию, чтобы корректно считать смещение
        startX = CameraTransform.position.x;
        targetX = startX;
    }

    void Update()
    {
        float direction = 0f;
        bool isPressing = false;
        Vector2 inputPosition = Vector2.zero;

        // 1. Проверка тачей для iOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Реагируем только на фазы удержания или движения пальца
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                inputPosition = touch.position;
                isPressing = true;
            }
        }
        // 2. Проверка мыши для тестов в Unity Editor
        #if UNITY_EDITOR
        else if (Input.GetMouseButton(0))
        {
            inputPosition = Input.mousePosition;
            isPressing = true;
        }
        #endif

        // Если экран зажат, проверяем, в какой именно зоне находится палец/курсор
        if (isPressing)
        {
            // Переводим координату X в диапазон от 0.0 до 1.0
            float normalizedX = inputPosition.x / Screen.width;

            if (normalizedX < edgeZoneSize)
            {
                direction = -1f; // Зажата левая сторона -> двигаемся влево
            }
            else if (normalizedX > (1f - edgeZoneSize))
            {
                direction = 1f;  // Зажата правая сторона -> двигаемся вправо
            }
        }

        // Если зажата одна из сторон, плавно изменяем целевую позицию
        if (direction != 0f)
        {
            targetX += direction * scrollSpeed * Time.deltaTime;
            // Ограничиваем движение в пределах [-maxOffset, maxOffset] от начальной позиции
            targetX = Mathf.Clamp(targetX, startX - maxOffset, startX + maxOffset);
        }

        // Плавное перемещение объекта к целевой точке (работает всегда, обеспечивая инерцию)
        Vector3 currentPos = CameraTransform.position;
        currentPos.x = Mathf.Lerp(currentPos.x, targetX, Time.deltaTime * smoothSpeed);
        CameraTransform.position = currentPos;
    }
}