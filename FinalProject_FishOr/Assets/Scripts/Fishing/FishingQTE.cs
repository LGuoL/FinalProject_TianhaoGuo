using System;
using UnityEngine;
using UnityEngine.UI;

public class FishingQTE : MonoBehaviour
{
    [Header("UI")]
    public GameObject qtePanel;
    public RectTransform qteBar;
    public RectTransform successZone;
    public RectTransform pointer;

    [Header("Base Settings")]
    public float basePointerSpeed = 450f;
    public float baseSuccessWidth = 120f;

    private bool isActive = false;
    private bool movingRight = true;

    private float barHalfWidth;
    private float pointerSpeed;
    private float successWidth;

    private Action<bool> onQTEFinished;

    private void Start()
    {
        if (qtePanel != null)
            qtePanel.SetActive(false);
    }

    private void Update()
    {
        if (!isActive)
            return;

        MovePointer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckResult();
        }
    }

    public void StartQTE(float difficultyMultiplier, Action<bool> callback)
    {
        if (qtePanel == null || qteBar == null || successZone == null || pointer == null)
        {
            Debug.LogWarning("FishingQTE UI references are missing.");
            callback?.Invoke(false);
            return;
        }

        onQTEFinished = callback;
        isActive = true;
        movingRight = true;

        qtePanel.SetActive(true);

        barHalfWidth = qteBar.rect.width / 2f;

        // difficultyMultiplier 越小越简单
        // Basic Rod = 1
        // Advanced Rod = 0.7
        pointerSpeed = basePointerSpeed * difficultyMultiplier;

        // 高级鱼竿让成功区域更大
        successWidth = baseSuccessWidth / difficultyMultiplier;

        successZone.sizeDelta = new Vector2(successWidth, successZone.sizeDelta.y);

        float maxZoneX = barHalfWidth - successWidth / 2f;
        float randomX = UnityEngine.Random.Range(-maxZoneX, maxZoneX);

        successZone.anchoredPosition = new Vector2(randomX, 0);
        pointer.anchoredPosition = new Vector2(-barHalfWidth, 0);
    }

    private void MovePointer()
    {
        float direction = movingRight ? 1f : -1f;

        pointer.anchoredPosition += new Vector2(direction * pointerSpeed * Time.deltaTime, 0);

        if (pointer.anchoredPosition.x >= barHalfWidth)
        {
            pointer.anchoredPosition = new Vector2(barHalfWidth, 0);
            movingRight = false;
        }
        else if (pointer.anchoredPosition.x <= -barHalfWidth)
        {
            pointer.anchoredPosition = new Vector2(-barHalfWidth, 0);
            movingRight = true;
        }
    }

    private void CheckResult()
    {
        float pointerX = pointer.anchoredPosition.x;
        float zoneX = successZone.anchoredPosition.x;

        float zoneMin = zoneX - successWidth / 2f;
        float zoneMax = zoneX + successWidth / 2f;

        bool success = pointerX >= zoneMin && pointerX <= zoneMax;

        EndQTE(success);
    }

    private void EndQTE(bool success)
    {
        isActive = false;

        if (qtePanel != null)
            qtePanel.SetActive(false);

        onQTEFinished?.Invoke(success);
        onQTEFinished = null;
    }
}