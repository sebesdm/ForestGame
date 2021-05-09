using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomPickupEventArgs
{
    public Player Player { get; set; }
    public Mushroom PickedUpMushroom { get; set; }
}
