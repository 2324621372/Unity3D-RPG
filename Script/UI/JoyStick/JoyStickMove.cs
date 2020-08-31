using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class JoyStickMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private Image m_MoveBtnImg;
    [SerializeField]
    private Image m_BackGroundImg;

    public bool m_bMoveCheck;

    private Vector2 m_vecDir = Vector2.zero;

    public float m_fLimit = 40;

    public Vector2 Dir
    {
        get { return m_vecDir.normalized; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_BackGroundImg.gameObject.SetActive(true);

        m_BackGroundImg.rectTransform.position = Input.mousePosition;
        m_bMoveCheck = true;
    }

    //private void Update()
    //{
    //    float forward = Input.GetAxis("Vertical");
    //    float horizontal = Input.GetAxis("Horizontal");

    //    m_vecDir = new Vector2(horizontal, forward);
    //}

    public void OnPointerUp(PointerEventData eventData)
    {
        m_BackGroundImg.gameObject.SetActive(false);
        m_bMoveCheck = false;

        m_vecDir = Vector2.zero;
        m_MoveBtnImg.rectTransform.anchoredPosition = m_vecDir;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPosition = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_BackGroundImg.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPosition
            );

        m_MoveBtnImg.transform.localPosition = localPosition;

        if (localPosition.magnitude > m_fLimit)
        {
            m_vecDir = localPosition.normalized;
            m_MoveBtnImg.transform.localPosition = m_vecDir * m_fLimit;
        }
        else
        {
            m_MoveBtnImg.transform.localPosition = localPosition;
            m_vecDir = localPosition.normalized;
        }
    }
}
