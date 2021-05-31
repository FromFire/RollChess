using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Random = System.Random;

/// <summary>
///   <para> 玩家 </para>
/// </summary>
public class Player : NetworkBehaviour {
    public NetworkConnection conn;
    public uint id;
    public string name;

}