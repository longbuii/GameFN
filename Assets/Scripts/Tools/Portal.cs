    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class Portal : MonoBehaviour
    {
        // Start is called before the first frame update
        public string sceneToLoad;
        public HealthEnemyBoss healthEnemyBoss;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && healthEnemyBoss.isDead)
            {
                // Next to the scene of Game.
                SceneManager.LoadScene(sceneToLoad);    
            }
        }
    }
