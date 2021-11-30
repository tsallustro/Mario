

using GameObjects;
using States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commands
{
    class ThrowBossBeamCommand : MarioCommand
    {
       
        public ThrowBossBeamCommand(Mario mario) : base(mario)
        {
            
        }
        public override void Execute(int pressType)
        {
            if (pressType == 1 && active && avatar.GetPowerState() is BossMario && !BossBeam.Instance.isActive)
            {
                this.avatar.GetPowerState().BigMario();
                BossBeam.Instance.ResetObject();
            }
           
        }
    }
}
