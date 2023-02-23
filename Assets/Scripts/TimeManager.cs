using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public VelocityEstimator head;
    public VelocityEstimator leftHand;
    public VelocityEstimator rightHand;

    public float sensitivity = 0.8f;
    public float minTimeScale = 0.05f;

    private float initialFixedDeltaTime;

    private void Start()
    {
        initialFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        float velocityMagnitude = head.GetVelocityEstimate().magnitude + leftHand.GetAngularVelocityEstimate().magnitude + rightHand.GetAngularVelocityEstimate().magnitude;

        Time.timeScale = Mathf.Clamp01(minTimeScale + velocityMagnitude * sensitivity);

        Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
    }

}
