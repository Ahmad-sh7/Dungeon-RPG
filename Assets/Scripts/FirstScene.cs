using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FirstScene : MonoBehaviour
{
    // "You attack. ´The enemy takes 2 damage."
    // "Enemy attacks. You take 2 damage."
    // "You defend."
    // "The enemy attacks. You defended and don't take damage."
    // "You heal yourself for 5 HP."
    // "The enemy is charging a deadly attack."


    int playerHP, enemyHP;
    bool playerTurn, playerDefendFlag;
    bool chargeSuperAttack = false;
    [SerializeField] TextMeshProUGUI infoTxt, hpTxt;
    [SerializeField] Image healBar;
    [SerializeField] Button attackBtn, defendBtn, healBtn, RunBtn;
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
        

        string message = string.Format("You attack. The enemy takes {0} damage.", playerAttackValue);

        UpdateInfoText(message);
        infoObject.gameObject.SetActive(true);
        if(enemyHP <= 0)
        {
            CheckWinner();
            return;
        }
        
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
        SceneManager.LoadScene("Game");
    }

    private void EnemyAttack()
    {
        if (chargeSuperAttack)
        {
            chargeSuperAttack= false;
            playerHP = playerDefendFlag ? playerHP : 0;
            string text = playerDefendFlag ? "You defend the deadly Attack." : "You couldn't defend the deadly Attack.";
            UpdateInfoText(text);
            anim.SetTrigger("super");


        }
        else
        {
            float choice = Random.Range(0f, 100f);
            if (choice < 65)
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


            }
            else
            {
                chargeSuperAttack= true;
                string text = "The AI is charging, next round the attack will be deadly";
                UpdateInfoText(text);
            }
        }
        playerDefendFlag= false;
        UpdateHealBar();
        if (playerHP <= 0 || enemyHP <= 0)
        {

            CheckWinner();
            return;
        }
        StartCoroutine(PerformUpdateInfoTextCoroutine());
    }

    private void GameSetup()
    {
        playerHP = 10;
        enemyHP = 2;
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
        RunBtn.interactable = false;
    }

    private void ActivateButtons()
    {
        attackBtn.interactable = true;
        defendBtn.interactable = true;
        healBtn.interactable = true;
        RunBtn.interactable = true;
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


    private void CheckWinner()
    {
        if (enemyHP <= 0)
        {
            // Player Won
            UpdateInfoText("You Won!");
            SpriteRenderer[] spriteRenderers = enemy.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.enabled = false;
            }
            Destroy(enemy);
            DeactivateButtons();
            StartCoroutine(PlayEnded());
        }
        if(playerHP<= 0)
        {
            UpdateInfoText("You Lost!");
            DeactivateButtons();
            StartCoroutine(PlayEnded());
        }
        
    }
    private IEnumerator PlayEnded()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Game");
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
