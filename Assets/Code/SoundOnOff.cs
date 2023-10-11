using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnOff : ButtonOnOff
{
    public override void Swap()
    {
        base.Swap();
        //AudioListener.pause = !On;
        AudioListener.volume = On ? 1 : 0;
    }


}
