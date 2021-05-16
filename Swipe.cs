using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe : MonoBehaviour
{
    [SerializeField] private RectTransform contentRect;
    [SerializeField] private HorizontalLayoutGroup horizontalLayout;
    [SerializeField] private GameObject bgLock;
    [SerializeField] private RectTransform[] elements;
    private WindowState state = WindowState.second;

    private int additiveXposition = 15;

    private Vector2 touchStart;
    private Vector2 touchFinished;


    private void Start()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].sizeDelta = new Vector2(Screen.width - 200, elements[i].sizeDelta.y);
        }
        additiveXposition = (int)(elements[0].rect.width + horizontalLayout.spacing);
        contentRect.anchoredPosition = new Vector2(-additiveXposition * (int)state, contentRect.anchoredPosition.y);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStart = touch.position;
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Ended:
                    if ((touch.position.x - touchStart.x) > 0)
                        MoveWindowLeft();
                    else
                        MoveWindowRight();
                    break;
            }
        }
    }

    public void MoveWindowRight()
    {
        if (state < WindowState.six)
        {
            state++;
            StartCoroutine(LerpMoved(-additiveXposition));
        }
    }
    public void MoveWindowLeft()
    {
        if (state > WindowState.first)
        {
            state--;
            StartCoroutine(LerpMoved(additiveXposition));
        }
    }


    private IEnumerator LerpMoved(int newPosition)
    {
        bgLock.SetActive(true); // ArrowBtnEnable(false);
        int startPosition = (int)contentRect.anchoredPosition.x;
        int finishPosititon = startPosition + newPosition;

        float timeStart = 0.0f;
        float timeFinish = 0.25f;

        while (timeStart < timeFinish)
        {
            timeStart += Time.deltaTime;
            var newX = Mathf.Lerp(startPosition, finishPosititon, timeStart / timeFinish);
            contentRect.anchoredPosition = new Vector2(newX, contentRect.anchoredPosition.y);
            yield return null;
        }
        yield return null;
        bgLock.SetActive(false); // ArrowBtnEnable(true);
    }
}

    public enum WindowState
{
    first   = 0,
    second  = 1,
    third   = 2,
    fourth  = 3,
    five    = 4,
    six     = 5,
}