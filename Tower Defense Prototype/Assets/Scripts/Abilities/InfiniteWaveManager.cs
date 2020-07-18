using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Level;
using TowerDefense.Agents.Data;
using TowerDefense.Nodes;
using System;

public class InfiniteWaveManager : MonoBehaviour
{
    private WaveManager wavemanager;

    public List<AgentConfiguration> agentConfigurations = new List<AgentConfiguration>();

    public GameObject waveObject;

    public Node startingNode;

    public int minEnemiesInWave = 20;
    public int maxEnemiesInWave = 30;

    public float minDelayTime = 0f;
    public float maxDelayTime = 3f;

    //public int abilityResourceFactor = 10; //ya no es necesario

    void Start()
    {
        wavemanager = GetComponent<WaveManager>();
        wavemanager.waveChanged += AddNewWave;
    }

    private void AddNewWave()
    {
        wavemanager.waves.Add(GenerateRandomWave());
        //AbilityResourceManager.ablRscManager.AddAbilityResource(abilityResourceFactor * wavemanager.waveNumber);
    }

    void Update()
    {
        
    }

    public Wave GenerateRandomWave()
    {
        int i = UnityEngine.Random.Range(minEnemiesInWave, maxEnemiesInWave);
        int t = 0;

        TimedWave newWave = Instantiate(waveObject).GetComponent<TimedWave>();
        newWave.spawnInstructions = new List<SpawnInstruction>();


        for (; i > 0; i--)
        {
            var s = GenerateRandomSpawnInstruction();
            t += Mathf.CeilToInt(s.delayToSpawn);
            newWave.spawnInstructions.Add(s);
        }

        newWave.timeToNextWave = t;

        return newWave;
    }

    public SpawnInstruction GenerateRandomSpawnInstruction()
    {
        SpawnInstruction newSpawnInstruction = new SpawnInstruction();
        newSpawnInstruction.agentConfiguration = agentConfigurations[UnityEngine.Random.Range(0, agentConfigurations.Count)];
        newSpawnInstruction.delayToSpawn = UnityEngine.Random.Range(minDelayTime, maxDelayTime);
        newSpawnInstruction.startingNode = startingNode;

        return newSpawnInstruction;
    }
}
