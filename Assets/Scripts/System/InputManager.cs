using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    /// <summary>
    /// 入力ボタンの種類
    /// </summary>
    [Flags]
    public enum ButtonType
    {
        None    = 0,
        Fire    = 1 << 0,
        Cancel  = 1 << 1,
        Option  = 1 << 2,
        Menu    = 1 << 3,

        MAX = 4
    }

    private PikoControls input;
    private Vector2 move;
    private ButtonType button;

    public void Setup()
    {
        input = new PikoControls();
        input.Enable();
    }

    public void OnUpdate()
    {
        CheckMoveTrigger();
        CheckButtonTrigger();
    }

    void CheckMoveTrigger()
    {
        move = Vector2.zero;
        move = input.Player.Move.ReadValue<Vector2>();
    }

    void CheckButtonTrigger()
    {
        var actionMap = input.Player;
        button = ButtonType.None;

        button |= actionMap.Fire.triggered   ? ButtonType.Fire      : ButtonType.None;
        button |= actionMap.Cancel.triggered ? ButtonType.Cancel    : ButtonType.None;
        button |= actionMap.Option.triggered ? ButtonType.Option    : ButtonType.None;
        button |= actionMap.Menu.triggered   ? ButtonType.Menu      : ButtonType.None;
    }

    /// <summary>
    /// 移動(長押し対応)
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMoveValue()
    {
        return input.Player.Move.ReadValue<Vector2>();
    }

    public bool GetMoveTrigger()
    {
        var value = input.Player.Move.triggered;
        Debug.Log(value);
        return value;
    }

    public bool GetButtonTrigger(ButtonType type)
    {
        return (button & type) > 0;
    }
}
