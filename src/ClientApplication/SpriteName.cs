using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplication
{
    public static class SpriteName
    {
        public static string Create(string @namespace, string id)
        {
            return $"{@namespace}:{id}";
        }

        public static string ExtractId(string spriteName)
        {
            return spriteName.Substring(spriteName.IndexOf(':') + 1);
        }
    }
}