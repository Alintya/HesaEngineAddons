using System.Reflection;
using HesaEngine.SDK;

namespace Run_it_down_mid
{
    public class Utils
    {
        public static void PrintChat(string msg)
        {
            Chat.Print("<font color = \"#ffdead\">[Feeder]:</font> <font color = \"#ffffff\">" + msg + "</font>");
        }

        public static string GetVersion()
        {
            string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return ver.Remove(ver.LastIndexOf('.'));
        }


    }

    public static class MenuExtension
    {
        public static bool GetCheckbox(this Menu c, string name)
        {
            return c.Get<MenuCheckbox>(name).Checked;
        }

        public static int GetSlider(this Menu c, string name)
        {
            return c.Get<MenuSlider>(name).CurrentValue;
        }

        public static int GetCombobox(this Menu c, string name)
        {
            return c.Get<MenuCombo>(name).CurrentValue;
        }
    }
}
