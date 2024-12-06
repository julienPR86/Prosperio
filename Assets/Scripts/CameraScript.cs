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

    private int _x_limit = 1920;
    private int _y_limit = 1080;

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
        //apply the camera movements
        movementVector = _move.ReadValue<Vector2>();
        movementVector += SideMovement(_mouse.ReadValue<Vector2>());
        transform.position += new Vector3(movementVector.x*_navigationSpeed*Time.deltaTime, movementVector.y*_navigationSpeed*Time.deltaTime, 0);
        //check if the camera is out of the map
        if (transform.position.x < 0)
        {
            transform.position = new(0, transform.position.y, 0);
        }
        else if (transform.position.x > _x_limit)
        {
            transform.position = new(_x_limit, transform.position.y, 0);
        }
        if (transform.position.y < 0)
        {
            transform.position = new(transform.position.x, 0, 0);
        }
        else if (transform.position.y > _y_limit)
        {
            transform.position = new(transform.position.x, _y_limit, 0);
        }
    }

    /// <summary>
    /// Based on the mouse position, return a Vector2
    /// </summary>
    /// <param name="mousePosition">The mouse position</param>
    /// <returns>Return a right, left, up or down Vector2</returns>
    private Vector2 SideMovement(Vector2 mousePosition)
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
