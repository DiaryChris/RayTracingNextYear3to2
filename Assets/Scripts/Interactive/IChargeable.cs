using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChargeable 
{
    void Charging(Transform Ts);
}

public interface INoPower
{
    void NoPower(Transform Ts);
}