using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUiButton : MonoBehaviour
{
    public Hability ability;

    [Space]
    public Image abilityIcon, cdBar;

    public Text texto;

    private float timeToNextCast = 0;

    private bool IsSkillAvilable = true;

    private Button btn;

    private AbilityResourceManager res;

    void Start()
    {
        btn = GetComponent<Button>();
        abilityIcon.sprite = ability.icon;
        ability.indicador.enabled = false;

        res = AbilityResourceManager.ablRscManager;

        if (ability.locked)
        {
            gameObject.SetActive(false);
        }
    }

    public void Unlock()
    {
        ability.locked = false;
        gameObject.SetActive(true);
        texto.text = ability.cost.ToString();
    }
   
    void Update()
    {
        if(timeToNextCast < 0 && ability.cost < res.CurrentAbilityResource)
        {
            IsSkillAvilable = true;
        }
        else
        {
            IsSkillAvilable = false;
            timeToNextCast -= Time.deltaTime;
        }

        if (IsSkillAvilable)
        {
            btn.interactable = true;
        }
        else
        {
            btn.interactable = false;
        }

        cdBar.fillAmount = 1 - timeToNextCast / ability.coolDown;
    }

    public void Cast()
    {
        timeToNextCast = ability.coolDown;
    }
}
