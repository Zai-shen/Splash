using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    
    private void LoadSceneWithIndex(int idx)
    {
        SceneManager.LoadScene(idx);
    }

    public void LoadGameScene()
    {
        LoadSceneWithIndex(1);
    }
    
    public void LoadMenuScene()
    {
        LoadSceneWithIndex(0);
    }
    
    public void LoadMenuScene(InputAction.CallbackContext ctx)
    {
        LoadSceneWithIndex(0);
    }

    public void ReLoadCurrentScene()
    {
        LoadSceneWithIndex(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ReLoadCurrentScene(InputAction.CallbackContext ctx)
    {
        LoadSceneWithIndex(SceneManager.GetActiveScene().buildIndex);
    }

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Player.Exit.performed += LoadMenuScene;
        _playerInput.Player.Reset.performed += ReLoadCurrentScene;
        _playerInput.Player.Exit.Enable();
        _playerInput.Player.Reset.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Player.Exit.Disable();
        _playerInput.Player.Reset.Disable();
    }
}
