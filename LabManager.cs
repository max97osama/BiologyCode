using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LabManager : MonoBehaviour
{

    //private:
    RaycastHit info;
    Ray cameraRay;
    private GameObject preToolTray;
    private GameObject readyToolTray;
    private GameObject infoPanel;
    //public:
    public static LabManager LM;
    public LabState m_LabState = LabState.Idle;

    public GameObject m_SelectedEffect;

    //Lab UI Dynamic Elements.
    public Animator m_MainCameraAnimator;
    public GameObject m_PrepareButton;
    public GameObject m_BackButton;
    public GameObject m_UseButton;
    public GameObject m_EmptyButton;
    public Text m_ToolText;
    public Text m_HoverText;
    public Text[] m_MissionsText;
    public Text[] m_RequiredToolsText;

    [HideInInspector]
    public bool isBeganPractice = false;
    [HideInInspector]
    public List<GameObject> m_ReadyTools;
    //[HideInInspector]
    public GameObject m_CurrentSelectedTool;

    private void Awake()
    { 
        LM = this;
        m_ReadyTools = new List<GameObject>();
        infoPanel = new GameObject();
        if (ApplicationManager.AM != null)
        {
            AssignTrays();
            AssignMissionsToText();
            if (m_PrepareButton == null || m_BackButton == null)
            {
                Debug.Log("Assign UI buttons to LabManager");
            }
        }
    }

    private void AssignMissionsToText()
    {
        if (m_MissionsText != null)
        {
            for (int i = 0; i < ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].m_Missions.Length; i++)
            {
                if (m_MissionsText[i+1].text == "")
                {
                    m_MissionsText[i+1].text = ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].m_Missions[i].m_MissionDescription;
                }
            }
        }

        if (m_RequiredToolsText != null)
        {
            for (int i = 0;  i< ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].m_RequiredTools.Length; i++)
            {
                if (m_RequiredToolsText[i].text == "")
                {
                    switch (ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].m_RequiredTools[i])
                    {
                        case ToolType.Bunsen_Burner:
                            m_RequiredToolsText[i].text = "ﺪﻗﻮﻣ";
                            break;
                        case ToolType.Dropper:
                            m_RequiredToolsText[i].text = "ﺓﺭﺎﻄﻗ";
                            break;
                        case ToolType.Container_Sample:
                            m_RequiredToolsText[i].text = "ﺔﻨﻴﻋ ﺀﺎﻋﻭ";
                            break;
                        case ToolType.MircoScope_GlassSection:
                            m_RequiredToolsText[i].text = "ﺔﻴﺟﺎﺟﺯ ﺔﺤﻳﺮﺷ";
                            break;
                        case ToolType.MircoScope:
                            m_RequiredToolsText[i].text = "ﺏﻮﻜﺳﻭﺮﻜﻴﻣ";
                            break;
                        case ToolType.Mortar_Pestle:
                            m_RequiredToolsText[i].text = "ﻥﻮﻫ";
                            break;
                        case ToolType.Scalple:
                            m_RequiredToolsText[i].text = "ﻁﺮﺸﻣ";
                            break;
                        case ToolType.TestingTubes_Rack:
                            m_RequiredToolsText[i].text = "ﺮﺒﺘﺨﻤﻟﺍ ﺐﻴﺑﺎﻧﺃ ﻞﻣﺎﺣ";
                            break;
                        case ToolType.Thermometer:
                            m_RequiredToolsText[i].text = "ﺮﺘﻣﻮﻣﺮﺗ";
                            break;
                        case ToolType.Tongs:
                            m_RequiredToolsText[i].text = "ﺮﺒﺘﺨﻤﻟﺍ ﺐﻴﺑﺎﻧﺃ ﻚﺳﺎﻣ";
                            break;
                        case ToolType.Iodine_Solution:
                            m_RequiredToolsText[i].text = "ﺩﻮﻴﻟﺍ ﻝﻮﻠﺤﻣ";
                            break;
                        case ToolType.Glucose_Solution:
                            m_RequiredToolsText[i].text = "ﺯﻮﻛﻮﻠﺠﻟﺍ ﻝﻮﻠﺤﻣ";
                            break;
                        case ToolType.Starch_Solution:
                            m_RequiredToolsText[i].text = "ﺎﺸﻨﻟﺍ ﻝﻮﻠﺤﻣ";
                            break;
                        case ToolType.Egg_Yolk:
                            m_RequiredToolsText[i].text = "ﺾﻴﺒﻟﺍ ﻝﻻﺯ";
                            break;
                        case ToolType.Distilled_Water:
                            m_RequiredToolsText[i].text = "ﺮﻄﻘﻣ ﺀﺎﻣ";
                            break;
                        case ToolType.Benedict_Reagent:
                            m_RequiredToolsText[i].text = "ﺖﻛﺪﻨﺑ ﻒﺷﺎﻛ";
                            break;
                        case ToolType.Peas_Seeds_Container:
                            m_RequiredToolsText[i].text = "ﻻﺯﺎﺒﻟﺍ ﺏﻮﺒﺣ ﺀﺎﻋﻭ";
                            break;
                        case ToolType.Tomatoes_Container:
                            m_RequiredToolsText[i].text = "ﻢﻃﺎﻤﻄﻟﺍ ﻑﺎﺼﻧﺃ ﺀﺎﻋﻭ";
                            break;
                        case ToolType.Wheat_Seeds_Container:
                            m_RequiredToolsText[i].text = "ﺢﻤﻘﻟﺍ ﺏﻮﺒﺣ ﺀﺎﻋﻭ";
                            break;
                        case ToolType.Bread_Pieces_Container:
                            m_RequiredToolsText[i].text = "ﺰﺒﺨﻟﺍ ﻊﻄﻗ ﺀﺎﻋﻭ";
                            break;
                        case ToolType.Peanut_Solution:
                            m_RequiredToolsText[i].text = "ﻰﻧﺍﺩﻮﺳ ﻝﻮﻓ ﺭﻭﺬﺑ";
                            break;
                        case ToolType.Peanut_Reagent:
                            m_RequiredToolsText[i].text = "ﻥﺍﺩﻮﺳ ﻒﺷﺎﻛ";
                            break;
                        case ToolType.Beans_Solution:
                            m_RequiredToolsText[i].text = "ﻝﻮﻓ ﺭﻭﺬﺑ";
                            break;
                        case ToolType.Scalpel:
                            m_RequiredToolsText[i].text = "ﻁﺮﺸﻣ";
                            break;
                        case ToolType.Ice:
                            m_RequiredToolsText[i].text = "ﺞﻠﺛ";
                            break;
                        case ToolType.Onion:
                            m_RequiredToolsText[i].text = "ﻞﺼﺑ";
                            break;
                        case ToolType.Potatos:
                            m_RequiredToolsText[i].text = "ﺲﻃﺎﻄﺑ";
                            break;
                        case ToolType.Combinator:
                            m_RequiredToolsText[i].text = "ﻁﺎﻘﻠﻣ";
                            break;
                        case ToolType.Injection:
                            m_RequiredToolsText[i].text = "ﻪﺑﺎّﺤﺳ";
                            break;
                        case ToolType.Biuret_Reagent:
                            m_RequiredToolsText[i].text = "ﻕﺭﺯﻷﺍ ﺖﻳﺭﻮﻴﺒﻟﺍ ﻒﺷﺎﻛ";
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (m_CurrentSelectedTool != null)
        {
            CheckMouseClick();
        }
    }

    private void AssignTrays()
    {
        //Tested
        if (ApplicationManager.AM.m_CurrentScene != "")
        {
            switch (ApplicationManager.AM.m_CurrentScene)
            {
                case "Detecting Sugar": 
                    preToolTray = GameObject.Find("CarbPreTray");
                    readyToolTray = GameObject.Find("CarbReadyTray");
                    infoPanel = GameObject.FindWithTag("Sugar Panel"); infoPanel.SetActive(true);
                    break;
                case "Detecting Starch":
                    preToolTray = GameObject.Find("CarbPreTray");
                    readyToolTray = GameObject.Find("CarbReadyTray");
                    infoPanel = GameObject.FindWithTag("Starch Panel"); infoPanel.SetActive(true);
                    break;
                default: Debug.Log("NothingFound"); break;
            }
        }
    }

    public bool isToolNeeded (Tool item)
    {
        foreach (ToolType checker in ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].m_RequiredTools)
        {
            if (item.m_ToolType == checker)
            {
                return true;
            }
            else if (item.transform.parent.gameObject.GetComponent<Tool>() != null)
            {
                if (item.transform.parent.gameObject.GetComponent<Tool>().m_ToolType == checker)
                {
                    LabManager.LM.m_CurrentSelectedTool = item.transform.parent.gameObject;
                    return true;
                }
            }
        }
        return false;
    }

    private void SetCheckMark()
    {   
        int Lenght = ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].m_RequiredTools.Length;
        bool[] doublecheck = new bool[Lenght];
        for (int i = 0; i < m_ReadyTools.Count; i++)
        {
            for (int j = 0; j < Lenght; j++)
            {
                if (m_ReadyTools[i].GetComponent<Tool>().m_ToolType == ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].m_RequiredTools[j])
                {
                    if (m_RequiredToolsText[j].transform.childCount < 1)
                    {


                        GameObject checkMark = new GameObject();
                        checkMark.name = m_ReadyTools[i].GetComponent<Tool>().name;
                        checkMark.gameObject.AddComponent<Image>().sprite = GameObject.Find("Check").gameObject.GetComponent<Image>().sprite;
                        checkMark.gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 1);
                        Instantiate(checkMark, new Vector3(m_RequiredToolsText[j].transform.position.x + 220, m_RequiredToolsText[j].transform.position.y, m_RequiredToolsText[j].transform.position.z), m_RequiredToolsText[j].transform.rotation, m_RequiredToolsText[j].transform);
                        m_RequiredToolsText[j].color = Color.green;
                    }
                    doublecheck[j] = true;
                    break;
                }
            }
        }

        for (int d = 0; d < Lenght; d++)
        {
            if (doublecheck[d] == false && m_RequiredToolsText[d].transform.childCount > 0)
            {
                DestroyObject(m_RequiredToolsText[d].transform.GetChild(0).gameObject);
                m_RequiredToolsText[d].color = Color.white;
            }
        }

    }

    private void CheckMouseClick()
    {
        //This function checks if the raycast fired from the mouse hit an object tagged tool or not.
        if (Input.GetKey(KeyCode.Mouse0) && m_LabState == LabState.Idle)
        {
            cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * 10, Color.red);
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (Physics.Raycast(cameraRay, 10f) == false)
                {
                    fn_ResetCurrentSelectedTool();
                }
            }

            if (m_MainCameraAnimator.GetBool("isLabReady") == false)
            {
                SetCheckMark();
            }
        }
    }

    private void SetPrepareBtns(bool isPrepared)
    {
        if (isPrepared == true)
        {
            m_BackButton.SetActive(true);
            m_PrepareButton.SetActive(false);
        }
        else
        {
            m_PrepareButton.SetActive(true);
            m_BackButton.SetActive(false);
        }
    }

    public void fn_ResetCurrentSelectedTool()
    {
        m_CurrentSelectedTool = null;
        m_ToolText.text = "";
        m_PrepareButton.SetActive(true);
        m_BackButton.SetActive(false);
        if (m_SelectedEffect != null)
        {
            m_SelectedEffect.SetActive(false);
        }
        else
        {
            //Debug.Log("No Selected Effect found");
        }
    }

    public bool fn_CheckReadyTools()
    {

        int successCounter = 0;
        if (m_ReadyTools.Count == ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].m_RequiredTools.Length)
        {
            for (int i = 0; i < m_ReadyTools.Count; i++)
            {
                for (int j = 0; j < m_ReadyTools.Count; j++)
                {
                    if (m_ReadyTools[i].GetComponent<Tool>().m_ToolType == ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].m_RequiredTools[j])
                    {
                        successCounter++;
                        break;
                    }
                }
            }

            if (successCounter == m_ReadyTools.Count)
            {
                m_MainCameraAnimator.SetBool("isLabReady", true);
                m_PrepareButton.GetComponent<Button>().enabled = false;
                m_PrepareButton.GetComponent<Image>().color = new Color(m_PrepareButton.GetComponent<Image>().color.r, m_PrepareButton.GetComponent<Image>().color.g, m_PrepareButton.GetComponent<Image>().color.b, m_PrepareButton.GetComponent<Image>().color.a - 0.5f);
                m_BackButton.GetComponent<Button>().enabled = false;
                m_BackButton.GetComponent<Image>().color = new Color(m_BackButton.GetComponent<Image>().color.r, m_BackButton.GetComponent<Image>().color.g, m_BackButton.GetComponent<Image>().color.b, m_BackButton.GetComponent<Image>().color.a - 0.5f);
                Tool[] LabTools = FindObjectsOfType<Tool>();
                foreach (Tool neededTool in LabTools)
                {
                    neededTool.originalPosition = neededTool.gameObject.transform.position;
                }
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }

    public GameObject fn_GetTray(bool isReady)
    {
        if (isReady == true)
        {
            return readyToolTray;
        }
        else
        {
            return preToolTray;
        }
    }

    public string fn_TextShower(GameObject theTool)
    {
        //check and right what is in side tubes
        string textSender = "";
        Tool thisTool = theTool.GetComponent<Tool>();
        if (thisTool.m_ToolType == ToolType.TestingTube && thisTool.chimicalContent != "")
        {

            Tool[] LabTools = FindObjectsOfType<Tool>();
            string tempContent = thisTool.chimicalContent;
            int plusCounter = 0;
            if (tempContent.Contains("+"))
            {

                foreach (char l in tempContent)
                {
                    if (l == '+')
                    {
                        plusCounter++;
                    }
                }
                if (plusCounter == 1)
                    //plusCounter = 2;
                for (int i = 0; i < plusCounter; i++)
                {

                    foreach (Tool element in LabTools)
                    {
                        if (tempContent.StartsWith(element.m_ToolType.ToString()))
                        {

                            textSender += element.m_ToolName + " + ";
                            int removed = element.m_ToolType.ToString().Length + 1;
                            //tempContent.Replace(removed, "");
                            int total = tempContent.Length - removed;
                            if (total > 0)
                            {
                                tempContent = tempContent.Substring(removed, total);
                            }


                        }
                    }

                }

            }
            foreach (Tool item in LabTools)
            {
                if (item.m_ToolType.ToString() == tempContent)
                {
                    textSender += item.m_ToolName;
                    return textSender;
                }
            }
            return textSender;
        }
        else
        {
            textSender = theTool.GetComponent<Tool>().m_ToolName;
            return textSender;
        }
    }

    public void fn_SelectTool(GameObject tool, bool isPrepared)
    {
        fn_ResetCurrentSelectedTool();
        m_CurrentSelectedTool = tool;
        m_ToolText.text = m_CurrentSelectedTool.GetComponent<Tool>().m_ToolName;

        string currentToolName = fn_TextShower(m_CurrentSelectedTool);
        if (currentToolName != "")
            m_ToolText.text = currentToolName;
        m_ToolText.color = Color.green;

        SetPrepareBtns(isPrepared);
        if (m_SelectedEffect != null)
        {
            m_SelectedEffect.transform.parent = m_CurrentSelectedTool.transform;
            m_SelectedEffect.transform.position = new Vector3(0, 0, 0);
            m_SelectedEffect.SetActive(true);
        }
        else
        {
            //Debug.Log("Please Assign selected effect");
        }
    }

    public void fn_UpdateMissionText(int index)
    {
        m_MissionsText[index].color = Color.green;
    }
    public void fn_UndoMissionColor (int index)
    {
        m_MissionsText[index].color = Color.red;
    }
    public GameObject fn_GetInfoPanel()
    {
        return infoPanel;
    }
}

public enum LabState { UsingItem, EmptyingItem, UsingMicrosope, Idle};