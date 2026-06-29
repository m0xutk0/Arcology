using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Пул ивентов")]
    [SerializeField] private BaseEvent[] allEvents;

    [Header("Настройки спавна")]
    [SerializeField] private float baseSpawnInterval = 15f; // Пауза между ивентами на старте
    [SerializeField] private float minSpawnInterval = 4f;   // Минимально возможная пауза в глубокой шахте

    private BaseEvent activeEvent;
    private float spawnTimer;
    private float timeInTrip; // Как долго лифт едет без остановки

    // Эту переменную твой скрипт лифта должен переключать в True/False
    [HideInInspector] public bool isElevatorMoving = false; 

    void Start()
    {
        foreach (var ev in allEvents) ev.Init(this);
        spawnTimer = baseSpawnInterval;
    }

    void Update()
    {
        // Если лифт стоит на месте — ивенты не тикают, сложность сбрасывается
        if (!isElevatorMoving)
        {
            timeInTrip = 0f;
            return;
        }

        // Если лифт движется, копим время поездки
        timeInTrip += Time.deltaTime;

        // 1. Если сейчас идет ивент — обновляем его логику
        if (activeEvent != null && activeEvent.isActive)
        {
            // Множитель сложности: каждые 10 секунд поездки увеличивают скорость роста опасности на 20%
            float currentDifficultyMultiplier = 1f + (timeInTrip * 0.02f);
            
            activeEvent.Tick(Time.deltaTime, currentDifficultyMultiplier);
        }
        // 2. Если ивента нет — считаем таймер до следующего случайного сбоя
        else
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnRandomEvent();
            }
        }
    }

    private void SpawnRandomEvent()
    {
        if (allEvents.Length == 0) return;

        // Выбираем случайный ивент из пула
        activeEvent = allEvents[Random.Range(0, allEvents.Length)];
        activeEvent.StartEvent();

        // Динамический расчет следующей паузы: чем дольше едем, тем чаще спавн
        float reduction = timeInTrip * 0.1f; // Каждая секунда в пути уменьшает паузу на 0.1 сек
        float currentInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - reduction);
        
        spawnTimer = currentInterval;
    }

    // Метод, который вызовет сам ивент, когда игрок его починит
    public void OnEventCompleted()
    {
        activeEvent = null;
    }
}