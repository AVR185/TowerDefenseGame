using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerDefense.Targetting;
using TowerDefense.Affectors;
using TowerDefense.Towers;
using TowerDefense.Agents;
using ActionGameFramework.Health;
using Core.Health;

public class AbilitiesUI : MonoBehaviour
{
    public AbilityHub abilityHub;

    public GameObject cancelButton;

    public LayerMask whatIsPlatform;

    private bool IsAbilitySelected = false;

    public GameObject abilityMarker;

    public SerializableIAlignmentProvider alignment;

    public IAlignmentProvider alignmentProvider
    {
        get { return alignment != null ? alignment.GetInterface() : null; }
    }

    private MeshRenderer quadRenderer;
    private ParticleSystem quadSystem;

    private Hability currentAbility;

    private AbilityUiButton currentAbilityButton;

    private Targetter targetter;

    private List<Targetable> targetsToBeAffected = new List<Targetable>();


    void Start()
    {
        cancelButton.SetActive(false);

        abilityMarker.transform.position = Vector3.zero;

        quadRenderer = abilityMarker.GetComponentInChildren<MeshRenderer>();
        quadSystem = abilityMarker.GetComponentInChildren<ParticleSystem>();

        targetter = abilityMarker.GetComponent<Targetter>();

        if (targetter != null)
        {
            targetter.targetEntersRange += IncludeTarget;
            targetter.targetExitsRange += ExcludeTarget;
        }
    }

    private void OnDestroy()
    {
        targetter.targetEntersRange -= IncludeTarget;
        targetter.targetExitsRange -= ExcludeTarget;
    }

    void Update()
    {
        if (IsAbilitySelected)
        {
            cancelButton.SetActive(true);
            currentAbility.indicador.enabled = true;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, whatIsPlatform))
            {
                var pos = hit.point + Vector3.up;
                abilityMarker.transform.position = pos;
            }

            //Clic de raton
            if ((Input.GetMouseButtonUp(0) || Input.touchCount > 1) && !EventSystem.current.IsPointerOverGameObject())
            {
                IsAbilitySelected = false;
                abilityMarker.SetActive(false);
                AbilityResourceManager.ablRscManager.BuyAbility(currentAbility.cost);
                CastAbility();
                currentAbilityButton.Cast();
                targetsToBeAffected.Clear();
            }
        }
        else
        {
            cancelButton.SetActive(false);
            abilityMarker.SetActive(false);
            if (currentAbility != null)
            {
                currentAbility.indicador.enabled = false;
            }
        }

    }


    public void SelectAbility(AbilityUiButton abilityBtn)
    {
        if (abilityMarker == null)
        {
            Debug.Log("missing ability marker");
            return;
        }
        quadRenderer.material.SetColor("_TintColor", abilityBtn.ability.abilityColor);
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
        quadSystem.startColor = abilityBtn.ability.abilityColor;
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
        abilityMarker.SetActive(true);
        IsAbilitySelected = true;
        abilityMarker.transform.localScale = Vector3.one * abilityBtn.ability.abilityArea;
        currentAbility = abilityBtn.ability;
        currentAbilityButton = abilityBtn;
        targetsToBeAffected.Clear();
    }

    public void CancelCast()
    {
        if (abilityMarker == null)
        {
            Debug.Log("missing ability marker");
            return;
        }
        abilityMarker.SetActive(false);
        IsAbilitySelected = false;
    }

    private void IncludeTarget(Targetable target)
    {
        targetsToBeAffected.Add(target);
    }

    private void ExcludeTarget(Targetable target)
    {
        if (targetsToBeAffected.Contains(target))
        {
            targetsToBeAffected.Remove(target);
        }
    }

    public void CastAbility()
    {
        switch (currentAbility.abilityType)
        {
            case Hability.AbilityType.DamageEnemies:
                foreach (Targetable t in targetsToBeAffected)
                {

                    if (t == null || t.isDead)
                    {

                        continue;
                    }
                    var agent = t.GetComponent<Agent>();

                    if (agent != null)
                    {
                        t.TakeDamage(abilityHub.fireBallDamage, t.transform.position, alignmentProvider);
                    }
                }
                break;

            case Hability.AbilityType.FreezeEnemies:
                foreach (Targetable t in targetsToBeAffected)
                {

                    if (t == null || t.isDead)
                    {

                        continue;
                    }
                    var agent = t.GetComponent<Agent>();

                    if (agent != null)
                    {
                        var s = agent.navMeshNavMeshAgent.speed;
                        var delay = abilityHub.freezeDuration;
                        StartCoroutine(BackToNormalSpeed(agent, s, delay));
                        agent.navMeshNavMeshAgent.speed = 0;
                    }
                }
                break;

            case Hability.AbilityType.HealTowers:
                foreach (Targetable t in targetsToBeAffected)
                {
                    if (t == null || t.isDead)
                    {

                        continue;
                    }
                    var tower = t.GetComponent<Tower>();
                    if (tower != null)
                    {
                        var d = t.GetComponent<DamageableBehaviour>().configuration;
                        if (d != null)
                            d.SetHealth(d.currentHealth + abilityHub.healthRegenAmount);
                    }
                }
                break;
        }
    }

    IEnumerator BackToNormalSpeed(Agent a, float s, float delay)
    {
        yield return new WaitForSeconds(delay);

        a.navMeshNavMeshAgent.speed = s;
    }
}

