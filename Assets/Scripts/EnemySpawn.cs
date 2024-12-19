
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawn; // Lista de poções no mapa
    [SerializeField] private float respawnInterval = 5f; // Tempo entre cada tentativa de reativação (em segundos)

    [SerializeField] private GameObject enemy;

    private void Start()
    {
        StartCoroutine(RespawnBats());
    }

    private IEnumerator RespawnBats()
    {
        while (true)
        {
            // Aguarda o intervalo antes de tentar reativar
            yield return new WaitForSeconds(respawnInterval);

            // Contabiliza as poções ativas

            int randomIndex = Random.Range(0, 9);
            
            Instantiate(enemy, spawn[randomIndex].transform.position, spawn[randomIndex].transform.rotation);


        }
    }



}
