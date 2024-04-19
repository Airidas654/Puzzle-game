using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.LowLevel;

public class Boxtest : InputTestFixture
{
    Keyboard keyboard;
    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/TestingScene");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    [UnityTest]
    public IEnumerator PushBoxTest()
    {

        yield return new WaitForSeconds(0.1f);

        GameObject box = GameObject.Instantiate((GameObject)Resources.Load("Box"), new Vector2(0.8f, 0), Quaternion.identity);

        Vector2 pos = box.transform.position;

        Press(keyboard.dKey, 0.3f);

        yield return new WaitForSeconds(0.4f);

        Assert.That(box.transform.position.x, Is.GreaterThan(pos.x));

        yield return null;
    }

    [UnityTest]
    public IEnumerator GrabBoxTest()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject box = GameObject.Instantiate((GameObject)Resources.Load("Box"), new Vector2(0.6f, 0), Quaternion.identity);

        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E, Key.A), 0.3f);

        Vector2 pos = box.transform.position;

        yield return new WaitForSeconds(0.4f);

        Assert.That(box.transform.position.x, Is.LessThan(pos.x));

        yield return null;
    }

    [UnityTest]
    public IEnumerator PushBoxOnButtonTest()
    {
        yield return new WaitForSeconds(0.1f);

        GameObject box = GameObject.Instantiate((GameObject)Resources.Load("Box"), new Vector2(0.6f, 0), Quaternion.identity);
        GameObject plate = GameObject.Instantiate((GameObject)Resources.Load("PressurePlatePrefab"), new Vector3(1, -1, 0), Quaternion.identity);
        GameObject spikes = GameObject.Instantiate((GameObject)Resources.Load("Spikes"), new Vector3(0, -2f, 0), Quaternion.identity);

        plate.GetComponent<PressurePlate>().receivers.Clear();
        plate.GetComponent<PressurePlate>().receivers.Add(spikes.GetComponent<Spikes>());

        yield return new WaitForSeconds(0.1f);

        spikes.GetComponent<Spikes>().UpdateReceivers();

        plate.GetComponent<PressurePlate>().colliderMask = 128+64;

        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E, Key.S), 0.5f);
        yield return new WaitForSeconds(1f);
        Release(keyboard.eKey);
        Press(keyboard.sKey, 1f);

        yield return new WaitForSeconds(1.5f);

        Assert.That(PlayerMovement.currPlayer.transform.position.y, Is.GreaterThan(-2f));
    }
}