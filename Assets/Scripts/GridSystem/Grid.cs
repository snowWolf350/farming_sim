using UnityEngine;

public class Grid<TgridObject>
{
    int height;
    int width;
    Vector3 originPosition;
    float cellSize;
    TgridObject[,] gridArray;

    public Grid(int height, int width,float cellSize,Vector3 originPosition)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TgridObject[height, width];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x+1,y),Color.white,100f);
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x,y+1),Color.white,100f);
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,0, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x/cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y/cellSize);
    }

    public void SetValue(int x, int y, TgridObject value)
    {
        if (x > 0 && y > 0 && x < width && y < height)
        {
            gridArray[x,y] = value;
        }
    }

    public void SetValue(Vector3 worldPosition, TgridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public TgridObject GetValue(int x, int y)
    {
        if (x > 0 && y > 0 && x < width && y < height)
        {
            return gridArray[x,y];
        }
        return default(TgridObject);
    }

    public TgridObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}
