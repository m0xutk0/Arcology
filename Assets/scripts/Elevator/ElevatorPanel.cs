using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
    public bool IsOpen;
    [SerializeField] private GameObject ElevatorPanelCanvas;
    [SerializeField] private CameraScroll CameraScroll;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Проверяем, есть ли хотя бы одно касание на экране
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Проверяем, что касание только что началось (фаза Began)
            if (touch.phase == TouchPhase.Began)
            {
                // Создаем луч из камеры через точку касания на экране
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Пускаем луч и проверяем, столкнулся ли он с чем-то
                if (Physics.Raycast(ray, out hit))
                {
                    // Проверяем, что луч попал именно в этот объект (на котором висит скрипт)
                    if (hit.collider.gameObject == gameObject)
                    {
                        if(!IsOpen)
                        {
                            IsOpen = true;
                            ElevatorPanelCanvas.SetActive(true);
                            CameraScroll.enabled = false;
                        }
                    }
                }
            }
        }
    }
    public void Close()
    {
        IsOpen = false;
        ElevatorPanelCanvas.SetActive(false);
        CameraScroll.enabled = true;
    }
}
