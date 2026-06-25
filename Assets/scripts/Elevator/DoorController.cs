using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Transform doorLeft;
    [SerializeField] private Transform doorRight;
    [SerializeField] private Vector3 openPositionLeft;
    [SerializeField] private Vector3 openPositionRight;
    [SerializeField] private Vector3 closedPositionLeft;
    [SerializeField] private Vector3 closedPositionRight;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private int framesPerSecond = 30;
    [SerializeField] private AudioClip OpenDoorSound;
    [SerializeField] private AudioClip CloseDoorSound;
    private AudioSource audioSource;

    private Coroutine moveCoroutine;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [ContextMenu("Open Door")]
    public void OpenDoor()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveDoors(openPositionLeft, openPositionRight));
        audioSource.PlayOneShot(OpenDoorSound);
    }

    [ContextMenu("Close Door")]
    public void CloseDoor()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveDoors(closedPositionLeft, closedPositionRight));
        audioSource.PlayOneShot(CloseDoorSound);
    }

    private IEnumerator MoveDoors(Vector3 targetLeft, Vector3 targetRight)
    {
        Vector3 startLeft = doorLeft.localPosition;
        Vector3 startRight = doorRight.localPosition;
        
        float elapsed = 0f;
        float interval = 1f / framesPerSecond;

        while (elapsed < moveDuration)
        {
            // Сначала считаем позицию (на первом круге elapsed = 0, прыжка не будет)
            float t = Mathf.Clamp01(elapsed / moveDuration);
            
            doorLeft.localPosition = Vector3.Lerp(startLeft, targetLeft, t);
            doorRight.localPosition = Vector3.Lerp(startRight, targetRight, t);
            
            // Ждем заданное время
            yield return new WaitForSeconds(interval);
            
            // И только ПОСЛЕ ожидания увеличиваем таймер для следующего кадра
            elapsed += interval;
        }

        doorLeft.localPosition = targetLeft;
        doorRight.localPosition = targetRight;
    }
}