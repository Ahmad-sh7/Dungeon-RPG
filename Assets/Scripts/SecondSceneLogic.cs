using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SecondSceneLogic : MonoBehaviour
{
    int playerHP, enemyHP;
    bool playerTurn, playerDefendFlag, enemySuperAttack, gameOver;
    [SerializeField] TextMeshProUGUI infoTxt, hpTxt;
    [SerializeField] Image healBar;
    [SerializeField] Button attackBtn, defendBtn, healBtn, runBtn;
    [SerializeField] GameObject enemy, infoObject;
    Animator anim;
    AudioScript audioScript;

    void Start()
    {
        audioScript = GameObject.Find("Audio Object").GetComponent<AudioScript>();
        // StartCoroutine(PerformRotationCoroutine());
        GameSetup();
    }

    void Update()
    {
        if (gameOver)
        {
            DeactivateButtons();
        }

        if (!playerTurn && !gameOver) // Enemy's Turn
        {
            playerTurn = true;
            float randomNumber = Random.Range(0f, 1f);

            if (enemySuperAttack) // Start Enemy Super Attack
                StartCoroutine(PerformSuperAttackCoroutine());
            else if (randomNumber <= 0.8f && !enemySuperAttack) // Normal Attack
                StartCoroutine(PerformEnemyAttackCoroutine());
            else // Charge Enemy Super Attack
                StartCoroutine(PerformChargeSuperAttackCoroutine());
            
        }
    }

    public void PlayerAttack()
    {
        playerDefendFlag = false;
        playerTurn = false;
        DeactivateButtons();

        anim.SetTrigger("defend");

        int playerAttackValue = RandomNumberGenerator(4); // Generate random number between 1 and 4
        enemyHP -= playerAttackValue;
        
        string message = string.Format("You attack. The enemy takes {0} damage.", playerAttackValue);
        
        if (!gameOver)
            UpdateInfoText(message);

        CheckEnemyHP();
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
        playerDefendFlag = false;
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
        LeaveScene();
    }

    private void EnemyAttack()
    {
        int enemyAttackValue = RandomNumberGenerator(4); // Generate random number between 1 and 4
        string message = !playerDefendFlag ? string.Format("Enemy attacks. You take {0} damage.", enemyAttackValue) : "The enemy attacks. You defended and don't take damage.";
        UpdateInfoText(message);

        if (!playerDefendFlag)
        {
            audioScript.EnemyAttackSFX();
            playerHP = (playerHP - enemyAttackValue) < 0 ? 0 : playerHP - enemyAttackValue;
        }
        
        playerDefendFlag = false;

        anim.SetTrigger("attack");

        CheckPlayerHP();
        if (!gameOver)
            StartCoroutine(PerformUpdateInfoTextCoroutine());
        UpdateHealBar();   
    }

    private void EnemySuperAttack()
    {
        int enemyAttackValue = 7;
        string message = !playerDefendFlag ? string.Format("Enemy attacks. You take {0} damage.", enemyAttackValue) : "The enemy attacks. You defended and don't take damage.";
        UpdateInfoText(message);

        if (!playerDefendFlag)
        {
            audioScript.EnemyAttackSFX();
            playerHP = (playerHP - enemyAttackValue) < 0 ? 0 : playerHP - enemyAttackValue;
        }

        anim.SetTrigger("super");

        CheckPlayerHP();
        if (!gameOver)
            StartCoroutine(PerformUpdateInfoTextCoroutine());
        UpdateHealBar();

        playerDefendFlag = false;
        enemySuperAttack = false;
    }

    private void GameSetup()
    {
        playerHP = 10;
        enemyHP = 10;
        UpdateHealBar();
        playerTurn = true;
        enemySuperAttack = false;
        playerDefendFlag = false;
        gameOver = false;
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
        float delay = 2f;
        if (enemySuperAttack)
            delay = 4f;
        yield return new WaitForSeconds(delay);
        infoObject.gameObject.SetActive(false);
        ActivateButtons();
    }


    private void DeactivateButtons()
    {
        attackBtn.interactable = false;
        defendBtn.interactable = false;
        healBtn.interactable = false;
        runBtn.interactable = false;
    }

    private void ActivateButtons()
    {
        attackBtn.interactable = true;
        defendBtn.interactable = true;
        healBtn.interactable = true;
        runBtn.interactable = true;
    }

    private IEnumerator PerformEnemyAttackCoroutine()
    {
        yield return new WaitForSeconds(2f);
        EnemyAttack();
    }

    private IEnumerator PerformChargeSuperAttackCoroutine()
    {
        yield return new WaitForSeconds(2f);
        ChargeSuperAttack();
    }

    private void ChargeSuperAttack()
    {
        string message = "The enemy is charging a deadly attack.";
        UpdateInfoText(message);
        StartCoroutine(PerformUpdateInfoTextCoroutine());
        enemySuperAttack = true;
    }

    private IEnumerator PerformSuperAttackCoroutine()
    {
        yield return new WaitForSeconds(2f);
        EnemySuperAttack();
    }


    private void CheckEnemyHP()
    {
        if (enemyHP <= 0)
        {
            // Player Won
            gameOver = true;
            GameOverLogic();
        }
    }

    private void CheckPlayerHP()
    {
        if (playerHP <= 0)
        {
            // Enemy Won
            gameOver = true;
            GameOverLogic();
        }
    }

    private IEnumerator PerformRotationCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            anim.SetTrigger("rotate");
        }
    }

    private void GameOverLogic()
    {
        string message = playerHP <= 0 ? "You Lost." : "You Won.";
        UpdateInfoText(message);
        infoObject.gameObject.SetActive(true);
        Invoke("LeaveScene", 4f);
    }

    private void LeaveScene()
    {
        PlayerPrefs.SetInt("LoadedFromAnotherScene", 1);
        SceneManager.LoadScene("Game");
    }
}
