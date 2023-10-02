using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] InputReader inputReader;
    [SerializeField] float scrollSensitivity = 1.0f;
    [SerializeField] float minCameraOrthoSize = 3;
    [SerializeField] float maxCameraOrthoSize = 20;
    [SerializeField] Rect cameraBounds = new Rect(new Vector2(-25, -25), new Vector2(50, 50));

    new Camera camera;

    Vector2 lastPointPos = Vector2.zero;

    Vector2? panStartPointPosWorld = null;
    Vector3? panStartTransformPos = null;

    bool pointerOverUI = false;

    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void OnEnable()
    {
        inputReader.ClickEvent += OnClick;
        inputReader.MMBEvent += OnMMB;
        inputReader.PointEvent += OnPoint;
        inputReader.ScrollEvent += OnScroll;
    }

    void OnDisable()
    {
        inputReader.ClickEvent -= OnClick;
        inputReader.MMBEvent += OnMMB;
        inputReader.PointEvent -= OnPoint;
        inputReader.ScrollEvent -= OnScroll;
    }

    void Update()
    {
        pointerOverUI = EventSystem.current.IsPointerOverGameObject();
    }

    void OnClick(bool clicking) { }

    void OnPoint(Vector2 pos)
    {
        lastPointPos = pos;
        if (panStartPointPosWorld is Vector2 startPointPosWorld && panStartTransformPos is Vector3 startPos)
        {
            Vector2 worldPos = camera.ScreenToWorldPoint(pos) - (transform.position - startPos);
            Vector2 delta = worldPos - startPointPosWorld;
            transform.position = startPos - (Vector3)delta;
            ClampCamera();
        }
    }

    void OnMMB(bool down)
    {
        if (down)
        {
            if (!pointerOverUI)
            {
                panStartPointPosWorld = camera.ScreenToWorldPoint(lastPointPos);
                panStartTransformPos = transform.position;
            }
        }
        else
        {
            panStartPointPosWorld = null;
            panStartTransformPos = null;
        }
    }

    void OnScroll(float delta)
    {
        if (pointerOverUI) return;
        Vector3 oldPoint = camera.ScreenToWorldPoint(lastPointPos);
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - delta * scrollSensitivity, minCameraOrthoSize, maxCameraOrthoSize);
        Vector3 newPoint = camera.ScreenToWorldPoint(lastPointPos);
        transform.position -= (newPoint - oldPoint);
        // reset pan start pos
        if(panStartPointPosWorld != null)
        {
            panStartPointPosWorld = camera.ScreenToWorldPoint(lastPointPos);
            panStartTransformPos = transform.position;
        }
        ClampCamera();
    }

    void ClampCamera()
    {
        float orthoWidth = camera.orthographicSize * camera.aspect;
        Rect cameraRect = new()
        {
            width = orthoWidth * 2,
            height = camera.orthographicSize * 2,
            center = transform.position,
        };
        float x = transform.position.x;
        float y = transform.position.y;
        transform.position = new Vector3(
            Mathf.Max(cameraBounds.xMin - cameraRect.xMin, 0)
            + Mathf.Min(cameraBounds.xMax - cameraRect.xMax, 0)
            + x,
            Mathf.Max(cameraBounds.yMin - cameraRect.yMin, 0)
            + Mathf.Min(cameraBounds.yMax - cameraRect.yMax, 0)
            + y,
            transform.position.z
        );
    }
}
