using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [Header("Elevator Settings")]
    public bool ElevatorIsMoving;
    public float ElevatorSpeed; // Сколько этажей лифт преодолевает за 1 секунду
    public float ElevatorFuel;
    [SerializeField] private float fuelConsumptionRate = 1f; // Расход топлива в секунду
    [SerializeField] private DoorController DoorController;
    [SerializeField] private FloorSlider FloorSlider;
    private AudioSource ElevatorAudioSource; // Аудио источник для звука лифта
    [SerializeField] private AudioClip ElevatorMoveClip; // Звук движения лифта

    [Header("Elevator floor settings")]
    public int CurrentFloor;
    public int TargetFloor;

    [Header("Event Manager Reference")]
    [SerializeField] private EventManager EventManager;

    // Внутренние переменные для плавной симуляции
    [HideInInspector] public float preciseCurrentFloor; 
    private bool isPaused; 

    void OnEnable()
    {
        // 1. Подписываемся на событие ввода номера из скрипта NumberPad
        NumberPad.OnNumberSubmitted += ReceiveFloorCode;
    }

    void OnDisable()
    {
        // Не забываем отписаться
        NumberPad.OnNumberSubmitted -= ReceiveFloorCode;
    }

    void Start()
    {
        ElevatorAudioSource = GetComponent<AudioSource>();
        // Синхронизируем точный этаж со стартовым
        preciseCurrentFloor = CurrentFloor;
    }

    void Update()
    {
        // Лифт "движется" только если поднят флаг и нет паузы
        if (ElevatorIsMoving && !isPaused)
        {
            SimulateMovement();
        }
    }

    // Метод, который срабатывает при получении кода с пульта
    private void ReceiveFloorCode(int code)
    {
        TargetFloor = code;

        if (TargetFloor == CurrentFloor)
        {
            Debug.Log("Лифт уже находится на этом этаже.");
            return;
        }

        // Место под метод закрытия дверей (вызовется перед стартом)
        DoorController.CloseDoor();

        // Сразу начинаем движение
        ElevatorIsMoving = true;
        ElevatorAudioSource.PlayOneShot(ElevatorMoveClip);
        EventManager.isElevatorMoving = true; // Сообщаем менеджеру ивентов, что лифт в движении
        isPaused = false;
    }

    // Логика перемещения и расхода топлива
    private void SimulateMovement()
    {

        // Проверка топлива
        if (ElevatorFuel <= 0)
        {
            ElevatorFuel = 0;
            ElevatorIsMoving = false;
            ElevatorAudioSource.Stop();
            EventManager.isElevatorMoving = false; // Сообщаем менеджеру ивентов, что лифт остановился
            Debug.LogError("Топливо закончилось! Лифт застрял.");
            return;
        }

        // 2. Движение с заданной скоростью (изменяем float-значение в сторону цели)
        preciseCurrentFloor = Mathf.MoveTowards(preciseCurrentFloor, TargetFloor, ElevatorSpeed * Time.deltaTime);
        
        // Обновляем отображаемый этаж (округляем до ближайшего целого)
        CurrentFloor = Mathf.RoundToInt(preciseCurrentFloor);

        // 3. Движение расходует топливо
        ElevatorFuel -= fuelConsumptionRate * Time.deltaTime;

        // 4. При достижении нужного этажа
        if (Mathf.Approximately(preciseCurrentFloor, TargetFloor))
        {
            ElevatorIsMoving = false;
            EventManager.isElevatorMoving = false; // Сообщаем менеджеру ивентов, что лифт остановился
            ElevatorAudioSource.Stop();
            DoorController.OpenDoor(); // Вызываем метод открытия дверей
        }
    }

    // 5. Метод внеплановой остановки (например, кнопка СТОП)
    public void EmergencyStop()
    {
        if (ElevatorIsMoving)
        {
            isPaused = true;
            Debug.LogWarning("Экстренная остановка лифта!");
        }
    }

    // 5. Метод продолжения движения после остановки
    public void ResumeMovement()
    {
        if (ElevatorIsMoving && isPaused)
        {
            isPaused = false;
            Debug.Log("Лифт продолжает движение.");
        }
    }
}