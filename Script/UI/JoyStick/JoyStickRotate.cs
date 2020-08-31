using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickRotate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Image m_RorateImg;
    private Vector3 m_vecDownScale = new Vector3(1.8f, 1.8f, 1.8f);
    private Vector3 m_vecUpScale;

    public Vector3 m_vecDirection = Vector3.zero;
    private Vector3 m_vecOld = Vector3.zero;

    private float m_fRotate;

    public float Rotate
    {
        get
        {
            if (m_fRotate > 360|| m_fRotate < -360)
            {
                m_fRotate = 0;
            }         
            return m_fRotate;
        }
    }

    private void Start()
    {
        m_RorateImg = GetComponent<Image>();
        m_vecUpScale = m_RorateImg.rectTransform.localScale;
    }
    
    public void OnDrag(PointerEventData eventData)
    {      
        m_vecDirection = Input.mousePosition - m_vecOld;
        m_vecOld = m_vecOld+ m_vecDirection;
        m_vecDirection.Normalize();

        m_fRotate += m_vecDirection.x*4;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_vecOld = Input.mousePosition;
        m_RorateImg.rectTransform.localScale = m_vecDownScale;         
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_vecDirection= Vector3.zero;
      
        m_RorateImg.rectTransform.localScale = m_vecUpScale;
    }

}
