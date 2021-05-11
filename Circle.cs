using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Transform thisTransform;

    private void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float start         = -5f;
        float finish        = 5f;
        float elapsedTime   = 0f;
        float time          = 3f;

        yield return new WaitForSeconds(Random.Range(1f, 2f));

        while (elapsedTime < time)
        {
            float x = Mathf.Lerp(start, finish, (elapsedTime / time));
            thisTransform.position = new Vector2(x, thisTransform.position.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }            
    }
}