using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Chemistry
{
    public string comingLiquid;
    public string currentLiquid;
    public Color mixedColor;
    public Vector3 colorComponent;
    
    public Chemistry ( string first , string second , Vector3 equalcolor)
    {
        comingLiquid = first;
        currentLiquid = second;
        colorComponent = equalcolor;
        mixedColor = new Color(equalcolor.x, equalcolor.y, equalcolor.z);
    }
    public Chemistry( string first, string second, Color madeColor)
    {
        comingLiquid = first;
        currentLiquid = second;
        colorComponent = new Vector3 (madeColor.r,madeColor.g,madeColor.b);
        mixedColor = madeColor;
    }
   
}

public class ChangeWaterColor : MonoBehaviour {

    public bool isFilled = false;
    public float speed = 0.7f;
    public float smoothChange = 2f;
    public float alpha = 0.5f;
    public float waveSize = 3f;
    private float increaseAmount = 0.22f;
    public Vector3 colors = Vector3.one;
    public Color SetColor;
    public Color tempColor;
    Color C;
    Renderer rend;
    MeshRenderer Visability;
    IEnumerator Passer;

    Chemistry [] Detecting_Sugar = new Chemistry[6];
	
	void Start () 
    {
		rend = gameObject.GetComponent<Renderer>();
        Visability = this.gameObject.GetComponent<MeshRenderer>();
        C = rend.material.color;
        C.a = alpha;
        //SetColor = rend.material.GetColor("_Color");
        tempColor = rend.material.GetColor("_Color");
        Visability.enabled = isFilled;
        if (isFilled == false)
        {
            C.a = 0f;
        }
       setUpColors();
	}
	
	void Update () 
    {
       float currentTime = Time.time;
       float moveWater = (float)(Mathf.PingPong(currentTime * speed, 100) * 0.15);

       rend.material.mainTextureOffset = new Vector2(moveWater, moveWater);
       rend.material.color = C;
       rend.material.mainTextureScale = new Vector2(waveSize, waveSize);
       
        if (Input.GetKeyUp(KeyCode.C))
       {
           Incremental();
       }
        
       if (Input.GetKeyUp(KeyCode.Space))
       {
           SetColor.r = Random.Range(0, 1f);
           SetColor.g = Random.Range(0, 1f);
           SetColor.b = Random.Range(0, 1f);

       }
       if (Input.GetKeyUp(KeyCode.Z))
       {
           SetColor.r = colors.x;
           SetColor.g = colors.y;
           SetColor.b = colors.z;
       }
       if (Input.GetKeyUp(KeyCode.X))
       {
           Visability.enabled = !isFilled;
           //isFilled = !isFilled;
           if (isFilled == false)
           {
               GetVector(new Vector3(0, 0.11765f, 1));
           }
           else
               isFilled = false;
       }
       //rend.material.SetColor("_Color", SetColor);
       tempColor = Color.Lerp(tempColor, SetColor, Time.deltaTime * smoothChange);
       rend.material.SetColor("_Color", tempColor);
	}

    public void SetMyColor ()
    {
        Visability.enabled = true;
        isFilled = true;
        SetColor.r = colors.x;
        SetColor.g = colors.y;
        SetColor.b = colors.z;
        //SetColor is equal to SugerDetect[0].mixedColor;
    }

    public void GetVector(Vector3 color)
    {
        if (isFilled == false)
        {
            Visability.enabled = false;
            SetColor.a = 0;
            SetColor.r = color.x;
            SetColor.g = color.y;
            SetColor.b = color.z;
            StartCoroutine("WaitToFill");
        }
        else
        {
            Passer = WaitColorChanger(color);
            StartCoroutine(Passer);
            //Incremental();
        }
        
    }
    public void GetColor(Color color)
    {
        if (isFilled == false)
        {
            Visability.enabled = false;
            SetColor = color;
            SetColor.a = 0;
            StartCoroutine("WaitToFill");
            SetColor.a = color.a;
        }
        else
        {
            SetColor.a = color.a;
            Passer = WaitColorChanger(new Vector3(color.r, color.g, color.b));
            StartCoroutine(Passer);
            //Incremental();
        }
    }
    public IEnumerator Incremental ()
    {
        yield return new WaitForSeconds(1.5f);
        if (this.gameObject.transform.localScale.y < 1f)
        {
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y + 0.2f , this.gameObject.transform.localScale.z);
            //if (this.gameObject.transform.localScale.y >= 0.4f)
            //{ increaseAmount *= 1.2f; }
            this.gameObject.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z + increaseAmount);
        }
    }
    public IEnumerator WaitColorChanger(Vector3 color)
    {

        yield return new WaitForSeconds(3f);
        SetColor.r = color.x;
        SetColor.g = color.y;
        SetColor.b = color.z;
    }
    
    public IEnumerator WaitToFill()
    {
        SetColor.a = 0f;
        yield return new WaitForSeconds(1f);
        isFilled = true;
        Visability.enabled = true;
        yield return new WaitForSeconds(0.3f);
        SetColor.a = 0.3f;
        yield return new WaitForSeconds(0.3f);
        SetColor.a = 0.6f;
        yield return new WaitForSeconds(0.3f);
        SetColor.a = 1f;
    
    }

    // Use this for initialization
    
    void setUpColors()
    {
        // 1-first(comming) , 2-second(current123)
        // "~" any thing in side simpole
        // if not found return the same color as you have setColor
        Detecting_Sugar[0] = new Chemistry("Glucose_Solution", "", new Vector3(1f,0.92f,0.196f));
        Detecting_Sugar[1] = new Chemistry("Starch_Solution", "", new Vector3(0.47f,0.588f,0.588f));
        Detecting_Sugar[2] = new Chemistry("Egg_Yolk", "", new Color(0.843f,0.784f,0.0588f,0.588f));
        Detecting_Sugar[3] = new Chemistry("Distilled_Water", "", new Vector3(0.294f,0.7255f,0.86f));
        Detecting_Sugar[4] = new Chemistry("Benedict_Reagent", "", new Vector3(0, 0.4f, 1));
        //Detecting_Sugar[5] = new Chemistry("Benedict_Reagent", "Glucose_Solution", new Vector3(0, 0.4f, 1));
        Detecting_Sugar[5] = new Chemistry("Heat", "Glucose_Solution+Benedict_Reagent", new Vector3(1f, 0.4f, 0f));
        
        //-------------------------------------------------------------------------------------------

        //fill all siuations
    }



    public Chemistry SearchColorName(string name, string first, string second)
    {
        Chemistry[] currentLab;
        switch (name)
        {
            case "Detecting Sugar": currentLab = Detecting_Sugar;
                break;
            case "Detecting Carbs": currentLab = Detecting_Sugar;
                break;
            //fill rest cases ...
            default: currentLab = Detecting_Sugar;
                break;
        }
        foreach (Chemistry Finder in currentLab)
        {
            if ((first == Finder.comingLiquid && second == Finder.currentLiquid) || (first == Finder.currentLiquid && second == Finder.comingLiquid))
            {
                return Finder;
            }
        }
        return new Chemistry("","",Vector3.zero);
    }
    

}
