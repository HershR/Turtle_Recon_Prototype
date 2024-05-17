using UnityEngine;

[ExecuteInEditMode]
public class WorldCurver : MonoBehaviour
{
	[Range(-20.0f, 20.0f)]
	public float sidewaysStrength = 0.0f;
    
    [Range(-20.0f, 20.0f)]
    public float backwardsStrength = 0.0f;

    int sidewaysStrengthID;
    int backwardStrengthID;

    private void OnEnable()
    {
        sidewaysStrengthID = Shader.PropertyToID("_Sideways_strength");
        backwardStrengthID = Shader.PropertyToID("_Backwards_strength");
    }

	void Update()
	{
		Shader.SetGlobalFloat(sidewaysStrengthID, sidewaysStrength);
		Shader.SetGlobalFloat(backwardStrengthID, backwardsStrength);
	}
}
