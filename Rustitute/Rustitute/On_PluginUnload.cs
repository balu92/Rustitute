namespace Rustitute
{
    partial class Rustitute
    {
        public void On_PluginUnload()
        {
            workTimer.Close();
            lanternTimer.Close();
            campingTimer.Close();
            disappearTimer.Close();

            ini.Save();
            iniArena.Save();

            //GlobalData["Rustitute_disappearList"] = disappearList;
            //GlobalData["Rustitute_disappearUnique"] = disappearUnique;
            GlobalData["Rustitute_disappearBlocks"] = disappearBlocks;
        }
    }
}
