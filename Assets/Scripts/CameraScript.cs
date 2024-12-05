using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private InputController _inputs;
    private InputAction _move;
    private InputAction _mouse;
    Vector2 movementVector = new(0,0);
    private int _borderSize = 50;
    private int _navigationSpeed = 1;

    void Awake()
    {
        _inputs = new InputController();
    }
    void OnEnable()
    {
        _inputs.Enable();
        _move = _inputs.Player.Move;
        _mouse = _inputs.Mouse.Position;
    }
    void OnDisable()
    {
        _inputs.Disable();
    }
    void Start()
    {

    }

    void Update()
    {
        Vector2 mousePosition = _mouse.ReadValue<Vector2>();
        movementVector = _move.ReadValue<Vector2>();
        movementVector += MouseMovement(mousePosition);
        transform.position += new Vector3(movementVector.x*_navigationSpeed*Time.deltaTime, 0, movementVector.y*_navigationSpeed*Time.deltaTime);
    }

    /// <summary>
    /// Based on the mouse position, return a Vector2
    /// </summary>
    /// <param name="mousePosition">The mouse position</param>
    /// <returns>Return a right, left, up or down Vector2</returns>
    private Vector2 MouseMovement(Vector2 mousePosition)
    {
        Vector2 movement = new(0,0);
        if (mousePosition.x < 0 + _borderSize)
        {
            movement.Set(-1, movement.y);
        }
        else if (mousePosition.x > Screen.width - _borderSize )
        {
            movement.Set(1, movement.y);
        }
        if (mousePosition.y < 0 + _borderSize)
        {
            movement.Set(movement.x, -1);
        }
        else if (mousePosition.y > Screen.height - _borderSize)
        {
            movement.Set(movement.x, 1);
        }
        return movement;
    }
}
