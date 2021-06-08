using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeGame : MonoBehaviour
{
    [SerializeField] Cell m_cellPrefab;
    [SerializeField] GridLayoutGroup m_gridLayoutGroup;
    [SerializeField] int m_numX = 0;
    [SerializeField] int m_numY = 0;
    [SerializeField] int m_initial = 0;
    Cell[,] m_cubes;
    int m_count = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Cellを綺麗に並べる
        if (m_numX < m_numY)
        {
            m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            m_gridLayoutGroup.constraintCount = m_numX;
        }
        else
        {
            m_gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            m_gridLayoutGroup.constraintCount = m_numY;
        }

        m_cubes = new Cell[m_numX, m_numY];

        //セルを生成する
        for (int i = 0; i < m_numX; i++)
        {
            for (int x = 0; x < m_numY; x++)
            {
                var cell = Instantiate(m_cellPrefab);
                cell.positionCell = new Vector2Int(i, x); //生成したセルのpositionを記憶
                var parent = m_gridLayoutGroup.gameObject.transform;
                cell.transform.SetParent(parent);
                m_cubes[i, x] = cell;
            }
        }

        //最初にランダムな位置に生きてるセルを生成する
        if (m_initial < m_numX * m_numY)
        {
            for (int i = 0; i < m_initial; i++)
            {
                var x = Random.Range(0, m_numX);
                var y = Random.Range(0, m_numY);
                var cell = m_cubes[x, y];

                if (cell.m_living == false)
                {
                    cell.m_living = true;

                }
                else
                {
                    i--;
                }
            }
        }
    }

    //周囲８近傍を調べる
    public Cell[] SearchCell(int r, int c)
    {
        var list = new List<Cell>();

        var left = c - 1;
        var right = c + 1;
        var top = r - 1;
        var bottom = r + 1;

        if (top >= 0)
        {
            if (left >= 0)
            {
                list.Add(m_cubes[top, left]);
            }
            list.Add(m_cubes[top, c]);
            if (right < m_numX)
            {
                list.Add(m_cubes[top, right]);
            }
        }
        if (left >= 0)
        {
            list.Add(m_cubes[r, left]);
        }
        if (right < m_numX)
        {
            list.Add(m_cubes[r, right]);
        }
        if (bottom < m_numY)
        {
            if (left >= 0)
            {
                list.Add(m_cubes[bottom, left]);
            }
            list.Add(m_cubes[bottom, c]);
            if (right < m_numX)
            {
                list.Add(m_cubes[bottom, right]);
            }
        }

        return list.ToArray();
    }

    public void LivingDead()
    {
        for (int i = 0; i < m_numX; i++)
        {
            for (int x = 0; x < m_numY; x++)
            {
                if (m_cubes[i, x].m_living == true)
                {
                    m_count++;
                }
                foreach (var item in SearchCell(i, x))
                {
                    if (item.m_living == true)
                    {
                        m_count++;
                    }
                }
                Conditions(i, x);
                m_count = 0;
            }
        }
    }

    public void Conditions(int i, int x)
    {
        if (m_count  == 3 && m_cubes[i,x].m_living == false)
        {
            m_cubes[i,x].m_living = true;
        }
        if (m_cubes[i,x].m_living == true)
        {
            if (m_count == 2 && m_count == 3)
            {
                m_cubes[i, x].m_living = true;
            }
        }
        if (m_cubes[i, x].m_living == true && m_count <= 1)
        {
            m_cubes[i, x].m_living = false;
        }
        if (m_cubes[i, x].m_living == true && m_count >= 4)
        {
            m_cubes[i, x].m_living = false;
        }
    }
}
