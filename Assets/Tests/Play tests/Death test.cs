using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;


public class Deathtest : InputTestFixture
{
    private Keyboard keyboard;

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/TestingScene");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    [UnityTest]
    public IEnumerator DeathWhenPressRTest()
    {
        yield return new WaitForSeconds(2f);

        PressAndRelease(keyboard.rKey);

        yield return new WaitForSeconds(0.2f);

        Assert.That(GameManager.inst.dead, Is.True);

        yield return null;
    }

    [UnityTest]
    public IEnumerator DeathWhenEnemyHitTest()
    {
        yield return new WaitForSeconds(2f);

        var enemy = Object.Instantiate((GameObject)Resources.Load("Enemy"), new Vector2(0, 1), Quaternion.identity);
        enemy.GetComponent<Enemy>().patrolPositions[0] = Vector2.up;
        enemy.GetComponent<Enemy>().patrolPositions[1] = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        Assert.That(GameManager.inst.dead, Is.True);

        yield return null;
    }

    [UnityTest]
    public IEnumerator DeathWhenSpikesHitTest()
    {
        var spike = Object.Instantiate((GameObject)Resources.Load("Spikes"), new Vector2(0, 0), Quaternion.identity);

        yield return null;

        spike.GetComponent<Spikes>().OnStateOn();

        yield return new WaitForSeconds(0.5f);

        Assert.That(GameManager.inst.dead, Is.True);

        yield return null;
    }

    [UnityTest]
    public IEnumerator DeathWhenArrowsHitTest()
    {
        var spike = Object.Instantiate((GameObject)Resources.Load("DartShooterBox"), new Vector2(-1, 0),
            Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        Assert.That(GameManager.inst.dead, Is.True);

        yield return null;
    }
}