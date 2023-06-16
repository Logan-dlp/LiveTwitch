using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(PlayerInteraction))]
public class PlayerController : MonoBehaviour
{
    private GameManager manager;
    
    private Vector3 direction;
    private Vector2 input;
    private float currentVelocity;
    
    private Animator animator;
    private GameObject light;
    
    [HideInInspector] public bool LightIsActive;
    
    private PlayerInput playerInput;
    private CharacterController controller;
    
    [Header("Physiques du joueur")]
    public float Gravity;
    public float Speed;
    public float RotateSmoothTime = .05f;
    
    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.Player.Add(gameObject);
        
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        light = GetComponentInChildren<Light>().gameObject;
        
        ApplyLight(false);

        InputAction _move = playerInput.actions["Move"];
        _move.performed += MovePerformed;
        _move.canceled += MoveCanceled;
    }

    private void Update()
    {
        ApplyGravity();
        Mouvement();
        ApplyAnimation();
    }

    void MovePerformed(InputAction.CallbackContext _ctx)
    {
        input = _ctx.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0, input.y);
    }

    void MoveCanceled(InputAction.CallbackContext _ctx)
    {
        input = Vector2.zero;
        direction = Vector3.zero;
    }

    void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            Vector3 _gravityDirection = Vector3.zero;
            _gravityDirection.y = -Gravity;
            controller.Move(_gravityDirection * Time.deltaTime);
        }
    }

    void Mouvement()
    {
        if (input.sqrMagnitude == 0) return;
        float _targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref currentVelocity, RotateSmoothTime);
        transform.rotation = Quaternion.Euler(0, _angle, 0);
        controller.Move(direction * Speed * Time.deltaTime);
    }

    void ApplyAnimation()
    {
        if (input != Vector2.zero) animator.SetBool("IsRunning", true);
        else animator.SetBool("IsRunning", false);
    }

    public void ApplyLight(bool _on)
    {
        light.SetActive(_on);
        LightIsActive = _on;
    }
}
