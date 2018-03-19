using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSceneManager : MonoBehaviour {


    public static HumanSceneManager HSM;
    public HumanSubObjects[] m_HumanSubObjects;

    [HideInInspector]
    public bool isAwaken = false;

    private void Awake()
    {
        HSM = this;
        for (int i = 0; i < m_HumanSubObjects.Length; i++)
        {
            m_HumanSubObjects[i].fn_AssignOriginalPositions();
        }
    }
}

[System.Serializable]
public class HumanSubObjects
{
    public string name;
    public GameObject[] m_GroupSubObjects;
    public Material [] m_MainMaterial;
    public Material [] m_FadeMaterial;

    private bool isFaded = false;
    private Vector3[] m_GroupSubObjectsOriginalPositions;

    public void fn_AssignOriginalPositions()
    {
        m_GroupSubObjectsOriginalPositions = new Vector3[m_GroupSubObjects.Length];
        for (int i = 0; i < m_GroupSubObjects.Length; i++)
        {
            m_GroupSubObjectsOriginalPositions[i] = m_GroupSubObjects[i].transform.position;
        }
    }
    public void fn_UnFocused()
    {
        if (isFaded == false)
        {
            for (int i = 0; i < m_GroupSubObjects.Length; i++)
            {
                if (m_GroupSubObjects[i].name == "Body3:body2")
                {
                    m_GroupSubObjects[i].SetActive(false);
                }
                else
                {
                    for (int j = 0; j < m_GroupSubObjects[i].GetComponent<MeshRenderer>().materials.Length; j++)
                    {
                        m_GroupSubObjects[i].GetComponent<MeshRenderer>().material = m_FadeMaterial[j];
                        m_GroupSubObjects[i].transform.position = m_GroupSubObjectsOriginalPositions[i];
                    }
                }
            }
        }
    }

    public void fn_UnFocused(bool isReseting)
    {
        if (isReseting == true)
        {
            for (int i = 0; i < m_GroupSubObjects.Length; i++)
            {
                if (m_GroupSubObjects[i].name == "Body3:body2")
                {
                    m_GroupSubObjects[i].SetActive(false);
                }
                else
                {
                    for (int j = 0; j < m_GroupSubObjects[i].GetComponent<MeshRenderer>().materials.Length; j++)
                    {
                        m_GroupSubObjects[i].GetComponent<MeshRenderer>().material = m_MainMaterial[j];
                        m_GroupSubObjects[i].transform.position = m_GroupSubObjectsOriginalPositions[i];
                    }
                }
            }
        }
    }

    public void fn_Focused()
    {
        if (isFaded == true)
        {
            for (int i = 0; i < m_GroupSubObjects.Length; i++)
            {
                if (m_GroupSubObjects[i].name == "Body3:body2")
                {
                    
                    m_GroupSubObjects[i].SetActive(true);
                }
                else
                {
                    for (int j = 0; j < m_GroupSubObjects[i].GetComponent<MeshRenderer>().materials.Length; j++)
                    {
                        m_GroupSubObjects[i].GetComponent<MeshRenderer>().material = m_MainMaterial[j];
                        m_GroupSubObjects[i].transform.position = new Vector3(m_GroupSubObjects[i].transform.position.x, m_GroupSubObjects[i].transform.position.y, m_GroupSubObjects[i].transform.position.z - 0.2f);
                    }
                }
            }
        }
    }

    public void fn_SetisFaded(bool isFaded)
    {
        this.isFaded = isFaded;
    }
}