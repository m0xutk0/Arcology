using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS; // Подключаем пространство имён iOS
#endif

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        // Отключаем ограничение и заставляем игру выдавать плавные 60 кадров
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_IOS && !UNITY_EDITOR
        // Заставляем iOS требовать двойной свайп снизу и сверху экрана
        UnityEngine.iOS.Device.deferSystemGesturesMode = SystemGestureDeferMode.BottomEdge | SystemGestureDeferMode.TopEdge;
        
        // Либо, если хочешь защитить вообще все края:
        // UnityEngine.iOS.Device.deferSystemGesturesMode = SystemGestureDeferMode.All;
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
