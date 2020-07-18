using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Targetting;
using TowerDefense.Affectors;
using TowerDefense.Towers;
using ActionGameFramework.Health;
using System;

public class BoostAffector : PassiveAffector
{
    [Range(1, 5)]
    public float boostFactor;

    public string boostFactorFormat = "<b>Boost Factor:</b> {0}";

    public GameObject boostFxPrefab;

    //boost audio
    public AudioSource audioSource;

	private List<Tower> boostedTowers = new List<Tower>();

	private List<GameObject> boostFxs = new List<GameObject>();


	protected void Awake()
	{
		towerTargetter.targetEntersRange += OnTargetEntersRange;
		towerTargetter.targetExitsRange += OnTargetExit;
	}


	/// <summary>
	/// Unsubsribes from the relevant targetter events
	/// </summary>
	void OnDestroy()
	{
		RemoveAllBoost();
		towerTargetter.targetEntersRange -= OnTargetEntersRange;
		towerTargetter.targetExitsRange -= OnTargetExit;
	}

	public void RemoveAllBoost()
	{
		
		var boosts = boostedTowers;
		int x = boosts.Count;
		
		for (int i = 0; i < x; i++)
		{
			RemoveBoost(boosts[i]);
		}

		boostedTowers.Clear();
		boostFxs.Clear();
	}

	protected void AddBoost(Tower target)
	{
		var attack = target.GetComponentInChildren<AttackAffector>();
		if (attack != null)
		{
			
			var boost = target.GetComponent<TowerBoosted>();
			if(boost != null)
			{
				return;
			}
			target.gameObject.AddComponent<TowerBoosted>();
			boostedTowers.Add(target);
			var fx = Instantiate(boostFxPrefab);
			boostFxs.Add(fx);
			fx.transform.parent = target.transform;
			fx.transform.localPosition = Vector3.zero;
			fx.transform.localScale *= 1;
			attack.fireRate *= boostFactor;
		}
		if (audioSource != null)
		{
			audioSource.Play();
		}
	}



	protected void RemoveBoost(Tower target)
	{
		if (target == null)
		{
			return;
		}
		
		if (boostedTowers.Contains(target))
		{
			var attack = target.GetComponentInChildren<AttackAffector>();
			
			if (attack != null)
			{
				var a = boostedTowers.IndexOf(target);
				var f = boostFxs[a];
				Destroy(f);
				attack.fireRate /= boostFactor;

				Destroy(target.gameObject.GetComponent<TowerBoosted>());
			}
		}
	}

	/// <summary>
	/// Fired when the targetter aquires a new targetable
	/// </summary>
	protected void OnTargetEntersRange(Targetable other)
	{
		
		var tower = other as Tower;
		if (tower == null)
		{
			return;
		}
		
		AddBoost(tower);
	}


	private void OnTargetExit(Targetable other)
	{
		var tower = other as Tower;
		if (tower == null)
		{
			return;
		}

		RemoveBoost(tower);
	}

}

