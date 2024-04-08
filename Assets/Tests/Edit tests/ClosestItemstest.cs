using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ClosestItemstest
{
    [Test]
    public void ClosestBoxTest()
    {
        PushableObjectManager.boxes.Clear();

        GameObject box1 = GameObject.Instantiate((GameObject)Resources.Load("Box"), new Vector2(0.6f, 0), Quaternion.identity);
        GameObject box2 = GameObject.Instantiate((GameObject)Resources.Load("Box"), new Vector2(1f, 1f), Quaternion.identity);
        GameObject box3 = GameObject.Instantiate((GameObject)Resources.Load("Box"), new Vector2(-0.6f, 1f), Quaternion.identity);

        PushableObjectManager.RegisterBox(box1);
        PushableObjectManager.RegisterBox(box2);
        PushableObjectManager.RegisterBox(box3);
        float dist;
        PushableObjectManager.GetClosestBox(Vector2.zero, out dist);

        PushableObjectManager.boxes.Clear();

        Assert.That(dist, Is.EqualTo(0.6f));
    }

    [Test]
    public void ClosestPickableTest()
    {
        PushableObjectManager.pickableObjs.Clear();

        GameObject pickable1 = GameObject.Instantiate((GameObject)Resources.Load("Pickable"), new Vector2(0.6f, 0), Quaternion.identity);
        GameObject pickable2 = GameObject.Instantiate((GameObject)Resources.Load("Pickable"), new Vector2(1f, 1f), Quaternion.identity);
        GameObject pickable3 = GameObject.Instantiate((GameObject)Resources.Load("Pickable"), new Vector2(-0.6f, 1f), Quaternion.identity);

        PushableObjectManager.RegisterPickable(pickable1);
        PushableObjectManager.RegisterPickable(pickable2);
        PushableObjectManager.RegisterPickable(pickable3);
        float dist;
        PushableObjectManager.GetClosestPickable(Vector2.zero, out dist);

        PushableObjectManager.pickableObjs.Clear();

        Assert.That(dist, Is.EqualTo(0.6f));
    }

    [Test]
    public void ClosestSwitchTest()
    {
        PushableObjectManager.switches.Clear();

        GameObject switch1 = GameObject.Instantiate((GameObject)Resources.Load("Switch"), new Vector2(0.6f, 0), Quaternion.identity);
        GameObject switch3 = GameObject.Instantiate((GameObject)Resources.Load("Switch"), new Vector2(1f, 1f), Quaternion.identity);
        GameObject switch2 = GameObject.Instantiate((GameObject)Resources.Load("Switch"), new Vector2(-0.6f, 1f), Quaternion.identity);

        PushableObjectManager.RegisterSwitch(switch1);
        PushableObjectManager.RegisterSwitch(switch2);
        PushableObjectManager.RegisterSwitch(switch3);
        float dist;
        PushableObjectManager.GetClosestSwitch(Vector2.zero, out dist);

        PushableObjectManager.switches.Clear();

        Assert.That(dist, Is.EqualTo(0.6f));
    }
}
