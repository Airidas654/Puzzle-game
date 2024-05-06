using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using NUnit.Framework.Internal;
using UnityEngine.SceneManagement;


public class PressurePlateTest : InputTestFixture
{
    private Keyboard keyboard;

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/TestingScene");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }

    [UnityTest]
    public IEnumerator PressurePlatePressedByAPlayerTest()
    {
        yield return new WaitForSeconds(0.1f);
        var plate = Object.Instantiate((GameObject)Resources.Load("PressurePlatePrefab"), new Vector3(0, 1, 0),
            Quaternion.identity);
        plate.GetComponent<PressurePlate>().colliderMask = 64;

        PlayerMovement.currPlayer.transform.position = Vector2.zero;

        Vector2 pos = PlayerMovement.currPlayer.transform.position;
        Press(keyboard.wKey, 0.2f);


        yield return new WaitForSeconds(0.3f);

        Assert.That(plate.GetComponent<SpriteRenderer>().sprite,
            Is.EqualTo(plate.GetComponent<PressurePlate>().platePressed));

        yield return null;
    }
}