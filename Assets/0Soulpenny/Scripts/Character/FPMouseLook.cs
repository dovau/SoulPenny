using ECM2.Examples.FirstPerson;
using UnityEngine;
using UnityEngine.InputSystem;
using ECM2;

namespace Soul
{
    public class FPMouseLook : MonoBehaviour
    {
        Vector2 lookInput;
        [Space(15.0f)]
        public bool invertLook = true;
        [Tooltip("Mouse look sensitivity")]
        public Vector2 mouseSensitivity = new Vector2(1.0f, 1.0f);

        [Space(15.0f)]
        [Tooltip("How far in degrees can you move the camera down.")]
        public float minPitch = -80.0f;
        [Tooltip("How far in degrees can you move the camera up.")]
        public float maxPitch = 80.0f;

        private FPPlayerCharacter _character;
        private MovementStateMachine _movement;


        private void Awake()
        {
            _character = GetComponent<FPPlayerCharacter>();
            _movement = GetComponent<MovementStateMachine>();
            if (_character != null)
            {
                Debug.Log("Character found");
            }
            else
            {
                Debug.Log("FPPlayerCharacter not found");
            }
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
        private void OnEnable()
        {
            _character.Controls.Base.MouseX.performed += OnMouseX;
            _character.Controls.Base.MouseY.performed += OnMouseY;
            _character.Controls.Base.MouseX.Enable();
            _character.Controls.Base.MouseY.Enable();
        }
        private void OnDisable()
        {
            if (_character != null && _character.Controls != null)
            {
                _character.Controls.Base.MouseX.performed -= OnMouseX;
                _character.Controls.Base.MouseY.performed -= OnMouseY;
                _character.Controls.Base.MouseX.Disable();
                _character.Controls.Base.MouseY.Disable();
            }
        }
        public void OnMouseX(InputAction.CallbackContext context)
        {

            lookInput.x = context.ReadValue<float>() * mouseSensitivity.x;
            //lookInput.x *= mouseSensitivity.x;

            _character.AddControlYawInput(lookInput.x);

        }

        public void OnMouseY(InputAction.CallbackContext context)
        {

            lookInput.y = context.ReadValue<float>()*mouseSensitivity.y;
            //lookInput.y *= mouseSensitivity.y;

            _character.AddControlPitchInput(invertLook ? -lookInput.y : lookInput.y);
        }
        private void Update()
        {

        }
    }
}
