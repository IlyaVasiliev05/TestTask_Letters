using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScale : MonoBehaviour {


    [SerializeField] AnimationCurve scaleAnimCurve;
    [SerializeField] bool useVectors = false;
    [SerializeField] Vector3 defaultScale;
    [SerializeField] Vector3 addScale;
    [SerializeField] float cycleDuration = 10;
    [SerializeField] float scaleMult;
    [SerializeField] bool randomCycle = false;
    [SerializeField] float randomOffset;

    void Start()
    {
        if (randomCycle)
            randomOffset = Random.Range(0, 1f);
    }

	void Update ()
    {
        if (!gameObject.activeSelf)
            return;
        float t = ((Time.time % cycleDuration) / cycleDuration + randomOffset) % 1;
        if (useVectors)
            transform.localScale = defaultScale + addScale * scaleAnimCurve.Evaluate(t) * scaleMult;
        else
            transform.localScale = Vector3.one * scaleAnimCurve.Evaluate(t) * scaleMult;
	}
}
