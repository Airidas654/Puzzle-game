using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;


public class NextLeveltest : InputTestFixture
{
    Keyboard keyboard;
    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/TestingScene");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    [UnityTest]
    public IEnumerator DoorToNextLevelTest()
    {
        GameObject door = GameObject.Instantiate((GameObject)Resources.Load("DoorPrefab"), new Vector2(0, 0), Quaternion.identity);
        door.GetComponent<Door>().exactLevel = 0;
        door.GetComponent<Door>().goToIncrimentedLevel = false;

        yield return new WaitForSeconds(1f);
        

        Assert.That(SceneManager.GetActiveScene().buildIndex, Is.EqualTo(0));

        yield return null;
    }

}