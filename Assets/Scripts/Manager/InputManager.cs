using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private bool _isMousePressed = false;

    void Update()
    {
        HandleGameInput();
    }

    private void HandleGameInput()
    {
        if (Input.touchCount > 0)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleTouchInput()
    {
        Touch touch = Input.GetTouch(0);
        var touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
        if (!hit.collider) return;

        switch (touch.phase)
        {
            case TouchPhase.Ended:
                Action(touch.position, hit);
                break;
        }
    }

    private void HandleMouseInput()
    {
        var mousePos = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (!hit.collider) return;

        if (Input.GetMouseButtonDown(0))
        {
            _isMousePressed = true;
        }
        else if (Input.GetMouseButtonUp(0) && _isMousePressed)
        {
            _isMousePressed = false;
            Action(Input.mousePosition, hit);
        }
    }
    private void Action(Vector2 inputPosition, RaycastHit2D hit)
    {
        var tile = hit.collider.GetComponent<Tile>();
        tile?.OnClick();
    }
}
