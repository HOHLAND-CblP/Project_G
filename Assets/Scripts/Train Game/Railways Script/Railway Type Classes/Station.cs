using UnityEngine;


public class Station : MonoBehaviour
{
    public bool endStation;

    bool horizontal = false;    //тип дороги для отрисовки
    bool vertical = false;
    bool diagonal_1 = false;    // Вертикаль от снизу слева до сверху справа
    bool diagonal_2 = false;    // Вертикаль от снизу справа до сверху слева

    [SerializeField]
    int curType;
    [SerializeField]
    int whichSide;
    GameObject[,] aroundCell = new GameObject[3, 3];

    public float horizontalScale; //размеры дорог
    public float digonalScale;

    [SerializeField]
    Vector2[] points_1 = new Vector2[1];    //точки для движения
    [SerializeField]
    Vector2[] points_2 = new Vector2[1];

    Vector2 dir_1;  //сектора входа поезда
    Vector2 dir_2;

    readonly Vector2[] vectorError = { new Vector2(0.7654f, 0.321f) };
    private RailwayScript railwayScript;


    //    Порядок создания станций:
    //      -> определение возможных типов соединения
    //      -> проверка не выбран ли тип - не выбран, ставим пустой по умолчанию
    //      -> создаем конекты по выбраному типу
    //      -> проверяем на конечность/проточность станции
    //      -> отрисовываем станцию по выбраному типу



    void Start()
    {
        railwayScript = GetComponent<RailwayScript>();
        Camera.main.GetComponent<RespawnMode>().AddStation(gameObject);
    }

    public Vector2[] GetPoints(Vector2 dir)
    {
        if (dir == dir_1)
            return points_1;
        else if (dir == dir_2)
            return points_2;
        else
            return vectorError;
    }

    public Vector2[] ChangeDirection(Vector2[] points)
    {
        if (points == points_1)
            return points_2;
        else if (points == points_2)
            return points_1;
        return vectorError;
    }

    public Vector3 GetAngles()
    {
        switch (curType)
        {
            case 1:
                return new Vector3(0, 0, 0);
            case 2:
                return new Vector3(0, 0, 90);
            case 3:
                return new Vector3(0, 0, 45);
            case 4:
                return new Vector3(0, 0, 135);
            default:
                return new Vector3(1, 2, 3);
        }
    }



    public void FullReset() // Функция полного обновления станции
    {
        PossibleTypes();    // Определяем возможный тип
        SelectCurType();    // Выбираем текущий тип
        IsItEndStation();   // Определяем конечная ли станция
        DrawStation();      // Обработака визульного вида станции
        MakeNewPoints();    // Создаем точки движения
    }

    public void BuildRailway()
    {
        MakeNewPoints();
    }



    // Создание точек и конектов
    void MakeNewPoints()
    {
        switch (curType)
        {
            case 0:
                GetComponent<RailwayScript>().DeleteConects();
                points_1[0] = new Vector2();
                points_2[0] = new Vector2();
                dir_1 = new Vector2();
                dir_2 = new Vector2();
                break;

            case 1:
                HorizontalPoints();
                HorizontalConects();
                break;

            case 2:
                VerticalPoints();
                VerticalConects();
                break;

            case 3:
                FirstDiagonalPoints();
                FirstDiagonalConects();
                break;

            case 4:
                SecondDiagonalPoints();
                SecondDiagonalConects();
                break;
        }

    }



    // Создание конектов
    public void MakeNewConects()
    {
        PossibleTypes();
        SelectCurType();
        IsItEndStation();


        switch (curType)
        {
            case 0:
                GetComponent<RailwayScript>().DeleteConects();
                break;

            case 1:
                HorizontalConects();
                break;

            case 2:
                VerticalConects();
                break;

            case 3:
                FirstDiagonalConects();
                break;

            case 4:
                SecondDiagonalConects();
                break;
        }
    }

    public void MakeVisualConects()
    {
        PossibleTypes();
        SelectCurType();
        IsItEndStation();

        switch (curType)
        {
            case 0:
                GetComponent<RailwayScript>().DeleteConects();
                break;

            case 1:
                HorizontalVisualConects();
                break;

            case 2:
                VerticalVisualConects();
                break;

            case 3:
                FirstDiagonalVisualConects();
                break;

            case 4:
                SecondDiagonalVisualConects();
                break;
        }
    }


    void PossibleTypes()        // Определение возможных типов соединений
    {
        horizontal = false;     // обнуляем все существующие соединенеия
        vertical = false;
        diagonal_1 = false;
        diagonal_2 = false;


        for (int i = -1; i < 2; i++)
            for (int k = -1; k < 2; k++)
                aroundCell[i + 1, k + 1] = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x + i, (int)transform.position.y + k);


        if (aroundCell[2, 1] || aroundCell[0, 1])   // Горизонтальное соединение
        {
            if (aroundCell[2, 1] && aroundCell[2, 1].GetComponent<RailwayScript>() && aroundCell[2, 1].GetComponent<RailwayScript>().IsConect(new Vector2(1, 0)))
                horizontal = true;
            else if (aroundCell[0, 1] && aroundCell[0, 1].GetComponent<RailwayScript>() && aroundCell[0, 1].GetComponent<RailwayScript>().IsConect(new Vector2(-1,0)))
                horizontal = true;
        }

        if (aroundCell[1, 2] || aroundCell[1, 0])   // Вертикальное соединение
        {
            if (aroundCell[1, 2] && aroundCell[1, 2].GetComponent<RailwayScript>() && aroundCell[1, 2].GetComponent<RailwayScript>().IsConect(new Vector2(0,1)))
                vertical = true;
            else if (aroundCell[1, 0] && aroundCell[1, 0].GetComponent<RailwayScript>() && aroundCell[1, 0].GetComponent<RailwayScript>().IsConect(new Vector2(0, -1)))
                vertical = true;
        }

        if (aroundCell[2, 2] || aroundCell[0, 0])   // Вертикаль первая
        {
            if (aroundCell[2, 2] && aroundCell[2, 2].GetComponent<RailwayScript>() && aroundCell[2, 2].GetComponent<RailwayScript>().IsConect(new Vector2(1, 1)))
                diagonal_1 = true;
            else if (aroundCell[0, 0] && aroundCell[0, 0].GetComponent<RailwayScript>() && aroundCell[0, 0].GetComponent<RailwayScript>().IsConect(new Vector2(-1, -1)))
                diagonal_1 = true;
        }

        if (aroundCell[0, 2] || aroundCell[2, 0])   // Вертикаль вторая
        {
            if (aroundCell[0, 2] && aroundCell[0, 2].GetComponent<RailwayScript>() && aroundCell[0, 2].GetComponent<RailwayScript>().IsConect(new Vector2(-1, 1)))
                diagonal_2 = true;
            else if (aroundCell[2, 0] && aroundCell[2, 0].GetComponent<RailwayScript>() && aroundCell[2, 0].GetComponent<RailwayScript>().IsConect(new Vector2(1, -1)))
                diagonal_2 = true;
        }
    }
    void SelectCurType()        // Задаем текущий тип
    {
        switch (curType)      // Если тип выбран, но он не соответствует одному из возможных в данный момент типов, то тип сбрасывается   
        {
            case 1:
                if (!horizontal)
                    curType = 0;
                break;

            case 2:
                if (!vertical)
                    curType = 0;
                break;

            case 3:
                if (!diagonal_1)
                    curType = 0;
                break;

            case 4:
                if (!diagonal_2)
                    curType = 0;
                break;
        }

        

        if (curType == 0)         // Если тип не выбран
        {
            if (horizontal)
                curType = 1;
            else if (vertical)
                curType = 2;
            else if (diagonal_1)
                curType = 3;
            else if (diagonal_2)
                curType = 4;
        }
    }
    void IsItEndStation()       // Определяется конечная ли станция
    {
        endStation = false;

        switch (curType)
        {
            case 1:
                if (aroundCell[0, 1] && aroundCell[0, 1].GetComponent<RailwayScript>()) // Соединение слева
                {
                    if (aroundCell[0, 1].GetComponent<RailwayScript>().IsConect(new Vector2(-1, 0)))
                    {
                        whichSide = -1;
                        endStation = true;
                    }
                }
                if (aroundCell[2, 1] && aroundCell[2, 1].GetComponent<RailwayScript>()) // Соединение справа
                {
                    if (aroundCell[2, 1].GetComponent<RailwayScript>().IsConect(new Vector2(1, 0)))
                    {
                        whichSide = 1;
                        endStation = !endStation;
                    }
                }
                break;

            case 2:
                if (aroundCell[1, 2] && aroundCell[1, 2].GetComponent<RailwayScript>())
                {
                    if (aroundCell[1, 2].GetComponent<RailwayScript>().IsConect(new Vector2(0, 1)))
                    {
                        whichSide = 1;
                        endStation = true;
                    }
                }
                if (aroundCell[1, 0] && aroundCell[1, 0].GetComponent<RailwayScript>())
                {
                    if (aroundCell[1, 0].GetComponent<RailwayScript>().IsConect(new Vector2(0, -1)))
                    {
                        whichSide = -1;
                        endStation = !endStation;
                    }
                }
                break;

            case 3:
                if (aroundCell[2, 2] && aroundCell[2, 2].GetComponent<RailwayScript>())
                {
                    if (aroundCell[2, 2].GetComponent<RailwayScript>().IsConect(new Vector2(1, 1)))
                    {
                        whichSide = 1;
                        endStation = true;
                    }
                }
                if (aroundCell[0, 0] && aroundCell[0, 0].GetComponent<RailwayScript>())
                {
                    if (aroundCell[0, 0].GetComponent<RailwayScript>().IsConect(new Vector2(-1, -1)))
                    {
                        whichSide = -1;
                        endStation = !endStation;
                    }
                }
                break;

            case 4:
                if (aroundCell[0, 2] && aroundCell[0, 2].GetComponent<RailwayScript>())
                {
                    if (aroundCell[0, 2].GetComponent<RailwayScript>().IsConect(new Vector2(-1, 1)))
                    {
                        whichSide = -1;
                        endStation = true;
                    }
                }
                if (aroundCell[2, 0] && aroundCell[2, 0].GetComponent<RailwayScript>())
                {
                    if (aroundCell[2, 0].GetComponent<RailwayScript>().IsConect(new Vector2(1, -1)))
                    {
                        whichSide = 1;
                        endStation = !endStation;
                    }
                }
                break;
        }

        if (!endStation)
            whichSide = 0;
    }
    public void DrawStation()   // Отрисовка дороги
    {
        switch (curType)
        {
            case 0:
                GameObject temp_ = transform.GetChild(0).gameObject;
                temp_.transform.localScale = new Vector3(0, temp_.transform.localScale.y, temp_.transform.localScale.z);
                break;

            case 1:
                if (endStation)
                {
                    GameObject leftCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x - 1, (int)transform.position.y);

                    if (leftCell && leftCell.GetComponent<RailwayScript>() && leftCell.GetComponent<RailwayScript>().IsConect(new Vector2(-1, 0)))
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
                        temp.transform.localScale = new Vector3(horizontalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(-horizontalScale / 4, 0, 0);
                    }
                    else
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 0);
                        var localScale = temp.transform.localScale;
                        localScale = new Vector3(horizontalScale / 2, localScale.y, localScale.z);
                        temp.transform.localScale = localScale;
                        temp.transform.localPosition = new Vector3(horizontalScale / 4, 0, 0);
                    }
                }
                else
                {
                    GameObject temp = transform.GetChild(0).gameObject;
                    temp.transform.rotation = Quaternion.Euler(0, 0, 0);
                    temp.transform.localScale = new Vector3(horizontalScale, temp.transform.localScale.y, temp.transform.localScale.z);
                    temp.transform.localPosition = new Vector3(0, 0, 0);
                }

                break;

            case 2:
                if (endStation)
                {
                    GameObject downCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x, (int)transform.position.y - 1);

                    if (downCell && downCell.GetComponent<RailwayScript>() && downCell.GetComponent<RailwayScript>().IsConect(new Vector2(0, -1)))
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 90);
                        temp.transform.localScale = new Vector3(horizontalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(0, -horizontalScale / 4, 0);
                    }
                    else
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 90);
                        temp.transform.localScale = new Vector3(horizontalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(0, horizontalScale / 4, 0);
                    }
                }
                else
                {
                    GameObject temp = transform.GetChild(0).gameObject;
                    temp.transform.rotation = Quaternion.Euler(0, 0, 90);
                    temp.transform.localScale = new Vector3(horizontalScale, temp.transform.localScale.y, temp.transform.localScale.z);
                    temp.transform.localPosition = new Vector3(0, 0, 0);
                }

                break;

            case 3:
                if (endStation)
                {
                    GameObject downLedtCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x - 1, (int)transform.position.y - 1);

                    if (downLedtCell && downLedtCell.GetComponent<RailwayScript>() && downLedtCell.GetComponent<RailwayScript>().IsConect(new Vector2(-1, -1)))
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 45);
                        temp.transform.localScale = new Vector3(digonalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(-horizontalScale / 4, -horizontalScale / 4, 0);
                    }
                    else
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 45);
                        temp.transform.localScale = new Vector3(digonalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(horizontalScale / 4, +horizontalScale / 4, 0);
                    }
                }
                else
                {
                    GameObject temp = transform.GetChild(0).gameObject;
                    temp.transform.rotation = Quaternion.Euler(0, 0, 45);
                    temp.transform.localScale = new Vector3(digonalScale, temp.transform.localScale.y, temp.transform.localScale.z);
                    temp.transform.localPosition = new Vector3(0, 0, 0);
                }

                break;

            case 4:
                if (endStation)
                {
                    GameObject upLedtCell = Camera.main.GetComponent<BuildingsGrid>().GetCellFromGrid((int)transform.position.x - 1, (int)transform.position.y + 1);

                    if (upLedtCell && upLedtCell.GetComponent<RailwayScript>() && upLedtCell.GetComponent<RailwayScript>().IsConect(new Vector2(-1, 1)))
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 135);
                        temp.transform.localScale = new Vector3(digonalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(-horizontalScale / 4, horizontalScale / 4, 0);
                    }
                    else
                    {
                        GameObject temp = transform.GetChild(0).gameObject;
                        temp.transform.rotation = Quaternion.Euler(0, 0, 135);
                        temp.transform.localScale = new Vector3(digonalScale / 2, temp.transform.localScale.y, temp.transform.localScale.z);
                        temp.transform.localPosition = new Vector3(horizontalScale / 4, -horizontalScale / 4, 0);
                    }

                }
                else
                {
                    GameObject temp = transform.GetChild(0).gameObject;
                    temp.transform.rotation = Quaternion.Euler(0, 0, 135);
                    temp.transform.localScale = new Vector3(digonalScale, temp.transform.localScale.y, temp.transform.localScale.z);
                    temp.transform.localPosition = new Vector3(0, 0, 0);
                }

                break;
        }
    }

    public void DrawDefault()
    {
        GameObject temp_ = transform.GetChild(0).gameObject;
        temp_.transform.localScale = new Vector3(0, temp_.transform.localScale.y, temp_.transform.localScale.z);
    }



    void HorizontalPoints()
    {
        switch (whichSide)
        {
            case -1:
                points_1[0] = new Vector2(transform.position.x - 0.501f, transform.position.y);
                points_2[0] = new Vector2(transform.position.x, transform.position.y);
                break;

            case 0:
                points_1[0] = new Vector2(transform.position.x - 0.501f, transform.position.y);
                points_2[0] = new Vector2(transform.position.x + 0.501f, transform.position.y);
                break;

            case 1:
                points_1[0] = new Vector2(transform.position.x, transform.position.y);
                points_2[0] = new Vector2(transform.position.x + 0.501f, transform.position.y);
                break;
        }
    }
    void VerticalPoints()
    {
        switch (whichSide)
        {
            case -1:
                points_1[0] = new Vector2(transform.position.x, transform.position.y - 0.501f);
                points_2[0] = new Vector2(transform.position.x, transform.position.y);
                break;

            case 0:
                points_1[0] = new Vector2(transform.position.x, transform.position.y - 0.501f);
                points_2[0] = new Vector2(transform.position.x, transform.position.y + 0.501f);
                break;

            case 1:
                points_1[0] = new Vector2(transform.position.x, transform.position.y);
                points_2[0] = new Vector2(transform.position.x, transform.position.y + 0.501f);
                break;
        }
    }
    void FirstDiagonalPoints()
    {
        switch (whichSide)
        {
            case -1:
                points_1[0] = new Vector2(transform.position.x - 0.501f, transform.position.y - 0.501f);
                points_2[0] = new Vector2(transform.position.x, transform.position.y);
                break;

            case 0:
                points_1[0] = new Vector2(transform.position.x - 0.501f, transform.position.y - 0.501f);
                points_2[0] = new Vector2(transform.position.x + 0.501f, transform.position.y + 0.501f);
                break;

            case 1:
                points_1[0] = new Vector2(transform.position.x, transform.position.y);
                points_2[0] = new Vector2(transform.position.x + 0.501f, transform.position.y + 0.501f);
                break;
        }               
    }
    void SecondDiagonalPoints()
    {
        switch (whichSide)
        {
            case -1:
                points_1[0] = new Vector2(transform.position.x - 0.501f, transform.position.y + 0.501f);
                points_2[0] = new Vector2(transform.position.x, transform.position.y);
                break;

            case 0:
                points_1[0] = new Vector2(transform.position.x - 0.501f, transform.position.y + 0.501f);
                points_2[0] = new Vector2(transform.position.x + 0.501f, transform.position.y - 0.501f);
                break;

            case 1:
                points_1[0] = new Vector2(transform.position.x, transform.position.y);
                points_2[0] = new Vector2(transform.position.x + 0.501f, transform.position.y - 0.501f);
                break;
        }
    }

    void HorizontalConects()
    {
        railwayScript.DeleteConects();

        switch (whichSide)
        {
            case -1:
                dir_1 = new Vector2();
                dir_2 = new Vector2(-1, 0);

                GetComponent<RailwayScript>().AddConect(dir_2);
                break;

            case 0:
                dir_1 = new Vector2(1, 0);
                dir_2 = new Vector2(-1, 0);

                GetComponent<RailwayScript>().AddConect(dir_1);
                GetComponent<RailwayScript>().AddConect(dir_2);
                break;

            case 1:
                dir_1 = new Vector2(1, 0);
                dir_2 = new Vector2();

                GetComponent<RailwayScript>().AddConect(dir_1);
                break;
        }
    }
    void VerticalConects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        switch (whichSide)
        {
            case -1:
                dir_1 = new Vector2();
                dir_2 = new Vector2(0, -1);

                GetComponent<RailwayScript>().AddConect(dir_2);
                break;

            case 0:
                dir_1 = new Vector2(0, 1);
                dir_2 = new Vector2(0, -1);

                GetComponent<RailwayScript>().AddConect(dir_1);
                GetComponent<RailwayScript>().AddConect(dir_2);
                break;

            case 1:
                dir_1 = new Vector2(0, 1);
                dir_2 = new Vector2();

                GetComponent<RailwayScript>().AddConect(dir_1);
                break;
        }
    }    
    void FirstDiagonalConects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        switch (whichSide)
        {
            case -1:
                dir_1 = new Vector2();
                dir_2 = new Vector2(-1, -1);

                GetComponent<RailwayScript>().AddConect(dir_2);
                break;

            case 0:
                dir_1 = new Vector2(1, 1);
                dir_2 = new Vector2(-1, -1);

                GetComponent<RailwayScript>().AddConect(dir_1);
                GetComponent<RailwayScript>().AddConect(dir_2);
                break;

            case 1:
                dir_1 = new Vector2(1, 1);
                dir_2 = new Vector2();

                GetComponent<RailwayScript>().AddConect(dir_1);
                break;
        }


    }
    void SecondDiagonalConects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        switch (whichSide)
        {
            case -1:
                dir_1 = new Vector2();
                dir_2 = new Vector2(-1, 1);

                GetComponent<RailwayScript>().AddConect(dir_2);
                break;

            case 0:
                dir_1 = new Vector2(1, -1);
                dir_2 = new Vector2(-1, 1);

                GetComponent<RailwayScript>().AddConect(dir_1);
                GetComponent<RailwayScript>().AddConect(dir_2);
                break;

            case 1:
                dir_1 = new Vector2(1, -1);
                dir_2 = new Vector2();

                GetComponent<RailwayScript>().AddConect(dir_1);
                break;
        }
    }


    void HorizontalVisualConects()
    {
        railwayScript.DeleteConects();

        switch (whichSide)
        {
            case -1:
                GetComponent<RailwayScript>().AddConect(new Vector2(-1, 0));
                break;

            case 0:
                GetComponent<RailwayScript>().AddConect(new Vector2(1, 0));
                GetComponent<RailwayScript>().AddConect(new Vector2(-1, 0));
                break;

            case 1:
                GetComponent<RailwayScript>().AddConect(new Vector2(1, 0));
                break;
        }
    }
    void VerticalVisualConects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        switch (whichSide)
        {
            case -1:
                GetComponent<RailwayScript>().AddConect(new Vector2(0, -1));
                break;

            case 0:
                GetComponent<RailwayScript>().AddConect(new Vector2(0, 1));
                GetComponent<RailwayScript>().AddConect(new Vector2(0, -1));
                break;

            case 1:
                GetComponent<RailwayScript>().AddConect(new Vector2(0, 1));
                break;
        }
    }
    void FirstDiagonalVisualConects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        switch (whichSide)
        {
            case -1:
                GetComponent<RailwayScript>().AddConect(new Vector2(-1, -1));
                break;

            case 0:
                GetComponent<RailwayScript>().AddConect(new Vector2(1, 1));
                GetComponent<RailwayScript>().AddConect(new Vector2(-1, -1));
                break;

            case 1:
                GetComponent<RailwayScript>().AddConect(new Vector2(1, 1));
                break;
        }


    }
    void SecondDiagonalVisualConects()
    {
        GetComponent<RailwayScript>().DeleteConects();

        switch (whichSide)
        {
            case -1:
                GetComponent<RailwayScript>().AddConect(new Vector2(-1, 1));
                break;

            case 0:
                GetComponent<RailwayScript>().AddConect(new Vector2(1, -1));
                GetComponent<RailwayScript>().AddConect(new Vector2(-1, 1));
                break;

            case 1:
                GetComponent<RailwayScript>().AddConect(new Vector2(1, -1));
                break;
        }
    }

    public void Turn(int k)
    {
        switch (curType)
        {
            case 1:
                if (k == 1)
                {
                    if (diagonal_2)
                        curType = 4;
                    else if (diagonal_1)
                        curType = 3;
                    else if (vertical)
                        curType = 2;
                }
                else
                {
                    if (vertical)
                        curType = 2;
                    else if (diagonal_1)
                        curType = 3;
                    else if (diagonal_2)
                        curType = 4;
                }
                break;

            case 2:
                if (k == 1)
                {
                    if (horizontal)
                        curType = 1;
                    else if (diagonal_2)
                        curType = 4;
                    else if (diagonal_1)
                        curType = 3;
                }
                else
                {
                    if (diagonal_1)
                        curType = 3;
                    else if (diagonal_2)
                        curType = 4;
                    else if (horizontal)
                        curType = 1;
                }
                break;

            case 3:
                if (k == 1)
                {
                    if (vertical)
                        curType = 2;
                    else if (horizontal)
                        curType = 1;
                    else if (diagonal_2)
                        curType = 4;
                }
                else
                {
                    if (diagonal_2)
                        curType = 4;
                    else if (horizontal)
                        curType = 1;
                    else if (vertical)
                        curType = 2;
                }
                break;

            case 4:
                if (k == 1)
                {
                    if (diagonal_1)
                        curType = 3;
                    else if (vertical)
                        curType = 2;
                    else if (horizontal)
                        curType = 1;
                }
                else
                {
                    if (horizontal)
                        curType = 1;
                    else if (vertical)
                        curType = 2;
                    else if (diagonal_1)
                        curType = 3;
                }
                break;

        }
    }


    public void SetColor(Color color)
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = color;
    }

    public void RemoveColor()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }
}