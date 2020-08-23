using UnityEngine;

public class RevealingLightManipulation : MonoBehaviour {
    
    private float xSmooth = 0;
    private float ySmooth = 0;
    
    private float distance = 5.0f;

    private Vector3 posSmooth = Vector3.zero;
    
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(ySmooth, xSmooth, 0);
        transform.position = transform.rotation * new Vector3(0.0f, 0.0f, -distance) + posSmooth;
    }
}
