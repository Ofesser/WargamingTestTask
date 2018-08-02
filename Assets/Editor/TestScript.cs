using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewTestScript {

    [Test]
    public void CalculateAngleOfArcBtw2Points() {
    
        float angle1 = Utils.CalculateAngleOfArc(2,0);
        float angle2 = Utils.CalculateAngleOfArc(2, 4);

        Assert.AreEqual(angle1, 0);
        Assert.AreEqual(angle2, 180);
    }

    [Test]
    public void CalculateAngVelocity()
    {

        float vel1 = Utils.CalculateAngVelByLinVel(5,5);
        float vel2 = Utils.CalculateAngVelByLinVel(5, 10);

        Assert.AreEqual(vel1, 1f);
        Assert.AreEqual(vel2, 0.5f);
    }

    [Test]
    public void CalculateMiddleValue()
    {

        float val1 = Utils.GetMiddleValue(8, 6);
        float val2 = Utils.GetMiddleValue(5, 10);

        Assert.AreEqual(val1, 7f);
        Assert.AreEqual(val2, 7.5f);
    }

    [Test]
    public void FindReturnTime()
    {

        float val1 = Utils.FindReturnTime(5, 10);
        float val2 = Utils.FindReturnTime(2, 1);

        Assert.AreEqual(val1, Mathf.PI);
        Assert.AreEqual(val2, 4 * Mathf.PI);
    }
}
