using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Camera _camera;
    private InputController _inputs;
    private InputAction _move;
    private InputAction _mouse;
    Vector2 movementVector = new(0,0);
    private int _borderSize = 50;
    private int _navigationSpeed = 1;
    private float _cameraSize;
    private Vector2 _limitOffset;
    private Vector2 _limits = new(1920, 1080);

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
        _camera = GetComponent<Camera>();
        _cameraSize = _camera.orthographicSize;
        float y =_cameraSize;
        float x = _cameraSize*Screen.width/Screen.height;
        _limitOffset.Set(x, y);
    }

    void Update()
    {
        //apply the camera movements
        movementVector = _move.ReadValue<Vector2>();
        movementVector += SideMovement(_mouse.ReadValue<Vector2>());
        transform.position += new Vector3(movementVector.x*_navigationSpeed*Time.deltaTime, movementVector.y*_navigationSpeed*Time.deltaTime, 0);
        //check if the camera is out of the map based on the map and camera size
        if (transform.position.x < 0 + _limitOffset.x)
        {
            transform.position = new(_limitOffset.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > _limits.x - _limitOffset.x)
        {
            transform.position = new(_limits.x - _limitOffset.x, transform.position.y, transform.position.z);
        }
        if (transform.position.y < 0 + _limitOffset.y)
        {
            transform.position = new(transform.position.x, _limitOffset.y, transform.position.z);
        }
        else if (transform.position.y > _limits.y - _limitOffset.y)
        {
            transform.position = new(transform.position.x, _limits.y - _limitOffset.y, transform.position.z);
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
