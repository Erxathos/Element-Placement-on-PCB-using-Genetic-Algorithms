using UnityEngine;

public class СameraScript : MonoBehaviour
{
    public Transform target;

    public float mouse_sens = 1f;
    public float touch_sens = 1f;

    float distance = 150;
#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
    float zoomSpeed = 0.5f; //скорость приближения камеры
#endif

    void LateUpdate()
    {
#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
        if (Input.touchSupported)
        {
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //position in the previous frame
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //length of the touch
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag; //length of the pinch

                distance = Mathf.Clamp(distance + deltaMagnitudeDiff * zoomSpeed, 15, 800);
                transform.position = transform.forward * (-distance) + target.position;
            }

            if (Input.touchCount == 1)
            {
                Touch touchZero = Input.GetTouch(0);
                float x_axis = touchZero.deltaPosition.x * mouse_sens;
                float y_axis = -touchZero.deltaPosition.y * mouse_sens;

                target.transform.Rotate(Vector3.up, x_axis, Space.World);
                target.transform.Rotate(Vector3.right, y_axis, Space.Self);
            }
        }
#else
        if (Input.GetMouseButton(0)) //left mouse button
        {
            float x_axis = Input.GetAxis("Mouse X") * mouse_sens;
            float y_axis = -Input.GetAxis("Mouse Y") * mouse_sens;

            target.Rotate(Vector3.up, x_axis, Space.World);
            target.Rotate(Vector3.right, y_axis, Space.Self);
        }

        if (Input.GetMouseButton(1)) //right mouse button
        {
            float x_axis = Input.GetAxis("Mouse X") * mouse_sens;
            float y_axis = -Input.GetAxis("Mouse Y") * mouse_sens;

            target.Rotate(Vector3.up, x_axis, Space.World);
            target.Rotate(Vector3.right, y_axis, Space.Self);
        }

        float zoom = Input.GetAxis("Mouse ScrollWheel");
        if (zoom != 0)
        {
            distance *= (1 - zoom);
            distance = Mathf.Clamp(distance, 15, 800);
            transform.position = transform.forward * (-distance) + target.position;
        }
#endif
    }
}