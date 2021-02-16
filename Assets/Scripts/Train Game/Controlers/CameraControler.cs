using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CameraControler : MonoBehaviour
{
    [Header("Components")]
    public bool mobileMovement = true;  //If true, then mobile input is active
    public bool editorMovement = true;  //If true, then editor input is active

    [Space(10)]

    public Camera cam;

    [Space(10)]

    public float smoot = 0.04f;         //Lower number is smooter and slower movement
    

    [Header("Movement")]
    public float editorMoveSpeed = 7f;
    public float mobileMoveSpeed = 7f;


    [Header("Clamped Positions of Camera")]
    public float minXpos = 0;       //Минимальные и максимальные координаты по осям
    public float maxXpos = 0;       
    public float minYpos = 0;       
    public float maxYpos = 0;       

    private Vector2 startMousePos;  //начальное положение курсора мыши
    private Vector2 startTouchPos;

    private bool canClamp;          // можем ли двигать камеру 

    private bool mobileFirstTouch;

    


    [Header("Zoom")]
    public float editorZoomSpeed;
    public float mobileZoomSpeed;

    [Space(10)] 

    public float maxZoom;
    public float minZoom;

    private bool mobileStartZoom;

    private float startDistBetwTouch;

    [Header("Debug")]
    public Text debug;



    void Start()
    {
        canClamp = true;
        mobileFirstTouch = false;
        mobileStartZoom = false;
    }


    void Update()
    {
        EditorCameraMovement();
        MobileCameraMovement();
    }


    void EditorCameraMovement()     // Вызов компьютерных функций контроля камеры
    {
        if (editorMovement)
        {
            EditorCameraDrag();
            EditorCameraZoom();
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXpos, maxXpos), Mathf.Clamp(transform.position.y, minYpos, maxYpos), transform.position.z);
        }
    }

    private void EditorCameraDrag()
    {
        if (Input.GetMouseButtonDown(0))    // При нажатии на ЛКМ
        {
            if (EventSystem.current.IsPointerOverGameObject())  // Если ЛКМ была нажата на UI элементе
                canClamp = false;

            startMousePos = cam.ScreenToWorldPoint(Input.mousePosition);    // Начальная позиция курсора мышки
        }

        if (Input.GetMouseButton(0) && canClamp)            // При зажатой ЛКМ
        {
            Vector2 curMousePos = cam.ScreenToWorldPoint(Input.mousePosition);  // Получение текущей позиции курсора

            transform.position = new Vector3(transform.position.x - (curMousePos - startMousePos).x, transform.position.y - (curMousePos - startMousePos).y, -10);              // Сдвиг камеры
            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXpos, maxXpos), Mathf.Clamp(transform.position.y, minYpos, maxYpos), transform.position.z);   // Корректировка положения камеры по границам
        }

        if(Input.GetMouseButtonUp(0))   // При отпущенной ЛКМ
        {
            canClamp = true;
        }    
    }

    void EditorCameraZoom()
    {
        float ortSize = cam.orthographicSize;

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && ortSize < maxZoom)            // Отдаляем камеру 
        {
            cam.orthographicSize = ortSize + editorZoomSpeed;                       // Меняем размер камеры
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);         // Получаем координаты мышки и двигаем камеру в сторону курсора
            float coefOfChangeOrtSize = 1 - ortSize / cam.orthographicSize;         // Получаем коэффициент изменения камеры

            transform.position = new Vector3(transform.position.x + (transform.position.x - mousePos.x) * coefOfChangeOrtSize, transform.position.y + (transform.position.y - mousePos.y) * coefOfChangeOrtSize, transform.position.z);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXpos, maxXpos), Mathf.Clamp(transform.position.y, minYpos, maxYpos), transform.position.z);   // Корректировка положения камеры по границам
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && ortSize > minZoom)            // Приближаем камеру
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);         // Получаем координаты мышки и двигаем камеру в сторону курсора 
            cam.orthographicSize = ortSize - editorZoomSpeed;                       // Меняем размер камеры
            float coefOfChangeOrtSize = 1 - cam.orthographicSize / ortSize;         // Получаем коэффициент изменения камеры

            transform.position = new Vector3(transform.position.x - (transform.position.x - mousePos.x) * coefOfChangeOrtSize, transform.position.y - (transform.position.y - mousePos.y) * coefOfChangeOrtSize, transform.position.z);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXpos, maxXpos), Mathf.Clamp(transform.position.y, minYpos, maxYpos), transform.position.z);   // Корректировка положения камеры по границам
        }
        
    }


    //---------------------------------------------------


    void MobileCameraMovement()         // Вызов мобильных функций контроля камеры
    {
        if (mobileMovement)
        {
            MobileCameraDrag();                    
            MobileCameraZoom();                    
        }
    }

    void MobileCameraDrag()
    {
        if (Input.touchCount == 1)
        {
            if (!mobileFirstTouch)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    canClamp = false;
                startTouchPos = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                mobileFirstTouch = true;
            }
            else if (canClamp)
            {
                
                Vector2 curTouchPos = cam.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(transform.position.x - (curTouchPos - startTouchPos).x, transform.position.y - (curTouchPos - startTouchPos).y, -10);
                //transform.Translate(-(curTouchPos- startTouchPos) * editorMoveSpeed * smoot);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXpos, maxXpos), Mathf.Clamp(transform.position.y, minYpos, maxYpos), transform.position.z);
            }
        }

        if (Input.touchCount == 0 || Input.touchCount > 1)
        {
            canClamp = true;
            mobileFirstTouch = false;
        }
    }

    void MobileCameraZoom()
    {
        

        if (Input.touchCount == 2)
        {
            float ortSize = cam.orthographicSize;   // Записываем размер камеры перед его изменением

            Touch firstTouch = Input.GetTouch(0);   // Записываем тачем
            Touch secondTouch = Input.GetTouch(1);

            Vector2 firstTouchLastPos = firstTouch.position - firstTouch.deltaPosition;    // Получаем прошлые координаты 2 пальцев
            Vector2 secondTouchLastPos = secondTouch.position - secondTouch.deltaPosition;

            float distTouch = (firstTouchLastPos - secondTouchLastPos).magnitude;
            float curDistTouch = (firstTouch.position - secondTouch.position).magnitude;

            float dif = curDistTouch - distTouch;

            Vector2 approachPoint = cam.ScreenToWorldPoint((firstTouch.position + secondTouch.position) / 2) ;        // Точка приближения, находиться по середине между двумя пальцами

            debug.text = mobileZoomSpeed.ToString();

            cam.orthographicSize = Mathf.Clamp(ortSize - dif * mobileZoomSpeed * 0.01f, minZoom, maxZoom);

            if (dif < 0 && ortSize < maxZoom)
            {
                float coefOfChangeOrtSize = 1 - cam.orthographicSize / ortSize;     // Получаем коэффициент изменения камеры

                transform.position = new Vector3(transform.position.x - (transform.position.x - approachPoint.x) * coefOfChangeOrtSize, transform.position.y - (transform.position.y - approachPoint.y) * coefOfChangeOrtSize, transform.position.z);
            }

            if (dif > 0 && ortSize > minZoom)
            {
                float coefOfChangeOrtSize = 1 - cam.orthographicSize / ortSize;     // Получаем коэффициент изменения камеры

                transform.position = new Vector3(transform.position.x - (transform.position.x - approachPoint.x) * coefOfChangeOrtSize, transform.position.y - (transform.position.y - approachPoint.y) * coefOfChangeOrtSize, transform.position.z);
            }
        }    

        if(Input.touchCount == 0)
        {
            debug.text = "";
            startDistBetwTouch = 0;
            mobileStartZoom = false;
        }
    }
}