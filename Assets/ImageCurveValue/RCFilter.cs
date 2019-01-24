using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://ehbtj.com/electronics/sensor-digital-filter/
[System.Serializable]
public class RCFilter  {

	float[] y;

	public float a = 1;

	public RCFilter()
	{
		y = new float[]{0,0};
	}
	public float GetFilteredValue(float v)
	{
		float outPut = a * y [0] + (1 - a) * v;

		y [1] = outPut;
		y [0] = y [1];
		return outPut;
	}
    public void SetDefaultValue(float v)
    {
        y[0] = v;
    }
}

[System.Serializable]
public class RCFilterVector3  {

	Vector3[] y;

	public float a = 1;

	public RCFilterVector3()
	{
		y = new Vector3[]{Vector3.zero,Vector3.zero};
	}
	public Vector3 GetFilteredValue(Vector3 v)
	{
		Vector3 outPut = (a * y [0]) + ((1 - a) * v);
		y [1] = outPut;
		y [0] = y [1];
		return outPut;
	}
}
