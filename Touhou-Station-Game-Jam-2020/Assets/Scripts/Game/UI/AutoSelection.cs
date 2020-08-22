using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class AutoSelection : MonoBehaviour
{
    static Vector3 m_lastKnownMousePos;

    void OnEnable()
    {
        m_lastKnownMousePos = Input.mousePosition;
        SelectSelf();
    }

    void SelectSelf()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        gameObject.GetComponent<Button>().OnSelect(null);
    }

    void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
            {
                SelectSelf();
            }
        }
        else
        {
            if (Input.mousePosition != m_lastKnownMousePos)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        m_lastKnownMousePos = Input.mousePosition;
    }
}
