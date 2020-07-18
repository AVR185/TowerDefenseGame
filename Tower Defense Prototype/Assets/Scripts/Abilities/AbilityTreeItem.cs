using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTreeItem : MonoBehaviour
{
    public List<Image> connectors = new List<Image>();

    public List<AbilityTreeItem> requiredNodes = new List<AbilityTreeItem>();

    public string nodeName;
    public string spanishNodeName;

    public string NodeDescription;
    public string spanishDescription;

    public int NodeCost;

    public AbilityHub.AbilityNodeType nodeType;

    [HideInInspector]
    public bool IsLocked = true;

    public AbilityHub hub;


    public void OnClicked()
    {
        hub.AbilityIconClicked(this);
    }

    public void Purchase()
    {
        GetComponent<Image>().color = Color.Lerp(Color.green, Color.blue, 0.2f);
        foreach (var c in connectors)
        {
            c.color = Color.Lerp(Color.green, Color.blue, 0.2f);
        }

        IsLocked = false;
    }
}
