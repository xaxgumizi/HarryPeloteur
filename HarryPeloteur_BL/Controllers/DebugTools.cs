using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace HarryPeloteur_BL.Controllers
{
    public class DebugTools
    {
        public void VarDump(object obj)
        {
            System.Diagnostics.Debug.WriteLine("{0,-18} {1}", "Name", "Value");
            string ln = @"-----------------------------------------------------------------";
            System.Diagnostics.Debug.WriteLine(ln);

            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("{0,-18} {1}",
                          props[i].Name, props[i].GetValue(obj, null));
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);  
                }
            }
            System.Diagnostics.Debug.WriteLine("");
        }
        public void dbg(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }

        public void PrintArray(int[] obj)
        {
            //string result = string.Join(", ", obj);
            //System.Diagnostics.Debug.WriteLine(result);

            System.Diagnostics.Debug.WriteLine("");
            foreach (int i in obj)
            {
                System.Diagnostics.Debug.Write(i + ", ");
            }
            System.Diagnostics.Debug.WriteLine("");
        }
    }
}