using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenRotate : MonoBehaviour {

    [SerializeField] private float degrees = 10;
    [SerializeField] private float degreesOffset = 0;
    [SerializeField] private float speed = 2;
    [SerializeField] AnimationCurve rotationCurve;
    [SerializeField] private float randomizeTimeFloat = -1;
    [SerializeField] RotateType rotationType = RotateType.defaultZ;
    [SerializeField] Vector3 rotationVector;
    [SerializeField] Space rotationSpace = Space.World;

    void Start()
    {
        if (randomizeTimeFloat > 0)
            speed = speed + Random.Range(-randomizeTimeFloat, randomizeTimeFloat);
    }

    void Update()
    {
        if (rotationType == RotateType.defaultZ)
            if (rotationSpace == Space.World)
                transform.eulerAngles = Vector3.forward * (rotationCurve.Evaluate(Time.time % speed / speed) * degrees + degreesOffset);
            else
                transform.localEulerAngles = Vector3.forward * (rotationCurve.Evaluate(Time.time % speed / speed) * degrees + degreesOffset);
        else if (rotationType == RotateType.byVector)
            transform.Rotate(rotationVector * speed * Time.deltaTime, rotationSpace);
    }

    private enum RotateType
    {
        defaultZ,
        byVector
    }

}
