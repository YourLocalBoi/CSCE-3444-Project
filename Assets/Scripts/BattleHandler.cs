using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Collections;
public enum BATTLE_STATES
{
    START,
    PLAYER_TURN,
    ENEMY_TURN,
    WON,
    LOST
}

public class BattleHandler : MonoBehaviour
{

    Unit playerUnit;
    Unit enemyUnit;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform battleStation;
    public BATTLE_STATES states;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    private void Start()
    {
        states = BATTLE_STATES.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        //  Enemies[] enemy; implement list if going to have multiple enemies in the fight scene; can use for each loop to iterate; maybe have turn order depending on speed, init. etc.?

        float playerPosX = -1; // implement player battle station, can probably be set position since not going to have multiple characters
        float playerPosY = -1;

        float enemyPosX = battleStation.position.x; // can also implement list of battle stations to find positions for each battlestation and enemy
        float enemyPosY = battleStation.position.y;


        GameObject playerGO = Instantiate(playerPrefab, new Vector2(playerPosX, playerPosY), Quaternion.identity);
        playerUnit = playerGO.GetComponent<Unit>();

        playerHUD.SetHUD(playerUnit);

        GameObject enemyGO = Instantiate(enemyPrefab, new Vector2(enemyPosX, enemyPosY), Quaternion.identity);
        enemyUnit = enemyGO.GetComponent<Unit>();

        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        PlayerTurn();
    }

    void PlayerTurn()
    {
        states = BATTLE_STATES.PLAYER_TURN;

        Debug.Log("Player Turn");
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHealth(enemyUnit.currHealth);

        states = (isDead) ? states = BATTLE_STATES.WON : states = BATTLE_STATES.ENEMY_TURN;

        yield return new WaitForSeconds(2f);

        if (states == BATTLE_STATES.ENEMY_TURN)
        {
            StartCoroutine(EnemyTurn());
        }
        else
        {
            BattleEnd();
        }

    }

    IEnumerator PlayerDefense()
    {
        playerUnit.isDefending = true;

        states = BATTLE_STATES.ENEMY_TURN;

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButtonClick()
    {
        if (states != BATTLE_STATES.PLAYER_TURN)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }

    public void OnDefenseButtonClick()
    {
        if (states != BATTLE_STATES.PLAYER_TURN)
        {
            return;
        }

        StartCoroutine(PlayerDefense());
    }

    IEnumerator EnemyTurn()
    {
        bool playerDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHealth(playerUnit.currHealth);

        yield return new WaitForSeconds(2f);

        if (playerDead)
        {
            states = BATTLE_STATES.LOST;
            BattleEnd();
        }
        else
        {
            states = BATTLE_STATES.PLAYER_TURN;
            PlayerTurn();
        }
    }
    public void BattleEnd()
    {
        if (states == BATTLE_STATES.WON)
        {
            // implement whatever the player will gain; experience, gold, etc.
        }
        else
        {
            // implement whatever happens when the player loses; back to last save, restart the fight, choice of these two?
        }
    }
}
