using UnityEngine;

namespace MptUnity.Test.IO.Behaviour
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        float movementSpeed = 3.0f;

        [SerializeField] 
        float rotationSpeed = 0.1f;
        
        // Key bindings
        public KeyCode forwards;
        public KeyCode backwards;
        public KeyCode right;
        public KeyCode left;
        public KeyCode up;
        public KeyCode down;

        // relative to display width and height.
        Vector2 m_mousePositionLast = new Vector2(0.5f, 0.5f);

        // Update is called once per frame
        void Update()
        {
            Rotate();
            Move();
        }

        private void Rotate()
        {
            Vector2 mousePositionLastDisplay = new Vector2(
                m_mousePositionLast.x * Display.main.renderingWidth,
                m_mousePositionLast.y * Display.main.renderingHeight
                );
            mousePositionLastDisplay = (Vector2) Input.mousePosition - mousePositionLastDisplay;
            Vector2 currentViewAngle = transform.eulerAngles;
            transform.eulerAngles = new Vector2(
                currentViewAngle.x + - mousePositionLastDisplay.y * rotationSpeed, 
                currentViewAngle.y + mousePositionLastDisplay.x * rotationSpeed
            );
                
            m_mousePositionLast = new Vector2(
                Input.mousePosition.x / Display.main.renderingWidth,
                Input.mousePosition.y / Display.main.renderingHeight
            );
        }

        private void Move()
        {
            var dir = Vector3.zero;
            if (Input.GetKey(forwards))
            {
                dir += new Vector3(0.0f, 0.0f, 1.0f);
            }
            if (Input.GetKey(backwards))
            {
                dir += new Vector3(0.0f, 0.0f, -1.0f);
            }
            if (Input.GetKey(left))
            {
                dir += new Vector3(-1.0f, 0.0f, 0.0f);
            }
            if (Input.GetKey(right))
            {
                dir += new Vector3(1.0f, 0.0f, 0.0f);
            }
            if (Input.GetKey(up))
            {
                dir += new Vector3(0.0f, 1.0f, 0.0f);
            }
            if (Input.GetKey(down))
            {
                dir += new Vector3(0.0f, -1.0f, 0.0f);
            }
            dir = movementSpeed * Time.deltaTime * dir;
            // actual movement.
            transform.Translate(dir); 
        }
    }
}
