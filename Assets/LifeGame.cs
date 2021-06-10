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
    [SerializeField] float m_intarval = 0;
    float m_timer = 0;
    Cell[,] m_cubes;
    int[,] m_count;

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
        m_count = new int[m_numX, m_numY];
        //セルを生成する
        for (int i = 0; i < m_numY; i++)
        {
            for (int x = 0; x < m_numX; x++)
            {
                var cell = Instantiate(m_cellPrefab);
                var parent = m_gridLayoutGroup.gameObject.transform;
                cell.transform.SetParent(parent);
                m_cubes[x, i] = cell;
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

    private void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_intarval)
        {
            LivingDead();
            m_timer = 0;
        }
    }

    //周囲８近傍を調べる
    public Cell[] SearchCell(int x, int y)
    {
        var list = new List<Cell>();

        var left = x - 1;
        var right = x + 1;
        var top = y - 1;
        var bottom = y + 1;

        if (top >= 0)
        {
            if (left >= 0 && m_cubes[left, top].m_living)
            {
                list.Add(m_cubes[left, top]);
            }
            if (m_cubes[x, top].m_living)
            {
                list.Add(m_cubes[x, top]);
            }
            if (right < m_numX && m_cubes[right, top].m_living)
            {
                list.Add(m_cubes[right, top]);
            }
        }
        if (left >= 0 && m_cubes[left, y].m_living)
        {
            list.Add(m_cubes[left, y]);
        }
        if (right < m_numX && m_cubes[right, y].m_living)
        {
            list.Add(m_cubes[right, y]);
        }
        if (bottom < m_numY)
        {
            if (left >= 0 && m_cubes[left, bottom].m_living)
            {
                list.Add(m_cubes[left, bottom]);
            }
            if (m_cubes[x, bottom].m_living)
            {
                list.Add(m_cubes[x, bottom]);
            }
            if (right < m_numX && m_cubes[right, bottom].m_living)
            {
                list.Add(m_cubes[right, bottom]);
            }
        }

        return list.ToArray();
    }

    public void LivingDead()
    {
        for (int y = 0; y < m_numY; y++)
        {
            for (int x = 0; x < m_numX; x++)
            {
                foreach (var item in SearchCell(x, y))
                {
                    if (item.m_living == true)
                    {
                        m_count[x,y]++;
                    }
                }
               
            }
        }
        for (int i = 0; i < m_numY; i++)
        {
            for (int k = 0; k < m_numX; k++)
            {
                Conditions(k, i);      
                m_count[k,i] = 0;
            }
        }
    }

    public void Conditions(int x, int y)
    {
        if (m_count[x,y]  == 3 && m_cubes[x,y].m_living == false)
        {
            m_cubes[x,y].m_living = true;
            return;
        }
        if (m_cubes[x,y].m_living == true)
        {
            if (m_count[x,y] == 2 || m_count[x,y] == 3)
            {
                m_cubes[x, y].m_living = true;
                return;
            }
        }
        if (m_cubes[x, y].m_living == true && m_count[x,y] <= 1)
        {
            m_cubes[x, y].m_living = false;
            return;
        }
        if (m_cubes[x, y].m_living == true && m_count[x,y] >= 4)
        {
            m_cubes[x, y].m_living = false;
            return;
        }
        m_cubes[x, y].m_living = false;
    }
}
