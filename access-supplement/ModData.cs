using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace AccessSupplement
{
    public class ModSound
    {
        public Dictionary<string, string> Soundeffect { get; set; } = new Dictionary<string, string>() { { "browseMark", "Sound" }, { "setMark", "Sound" }, { "openMenu", "Sound" }, { "closeMenu", "Sound" } };
    }

    public class ModHash
    {
        public Dictionary<string, Dictionary<string, dynamic>> TravelData { get; set; } = new Dictionary<string, Dictionary<string, dynamic>>();
        public Dictionary<int, string> TravelIndex { get; set; } = new Dictionary<int, string>();
    }

    public class ModMark
    {
        public Dictionary<int , Dictionary<string, dynamic>> MarkData { get; set; } = new Dictionary<int, Dictionary<string, dynamic>>();

    }

    public class ModNavigation
    {
        public Dictionary<string, Dictionary<string, NavigationPoint>> NavigationData { get; set; } = new Dictionary<string, Dictionary<string, NavigationPoint>>() { {"交互点", new Dictionary<string, NavigationPoint>()},{"传送点", new Dictionary<string, NavigationPoint>()} };


    }
}