using System.Collections;
using System.Collections.Generic;
using TowerDefense.UI.HUD;
using UnityEngine;
using UnityEngine.UI;
using GameUIState = TowerDefense.UI.HUD.GameUI.State;

public class AbilityHub : MonoBehaviour
{
    /// <summary>
    /// Enum to represent state of pause menu
    /// </summary>
    public enum State
    {
        Open,
        Closed
    }

    public GameObject[] menusDeshabilitar = new GameObject[3];

    /// <summary>
    /// State of ability menu
    /// </summary>
    private State _currentState = State.Closed;

    public State CurrentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
        }
    }

    public static AbilityHub Abilityhub { get; set; }

    /// <summary>
    /// The CanvasGroup that holds the ability menu UI
    /// </summary>
    public Canvas abilityMenuCanvas;

    /// <summary>
    /// If the ability menu was opened/closed this frame
    /// </summary>
    bool m_MenuChangedThisFrame;

    [Header("Skill specific")]
    [Space]
    public float fireBallDamage = 10;
    [Space]
    public float healthRegenAmount = 5;
    [Space]
    public float freezeDuration = 5;

    [Space]
    public AbilityUiButton fireBallUiButton, healUiButton, freezeUiButton;

    [Header("Ability Tree UI")]
    [Space]
    public GameObject purchaseButton, Description;
    [Space]
    public Text abilityTitle;
    [Space]
    public Text abilityDescription;
    [Space]
    public Text abilityCost;

    [Header("Ability points related")]
    [Space]
    public Text abilityPointsText;

    [Space]
    public int startAbilityPoints = 10;

    public int CurrentAbilityPoints { get; set; }

    private AbilityTreeItem CurrentlySelectedNode;

    public enum AbilityNodeType { fireballI, fireballII, healI, healII, freezeI, freezeII, freezeIII, freezeIV }

    /// <summary>
    /// Subscribe to GameUI's stateChanged event
    /// </summary>
    protected void Start()
    {
        if (GameUI.instanceExists)
        {
            CurrentAbilityPoints = startAbilityPoints;

            purchaseButton.SetActive(false);

            UpdateAbilityPoints();
        }
    }

    /// <summary>
    /// Hide the abilitie menu on awake
    /// </summary>
    private void Awake()
    {
        SetAbilityMenuCanvas(false);

        CurrentState = State.Closed;
    }

    /// <summary>
    /// Show/Hide the ability menu canvas group
    /// </summary>
    protected void SetAbilityMenuCanvas(bool enable)
    {
        abilityMenuCanvas.enabled = enable;
    }

    /// <summary>
    /// Open the pause menu
    /// </summary>
    public void OpenAbilityMenu()
    {
        CurrentState = State.Open;
        SetAbilityMenuCanvas(true);
        //Ralentizamos el tiempo
        Time.timeScale = 0.2f;

        foreach (GameObject go in menusDeshabilitar)
        {
            go.SetActive(false);
        }

        if (GameUI.instanceExists)
        {
            GameUI.instance.SetAbilityState();
        }
    }

    /// <summary>
    /// Close the pause menu
    /// </summary>
    public void CloseAbilityMenu()
    {
        SetAbilityMenuCanvas(false);

        CurrentState = State.Closed;

        //Reanudamos la marcha normal
        Time.timeScale = 1f;

        if (GameUI.instanceExists)
        {
            GameUI.instance.Unpause();
        }

        foreach (GameObject go in menusDeshabilitar)
        {
            go.SetActive(true);
        }
    }

    public void PurchaseTreeNode(int abilityPoints)
    {
        CurrentAbilityPoints -= abilityPoints;
    }

    //Cuando pulsas un icono de habilidad
    public void AbilityIconClicked(AbilityTreeItem item)
    {
        if (item.NodeCost <= CurrentAbilityPoints)
        {
            purchaseButton.SetActive(true);
        }
        else
        {
            purchaseButton.SetActive(false);
        }
        if (!item.IsLocked)
        {
            purchaseButton.SetActive(false);
        }

        foreach (var r in item.requiredNodes)
        {
            if (r.IsLocked)
            {
                purchaseButton.SetActive(false);
            }
        }

        Description.SetActive(true);
        abilityCost.text = item.NodeCost.ToString();

        //Translation
        if (SystemLanguage.Spanish.Equals(Application.systemLanguage)
                & !string.IsNullOrEmpty(item.spanishNodeName))
        {
            abilityTitle.text = item.spanishNodeName;
        } else
        {
            abilityTitle.text = item.nodeName;
        }
        //Translation
        if (SystemLanguage.Spanish.Equals(Application.systemLanguage)
                & !string.IsNullOrEmpty(item.spanishDescription))
        {
            abilityDescription.text = item.spanishDescription;
        } else
        {
            abilityDescription.text = item.NodeDescription;
        }

        CurrentlySelectedNode = item;
    }

    //Comprar habilidad
    public void PurchaseNode()
    {
        CurrentlySelectedNode.Purchase();
        CurrentAbilityPoints -= CurrentlySelectedNode.NodeCost;
        UpdateAbilityPoints();
        purchaseButton.SetActive(false);
        Description.SetActive(false);

        switch (CurrentlySelectedNode.nodeType)
        {
            case AbilityNodeType.fireballI:
                IncreaseFireballDamage();
                break;
            case AbilityNodeType.fireballII:
                DecreaseFireBallCooldown();
                break;
            case AbilityNodeType.healI:
                UnlockHeal();
                break;
            case AbilityNodeType.healII:
                IncreaseHealAmount();
                break;
            case AbilityNodeType.freezeI:
                UnlockFreeze();
                break;
            case AbilityNodeType.freezeII:
                ReduceFreezeCooldown();
                break;
            case AbilityNodeType.freezeIII:
                ReduceFreezeCost();
                break;
            case AbilityNodeType.freezeIV:
                IncreaseFreezeDuration();
                break;
            default:
                break;
        }
    }


    public void UpdateAbilityPoints()
    {
        abilityPointsText.text = CurrentAbilityPoints.ToString();
    }


    //to increase fireball damage
    public void IncreaseFireballDamage()
    {
        fireBallDamage *= 2;
    }

    //to reduce fireball cooldown
    public void DecreaseFireBallCooldown()
    {
        float cd = fireBallUiButton.ability.coolDown;
        fireBallUiButton.ability.coolDown = Mathf.Clamp(cd / 2, 1f, cd);
    }

    //to unlock heal
    public void UnlockHeal()
    {
        healUiButton.Unlock();
    }

    //double heal
    public void IncreaseHealAmount()
    {
        healthRegenAmount *= 2f;
    }

    public void UnlockFreeze()
    {
        freezeUiButton.Unlock();
    }

    public void ReduceFreezeCooldown()
    {
        float cd = freezeUiButton.ability.coolDown;
        freezeUiButton.ability.coolDown = Mathf.Clamp(cd / 2, 1f, cd);
    }

    public void ReduceFreezeCost()
    {
        float cost = freezeUiButton.ability.cost;
        freezeUiButton.ability.cost = Mathf.CeilToInt(cost / 2);
    }

    public void IncreaseFreezeDuration()
    {
        freezeDuration *= 1.5f;
    }

    public void BuyAbilityPoints(int num)
    {
        if (AbilityResourceManager.ablRscManager.CurrentAbilityResource < num)
        {
            return;
        }
        CurrentAbilityPoints += 1;
        UpdateAbilityPoints();
        AbilityResourceManager.ablRscManager.CurrentAbilityResource -= num;
        AbilityResourceManager.ablRscManager.UpdateAbilityText();
    }
}