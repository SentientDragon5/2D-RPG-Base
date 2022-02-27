using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ActionType { None, Fight, Item, Run, WHY};
public class TurnBattleManager : MonoBehaviour
{
    public BattleData data;
    public List<GameObject> TeamPanel = new List<GameObject>();
    public GameObject characterVisualPrefab;

    public List<CharacterStats> teamA = new List<CharacterStats>();
    public List<CharacterStats> teamB = new List<CharacterStats>();

    public List<BattleCharacter> turnOrder = new List<BattleCharacter>();
    public int currentTurn;
    public BattleCharacter current
    {
        get
        {
            if (currentTurn >= turnOrder.Count) currentTurn -= turnOrder.Count;
            else if (currentTurn < 0) currentTurn += turnOrder.Count;

            return turnOrder[currentTurn];
        }
    }
    public GameObject choicePrefab;
    public List<GameObject> choices = new List<GameObject>();

    public Transform choiceParent;

    private BattleCharacter target;
    public ActionType action;
    public MoveSO move;

    public TextMeshProUGUI output;

    private void Awake()
    {
        teamA = data.teamA;
        teamB = data.teamB;

        MakeBattleCharacters();//Initiative
        ResetParams();
        MakeCharacterVisuals();
        Refresh();
    }

    [ContextMenu("REFRESH")]
    public void Refresh()
    {
        if(IsAllDead(0))
        {
            output.text = "You Lose!";
            MakeEndButton();
            return;
        }
        if (IsAllDead(1))
        {
            output.text = "You Win!";
            MakeEndButton();
            return;
        }

        if (current.userControlled)
        {
            if (action == ActionType.None)
            {
                output.text = "What shall " + current.Name + " do?";
                MakeActionOptions();
            }
            else
            {
                if (action == ActionType.Fight)
                {
                    if ((target == null))
                    {
                        output.text = "Who shall " + current.Name + " fight?";
                        MakeTargetOptions(current.team);
                    }
                    else
                    {
                        //Do DMG, end Turn
                        int dmg = current.Damage();
                        target.HP = target.HP - dmg;
                        EndTurn(current, " attacked and did " + dmg + " Damage.");
                    }
                }
                else if (action == ActionType.Item)
                {
                    foreach (BattleCharacter b in turnOrder)
                    {
                        if (b.team == current.team)
                        {
                            b.HP += 1;
                        }
                    }
                    EndTurn(current, " used <ITEM> to heal <HP>");
                }
                else
                {
                    //run
                    ExitBattle();
                }
            }
        }
        else
        {
            action = (ActionType)current.myStats.ai.Action();
            if (action == ActionType.Fight)
            {
                target = turnOrder[current.myStats.ai.RandTarget(NumTargets(current.team))];
                int dmg = current.Damage();
                target.HP = target.HP - dmg;
                EndTurn(current, " attacked and did " + dmg + " Damage.");
            }
            else if(action == ActionType.Item)
            {
                foreach (BattleCharacter b in turnOrder)
                {
                    if (b.team == current.team)
                    {
                        b.HP += 1;
                    }
                }
                EndTurn(current, " used <ITEM> to heal <HP>");
            }
            else
            {
                Debug.Log("AI did not choose to fight or Item.");
                EndTurn(current, " did nothing.");
            }
        }
    }

    [ContextMenu("MakeBattleCharacters")]
    public void MakeBattleCharacters()
    {
        turnOrder.Clear();
        foreach(CharacterStats c in teamA)
        {
            turnOrder.Add(new BattleCharacter(c, true,0));
        }
        foreach (CharacterStats c in teamB)
        {
            turnOrder.Add(new BattleCharacter(c, false,1));
        }
    }

    public List<BattleCharacter> Targets(int myTeam)
    {
        List<BattleCharacter> targets = new List<BattleCharacter>();
        foreach(BattleCharacter b in turnOrder)
        {
            if(b.team != myTeam)
            {
                targets.Add(b);
            }
        }
        return targets;
    }
    public List<int> TargetsIndex(int myTeam)
    {
        List<int> targets = new List<int>();
        for(int i=0;i<turnOrder.Count;i++)
        {
            if (turnOrder[i].team != myTeam && turnOrder[i].HP > 0)
            {
                targets.Add(i);
            }
        }
        return targets;
    }
    

    public void EndTurn(BattleCharacter turn, string action)
    {
        output.text = turn.Name + action;

        ResetParams();
        currentTurn++;

        if (currentTurn >= turnOrder.Count) currentTurn -= turnOrder.Count;
        else if (currentTurn < 0) currentTurn += turnOrder.Count;

        MakeContinueButton();
        //Wait(0.1f, Refresh);
        //Refresh();
    }
    public void ExitBattle()
    {
        SceneManager.LoadScene(1);
    }

    [ContextMenu("Reset")]
    public void ResetParams()
    {
        target = null;
        action = ActionType.None;
    }

    /// <summary>
    /// Using turnorder's index.
    /// </summary>
    /// <param name="index"></param>
    public void SetTarget(int index)
    {
        target = turnOrder[index];
    }

    [ContextMenu("MakeTargetOptions")]
    public void MakeTargetOptions(int team)
    {
        choices.Clear();
        DestroyAllChildren(choiceParent);
        List<int> targets = TargetsIndex(team);
        for (int i=0; i< targets.Count;i++)
        {
            GameObject c = Instantiate(choicePrefab, choiceParent);
            Debug.Log(targets[i]);
            ButtonExtra b = c.GetComponent<ButtonExtra>();
            b.value = targets[i];
            c.GetComponent<Button>().onClick.AddListener(b.SetTarget);
            //c.GetComponent<Button>().onClick.AddListener(delegate { SetTarget(targets[i]); });
            c.GetComponent<Button>().onClick.AddListener(Refresh);
            c.name = "Choice " + i;
            c.GetComponentInChildren<TextMeshProUGUI>().text = turnOrder[targets[i]].Name;

            choices.Add(c);
        }
    }
    [ContextMenu("MakeContinue")]
    public void MakeContinueButton()
    {
        choices.Clear();
        DestroyAllChildren(choiceParent);
        GameObject c = Instantiate(choicePrefab, choiceParent);
        ButtonExtra b = c.GetComponent<ButtonExtra>();
        b.value = 0;
        c.GetComponent<Button>().onClick.AddListener(Refresh);
        c.name = "Continue";
        c.GetComponentInChildren<TextMeshProUGUI>().text = "Proceed";

        choices.Add(c);
    }
    [ContextMenu("MakeEnd")]
    public void MakeEndButton()
    {
        choices.Clear();
        DestroyAllChildren(choiceParent);
        GameObject c = Instantiate(choicePrefab, choiceParent);
        ButtonExtra b = c.GetComponent<ButtonExtra>();
        b.value = 0;
        c.GetComponent<Button>().onClick.AddListener(ExitBattle);
        c.name = "Continue";
        c.GetComponentInChildren<TextMeshProUGUI>().text = "Proceed";

        choices.Add(c);
    }
    int NumTargets(int team)
    {
        List<int> targets = TargetsIndex(team);
        return targets.Count;
    }

    public void SetAction(int index)
    {
        action = (ActionType)index;
    }
    public void SetAction(ActionType index)
    {
        Debug.Log(index.ToString());
        action = index;
    }
    public void SetInt(int index)
    {
        //Debug.Log(index);
    }
    [ContextMenu("MakeActionOptions")]
    public void MakeActionOptions()
    {
        choices.Clear();
        DestroyAllChildren(choiceParent);
        for (int i = 1; i <= 3; i++)
        {
            GameObject c = Instantiate(choicePrefab, choiceParent);
            ButtonExtra b = c.GetComponent<ButtonExtra>();
            b.value = i;
            c.GetComponent<Button>().onClick.AddListener(b.SetAction);
            c.GetComponent<Button>().onClick.AddListener(Refresh);
            c.name = "Choice " + (ActionType)i;
            c.GetComponentInChildren<TextMeshProUGUI>().text = ((ActionType)i).ToString();

            choices.Add(c);
        }
    }

    void DestroyAllChildren(Transform t)
    {
        for(int i=0;i<t.childCount;i++)
        {
            Destroy(t.GetChild(i).gameObject);
        }
    }

    void MakeCharacterVisuals()
    {
        for (int i = 0; i < TeamPanel.Count; i++)
        {
            DestroyAllChildren(TeamPanel[i].transform);
        }
        for (int i = 0; i < turnOrder.Count; i++)
        {
            GameObject c = Instantiate(characterVisualPrefab, TeamPanel[turnOrder[i].team].transform);
            c.GetComponent<CharacterVisual>().stats = turnOrder[i].myStats;
            CharacterVisual v = c.GetComponent<CharacterVisual>();
            turnOrder[i].OnHit.AddListener(v.OnHit);
            
        }
    }
    bool IsAllDead(int team)
    {
        bool allDead = true;
        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnOrder[i].team == team)
            {
                if (turnOrder[i].HP > 0)
                {
                    return false;
                }
            }
        }
        return true;
    }


    IEnumerator Wait(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
}


[System.Serializable]
public class BattleCharacter
{
    public string Name;
    public CharacterStats myStats;

    public bool userControlled = false;
    public int hp => myStats.hp;
    public int damage => myStats.baseDMG;

    public int team;

    public bool IsDead
    {
        get
        {
            return hp == 0;
        }
    }

    public int HP
    {
        get => myStats.hp;
        set
        {
            if(value < myStats.hp)
            {
                //OnDamage.Invoke;
                OnHit.Invoke();
            }
            if(value > myStats.hp)
            {
                //OnHeal.Invoke();
            }
            if(value <= 0)
            {
                myStats.hp = 0;
            }
            else if(value > myStats.maxHp)
            {
                myStats.hp = myStats.maxHp;
            }
            else
            {
                myStats.hp = value;
            }
            //hp = value;
        }
    }

    public UnityEvent OnHit = new UnityEvent();

    public BattleCharacter(CharacterStats stats, bool user, int team)
    {
        myStats = stats;
        userControlled = user;
        this.team = team;

        //hp = stats.hp;
        //damage = stats.baseDMG;
        Name = stats.name;

        OnHit = new UnityEvent();
    }

    public int Damage()
    {
        int baseDMG = damage;
        int variance = (int)(damage * 0.5f);
        return Random.Range(0, variance) + baseDMG;
    }
}