using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
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
    private void Action(Vector2 inputPosition, RaycastHit2D hit)
    {
        var tile = hit.collider.GetComponent<Tile>();
        tile?.OnClick();
    }
}
