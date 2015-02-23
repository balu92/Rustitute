using Pluton.Events;

namespace Rustitute
{
    partial class Rustitute
    {
        public void On_CorpseDropped(CorpseInitEvent ie)
        {
            if (GetSettingBool("user_" + ie.Parent.ToPlayer().SteamID, "inArena"))
                ie.Corpse.RemoveCorpse();
        }
    }
}
