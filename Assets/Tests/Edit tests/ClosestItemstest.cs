using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ClosestItemstest
{
    [Test]
    [TestCase(0, 0, 0.6f)]
    [TestCase(1, 0, 0.4f)]
    [TestCase(0, 1, 0.6f)]
    public void ClosestBoxTest(float x, float y, float ans)
    {
        PushableObjectManager.boxes.Clear();

        var box1 = Object.Instantiate((GameObject)Resources.Load("Box"), new Vector2(0.6f, 0), Quaternion.identity);
        var box2 = Object.Instantiate((GameObject)Resources.Load("Box"), new Vector2(1f, 1f), Quaternion.identity);
        var box3 = Object.Instantiate((GameObject)Resources.Load("Box"), new Vector2(-0.6f, 1f), Quaternion.identity);

        PushableObjectManager.RegisterBox(box1);
        PushableObjectManager.RegisterBox(box2);
        PushableObjectManager.RegisterBox(box3);
        float dist;
        PushableObjectManager.GetClosestBox(new Vector2(x, y), out dist);

        PushableObjectManager.boxes.Clear();
        Assert.IsTrue(Mathf.Approximately(dist, ans));
        // Assert.That(dist, Is.EqualTo(ans));
    }

    [Test]
    [TestCase(0, 0, 0.6f)]
    [TestCase(1, 0, 0.4f)]
    [TestCase(0, 1, 0.6f)]
    public void ClosestPickableTest(float x, float y, float ans)
    {
        PushableObjectManager.pickableObjs.Clear();

        var pickable1 = Object.Instantiate((GameObject)Resources.Load("Pickable"), new Vector2(0.6f, 0),
            Quaternion.identity);
        var pickable2 = Object.Instantiate((GameObject)Resources.Load("Pickable"), new Vector2(1f, 1f),
            Quaternion.identity);
        var pickable3 = Object.Instantiate((GameObject)Resources.Load("Pickable"), new Vector2(-0.6f, 1f),
            Quaternion.identity);

        PushableObjectManager.RegisterPickable(pickable1);
        PushableObjectManager.RegisterPickable(pickable2);
        PushableObjectManager.RegisterPickable(pickable3);
        float dist;
        PushableObjectManager.GetClosestPickable(new Vector2(x, y), out dist);

        PushableObjectManager.pickableObjs.Clear();

        Assert.IsTrue(Mathf.Approximately(dist, ans));
        //Assert.That(dist, Is.EqualTo(0.6f));
    }

    [Test]
    [TestCase(0, 0, 0.6f)]
    [TestCase(1, 0, 0.4f)]
    [TestCase(0, 1, 0.6f)]
    public void ClosestSwitchTest(float x, float y, float ans)
    {
        PushableObjectManager.switches.Clear();

        var switch1 = Object.Instantiate((GameObject)Resources.Load("Switch"), new Vector2(0.6f, 0),
            Quaternion.identity);
        var switch3 =
            Object.Instantiate((GameObject)Resources.Load("Switch"), new Vector2(1f, 1f), Quaternion.identity);
        var switch2 = Object.Instantiate((GameObject)Resources.Load("Switch"), new Vector2(-0.6f, 1f),
            Quaternion.identity);

        PushableObjectManager.RegisterSwitch(switch1);
        PushableObjectManager.RegisterSwitch(switch2);
        PushableObjectManager.RegisterSwitch(switch3);
        float dist;
        PushableObjectManager.GetClosestSwitch(new Vector2(x, y), out dist);

        PushableObjectManager.switches.Clear();

        Assert.IsTrue(Mathf.Approximately(dist, ans));
        // Assert.That(dist, Is.EqualTo(0.6f));
    }
}