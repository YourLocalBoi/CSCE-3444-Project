using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using System.IO;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices.WindowsRuntime;
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

    public Transform enemyBattleStation;
    public Transform playerBattleStation;

    public SpriteRenderer playerSpritePosition;
    public SpriteRenderer enemySpritePosition;

    public BATTLE_STATES states;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    Animator playerAnimator;
    Animator enemyAnimator;

    Vector2 direction;

    private void Start()
    {
        states = BATTLE_STATES.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        //  Enemies[] enemy; implement list if going to have multiple enemies in the fight scene; can use for each loop to iterate; maybe have turn order depending on speed, init. etc.?

        float playerPosX = playerBattleStation.position.x;
        float playerPosY = playerBattleStation.position.y;

        float enemyPosX = enemyBattleStation.position.x; // can also implement list of battle stations to find positions for each battlestation and enemy
        float enemyPosY = enemyBattleStation.position.y;


        GameObject playerGO = Instantiate(playerPrefab, new Vector2(playerPosX, playerPosY), Quaternion.identity);
        playerUnit = playerGO.GetComponentInChildren<Unit>();
        playerAnimator = playerGO.GetComponentInChildren<Animator>();
        playerSpritePosition = playerGO.GetComponentInChildren<SpriteRenderer>();

        playerHUD.SetHUD(playerUnit);

        GameObject enemyGO = Instantiate(enemyPrefab, new Vector2(enemyPosX, enemyPosY), Quaternion.identity);
        enemyUnit = enemyGO.GetComponentInChildren<Unit>();
        enemyAnimator = enemyGO.GetComponentInChildren<Animator>();
        enemySpritePosition = enemyGO.GetComponentInChildren<SpriteRenderer>();

        direction = (enemySpritePosition.transform.position - playerSpritePosition.transform.position).normalized;


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
        Debug.Log("Player attack trigger");

        // float attackDistance = 0.75f;
        float speed = 3f;

        Vector3 returnPosition = playerSpritePosition.transform.position;

        // Vector3 direction = (enemySpritePosition.transform.position - playerSpritePosition.transform.position).normalized;
        Vector3 targetPosition = enemySpritePosition.transform.position;

        playerAnimator.SetTrigger("Attack");

        while (Vector3.Distance(playerSpritePosition.transform.position, targetPosition) > 0.01f)
        {
            playerSpritePosition.transform.position = Vector3.MoveTowards(
                playerSpritePosition.transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
            yield return null;
        }

        playerAnimator.SetTrigger("AttackDone");

        while (Vector3.Distance(playerSpritePosition.transform.position, returnPosition) > 0.01f)
        {
            playerSpritePosition.transform.position = Vector3.MoveTowards(
                playerSpritePosition.transform.position,
                returnPosition,
                speed * Time.deltaTime
            );
            yield return null;
        }

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHealth(enemyUnit.currHealth);

        states = (isDead) ? BATTLE_STATES.WON : BATTLE_STATES.ENEMY_TURN;

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
