using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIInfoDisplayManager : MonoBehaviour
{
    public Text label1;
    public Text label2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("UpdateInfoDisplay");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator UpdateInfoDisplay()
    {
        while (true)
        {
            label1.text = "Nodes = " + GetNodesString();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void UpdateEval(float to)
    {
        label2.text = "Evaluation = " + to.ToString();
    }

    public string GetNodesString()
    {
        int lNodes = AlphaBateAI.nodes;
        if (lNodes >= 1000000000)
        {
            return string.Format("{0:0.00}", lNodes / 1000000000f) + "B";
        }
        else if (lNodes >= 1000000)
        {
            return string.Format("{0:0.00}", lNodes / 1000000f) + "M";
        }
        else if (lNodes >= 1000)
        {
            return string.Format("{0:0.00}", lNodes / 1000f) + "K";
        }
        else
        {
            return lNodes.ToString();
        }
    }
}
