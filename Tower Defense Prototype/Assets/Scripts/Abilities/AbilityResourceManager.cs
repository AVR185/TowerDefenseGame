using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityResourceManager : MonoBehaviour
{
    public static AbilityResourceManager ablRscManager;

    public int startAbilityResource = 10;

    private int currentAbilityResource;

    public int CurrentAbilityResource { get; set; }

    /* Poner tope?
     *      if (currentAbilityResource > 100)
            {
                currentAbilityResource = 100;
            }
     */

    public Text abilityResourceText;

    private void Awake()
    {
        if(ablRscManager != null && ablRscManager != this)
        {
            Destroy(ablRscManager);
        }
        ablRscManager = this;

        CurrentAbilityResource = startAbilityResource;
    }

    void Start()
    {
        UpdateAbilityText();
    }
    
    public void UpdateAbilityText()
    {
        abilityResourceText.text = CurrentAbilityResource.ToString();
    }

    public void BuyAbility(int abilityCost)
    {
        CurrentAbilityResource -= abilityCost;
        UpdateAbilityText();
    }

    public void AddAbilityResource(int amount)
    {
        CurrentAbilityResource += amount;
        UpdateAbilityText();
    }
}
