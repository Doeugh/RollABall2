
using UnityEngine;

public class UIElementScaler : MonoBehaviour
{
    public RectTransform elementA;
    public RectTransform elementD;

    public float enlargedScale = 1.3f;

    private Vector3 defaultScaleA;
    private Vector3 defaultScaleD;

    void Start()
    {
        defaultScaleA = elementA.localScale;
        defaultScaleD = elementD.localScale;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            elementA.localScale = defaultScaleA * enlargedScale;
        }
        else
        {
            elementA.localScale = defaultScaleA;
        }

        if (Input.GetKey(KeyCode.D))
        {
            elementD.localScale = defaultScaleD * enlargedScale;
        }
        else
        {
            elementD.localScale = defaultScaleD;
        }
    }
}
