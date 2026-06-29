using UnityEngine;

public abstract class BaseEvent : MonoBehaviour
{
    [Range(0f, 1f)] 
    public float difficulty = 0f; // Текущий прогресс опасности (от 0 до 1)
    public bool isActive { get; protected set; } = false;

    protected EventManager manager;

    public virtual void Init(EventManager eventManager)
    {
        manager = eventManager;
    }

    // 1. Старт ивента
    public virtual void StartEvent()
    {
        difficulty = 0f;
        isActive = true;
    }

    // 2. Действия для исправления (вызывай из своих кнопок/команд)
    public abstract void FixAction();

    // 3. Победа ивента
    protected virtual void WinEvent()
    {
        isActive = false;
        difficulty = 0f;
        manager.OnEventCompleted(); // Сообщаем менеджеру, что мы свободны
    }

    // 4. Проигрыш (Скример)
    protected virtual void LoseEvent()
    {
        isActive = false;
        // Здесь твоя логика смерти, вызов экрана Game Over и т.д.
    }

    // Метод для обновлений в кадрах (вызывается из Менеджера)
    public abstract void Tick(float deltaTime, float speedMultiplier);
}