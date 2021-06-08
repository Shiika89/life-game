using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cell : MonoBehaviour
{
    public bool m_living = false;
    public Vector2Int positionCell;
    GameObject m_life;
    LifeGame lifegame;
    Image m_color;
    

    // Start is called before the first frame update
    void Start()
    {
        m_color = GetComponent<Image>();
        m_life = GameObject.Find("LifeGame");
        lifegame = m_life.GetComponent<LifeGame>();
        //m_living = false;
    }

    // Update is called once per frame
    void Update()
    {
        LivingColor();
        lifegame.LivingDead();
    }

    public void LivingColor()
    {
        if (m_living == false)
        {
            m_color.color = Color.black;
        }
        if (m_living == true)
        {
            m_color.color = Color.green;
        }
    }
}
