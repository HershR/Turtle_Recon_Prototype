﻿using UnityEngine;

[ExecuteInEditMode]
public class WorldCurver : MonoBehaviour
{
    [Range(-20.0f, 20.0f)]
    [SerializeField] private float sidewaysStrength = 0.0f;

    [Range(-20.0f, 20.0f)]
    [SerializeField] private float backwardsStrength = 0.0f;

    [Range(-20.0f, 20.0f)]
    [SerializeField] private float spiralStrength = 0.0f;

    [SerializeField] private float curveRate = 0.5f;

    [SerializeField] private float sidewaysChangeTimerMax = 30f;

    private float sidewaysChangeTimer = 30f;

    public float minStrength { get; private set; } = -20f;
    public float maxStrength { get; private set; } = 20f;



    private int sidewaysStrengthID;
    private int backwardStrengthID;
    private int spiralStrengthID;

    private void OnEnable()
    {
        sidewaysStrengthID = Shader.PropertyToID("_Sideways_strength");
        backwardStrengthID = Shader.PropertyToID("_Backwards_strength");
        spiralStrengthID = Shader.PropertyToID("_Spiral_strength");
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            if (sidewaysChangeTimer > 0.0f)
            {
                sidewaysChangeTimer -= Time.deltaTime;
            }
            else
            {
                if(sidewaysStrength == 0)
                {
                    float[] options = new float[3] { -10.0f, 0f, 10f };
                    sidewaysStrength = options[Random.Range(0, 2)];
                }
                else
                {
                    float[] options = new float[2] { 0f, sidewaysStrength };
                    sidewaysStrength = options[Random.Range(0, 2)];
                }
                sidewaysChangeTimer = Random.Range(0, sidewaysChangeTimerMax);
            }

            if (Shader.GetGlobalFloat(sidewaysStrengthID) != sidewaysStrength)
            {
                float currentStrength = Shader.GetGlobalFloat(sidewaysStrengthID);
                float newStrength = Mathf.Lerp(currentStrength, sidewaysStrength, curveRate * Time.deltaTime);
                if (Mathf.Abs(newStrength - sidewaysStrength) < 0.01f)
                {
                    newStrength = sidewaysStrength;
                }
                Shader.SetGlobalFloat(sidewaysStrengthID, newStrength);
            }
            if (Shader.GetGlobalFloat(backwardStrengthID) != backwardsStrength)
            {
                float currentStrength = Shader.GetGlobalFloat(backwardStrengthID);
                float newStrength = Mathf.Lerp(currentStrength, backwardsStrength, curveRate * Time.deltaTime);
                if (Mathf.Abs(newStrength - backwardsStrength) < 0.01f)
                {
                    newStrength = backwardsStrength;
                }
                Shader.SetGlobalFloat(backwardStrengthID, newStrength);
            }
            if (Shader.GetGlobalFloat(spiralStrengthID) != spiralStrength)
            {
                float currentStrength = Shader.GetGlobalFloat(spiralStrengthID);
                float newStrength = Mathf.Lerp(currentStrength, spiralStrength, curveRate * Time.deltaTime);
                if (Mathf.Abs(newStrength - spiralStrength) < 0.01f)
                {
                    newStrength = spiralStrength;
                }
                Shader.SetGlobalFloat(spiralStrengthID, newStrength);
            }
        }
        else
        {
            Shader.SetGlobalFloat(sidewaysStrengthID, sidewaysStrength);
            Shader.SetGlobalFloat(backwardStrengthID, backwardsStrength);
            Shader.SetGlobalFloat(spiralStrengthID, spiralStrength);
        }
    }

    public void ResetStrenghts()
    {
        sidewaysStrength = 0f;
        backwardsStrength = 0f;
    }
    public void UpdateSidewaysStrength(float sideways)
    {
        sidewaysStrength = sideways;
    }
    public void UpdateBackwardsStrength(float backwards)
    {
        backwardsStrength = backwards;
    }
}
