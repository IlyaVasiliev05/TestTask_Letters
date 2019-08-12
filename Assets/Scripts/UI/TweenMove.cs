using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMove : MonoBehaviour
{

    [SerializeField] Vector3 targetPos;
    [SerializeField] Space space;
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] float moveSpeed;
    [SerializeField] bool useRandomOffset = false;
    Vector3 startPos;
    float randomOffset = 0;

    void Start()
    {
        if (useRandomOffset)
            randomOffset = Random.Range(0, 1f);
        startPos = space == Space.World ? transform.position : transform.localPosition;
    }

    void Update()
    {
        if (space == Space.World)
        {
            transform.position = Vector3.LerpUnclamped(startPos, targetPos, moveCurve.Evaluate((Time.time * moveSpeed + randomOffset)  % 1));
        }
        else
        {
            transform.localPosition = Vector3.LerpUnclamped(startPos, targetPos, moveCurve.Evaluate((Time.time * moveSpeed + randomOffset) % 1));
        }
    }

}