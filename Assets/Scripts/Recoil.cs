using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil {

    private float xKick;
    private float yKick;
    private float maxAngle;

    public enum RecoilType
    {
        None,
        Cone,
        Simulated
    };

    private RecoilType type = RecoilType.None;

	public Recoil()
    {

    }

    public RecoilType Type
    {
        get { return type; }
    }
}
