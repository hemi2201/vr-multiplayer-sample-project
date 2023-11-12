using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class LocalVRMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionReference moveInput;

    [SerializeField]
    private InputActionReference rotateInput;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private Transform headTransform;

    private CharacterController characterController;

    private float gravity = 0;

    private void OnEnable()
    {
        moveInput.action.Enable();
        rotateInput.action.Enable();
    }

    private void OnDisable()
    {
        moveInput.action.Disable();
        rotateInput.action.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        Gravity();
    }

    private void Move()
    {
        Vector2 movement = moveInput.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(movement.x, 0, movement.y);
        direction = headTransform.TransformDirection(direction);
        direction = Vector3.Scale(direction, new Vector3(1f, 0f, 1f)).normalized;
        characterController.Move(direction * movementSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector2 rotation = rotateInput.action.ReadValue<Vector2>();
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y + rotation.x, 0) * rotationSpeed);
    }

    private void Gravity() 
    {
        if (characterController.isGrounded)
        {
            gravity = 0f;
        }
        else
        {
            gravity -= 9.81f * Time.deltaTime;
            characterController.Move(new Vector3(0, gravity, 0));
        }
    }
}
