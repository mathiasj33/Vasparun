using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour
{
    public GameObject Camera { get { return gameObject; } }
    public float cameraSensitivity = 90;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private float rotationZ = 0.0f;

    private bool rotateRight;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
        transform.localRotation *= Quaternion.AngleAxis(rotationZ, Vector3.forward);
    }

    public Vector2 CalculateHeadBob()
    {
        Vector2 bob = new Vector2();
        bob.x = (float) Mathf.Sin(Time.time * 7) * .15f;
        bob.y = (float) Mathf.Sin((Time.time + 0.3f) * 4) * .1f;
        return bob;
    }

    public void Rotate(bool right)
    {
        rotateRight = right;
        StartCoroutine("RotateCamera");
    }

    public void RotateBack()
    {
        StartCoroutine("RotateCameraBack");
    }

    private IEnumerator RotateCamera()
    {
        for (int i = 0; i <= 13; i++)
        {
            rotationZ = rotateRight ? i : -i;
            rotationZ *= 1.5f;
            yield return null;
        }
    }

    private IEnumerator RotateCameraBack()
    {
        for (int i = 0; i <= 13; i++)
        {
            int angle = 13 - i;
            angle *= 2;
            rotationZ = rotateRight ? angle : -angle;
            yield return null;
        }
        rotationZ = 0;
    }
}
