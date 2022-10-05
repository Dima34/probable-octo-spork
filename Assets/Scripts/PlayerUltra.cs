using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerUltra : MonoBehaviour
{
    public bool IsUltraReady { get => isUltraReady; }

    [SerializeField] Spawner _spawner;

    Player player;
    bool isUltraReady = false;
    
    void OnEnable()
    {
        player = GetComponent<Player>();
        GlobalEventManager.OnStrenghtChange.AddListener(checkUltra);
    }

    void checkUltra(float strenght, float maxStrenght)
    {
        if(strenght == maxStrenght)
        {
            setUltra(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            UseUltra();
        }

        Debug.Log("Enemy amount " + _spawner.BlueEnemiesList.Count + _spawner.RedEnemiesList.Count);
    }

    void setUltra(bool state)
    {
        isUltraReady = state;
        GlobalEventManager.OnUltraStateChanged.Fire(state);
    }

    public void UseUltra()
    {
        if (isUltraReady)
        {
            for(int i = 0; i < _spawner.BlueEnemiesList.Count; i++)
            {
                if (_spawner.BlueEnemiesList[i] != null)
                {
                    Enemy enemyScript = _spawner.BlueEnemiesList[i].GetComponent<Enemy>();
                    GlobalEventManager.OnEnemyDeath.Fire(enemyScript, enemyScript.StrengthReward);
                    DestroyObject(enemyScript.gameObject);
                }
            }

            for (int i = 0; i < _spawner.RedEnemiesList.Count; i++)
            {
                if (_spawner.RedEnemiesList[i] != null)
                {
                    Enemy enemyScript = _spawner.RedEnemiesList[i].GetComponent<Enemy>();
                    GlobalEventManager.OnEnemyDeath.Fire(enemyScript, enemyScript.StrengthReward);
                    DestroyObject(enemyScript.gameObject);
                }
            }

            player.ApplyStrenghtChanges(player.MaxStrenght);

            setUltra(false);
        }
    }

    private void OnDisable()
    {
        GlobalEventManager.OnStrenghtChange.RemoveListener(checkUltra);
    }
}
