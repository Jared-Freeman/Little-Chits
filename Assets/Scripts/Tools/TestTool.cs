using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTool : Tool
{
    public override void StartAction()
    {
        print("started an action");
    }

    public override void EndAction()
    {
        print("ended an action");

    }
}