using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;


public class Movetest : InputTestFixture
{
    Keyboard keyboard;
    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/TestingScene");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    [UnityTest]
    public IEnumerator MoveForwardTest()
    {
        yield return new WaitForSeconds(0.1f);

        PlayerMovement.currPlayer.transform.position = Vector2.zero;

        Vector2 pos = PlayerMovement.currPlayer.transform.position;
        Press(keyboard.wKey, 0.2f);

        yield return new WaitForSeconds(0.3f);

        Assert.That(PlayerMovement.currPlayer.transform.position.y, Is.GreaterThan(pos.y));

        yield return null;
    }

    [UnityTest]
    public IEnumerator MoveBackTest()
    {
        yield return new WaitForSeconds(0.1f);

        PlayerMovement.currPlayer.transform.position = Vector2.zero;

        Vector2 pos = PlayerMovement.currPlayer.transform.position;
        Press(keyboard.sKey, 0.2f);

        yield return new WaitForSeconds(0.3f);

        Assert.That(PlayerMovement.currPlayer.transform.position.y, Is.LessThan(pos.y));

        yield return null;
    }

    [UnityTest]
    public IEnumerator MoveRightTest()
    {
        yield return new WaitForSeconds(0.1f);

        PlayerMovement.currPlayer.transform.position = Vector2.zero;

        Vector2 pos = PlayerMovement.currPlayer.transform.position;
        Press(keyboard.dKey, 0.2f);

        yield return new WaitForSeconds(0.3f);

        Assert.That(PlayerMovement.currPlayer.transform.position.x, Is.GreaterThan(pos.x));

        yield return null;
    }

    [UnityTest]
    public IEnumerator MoveLeftTest()
    {
        yield return new WaitForSeconds(0.1f);

        PlayerMovement.currPlayer.transform.position = Vector2.zero;

        Vector2 pos = PlayerMovement.currPlayer.transform.position;
        Press(keyboard.aKey, 0.2f);

        yield return new WaitForSeconds(0.3f);

        Assert.That(PlayerMovement.currPlayer.transform.position.x, Is.LessThan(pos.x));

        yield return null;
    }
}