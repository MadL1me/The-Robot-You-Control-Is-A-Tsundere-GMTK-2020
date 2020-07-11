using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMTK2020.Level
{
    public class LevelManager : MonoBehaviour
    {
        private List<Enemy> _levelEnemies;

        [SerializeField] private int _totalEnemiesCount;
        [SerializeField] private int _enemiesKilled;

        [SerializeField] private float _delayBetweenRestartAndLevelSwitch;

        private MusicManager _music;

        private AstarPath _pathfinder;

        public bool IsCompleted { get; private set; }

        private void Start()
        {
            _enemiesKilled = 0;

            FindEnemies();
            FindPlayer();

            _music = FindObjectOfType<MusicManager>();
        }

        public void Intensify() =>
            _music.Intensify();

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
            if (_enemiesKilled >= _totalEnemiesCount && !IsCompleted)
            {
                IsCompleted = true;
                Debug.Log("Level completed");
            }
        }

        public void ProceedToNextLevel() => StartCoroutine(NextLevelProceedCoroutine());

        public void RestartLevel() =>   StartCoroutine(RestartCoroutine());
        
        private IEnumerator NextLevelProceedCoroutine()
        {
            yield return new WaitForSeconds(_delayBetweenRestartAndLevelSwitch);
            Application.LoadLevel(Application.loadedLevel);
        }
        
        private IEnumerator RestartCoroutine()
        {
            Debug.Log("DIUEEEEEEE");
            
            yield return new WaitForSeconds(_delayBetweenRestartAndLevelSwitch);
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}