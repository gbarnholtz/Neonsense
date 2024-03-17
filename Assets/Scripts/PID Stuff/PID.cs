using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PID {
    public float pGain, iGain, dGain, saturation;
    private float valueLast;
    private bool dInitialized;
    private float integrationStored, P, I, D;

    public PID(float p, float i, float d, float s) { SetData(p, i, d, s); }

    public PID(PIDData data) { SetData(data); }

    public void SetData(PIDData data) {
        pGain = data.p;
        iGain = data.i;
        dGain = data.d;
        saturation = data.s;
    }

    public void SetData(float p, float i, float d, float s)
    {
        pGain = p;
        iGain = i;
        dGain = d;
        saturation = s;
    }

    public float Update(float dt, float currentValue, float targetValue)
    {
        P = targetValue - currentValue;
        I += Mathf.Clamp(P * dt, -saturation, saturation);

        D = 0;
        if (dInitialized) {
            D = (valueLast - currentValue) / dt;
        } else {
            dInitialized = true;
        }

        valueLast = currentValue;

        return P * pGain + I * iGain + D * dGain;
    }

    public float UpdateAngle(float dt, float currentAngle, float targetAngle)
    {

        P = AngleDifference(targetAngle, currentAngle);
        I += Mathf.Clamp(P * dt, -saturation, saturation);

        D = 0;
        if (dInitialized) {
            D = (valueLast - currentAngle) / dt;
        } else {
            dInitialized = true;
        }

        valueLast = currentAngle;

        return P * pGain + I * iGain + D * dGain;
    }

    public void Reset() {
        dInitialized = false;
    }

    float AngleDifference(float a, float b)
    {
        return (a - b + 540) % 360 - 180;   //calculate modular difference, and remap to [-180, 180]
    }

}

[System.Serializable]
public class PIDData {
    public float p, i, d, s;
}
