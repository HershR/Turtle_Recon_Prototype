using AYellowpaper.SerializedCollections;
using System;
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
        dialogueManager.ShowDialogue(
            "Hello! Welcome to the Ocean Rescue Mission Organization! " +
            "We’re super stoked you decided to take part in this global effort of saving your home. " +
            "We need all the help we can get.\nThat’s where you come in! " +
            "We can strap on your back a gadget that can help us collect data, " +
            "which we need to do research on methods for saving the ocean.\n" +
            "Your first task is to get used to swimming with the gadget on your back. " +
            "To move, use WASD.");
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
        dialogueManager.ShowDialogue("Now that you are more familiar with moving around, " +
            "we can move onto how to collect data.\n" +
            "Data in the form of tokens that you can collect will float along in the ocean. " +
            "In addition, jellyfish that you can eat to regain health will also occasionally appear.\n" +
            "The more data you collect, the more we can invest in groundbreaking research to clean up the ocean. " +
            "For your second task, collect 5 tokens.\n");
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
        dialogueManager.ShowDialogue("Good job! Now that you have collected 5 tokens, " +
            "it’s time to bank them. Here comes a research drone! " +
            "We will periodically send in a drone for you to deposit the tokens.\n" +
            "Stay close to it to transfer the data back to us on the ship. " +
            "Do be quick since the drone won’t stay forever.");
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
        dialogueManager.ShowDialogue(
            "Look out! Here comes some trash! If you get hit, you will lose health. " +
            "In addition, some trash will inflict additional effects on you. Oil barrels will blind you with the black discharge. " +
            "Wires will tangle you and slow you down. Sharp objects will cut you, causing you to lose more health.\n" +
            "Luckily, you can parry away the trash by pressing the ‘Space Bar’. " +
            "If you miss time it, you will take damage and your parry will go on cooldown. " +
            "If you successfully parry, the cooldown will reset instead.Give it a try!");
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
        dialogueManager.ShowDialogue(
            "Great job! Now it’s time to go back to the research vessel whenever you’re ready. " +
            "You can purchase upgrades on the ship that will make your future runs easier. " +
            "Try to dodge as much trash as you can while collecting data tokens and good luck!");
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
