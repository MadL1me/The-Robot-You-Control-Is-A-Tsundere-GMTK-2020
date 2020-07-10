using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMTK2020.Level
{
    public class LevelManager : MonoBehaviour
    {
        private List<Enemy> _levelEnemies;
        
        private int _totalEnemiesCount;
        private int _enemiesKilled;

        [SerializeField] private float _delayBetweenRestartAndLevelSwitch;
        
        private void Start()
        {
            _enemiesKilled = 0;
            
            FindEnemies();
            FindPlayer();
            
            ScanLevelField();
        }

        private void ScanLevelField()
        {
            
        }

        private void FindPlayer()
        {
            var player = FindObjectOfType<Player>();
            player.OnActorDeath += RestartLevel;
        }

        private void FindEnemies()
        {
            _levelEnemies = FindObjectsOfType<Enemy>().ToList();
            _totalEnemiesCount = _levelEnemies.Count;

            foreach (var enemy in _levelEnemies)
            {
                enemy.OnActorDeath += OnEnemyDie;
            }
        }
        
        public void OnEnemyDie()
        {
            _enemiesKilled++;
            if (_enemiesKilled >= _totalEnemiesCount)
                ProceedToNextLevel();
        }

        public void ProceedToNextLevel()
        {
            //TODO some cool looking coroutine
        }

        public void RestartLevel()
        {
            //TODO some cool loolking restart
        }
    }
}