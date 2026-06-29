using UnityEngine;
using UnityEngine.UI;

public class FloorSlider : MonoBehaviour
{
    [SerializeField] private ElevatorController elevatorController;
    [SerializeField] private Slider slider;
    [SerializeField] private Text Floor;
    [SerializeField] private Text FloorUp;
    [SerializeField] private Text FloorDown;

    private void Update()
    {
        slider.value = elevatorController.preciseCurrentFloor;
        Floor.text = elevatorController.CurrentFloor.ToString();
    }

    public void SliderValue()
    {
        if (elevatorController.CurrentFloor > elevatorController.TargetFloor)
        {
            slider.minValue = elevatorController.TargetFloor;
            slider.maxValue = elevatorController.CurrentFloor;
            FloorUp.text = elevatorController.CurrentFloor.ToString();
            FloorDown.text = elevatorController.TargetFloor.ToString();
        }
        else if (elevatorController.CurrentFloor < elevatorController.TargetFloor)
        {
            slider.minValue = elevatorController.CurrentFloor;
            slider.maxValue = elevatorController.TargetFloor;
            FloorUp.text = elevatorController.TargetFloor.ToString();
            FloorDown.text = elevatorController.CurrentFloor.ToString();
        }
    }
}