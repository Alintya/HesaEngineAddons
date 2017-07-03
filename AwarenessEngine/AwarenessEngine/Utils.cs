using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace AwarenessEngine
{
    public class Utils
    {
        public static void PrintChat(string msg)
        {
            Chat.Print("<font color = \"#ffdead\">[AwarenessEngine]:</font> <font color = \"#ffffff\">" + msg + "</font>");
        }

        public static string GetVersion()
        {
            string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return ver.Remove(ver.LastIndexOf('.'));
        }

        public static string GetFullVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
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

        // TODO: overload to remove components of c
        public static void Remove(this Menu c)
        {
            Menu.Remove(c);
        }

        public static void Remove(this Menu c, MenuElement[] menuElements)
        {
            throw new NotImplementedException();

            foreach (var elem in menuElements)
            {
                // la remova hesa
            }
        }

        public static void Remove(this MenuElement e)
        {
            throw new NotImplementedException();
        }
    }
}
