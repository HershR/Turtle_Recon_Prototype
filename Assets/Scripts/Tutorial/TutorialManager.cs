using AYellowpaper.SerializedCollections;
using System.Collections;
using UnityEngine;

public class TutorialManager : GameManager
{
    [SerializeField] PlayerHUDController playerHUD;
    [Header("Debug")]
    public string TutorialState = "Not Started";
    
    [SerializeField] private DialogueManager dialogueManager;

    private void Start()
    {
        droneSpawner.gameObject.SetActive(false);
        obstacleSpawner.gameObject.SetActive(false);
        generator.OverrideEnvironment(EnvironmentType.Normal);
        playerHUD.ToggleHealthUI(false);
        playerHUD.ToggleProgressUI(false);
        playerHUD.ToggleTokenUI(false);
        player.health = 1000;
        StartCoroutine(TutorialSequence());
    }
    private void Update()
    {
        if (isGameOver)
        {
            return;
        }
        distance += generator.GetSpeed() * Time.deltaTime;
    }

    public IEnumerator TutorialSequence()
    {
        yield return StartCoroutine(PlayerMove());
        yield return StartCoroutine(PlayerCollectCoin());
        yield return StartCoroutine(PlayerDepositCoin());
        yield return StartCoroutine(PlayerParry());
        yield return StartCoroutine(KillPlayer());
        SceneTransitionManager.instance.LoadStore();

    }

    private IEnumerator PlayerMove()
    {
        TutorialState = "wait for Player Move";
        var playerStartPos = player.transform.position;
        //Trigger dialogue
        Debug.Log("Use WASD to move");
        dialogueManager.ShowDialogue("Use WASD to move");
        //wait for player to move
        while (player.transform.position == playerStartPos)
        {
            yield return null;
        }
    }

    private IEnumerator PlayerCollectCoin()
    {
        TutorialState = "wait to Collect 5 Coin";
        obstacleSpawner.gameObject.SetActive(true);
        playerHUD.ToggleTokenUI(true);
        //Trigger dialogue
        Debug.Log("Collect Research tokens, the more the better");
        dialogueManager.ShowDialogue("Collect Research tokens, the more the better");
        //collect 5 coins
        while (TokensCollected < 5)
        {
            yield return null;
        }
    }
    private IEnumerator PlayerDepositCoin()
    {
        TutorialState = "wait to Depo 5 Coin";
        //Trigger dialogue
        Debug.Log("Here is the Research Collection Drone!\n" +
            "Stay in its range 1 second to deposit your research.\n" +
            "But do be quick, the drone won't stay forever.");
        dialogueManager.ShowDialogue("Here is the Research Collection Drone!\n" +
            "Stay in its range 1 second to deposit your research.\n" +
            "But do be quick, the drone won't stay forever.");
        droneSpawner.SpawnDrone(30f);
        //wait to deposit coins
        while (TokensDeposited < 5 && droneSpawner.isDroneActive)
        {
            yield return null;
        }
        droneSpawner.RecallDone();
    }
    private IEnumerator PlayerParry()
    {
        generator.OverrideEnvironment(EnvironmentType.TrashField);
        TutorialState = "Try Parry";
        //Trigger dialogue
        Debug.Log("Watch out for the incoming trash!" +
            "You can press the 'Space Bar' to parry trash." +
            "However, if you miss time it, you will take damage, and your parry will go on cool down." +
            "Your parry cool down resets if you do a successful parry");
        dialogueManager.ShowDialogue("Watch out for the incoming trash!" +
            "You can press the 'Space Bar' to parry trash." +
            "However, if you miss time it, you will take damage, and your parry will go on cool down." +
            "Your parry cool down resets if you do a successful parry");
        //wait to trigger parry key
        while (player.parrySucceed == false)
        {
            yield return null;
        }
    }
    private IEnumerator KillPlayer()
    {
        TutorialState = "Done kill player";
        //trigger dialogue
        player.health = player.maxHealth;
        playerHUD.ToggleHealthUI(true);
        gameTimeDelta = 300f;
        //spawn lot of trash or giant trash?
        while (player.health > 0)
        {
            yield return null;
        }
        OnGameOver();
        StartCoroutine(RemovePlayer());
        //trigger dialogue
        //go to shop
    }
}
