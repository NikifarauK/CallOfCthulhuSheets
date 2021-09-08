using CallOfCthulhuSheets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallOfCthulhuSheets.Extensions
{
    internal static class PlayerAuthExtension
    {
        internal static int GetLoginAccessHashingByPassword(this Player player, string password)
        {
            return (player.Name + "p,i0fe>" + password).GetHashCode();
        }


    }
}
