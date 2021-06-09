using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Cell : MonoBehaviour
{
    public bool m_living = false;
    Image m_color;
    

    // Start is called before the first frame update
    void Start()
    {
        m_color = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        LivingColor();
    }

    public void LivingColor()
    {
        if (m_living == false)
        {
            m_color.color = Color.black;
        }
        else
        {
            m_color.color = Color.green;
        }
    }
}
