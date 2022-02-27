using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Character.Interactions
{
    public class EnterBattle : Interactable
    {
        public List<CharacterStats> myTeam = new List<CharacterStats>();
        public override void Interact(Interactor interactor)
        {
            BattleManager.data.teamA = interactor.myTeam;
            BattleManager.data.teamB = myTeam;
            SceneManager.LoadScene(0);
        }

        private void Awake()
        {
            int totalHP = 0;
            foreach(CharacterStats stats in myTeam)
            {
                totalHP += stats.hp;
            }
            if(totalHP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}