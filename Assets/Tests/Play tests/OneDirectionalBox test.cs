using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.LowLevel;

public class OneDirectionalBoxtest : InputTestFixture
{
    private Keyboard keyboard;

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/TestingScene");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    [UnityTest]
    public IEnumerator PushOneDirectionalBoxCorrectWayTest()
    {
        yield return new WaitForSeconds(0.1f);

        var box = Object.Instantiate((GameObject)Resources.Load("horizontal box"), new Vector2(0.8f, 0),
            Quaternion.identity);

        Vector2 pos = box.transform.position;

        Press(keyboard.dKey, 0.3f);

        yield return new WaitForSeconds(0.4f);

        Assert.That(box.transform.position.x, Is.GreaterThan(pos.x));

        yield return null;
    }

    [UnityTest]
    public IEnumerator PushOneDirectionalBoxIncorrectWayTest()
    {
        yield return new WaitForSeconds(0.1f);

        var box = Object.Instantiate((GameObject)Resources.Load("horizontal box"), new Vector2(0.8f, 0),
            Quaternion.identity);
        PlayerMovement.currPlayer.transform.position = new Vector2(0.8f, -0.8f);
        Vector2 pos = box.transform.position;

        Press(keyboard.wKey, 0.3f);

        yield return new WaitForSeconds(0.5f);

        Assert.That(pos.y,
            Is.InRange(box.transform.position.y - float.Epsilon, box.transform.position.y + float.Epsilon));

        yield return null;
    }
}