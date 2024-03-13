using UnityEngine;


//FIXME: camera osa y, parralax efekt - zákázat pohyb na y


[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;

    private float oldPositionX;

    void Start()
    {
        oldPositionX = transform.position.x;
    }

    void Update()
    {
        if (transform.position.x != oldPositionX)
        {
            if (onCameraTranslate != null)
            {
                float deltaX = oldPositionX - transform.position.x;
                onCameraTranslate(deltaX);
            }

            oldPositionX = transform.position.x;
        }
    }
}
