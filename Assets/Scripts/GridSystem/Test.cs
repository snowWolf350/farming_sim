using UnityEngine;

public class Test : MonoBehaviour
{

    Grid<bool> grid;
    void Start()
    {
        grid = new Grid<bool>(4, 2, 10,Vector3.zero);
    }

}
