using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPart : MonoBehaviour {

    public GameObject[] m_ActiavtionObjects;
    public GameObject[] m_DestrctionObjects;

    public Animation m_AnimationComponent;
    public string m_AnimationName;

    private void ActivateObjects()
    {
        for (int i = 0; i < m_ActiavtionObjects.Length; i++)
        {
            m_ActiavtionObjects[i].SetActive(true);
        }
        for (int i = 0; i < m_DestrctionObjects.Length; i++)
        {
            m_DestrctionObjects[i].SetActive(false);
        }
    }

    private void PlayAnimation()
    {
        if (HumanSceneManager.HSM.isAwaken == false)
        {
            m_AnimationComponent.Play(m_AnimationName);
            HumanSceneManager.HSM.isAwaken = true;
        }
    }
    private void OnMouseUp()
    {
        ActivateObjects();
        PlayAnimation();
    }
}
