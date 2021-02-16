using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingsGrid : MonoBehaviour
{
    [Header("Components")]
    public GameObject Canvas;           // канвас

    [Header("Grid Size And Position")]
    [Space][Space]
    public Vector2Int gridSize;         // размер поля для строительства
    public Vector2Int originPosition;   // начальная позиция поля (нижний левый угол поля)
    private Building[,] grid;            // массив со всеми зданиями построиными на полу


    private Building flyingBuilding;    // объект который строится в текущий момент

 
    private bool destroyMode;       //режим сноса
    private bool inDestroyProcess;  //выбран ли объект для сноса
    private GameObject cellDestroy; //задний фон выбранной для сноса клетки


    private bool modificationMode;
    private GameObject modifBuild;
    private GameObject modifBuildCell;



    [Header("Objects On Scene")]
    [Space][Space]
    [SerializeField] GameObject Buildings;    // объекты на сцене в которые кладутся объекты строительства
    [SerializeField] GameObject Cells;        // и клетки, чтобы иерархия сцены не загаживалась и была удобна в использовании
    

    [Header("Cells Prefab")]
    [Space][Space]
    public GameObject CellsPref;            // префабы различных клеток
    public GameObject CellsEmptyPref;
    public GameObject CellsFilledPref;
    public GameObject CellsDestroyPref;
    public GameObject cellModifPref;

    private GameObject cellBackground;      // Объект в котором храниться задний фон объекта при строительстве, уничтожении и т.д.


    [Header("Turn Info Prefab")]
    [Space][Space]
    public GameObject TurnInfoPref;
    GameObject TurnInfo;                // стрелочки показывающие, что нажать, чтобы повернуть объект


    [Header("UI Elements")]
    [Space][Space]
    public GameObject BuildingButtons;          // кнопки для режима строительства
    public GameObject BuildingControlButtons;
    public GameObject DestroyButton;
    public GameObject modifModeButton;


    private void Awake()
    {
        grid = new Building[gridSize.x, gridSize.y];
    }

    void Start()
    { 
        //BuildingButtons.SetActive(true);
        //BuildingControlButtons.SetActive(false);

        destroyMode = false;
        inDestroyProcess = false;

        modificationMode = false;

        for (int x = 0; x < gridSize.x; x++)
            for (int y = 0; y < gridSize.y; y++)
            {
                Instantiate(CellsPref, new Vector3Int(x + originPosition.x, y + originPosition.y, 0), Quaternion.identity, Cells.transform);
            }
    }


    void Update()
    {
        if (flyingBuilding != null)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 position = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);

                if (IsCellInsideGrid(x, y))
                {
                    bool available = IsCellAvailable(x, y);

                    TurnInfo.transform.position = new Vector2(x, y);

                    if (available && x == flyingBuilding.transform.position.x && y == flyingBuilding.transform.position.y)
                        PlaceFlyingBuilding(x, y);
                    else
                    {
                        flyingBuilding.transform.position = new Vector3(x, y, flyingBuilding.transform.position.z);
                        DrawCellBackGround(available);
                        cellBackground.transform.position = flyingBuilding.transform.position;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
                Turn(1);

            if (Input.GetKeyDown(KeyCode.E))
                Turn(-1);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelSelection();
            }
        }


        if(destroyMode) // режим сноса
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector2 position = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);

                if (inDestroyProcess && x == cellDestroy.transform.position.x && y == cellDestroy.transform.position.y) // если выбрана клетка для уничтожения и она нажата повторно. Объект с сетки уничтожается по двойному клику
                {
                    DestroyBuilding(x, y); // уничтожаем объект на выбранной клетке
                }
                else // если выбирается другая клетка 
                {
                    inDestroyProcess = true; // объект для уничтожения выбран
                    if (cellDestroy != null) // если клетка с объектом для уничтожения уже выбраны
                        Destroy(cellDestroy);  // уничтожаем подсветку заднего фона
                    cellDestroy = Instantiate(CellsDestroyPref, new Vector3Int(x, y, 0), Quaternion.identity, Cells.transform);     //  создаем фон на выбранной клетке
                }
            }
        } 


        if (modificationMode)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 position = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
                int x = Mathf.RoundToInt(position.x);
                int y = Mathf.RoundToInt(position.y);

                GameObject build = GetCellFromGrid(x, y);

                if (build != modifBuild)
                {
                    if (modifBuild != null)
                        Destroy(modifBuild.GetComponent<Building>().cellBackground);

                    if (IsCellInsideGrid(x, y))
                    {
                        modifBuild = build;

                        if (modifBuild != null)
                            modifBuild.GetComponent<Building>().cellBackground = Instantiate(cellModifPref, new Vector3(x, y, 0), Quaternion.identity);
                    }
                    else
                    {
                        modifBuild = null;
                    }
                }
            }

            if (modifBuild != null)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                    Turn(1);

                if (Input.GetKeyDown(KeyCode.E))
                    Turn(-1);
            }
        }


        if (Input.GetKeyDown(KeyCode.D))    // активация/деактивация режима сноса по нажтию кнопки клавиатуры
        {
            ActDeactDestroyMode();
        }
    }


    bool IsCellInsideGrid(int x, int y)     // Проверка на попадание вне сетки
    {
        x -= originPosition.x;
        y -= originPosition.y;

        if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
        {
            return true;
        }

        return false;
    }

    bool IsCellAvailable(int x, int y)      // Проверка на возможность поставить объект в сетку
    {
        bool available = true;

        x -= originPosition.x;
        y -= originPosition.y;

        if (grid[x, y] != null)
            available = false;

        if (available && flyingBuilding.GetComponent<RailwayScript>() && flyingBuilding.GetComponent<RailwayScript>().roads == RailwayScript.Roads.Station)
            for (int _x = -1; _x <= 1; _x++)
                for (int _y = -1; _y <= 1; _y++)
                {
                    GameObject build = GetCellFromGrid(x - _x + originPosition.x, y - _y + originPosition.y);

                    if (build && build.GetComponent<RailwayScript>() && build.GetComponent<RailwayScript>().roads == RailwayScript.Roads.Station)
                    {
                        available = false;
                        break;
                    }
                }

        return available;
    }

    void DrawCellBackGround(bool available)
    {
        Destroy(cellBackground);

        if (!available)
            cellBackground = Instantiate(CellsFilledPref, new Vector3Int((int)flyingBuilding.transform.position.x, (int)flyingBuilding.transform.position.y, 0), Quaternion.identity);
        else
            cellBackground = Instantiate(CellsEmptyPref, new Vector3Int((int)flyingBuilding.transform.position.x, (int)flyingBuilding.transform.position.y, 0), Quaternion.identity);
    }
   


    void PlaceFlyingBuilding(int x, int y) // установка здания
    {
        x -= originPosition.x;
        y -= originPosition.y;

        grid[x, y] = flyingBuilding;
        Destroy(cellBackground);

        if (flyingBuilding.GetComponent<RailwayScript>()!=null)
        { 
            flyingBuilding.GetComponent<RailwayScript>().RailwayIsBuid();
        }

        flyingBuilding = null;
        BuildingControlButtons.SetActive(false);
        Destroy(TurnInfo.gameObject);
    }


    public void FillGrid(float x, float y, GameObject build)
    {
        grid[(int)x - originPosition.x, (int)y - originPosition.y] = build.GetComponent<Building>();
    } // добавление объекта на сетку строительства


    public void StartPlacingBuilding(Building BuildingPref) // создание нового объекта строительства
    {
        if (!destroyMode && !modificationMode)
        {
            if (flyingBuilding != null)
            {
                Destroy(flyingBuilding.gameObject);
                Destroy(TurnInfo.gameObject);
            }

            Vector2 flyBuildPos = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth/2, Camera.main.pixelHeight / 2));
            flyBuildPos = new Vector2(Mathf.RoundToInt(flyBuildPos.x), Mathf.RoundToInt(flyBuildPos.y));

            flyingBuilding = Instantiate(BuildingPref, flyBuildPos, Quaternion.identity,Buildings.transform);
            TurnInfo = Instantiate(TurnInfoPref, flyingBuilding.transform.position, Quaternion.identity, Canvas.transform);
            DrawCellBackGround(IsCellAvailable((int)flyingBuilding.transform.position.x, (int)flyingBuilding.transform.position.y));
            BuildingControlButtons.SetActive(true);
        }
    }


    public void CancelSelection() // отмена выбраного объекта строительства
    {
        if (flyingBuilding != null)
        {
            Destroy(TurnInfo.gameObject);
            Destroy(flyingBuilding.gameObject);
            flyingBuilding = null;
            BuildingControlButtons.SetActive(false);
        }
    }
    

    void DestroyBuilding(int x, int y) // уничтожение объекта строительства на сетке
    {
        x -= originPosition.x;
        y -= originPosition.y;

        if (grid[x, y] != null)
        {
            if (grid[x, y].GetComponent<RailwayScript>())
            {
                grid[x, y].GetComponent<RailwayScript>().DeleteRailway();
            }

            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }

        GetComponent<RailwayControler>().UpdateCellsAround(x + originPosition.x, y + originPosition.y);

        inDestroyProcess = false;
        Destroy(cellDestroy);
    }


    void DiactAllModes()
    {
        if (destroyMode)
        {
            ActDeactDestroyMode();
        }

        if (modificationMode)
        {
            ActDeactModificationMode();
        }
    }

    public void ActDeactDestroyMode() //Активация/деактивация режима сноса
    {
        if (flyingBuilding == null)
        {
            if (!destroyMode)
            {
                DiactAllModes();
                DestroyButton.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 28, 0, 255);
            }
            else
            {
                if (cellDestroy != null)
                    Destroy(cellDestroy);
                DestroyButton.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                inDestroyProcess = false;
            }
            BuildingButtons.SetActive(!BuildingButtons.activeSelf);
            destroyMode = !destroyMode;
        }
    }

    public void ActDeactModificationMode()
    {
        if (modificationMode)
        {
            modifBuild = null;
            modifModeButton.transform.GetChild(0).GetComponent<Text>().color = Color.black;
        }
        else
        {
            DiactAllModes();
            modifModeButton.transform.GetChild(0).GetComponent<Text>().color = Color.red;
        }

        BuildingButtons.SetActive(!BuildingButtons.activeSelf);
        modificationMode = !modificationMode;
    }



    public void DeactBuildingMode() // выключить режим строительства
    {
        enabled = false;

        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
            Destroy(TurnInfo.gameObject);
        }

        Destroy(cellBackground);

        DiactAllModes();
    }


    public void Turn(int rotate)   // поворот выбранного объекта для строительтва
    {
        if(flyingBuilding != null)
        {
            if (flyingBuilding.GetComponent<RailwayScript>())
            {
                flyingBuilding.GetComponent<RailwayScript>().Turn(rotate);
            }
        }

        if (modifBuild != null)
        {
            
            if (modifBuild.GetComponent<RailwayScript>())
            {
                modifBuild.GetComponent<RailwayScript>().Turn(rotate);
            }
        }
    }


    public GameObject GetCellFromGrid(int x, int y) // получение клетки из массива с объектами Building
    {
        x -= originPosition.x;
        y -= originPosition.y;

        if (x<0 || y<0 || x> gridSize.x-1 || y >gridSize.y-1)
        {
            return null;
        }
 
        if (grid[x, y] != null)
            return grid[x, y].gameObject;
        else
            return null;
    }
}