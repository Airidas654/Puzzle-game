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
    Keyboard keyboard;
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

        GameObject enemy = GameObject.Instantiate((GameObject)Resources.Load("Enemy"), new Vector2(0, 1), Quaternion.identity);
        enemy.GetComponent<Enemy>().positions[0] = Vector2.up;
        enemy.GetComponent<Enemy>().positions[1] = Vector2.zero;
        Press(keyboard.wKey, 0.5f);

        yield return new WaitForSeconds(0.5f);

        Assert.That(GameManager.inst.dead, Is.True);

        yield return null;
    }
}