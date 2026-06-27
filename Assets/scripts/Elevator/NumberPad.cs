using UnityEngine;
using UnityEngine.UI; // Обязательно для работы с Legacy UI
using System;

public class NumberPad : MonoBehaviour
{
    [Header("UI Элементы")]
    [SerializeField] private Text displayText; // Ссылка на компонент Text для отображения
    [SerializeField] private FloorSlider FloorSlider; // Ссылка на FloorSlider для обновления слайдера

    [Header("Настройки")]
    [SerializeField] private int maxDigits = 9; // Максимальная длина числа
    [SerializeField] private AudioClip ButtonClickSound;

    private AudioSource audioSource;

    private string currentInput = "";

    // Событие, которое вызовется, когда игрок нажмет "Готово"
    // Другие скрипты могут подписаться на него, чтобы получить число
    public static event Action<int> OnNumberSubmitted;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateDisplay();
    }

    /// <summary>
    /// Метод для кнопок от 0 до 9
    /// </summary>
    public void AppendDigit(int digit)
    {
        if (currentInput.Length >= maxDigits) return;

        // Защита от ввода множества нулей в начале (например, "0005")
        if (currentInput == "0" && digit == 0) return;
        if (currentInput == "0" && digit != 0) currentInput = "";

        currentInput += digit.ToString();

        audioSource.PlayOneShot(ButtonClickSound);

        UpdateDisplay();
    }

    /// <summary>
    /// Метод для кнопки "Назад" (Backspace)
    /// </summary>
    public void Backspace()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            audioSource.PlayOneShot(ButtonClickSound);
            UpdateDisplay();
        }
    }

    /// <summary>
    /// Метод для кнопки "Готово"
    /// </summary>
    public void Submit()
    {
        if (string.IsNullOrEmpty(currentInput))
        {
            Debug.LogWarning("Поле ввода пустое!");
            return;
        }

        if (int.TryParse(currentInput, out int finalNumber))
        {
            Debug.Log($"Число успешно собрано: {finalNumber}");
            
            // Отправляем число всем, кто подписан на событие
            OnNumberSubmitted?.Invoke(finalNumber);

            audioSource.PlayOneShot(ButtonClickSound);
            FloorSlider.SliderValue();
            // Опционально: очищаем поле после ввода
            Clear();
        }
    }

    /// <summary>
    /// Очистка поля
    /// </summary>
    public void Clear()
    {
        currentInput = "";
        UpdateDisplay();
    }

    // Обновление текста на экране
    private void UpdateDisplay()
    {
        if (displayText != null)
        {
            // Если ничего не введено, показываем "0" или пустоту (по желанию)
            displayText.text = string.IsNullOrEmpty(currentInput) ? "0" : currentInput;
        }
    }
}