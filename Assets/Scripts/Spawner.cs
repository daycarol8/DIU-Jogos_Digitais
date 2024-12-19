
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
   public List<GameObject> potions; // Lista de poções no mapa
    public float respawnInterval = 5f; // Tempo entre cada tentativa de reativação (em segundos)
    public int maxActivePotions = 3; // Quantidade máxima de poções ativas ao mesmo tempo

    private void Start()
    {
        // Desativa todas as poções no início
        foreach (GameObject potion in potions)
        {
            potion.SetActive(false);
        }

        // Começa o ciclo de respawn
        StartCoroutine(RespawnPotions());
    }

    private IEnumerator RespawnPotions()
    {
        while (true)
        {
            // Aguarda o intervalo antes de tentar reativar
            yield return new WaitForSeconds(respawnInterval);

            // Contabiliza as poções ativas
            int activePotionsCount = CountActivePotions();

            // Verifica se ainda há espaço para ativar mais poções
            if (activePotionsCount < maxActivePotions)
            {
                // Escolhe uma poção aleatória da lista
                GameObject randomPotion = GetRandomInactivePotion();

                // Ativa a poção se ela estiver inativa
                if (randomPotion != null)
                {
                    randomPotion.SetActive(true);
                    Debug.Log($"Poção ativada: {randomPotion.name}");
                }
            }
        }
    }

    private int CountActivePotions()
    {
        int count = 0;
        foreach (GameObject potion in potions)
        {
            if (potion.activeSelf) count++;
        }
        return count;
    }

    private GameObject GetRandomInactivePotion()
    {
        // Cria uma lista de poções inativas
        List<GameObject> inactivePotions = new List<GameObject>();

        foreach (GameObject potion in potions)
        {
            if (!potion.activeSelf)
            {
                inactivePotions.Add(potion);
            }
        }

        // Retorna uma poção aleatória, se houver alguma inativa
        if (inactivePotions.Count > 0)
        {
            int randomIndex = Random.Range(0, inactivePotions.Count);
            return inactivePotions[randomIndex];
        }

        return null;
    }

}
