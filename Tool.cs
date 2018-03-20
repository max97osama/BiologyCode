using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour {

    //private:
    public Vector3 originalPosition;
    private Vector3 originalTestingTubePosition;
    private bool isMoving;
    private float startTime;
    private Vector3 Direction;
    public bool isFull = false;
    public string chimicalContent;
    //public:
    public string m_ToolName;
    public ToolType m_ToolType = ToolType.Beaker;

    public GameObject m_Content;
    public Transform m_ContentPosition;
    public Transform m_InteractEntryPoint;

    public GameObject[] m_ChildTools;
    public float m_BunsenBurner_Timer = 4;

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
        if (isMoving)
        {
            float fracComplete = (Time.time - startTime) / 2.0f;
            this.transform.position = Vector3.Slerp(this.transform.position,Direction,fracComplete);
        }
        if (this.transform.position == Direction)
        {
            isMoving = false;
        }
    }
    void OnMouseOver()
    {
        if (LabManager.LM != null)
        {
            LabManager.LM.m_HoverText.color = Color.yellow;
            LabManager.LM.m_HoverText.text = this.m_ToolName;
            if (this.m_ToolType == ToolType.TestingTube && this.chimicalContent != "")
            {
                LabManager.LM.m_HoverText.text = LabManager.LM.fn_TextShower(this.gameObject);
            }
        }
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
                    if (ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].m_Missions.Length > ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex)
                    {
                        ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex++;
                        LabManager.LM.m_ToolText.text = "ﺔﺤﻴﺤﺻ ﺓﻮﻄﺧ";
                    }
                    else
                    {
                        LabManager.LM.m_ToolText.text = "ﺡﺎﺠﻨﺑ ﺔﺑﺮﺠﺘﻟﺍ ﺖﻤﻤﺗﺍ ﺪﻘﻟ";
                        GameObject.Find("Bunsen Burner").GetComponent<Tool>().m_Content.SetActive(false);
                        GameObject.Find("Beaker").GetComponent<Tool>().m_Content.SetActive(false);
                    }
                    
                    
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
        this.chimicalContent = "";
        isFull = false;
    }

    public IEnumerator ComeBack ()
    {
        yield return new WaitForSeconds(6.0f);
        Direction = originalPosition;
        isMoving = true;
        startTime = Time.time;
        
        if (this.m_ToolType == ToolType.Dropper)
        {
            gameObject.transform.localScale = new Vector3(1.4f, 1f, 0.7f);
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    public IEnumerator WaitBoiling ()
    {
        yield return new WaitForSeconds(4.0f);
        if (this.m_ToolType == ToolType.Beaker)
        {
            m_Content.SetActive(true);
        }
        else
        {
            GameObject.Find("Beaker").GetComponent<Tool>().m_Content.SetActive(true);
        }
    }
    public void fn_Bunsen_Burner(LabState LS)
    {

            m_Content.SetActive(true);
            LabManager.LM.m_LabState = LabState.Idle;
            LabManager.LM.fn_ResetCurrentSelectedTool();

            ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].
            m_Missions[ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex].isDone = true;
            LabManager.LM.fn_UpdateMissionText(ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex + 1);
            ApplicationManager.AM.m_Scenes[ApplicationManager.AM.m_CurrentScenesIndex].LastMissionIndex++;
            LabManager.LM.m_ToolText.text = "ﺔﺤﻴﺤﺻ ﺓﻮﻄﺧ";

            

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
                    isFull = true;
                    m_Content.SetActive(false);
                    this.chimicalContent = OtherTool.chimicalContent;
                    LabManager.LM.m_LabState = LabState.Idle;
                    LabManager.LM.fn_ResetCurrentSelectedTool();
                }
                else
                {
                    LabManager.LM.m_ToolText.text = "ﺓﺍﺩﻷﺍ ﻩﺬﻫ ﻊﻣ ﺓﺭﺎﻄﻘﻟﺍ ﻡﺍﺪﺨﺘﺳﺍ ﻚﻨﻜﻤﻳ ﻻ";
                }
            }
            else
            {
                LabManager.LM.m_ToolText.text = "ﻻﻭﺍ ﺎﻬﻏﺍﺮﻓﺍ ﻚﻴﻠﻋ ﺔﺌﻠﺘﻤﻣ ﺓﺭﺎﻄﻘﻟﺍ";
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
                    this.m_Content.SetActive(true);
                    Chemistry Makeit = OtherTool.gameObject.GetComponentInChildren<ChangeWaterColor>().SearchColorName("Detecting Sugar", this.chimicalContent, OtherTool.chimicalContent);
                    if (Makeit.colorComponent != Vector3.zero)
                    {
                        OtherTool.gameObject.GetComponentInChildren<ChangeWaterColor>().GetVector(Makeit.colorComponent);
                    }   
                    
                    if (OtherTool.chimicalContent == "")
                        OtherTool.chimicalContent = this.chimicalContent;
                    else
                    {
                        OtherTool.chimicalContent += "+" + this.chimicalContent;
                        OtherTool.gameObject.GetComponentInChildren<ChangeWaterColor>().StartCoroutine("Incremental");
                    }
                    
                    
                    fn_ClearContent();
                    LabManager.LM.m_LabState = LabState.Idle;
                    LabManager.LM.fn_ResetCurrentSelectedTool();
                }
                else
                {
                    LabManager.LM.m_ToolText.text = "ﺓﺍﺩﻷﺍ ﻩﺬﻫ ﻲﻓ ﺓﺭﺎﻄﻘﻟﺍ ﻯﻮﺘﺤﻣ ﻎﻳﺮﻔﺗ ﻚﻨﻜﻤﻳ ﻻ";
                }
            }
            else
            {
                LabManager.LM.m_ToolText.text = "ﻪﻏﺭﺎﻓ ﺓﺭﺎﻄﻘﻟﺍ";
            }
        }

        fn_Interact(OtherTool, 90f, 0, 0);
        StartCoroutine("ComeBack");
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
                LabManager.LM.m_ToolText.text = "ﻞﻌﻔﻟﺎﺑ ﺎﻬﺘﻨﺤﻃ ﺪﻘﻟ";
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
                    LabManager.LM.m_ToolText.text = "ﻩﺍﺩﻷﺍ ﻚﻠﺗ ﻊﻣ ﺎﻬﻣﺍﺪﺨﺘﺳﺍ ﻚﻨﻜﻤﻳ ﻻ";
                }
            }
            else
            {
                LabManager.LM.m_ToolText.text = "ﺎﻬﻠﺧﺍﺪﺑ ﺪﻌﺑ ًﺎﺌﻴﺷ ﻊﻀﺗ ﻢﻟ";
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

            if (Beaker.chimicalContent == "Heat")
            {
                this.m_ToolTempreture = 100;
            }
            Chemistry Makeit = this.gameObject.GetComponentInChildren<ChangeWaterColor>().SearchColorName("Detecting Sugar", Beaker.chimicalContent, this.chimicalContent);
            if (Makeit.colorComponent != Vector3.zero)
            {
                this.gameObject.GetComponentInChildren<ChangeWaterColor>().GetVector(Makeit.colorComponent);
            }

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
        //gameObject.transform.position = OtherTool.m_InteractEntryPoint.position;
        Direction = OtherTool.m_InteractEntryPoint.position;
        isMoving = true;
        startTime = Time.time;
    }

    public void fn_Interact (Tool OtherTool, float XRotation, float YRotation, float ZRotation)
    {
        //gameObject.transform.position = OtherTool.m_InteractEntryPoint.position;
        gameObject.transform.eulerAngles = new Vector3(XRotation, YRotation, ZRotation);
        Direction = OtherTool.m_InteractEntryPoint.position;
        isMoving = true;
        startTime = Time.time;
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
