using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public SpawnManager spawnManager;
    public bool startSpawn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (startSpawn){
                 spawnManager.StartSpawning();
                 Debug.Log("Player entered spawn trigger, starting spawn.");
                 }


            else
            {
                spawnManager.StopSpawning();
                Debug.Log("Player entered spawn trigger, stopping spawn.");
            }
                
        }
    }
//   private void OnTriggerExit2D(Collider2D other)
// {
//     if (other.CompareTag("Player"))
//     {
//         if (startSpawn)
//         {
//             spawnManager.StopSpawning();
//             spawnManager.ClearAllEnemies(); //
//             Debug.Log("Player exited spawn trigger, stopping spawn and clearing enemies.");
//         }
//         else
//         {
//             spawnManager.StartSpawning();
//             Debug.Log("Player exited spawn trigger, starting spawn.");
//         }
//     }
// }
}