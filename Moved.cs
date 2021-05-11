using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Moved : MonoBehaviour
{

    [SerializeField] private RectTransform rope;
    [SerializeField] private GameObject fakeCircle;

    private GrabState grabState = GrabState.Move;

    private void Start()
    {
        StartCoroutine(Move());
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && grabState != GrabState.Wait)
        {
            grabState = GrabState.Grab;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            grabState = GrabState.Wait;
        }
    }

    private IEnumerator Move()
    {
        float pingPongTime = 0;
        float startHeight = 250f;
        float finishHeight = 1500f;

        float waitTime = 0f;
        float grabTime = 0f;


        while (true) {
            if (grabState == GrabState.Move)
            {
                pingPongTime += Time.deltaTime;
                float zAngle = Mathf.PingPong(pingPongTime * 50f, 100f) - 50f;
                rope.localRotation = Quaternion.Euler(0,0, zAngle);
            }
            else if (grabState == GrabState.Wait)
            {
                if (rope.rect.height > startHeight)
                {
                    //float y = Mathf.Lerp(startHeight, finishHeight, waitTime);
                    rope.sizeDelta -= new Vector2(0, 5f);
                    waitTime += Time.deltaTime;
                }
                else
                    grabState = GrabState.Move;
            }
            else if (grabState == GrabState.Grab)
            {
                if (rope.rect.height < finishHeight)
                {
                    //float y = Mathf.Lerp(finishHeight, startHeight, waitTime);
                    rope.sizeDelta += new Vector2(0, 5f);
                    grabTime += Time.deltaTime;
                }
                else
                    grabState = GrabState.Wait;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("circle"))
        {
            fakeCircle.SetActive(true);
            collision.gameObject.SetActive(false);
            grabState = GrabState.Wait;
            Debug.Log("collision");
        }
    }
}

public enum GrabState
{
    Grab,
    Move,
    Wait,
}