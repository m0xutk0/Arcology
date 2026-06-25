using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightContoller : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Elevator;
    [SerializeField] private Sprite ElevatorLightOn;
    [SerializeField] private Sprite ElevatorLightOff;
    [SerializeField] private SpriteRenderer LeftDoor;
    [SerializeField] private Sprite LeftDoorLightOn;
    [SerializeField] private Sprite LeftDoorLightOff;
    [SerializeField] private SpriteRenderer RightDoor;
    [SerializeField] private Sprite RightDoorLightOn;
    [SerializeField] private Sprite RightDoorLightOff;

    [ContextMenu("Elevator Light On")]
    public void ElevatorLightOnMethod()
    {
        Elevator.sprite = ElevatorLightOn;
        LeftDoor.sprite = LeftDoorLightOn;
        RightDoor.sprite = RightDoorLightOn;
    }

    [ContextMenu("Elevator Light Off")]
    public void ElevatorLightOffMethod()
    {
        Elevator.sprite = ElevatorLightOff;
        LeftDoor.sprite = LeftDoorLightOff;
        RightDoor.sprite = RightDoorLightOff;
    }
    
}
