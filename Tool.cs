using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour {

    //private:
    public Vector3 originalPosition;
    private Vector3 originalTestingTubePosition;
    public bool isFull = false;
    public string chimicalContent;
    //public:
    public string m_ToolName;
    public ToolType m_ToolType = ToolType.Beaker;

    public GameObject m_Content;
    public Transform m_ContentPosition;
    public Transform m_InteractEntryPoint;

    public GameObject[] m_ChildTools;
    public float m_BunsenBurner_Timer = 10;

    [HideInInspector]
    public bool isPrepared = false;
    [HideInInspector]
    public float m_ToolTempreture = 0;

    private void Start()
    {
        originalPosition = gameObject.transform.position;
    }

    void Update ()
    {
        //slerp goes here

    }
    void OnMouseOver()
    {
        if (LabManager.LM != null)
        {
            LabManager.LM.m_HoverText.text = this.m_ToolName;
            if (this.m_ToolType == ToolType.TestingTube && this.chimicalContent != "")
            {

            }
        }
    }
    public string fn_ContentShower()
    {
        Tool[] LabTools = GameObject.FindObjectsOfType<Tool>();
        foreach (Tool item in LabTools)
        {
            if (this.chimicalContent == item.m_ToolType.ToString())
            {
                LabManager.LM.m_HoverText.text = item.m_ToolName;
            }
        }
        return "";
    }
    void OnMouseExit()
    {
        if (LabManager.LM != null)
        {
            LabManager.LM.m_HoverText.text = "";
        }
    }
    private void OnMouseUp()
    {
        if (LabManager.LM != null)
        {
            if (LabManager.LM.m_LabState == LabState.Idle)
            {
                LabManager.LM.fn_SelectTool(gameObject, isPrepared);
            }
            else
            {
                Mission thisMission = ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].
                    m_Missions[ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex];
                Tool currentSelectedTool = LabManager.LM.m_CurrentSelectedTool.GetComponent<Tool>();
                if (currentSelectedTool.m_ToolType == thisMission.m_CurrentNeededTool
                    && m_ToolType == thisMission.m_NextNeededTool 
                    && LabManager.LM.m_LabState == thisMission.m_ExpectedAction)
                {
                    thisMission.isDone = true;
                    LabManager.LM.fn_UpdateMissionText(ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex + 1);
                    ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex++;
                    LabManager.LM.m_ToolText.text = "ﺔﺤﻴﺤﺻ ﺓﻮﻄﺧ";
                    switch (currentSelectedTool.m_ToolType)
                    {
                        case ToolType.Dropper: currentSelectedTool.fn_Dropper(LabManager.LM.m_LabState, this);
                            break;
                        case ToolType.MircoScope_GlassSection:
                            break;
                        case ToolType.MircoScope:
                            break;
                        case ToolType.Mortar_Pestle: currentSelectedTool.fn_Mortar_Pestle(LabManager.LM.m_LabState, this);
                            break;
                        case ToolType.Scalple:
                            break;
                        case ToolType.TestingTube: currentSelectedTool.fn_TestingTube(LabManager.LM.m_LabState, this);
                            break;
                        case ToolType.Thermometer:
                            break;
                        case ToolType.Tongs:
                            break;
                        case ToolType.Peas_Seeds_Container: currentSelectedTool.fn_Container(LabManager.LM.m_LabState, this);
                            break;
                        case ToolType.Tomatoes_Container: currentSelectedTool.fn_Container(LabManager.LM.m_LabState, this);
                            break;
                        case ToolType.Wheat_Seeds_Container: currentSelectedTool.fn_Container(LabManager.LM.m_LabState, this);
                            break;
                        case ToolType.Bread_Pieces_Container: currentSelectedTool.fn_Container(LabManager.LM.m_LabState, this);
                            break;
                        default: Debug.Log("Tool not found");
                            break;
                    }
                }
                else
                {
                    LabManager.LM.m_ToolText.text = "ﺔﺌﻃﺎﺧ ﺓﻮﻄﺧ";
                    LabManager.LM.m_CurrentSelectedTool = null;
                    LabManager.LM.m_LabState = LabState.Idle;
                }
            }
        }
    }

    private void Smash(string SmashableObjectTag)
    {
        /*for (int i = 0; i < m_ChildTools.Length; i++)
        {
            if (SmashableObjectTag == m_ChildTools[i].tag)
            {
                m_ChildTools[i].SetActive(true);
                GameObject.Destroy(m_Content.gameObject);
                m_Content = m_ChildTools[i];
                return;
            }
        }*/
    }

    public void fn_SwitchToolParent(bool isReadyTool)
    {
        gameObject.transform.parent = LabManager.LM.fn_GetTray(isReadyTool).transform;
        if (isReadyTool == true)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, (gameObject.transform.position.z - LabManager.LM.fn_GetTray(!isReadyTool).transform.localScale.z) + (LabManager.LM.fn_GetTray(!isReadyTool).transform.position.z + 0.5f));
            LabManager.LM.m_ReadyTools.Add(gameObject);
            isPrepared = true;
        }
        else
        {
            gameObject.transform.position = originalPosition;
            LabManager.LM.m_ReadyTools.Remove(gameObject);
            isPrepared = false;
        }
    }

    public void fn_ClearContent()
    {
        //GameObject.Destroy(m_Content.gameObject);
        m_Content = null;
        isFull = false;
    }

    public IEnumerator fn_Bunsen_Burner(LabState LS)
    {
        // if (LS == LabState.UsingItem)
        //{
            m_Content.SetActive(true);
            LabManager.LM.m_LabState = LabState.Idle;
            LabManager.LM.fn_ResetCurrentSelectedTool();

            ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].
            m_Missions[ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex].isDone = true;
            LabManager.LM.fn_UpdateMissionText(ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex + 1);
            ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex++;
            LabManager.LM.m_ToolText.text = "ﺔﺤﻴﺤﺻ ﺓﻮﻄﺧ";

            yield  return new WaitForSeconds (m_BunsenBurner_Timer);

            if (m_ChildTools != null)
            {
                m_ChildTools[0].GetComponent<Tool>().m_ToolTempreture = 10;
            }
            else
            {
                Debug.Log("Beaker not a child to bunsen burner"); 
            }

        //}
        // else
        //{
        //  Debug.Log("Action Can't be applied to this tool"); // TODO Change to dynamic on screen arabic text
        // }
    }

    public void fn_Dropper(LabState LS, Tool OtherTool)
    {
        if (LS == LabState.UsingItem)
        {
            if (isFull == false)
            {
                if (OtherTool.m_ToolType == ToolType.Distilled_Water || OtherTool.m_ToolType == ToolType.Egg_Yolk || OtherTool.m_ToolType == ToolType.Glucose_Solution ||
                OtherTool.m_ToolType == ToolType.Iodine_Solution || OtherTool.m_ToolType == ToolType.Starch_Solution || OtherTool.m_ToolType == ToolType.Benedict_Reagent)
                {
                    //GameObject newContent = Instantiate(OtherTool.m_Content, m_ContentPosition.position, transform.rotation, transform);
                    //m_Content = newContent;
                    isFull = true;
                    LabManager.LM.m_LabState = LabState.Idle;
                    LabManager.LM.fn_ResetCurrentSelectedTool();
                }
                else
                {
                    Debug.Log("Can't Use dropper with this tool"); // TODO Change to dynamic on screen arabic text
                }
            }
            else
            {
                Debug.Log("Dropper is full, empty it first"); // TODO Change to dynamic on screen arabic text
            }
        }

        else if (LS == LabState.EmptyingItem)
        {
            if (isFull == true)
            {
                if (OtherTool.m_ToolType == ToolType.Container_Sample || OtherTool.m_ToolType == ToolType.MircoScope_GlassSection || OtherTool.m_ToolType == ToolType.Mortar_Pestle
                    || OtherTool.m_ToolType == ToolType.TestingTube)
                {
                   // GameObject DropedContent = Instantiate(m_Content, OtherTool.m_ContentPosition.position, OtherTool.gameObject.transform.rotation, OtherTool.gameObject.transform);
                   // OtherTool.m_Content = DropedContent;

                   /* switch (OtherTool.m_Content.tag)
                    {
                        case "Glucose Solution":
                            if (m_Content.tag == "Bendict Reagent")
                            {
                                // TODO CHANGE COLOR FROM BLUE TO ORANGE
                            }
                            break;

                        case "Bread":
                            if (m_Content.tag == "Iodine Solution")
                            {
                                // TODO CHANGE COLOR FROM ORANGE TO DARK BLUE
                            }
                            break;

                        case "Wheat Seeds":
                            if (m_Content.tag == "Iodine Solution")
                            {
                                // TODO CHANGE COLOR FROM ORANGE TO DARK BLUE
                            }
                            break;

                        case "Peas Seeds":
                            if (m_Content.tag == "Iodine Solution")
                            {
                                // TODO CHANGE COLOR FROM ORANGE TO LIGHT BLUE
                            }
                            break;

                        default: Debug.Log("No specified behavior");
                            break;
                    }*/

                    fn_ClearContent();
                    LabManager.LM.m_LabState = LabState.Idle;
                    LabManager.LM.fn_ResetCurrentSelectedTool();
                }
                else
                {
                    Debug.Log("Can't Empty Dropper in this tool"); // TODO Change to dynamic on screen arabic text
                }
            }
            else
            {
                Debug.Log("Dropper has no content"); // TODO Change to dynamic on screen arabic text
            }
        }

        fn_Interact(OtherTool, 90f, 0, 0);
    }

    public void fn_Mortar_Pestle(LabState LS, Tool OtherTool)
    {
        if (LS == LabState.UsingItem)
        {
            if (isFull == false)
            {
               Smash(m_Content.tag);
               isFull = true;
               LabManager.LM.m_LabState = LabState.Idle;
               LabManager.LM.fn_ResetCurrentSelectedTool();
            }
            else
            {
                Debug.Log("Already Smashed!"); // TODO Change to dynamic arabic text
            }
        }

        else if (LS == LabState.EmptyingItem)
        {
            if (isFull == true)
            {
                if (OtherTool.m_ToolType == ToolType.Container_Sample)
                {
                    //GameObject DropedContent = Instantiate(m_Content, OtherTool.m_ContentPosition.position, OtherTool.gameObject.transform.rotation, OtherTool.gameObject.transform);
                    //OtherTool.m_Content = DropedContent;
                    //GameObject.Destroy(m_Content.gameObject);
                    isFull = false;
                    LabManager.LM.m_LabState = LabState.Idle;
                    LabManager.LM.fn_ResetCurrentSelectedTool();
                }
                else
                {
                    Debug.Log("Can't empty content in this tool"); //TODO Change to dynamic on screen arabic text
                }
            }
            else
            {
                Debug.Log("No content!"); // TODO Change to dynamic arabic text
            }
        }
    }

    public void fn_TestingTube(LabState LS, Tool Beaker)
    {
        if (Beaker.m_ToolType == ToolType.Beaker)
        {
            if (Beaker.isFull == true && Beaker.fn_GetBeakerContentPrePosition() != new Vector3 (0,0,0) )
            {
                gameObject.transform.position = Beaker.fn_GetBeakerContentPrePosition();
            }
            else
            {
                Debug.Log("Something went wrong!");
            }
            Beaker.originalTestingTubePosition = transform.position;
            gameObject.transform.position = Beaker.m_ContentPosition.position;
            Beaker.m_Content = gameObject;
            Beaker.isFull = true;
        }
    }

    public void fn_Container(LabState LS, Tool OtherTool)
    {
        if (LS == LabState.EmptyingItem)
        {
            if (OtherTool.m_ToolType == ToolType.Container_Sample || OtherTool.m_ToolType == ToolType.Mortar_Pestle)
            {
               // GameObject DropedContent = Instantiate(m_Content, OtherTool.m_ContentPosition.position, OtherTool.gameObject.transform.rotation, OtherTool.gameObject.transform);
                //OtherTool.m_Content = DropedContent;
                LabManager.LM.m_LabState = LabState.Idle;
                LabManager.LM.fn_ResetCurrentSelectedTool();
            }
        }
    }

    public void fn_Interact (Tool OtherTool)
    {
        gameObject.transform.position = OtherTool.m_InteractEntryPoint.position;
    }

    public void fn_Interact (Tool OtherTool, float XRotation, float YRotation, float ZRotation)
    {
        gameObject.transform.position = OtherTool.m_InteractEntryPoint.position;
        gameObject.transform.eulerAngles = new Vector3 (XRotation, YRotation, ZRotation);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public Vector3 fn_GetBeakerContentPrePosition()
    {
        if (originalTestingTubePosition != null)
        {
            return originalTestingTubePosition;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
        
    }
}
