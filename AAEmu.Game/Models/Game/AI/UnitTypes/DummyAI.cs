using AAEmu.Game.Models.Game.AI.Framework;

namespace AAEmu.Game.Models.Game.AI.UnitTypes
{
    public class DummyAI : AbstractUnitAI
    {
        public override uint GetNextState(State previous)
        {
            return 0;
        }
    }
}
