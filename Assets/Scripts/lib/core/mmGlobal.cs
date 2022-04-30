using FairyGUI;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace mm
{
    public class Global
    {
        public static string fairyPath;
        private static Dictionary<string, Object> instHistory = new Dictionary<string, object>();
        private static Assembly assembly = Assembly.GetExecutingAssembly();
        public static Object getFairyInstence(string type, Object[] obj)
        {
            // var args = [];
            // for (var _i = 1; _i < arguments.length; _i++)
            // {
            //     args[_i - 1] = arguments[_i];
            // }
            // for (var i = 0; i < instHistory.length; ++i)
            // {
            //     var item = instHistory[i];
            //     if (item.type == type)
            //         return item.inst;
            // }
            foreach (KeyValuePair<string, object> kv in instHistory)
            {
                if (kv.Key == type)
                {
                    return kv.Value;
                }
            }
            // var inst = new(type.bind.apply(type, [void 0].concat(args)))();
            Type classType = Type.GetType(type, true);
            var inst = Activator.CreateInstance(classType);
            // instHistory.push({ type: type, inst: inst });
            instHistory.Add(type, inst);
            return inst;
        }
    }
}