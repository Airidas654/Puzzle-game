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
    Keyboard keyboard;
    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/TestingScene");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    [UnityTest]
    public IEnumerator SpikesCollision()
    {
        yield return new WaitForSeconds(0.1f);
        

       // GameObject spikes1 = GameObject.Instantiate((GameObject)Resources.Load("Spikes"), new Vector3 (1 ,0.2f, 0), Quaternion.identity);
        GameObject spikes2 = GameObject.Instantiate((GameObject)Resources.Load("Spikes"), new Vector3 (2 ,0.2f, 0), Quaternion.identity);


        spikes2.GetComponent<Spikes>().OnStateOn();

        yield return new WaitForSeconds(0.1f);
        PlayerMovement.currPlayer.transform.position = Vector2.zero;

        Vector2 pos = PlayerMovement.currPlayer.transform.position;
        Press(keyboard.aKey, 0.3f);


        yield return new WaitForSeconds(2);

       // Assert.That(spikes.GetComponent<Spikes>().state, Is.True);

        yield return null;
    }

    
}