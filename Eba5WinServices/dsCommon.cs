using System;
using System.Text;
using System.Reflection;
namespace Eba5WinServices
{
    public class dsCommon
    {
        public dsCommon()
        {
        }

        public static string getAppVersion(string sAssemblyName)
        {
            Assembly myAsm = Assembly.Load(sAssemblyName);
            AssemblyName aName = myAsm.GetName();
            return aName.Version.ToString();
        }

        public static string getFrmVersion()
        {
            return System.Threading.Thread.GetDomain().GetAssemblies()[0].GetName().Version.ToString();

            /* Bu kodlar framework versiyonunu getiriyor..
            int MajorVersion = System.Threading.Thread.GetDomain().GetAssemblies()[0].GetName().Version.Major;
            int MinorVersion = System.Threading.Thread.GetDomain().GetAssemblies()[0].GetName().Version.Minor;
            int BuildVersion = System.Threading.Thread.GetDomain().GetAssemblies()[0].GetName().Version.Build;
            int RevVersion   = System.Threading.Thread.GetDomain().GetAssemblies()[0].GetName().Version.Revision;
            string FullVersion = System.Threading.Thread.GetDomain().GetAssemblies()[0].GetName().Version.ToString();
            */
        }
    }
}
