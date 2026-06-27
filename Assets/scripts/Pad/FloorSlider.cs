using UnityEngine;
using UnityEngine.UI;

public class FloorSlider : MonoBehaviour
{
    [SerializeField] private ElevatorController elevatorController;
    [SerializeField] private Slider slider;

    private void Update()
    {
        slider.value = elevatorController.preciseCurrentFloor;
    }

    public void SliderValue()
    {
        if (elevatorController.CurrentFloor > elevatorController.TargetFloor)
        {
            slider.minValue = elevatorController.TargetFloor;
            slider.maxValue = elevatorController.CurrentFloor;
        }
        else if (elevatorController.CurrentFloor < elevatorController.TargetFloor)
        {
            slider.minValue = elevatorController.CurrentFloor;
            slider.maxValue = elevatorController.TargetFloor;
        }
    }
}