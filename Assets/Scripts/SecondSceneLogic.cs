using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class SecondSceneLogic : MonoBehaviour
{
    // "You attack. ´The enemy takes 2 damage."
    // "Enemy attacks. You take 2 damage."
    // "You defend."
    // "The enemy attacks. You defended and don't take damage."
    // "You heal yourself for 5 HP."
    // "The enemy is charging a deadly attack."


    int playerHP, enemyHP;
    bool playerTurn, playerDefendFlag;
    [SerializeField] TextMeshProUGUI infoTxt, hpTxt;
    [SerializeField] Image healBar;
    [SerializeField] Button attackBtn, defendBtn, healBtn;
    [SerializeField] GameObject enemy, infoObject;
    Animator anim;
    AudioScript audioScript;

    void Start()
    {
        audioScript = GameObject.Find("Audio Object").GetComponent<AudioScript>();
        GameSetup();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerTurn) // Enemy's Turn
        {
            // Play Enemy Logic
            // Player Turn set to True
            // playerTurn = true;
            // StartCoroutine(PerformDelayCoroutine(2f));
            // playerTurn = true;
            // DeactivateButtons();
            playerTurn = true;
            // StartCoroutine(PerformSwitchTurnCoroutine(4f));
            StartCoroutine(PerformEnemyAttackCoroutine());
            // EnemyAttack();     
        }
        else
        {
            // Player Turn Logic
        }

    }

    public void PlayerAttack()
    {
        playerTurn = false;
        DeactivateButtons();

        anim.SetTrigger("defend");

        int playerAttackValue = RandomNumberGenerator(4); // Generate random number between 1 and 4
        enemyHP -= playerAttackValue;
        CheckEnemyHP();

        string message = string.Format("You attack. The enemy takes {0} damage.", playerAttackValue);
        UpdateInfoText(message);
        infoObject.gameObject.SetActive(true);
        audioScript.PlayerAttackSFX();

    }

    public void PlayerDefend()
    {
        playerTurn = false;
        playerDefendFlag = true;

        DeactivateButtons();

        string message = "You defend.";
        UpdateInfoText(message);
        infoObject.gameObject.SetActive(true);
        audioScript.PlayerDefendSFX();
    }

    public void PlayerHeal()
    {
        playerTurn = false;
        DeactivateButtons();

        playerHP = (playerHP + 5) < 10 ? playerHP + 5 : 10;
        UpdateHealBar();

        string message = "You heal yourself for 5 HP.";
        UpdateInfoText(message);
        infoObject.gameObject.SetActive(true);
        audioScript.PlayerHealSFX();
    }

    public void PlayerRun()
    {
        Debug.Log("Player Run");
    }

    private void EnemyAttack()
    {
        int enemyAttackValue = RandomNumberGenerator(4); // Generate random number between 1 and 4
        string message = !playerDefendFlag ? string.Format("Enemy attacks. You take {0} damage.", enemyAttackValue) : "The enemy attacks. You defended and don't take damage.";
        UpdateInfoText(message);

        audioScript.EnemyAttackSFX();

        if (!playerDefendFlag)
        {
            // audioScript.PlayerDamageSFX();
            playerHP = (playerHP - enemyAttackValue) < 0 ? 0 : playerHP - enemyAttackValue;
        }
        
        playerDefendFlag = false;

        anim.SetTrigger("attack");

        StartCoroutine(PerformUpdateInfoTextCoroutine());
        UpdateHealBar();
    }

    private void GameSetup()
    {
        playerHP = 10;
        enemyHP = 10;
        UpdateHealBar();
        playerTurn = true;
        playerDefendFlag = false;
        infoObject.gameObject.SetActive(false);
        anim = enemy.GetComponent<Animator>();
    }

    private int RandomNumberGenerator(int limit)
    {
        return new System.Random().Next(1, limit);
    }

    private void UpdateHealBar()
    {
        hpTxt.text = playerHP.ToString();
        healBar.fillAmount = playerHP / 10.0f;
    }

    private void UpdateInfoText(string message)
    {
        infoTxt.text = message;
    }

    private IEnumerator PerformUpdateInfoTextCoroutine()
    {
        yield return new WaitForSeconds(2f);
        infoObject.gameObject.SetActive(false);
        ActivateButtons();
    }


    private void DeactivateButtons()
    {
        attackBtn.interactable = false;
        defendBtn.interactable = false;
        healBtn.interactable = false;
    }

    private void ActivateButtons()
    {
        attackBtn.interactable = true;
        defendBtn.interactable = true;
        healBtn.interactable = true;
    }

    private IEnumerator PerformSwitchTurnCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        ActivateButtons();
    }

    private IEnumerator PerformEnemyAttackCoroutine()
    {
        yield return new WaitForSeconds(2f);
        EnemyAttack();
    }


    private void CheckEnemyHP()
    {
        if (enemyHP <= 0)
        {
            // Player Won
        }
    }

    /*
    private IEnumerator PerformRotationCoroutine()
    {
        yield beak;
        while (true)
        {
            // yield return new WaitForSeconds(2f);
            // anim.SetTrigger("attack");

            yield return new WaitForSeconds(rotateDelay);
            anim.SetTrigger("rotate");
        }
    }
    */
}
