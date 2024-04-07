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
        //SceneManager.LoadScene("Scenes/SimpleTesting");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();

        var mouse = InputSystem.AddDevice<Mouse>();
        Press(mouse.rightButton);
        Release(mouse.rightButton);
    }

    [UnityTest]
    public IEnumerator MovetestWithEnumeratorPasses()
    {
        yield return new WaitForSeconds(1);

        Vector2 pos = PlayerMovement.currPlayer.transform.position;
        Press(keyboard.wKey, 1);

        //Debug.Log(pos);

        yield return new WaitForSeconds(2);

        //Debug.Log(PlayerMovement.currPlayer.transform.position.y);

        Assert.That(PlayerMovement.currPlayer.transform.position.y, Is.GreaterThan(pos.y));

        yield return null;
    }
}