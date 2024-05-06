using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;


public class SpikeColliderTest : InputTestFixture
{
    private Keyboard keyboard;

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/TestingScene");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    [UnityTest]
    public IEnumerator SpikesCollisionTest()
    {
        yield return new WaitForSeconds(0.1f);


        var spikes1 = Object.Instantiate((GameObject)Resources.Load("Spikes"), new Vector3(1, 0.2f, 0),
            Quaternion.identity);
        var spikes2 = Object.Instantiate((GameObject)Resources.Load("Spikes"), new Vector3(3, 0.2f, 0),
            Quaternion.identity);

        yield return null;

        spikes2.GetComponent<Spikes>().OnStateOn();

        yield return new WaitForSeconds(0.1f);
        PlayerMovement.currPlayer.transform.position = Vector2.zero;

        Vector2 pos = PlayerMovement.currPlayer.transform.position;
        Press(keyboard.dKey, 0.3f);


        yield return new WaitForSeconds(2);

        Assert.That(PlayerMovement.currPlayer.transform.position.x,
            Is.GreaterThan(spikes1.transform.position.x).And.LessThan(spikes2.transform.position.x));

        yield return null;
    }
}