using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;


public class Pausetest : InputTestFixture
{
    private Keyboard keyboard;

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/TestingScene");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    [UnityTest]
    public IEnumerator MovementAfterPauseTest()
    {
        yield return new WaitForSeconds(2f);

        PressAndRelease(keyboard.pKey, 0.1f);

        PlayerMovement.currPlayer.transform.position = Vector2.zero;

        Vector2 pos = PlayerMovement.currPlayer.transform.position;
        PressAndRelease(keyboard.wKey, 0.2f);

        yield return new WaitForSecondsRealtime(1f);

        Assert.That(PlayerMovement.currPlayer.transform.position.y, Is.EqualTo(pos.y));

        yield return null;
    }

    [UnityTest]
    public IEnumerator MovementAfterUnpauseTest()
    {
        yield return new WaitForSeconds(2f);

        PressAndRelease(keyboard.pKey, 0.1f);

        PlayerMovement.currPlayer.transform.position = Vector2.zero;
        Vector2 pos = PlayerMovement.currPlayer.transform.position;

        yield return new WaitForSecondsRealtime(1f);

        PressAndRelease(keyboard.pKey, 0.1f);

        yield return new WaitForSecondsRealtime(1f);

        Press(keyboard.wKey, 0.2f);

        yield return new WaitForSecondsRealtime(0.3f);

        Assert.That(PlayerMovement.currPlayer.transform.position.y, Is.GreaterThan(pos.y));

        yield return null;
    }
}