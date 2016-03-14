using UnityEngine;
using System.Collections;

namespace Scripts
{
    public class FlyCameraControl : MonoBehaviour
    {
        public float cameraSensitivity = 90;
        public float moveSpeed = 10;
        private float rotationX = 0.0f;
        private float rotationY = 0.0f;

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

            transform.position += transform.forward * moveSpeed * Input.GetAxisRaw("Vertical") * Time.deltaTime;
            transform.position += transform.right * moveSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        }
    }
}