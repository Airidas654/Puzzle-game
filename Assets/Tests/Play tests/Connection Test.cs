using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;


public class ConnectorsTest : InputTestFixture
{
    Keyboard keyboard;
    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/TestingScene");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    [UnityTest]
    public IEnumerator SpikesConnectionTest()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject plate = GameObject.Instantiate((GameObject)Resources.Load("PressurePlatePrefab"), new Vector3(0, 1, 0), Quaternion.identity);

        GameObject spikes = GameObject.Instantiate((GameObject)Resources.Load("Spikes"), new Vector3(3, 1, 0), Quaternion.identity);
        
        plate.GetComponent<PressurePlate>().receivers.Clear();
        plate.GetComponent<PressurePlate>().receivers.Add(spikes.GetComponent<Spikes>());
        spikes.GetComponent<Spikes>().OnValidate();

        plate.GetComponent<PressurePlate>().colliderMask = 64;

        PlayerMovement.currPlayer.transform.position = Vector2.zero;

        Vector2 pos = PlayerMovement.currPlayer.transform.position;
        Press(keyboard.wKey, 0.2f);


        yield return new WaitForSeconds(0.3f);

        Assert.That(spikes.GetComponent<Spikes>().state, Is.True);

        yield return null;
    }

    [UnityTest]
    public IEnumerator SwitchConnectionTest()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject sw = GameObject.Instantiate((GameObject)Resources.Load("Switch"), new Vector3(0, 1, 0), Quaternion.identity);

        GameObject spikes = GameObject.Instantiate((GameObject)Resources.Load("Spikes"), new Vector3(3, 1, 0), Quaternion.identity);

        sw.GetComponent<Switch>().receivers.Clear();
        sw.GetComponent<Switch>().receivers.Add(spikes.GetComponent<Spikes>());
        spikes.GetComponent<Spikes>().OnValidate();

        PlayerMovement.currPlayer.transform.position = Vector2.zero;

        PressAndRelease(keyboard.eKey);


        yield return new WaitForSeconds(0.3f);

        Assert.That(spikes.GetComponent<Spikes>().state, Is.True);

        yield return null;
    }

}