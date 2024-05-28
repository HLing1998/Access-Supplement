using System;
using System.Collections.Generic;
using StardewModdingAPI.Utilities;

namespace AccessSupplement
{
    public class ModConfig
    {
        //阅读技能经验值及NPC友好度
        public KeybindList ReadXPAndFriendship { get; set; } = KeybindList.Parse("O");
        //阅读幸运值
        public KeybindList ReadLuck { get; set; } = KeybindList.Parse("L");
        //公用
        public KeybindList CloseMenu { get; set; } = KeybindList.Parse("Escape");

        //动物商店
        public KeybindList AnimalShopBuildUp { get; set; } = KeybindList.Parse("PageUp");
        public KeybindList AnimalShopBuildDown { get; set; } = KeybindList.Parse("PageDown");
        public KeybindList AnimalShopPurchase { get; set; } = KeybindList.Parse("Enter");

        //标记列表及木匠商店
        public KeybindList OpenMarkMenu { get; set; } = KeybindList.Parse("LeftAlt + C , RightAlt + C");
        public KeybindList ReadMarkPoint1 { get; set; } = KeybindList.Parse("D1");
        public KeybindList ReadMarkPoint2 { get; set; } = KeybindList.Parse("D2");
        public KeybindList ReadMarkPoint3 { get; set; } = KeybindList.Parse("D3");
        public KeybindList ReadMarkPoint4 { get; set; } = KeybindList.Parse("D4");
        public KeybindList ReadMarkPoint5 { get; set; } = KeybindList.Parse("D5");
        public KeybindList ReadMarkPoint6 { get; set; } = KeybindList.Parse("D6");
        public KeybindList ReadMarkPoint7 { get; set; } = KeybindList.Parse("D7");
        public KeybindList ReadMarkPoint8 { get; set; } = KeybindList.Parse("D8");
        public KeybindList ReadMarkPoint9 { get; set; } = KeybindList.Parse("D9");
        public KeybindList SetAndUseMark1 { get; set; } = KeybindList.Parse("LeftAlt + D1, RightAlt +D1");
        public KeybindList SetAndUseMark2 { get; set; } = KeybindList.Parse("LeftAlt + D2, RightAlt +D2");
        public KeybindList SetAndUseMark3 { get; set; } = KeybindList.Parse("LeftAlt + D3, RightAlt +D3");
        public KeybindList SetAndUseMark4 { get; set; } = KeybindList.Parse("LeftAlt + D4, RightAlt +D4");
        public KeybindList SetAndUseMark5 { get; set; } = KeybindList.Parse("LeftAlt + D5, RightAlt +D5");
        public KeybindList SetAndUseMark6 { get; set; } = KeybindList.Parse("LeftAlt + D6, RightAlt +D6");
        public KeybindList SetAndUseMark7 { get; set; } = KeybindList.Parse("LeftAlt + D7, RightAlt +D7");
        public KeybindList SetAndUseMark8 { get; set; } = KeybindList.Parse("LeftAlt + D8, RightAlt +D8");
        public KeybindList SetAndUseMark9 { get; set; } = KeybindList.Parse("LeftAlt + D9, RightAlt +D9");
        public KeybindList SetAtCarpenterMark1 { get; set; } = KeybindList.Parse("LeftControl + D1, RightControl +D1");
        public KeybindList SetAtCarpenterMark2 { get; set; } = KeybindList.Parse("LeftControl + D2, RightControl +D2");
        public KeybindList SetAtCarpenterMark3 { get; set; } = KeybindList.Parse("LeftControl + D3, RightControl +D3");
        public KeybindList SetAtCarpenterMark4 { get; set; } = KeybindList.Parse("LeftControl + D4, RightControl +D4");
        public KeybindList SetAtCarpenterMark5 { get; set; } = KeybindList.Parse("LeftControl + D5, RightControl +D5");
        public KeybindList SetAtCarpenterMark6 { get; set; } = KeybindList.Parse("LeftControl + D6, RightControl +D6");
        public KeybindList SetAtCarpenterMark7 { get; set; } = KeybindList.Parse("LeftControl + D7, RightControl +D7");
        public KeybindList SetAtCarpenterMark8 { get; set; } = KeybindList.Parse("LeftControl + D8, RightControl +D8");
        public KeybindList SetAtCarpenterMark9 { get; set; } = KeybindList.Parse("LeftControl + D9, RightControl +D9");
        public KeybindList CarpenterBuildUp { get; set; } = KeybindList.Parse("PageUp");
        public KeybindList CarpenterBuildDown { get; set; } = KeybindList.Parse("PageDown");
        public KeybindList CarpenterBuildEnter { get; set; } = KeybindList.Parse("Enter");
        public KeybindList CarpenterReadMousePosition { get; set; } = KeybindList.Parse("O");
        public KeybindList RenovateCursorUp { get; set; } = KeybindList.Parse("Up");
        public KeybindList RenovateCursorDown { get; set; } = KeybindList.Parse("Down");
        public KeybindList RenovateCursorLeft { get; set; } = KeybindList.Parse("Left");
        public KeybindList RenovateCursorRight { get; set; } = KeybindList.Parse("Right");


        //俱乐部21点游戏
        public KeybindList ReadPlayerCards21D { get; set; } = KeybindList.Parse("D1");
        public KeybindList ReadDealerCards21D { get; set; } = KeybindList.Parse("D2");
        public KeybindList ReadBet21D { get; set; } = KeybindList.Parse("D3");
        public KeybindList ReadClubCoins21D { get; set; } = KeybindList.Parse("R");
        public KeybindList Hit21D { get; set; } = KeybindList.Parse("E");
        public KeybindList Cease21D { get; set; } = KeybindList.Parse("Q");
        public KeybindList NewGame21D { get; set; } = KeybindList.Parse("N");
        public KeybindList doubleOrNothing21D { get; set; } = KeybindList.Parse("D");
        public KeybindList EndGame21D { get; set; } = KeybindList.Parse("Escape");

        //老虎机游戏
        public KeybindList SlotsBet10 { get; set; } = KeybindList.Parse("D1");
        public KeybindList SlotsBet100 { get; set; } = KeybindList.Parse("D2");
        public KeybindList SlotsReadBet { get; set; } = KeybindList.Parse("D3");
        public KeybindList SlotsReadResult { get; set; } = KeybindList.Parse("D4");
        public KeybindList SlotsReadClubCoin { get; set; } = KeybindList.Parse("D5");

        //自由履行
        public KeybindList FreeTravel_OpenTravelList { get; set; } = KeybindList.Parse("V");
        public KeybindList FreeTravel_CreateTravelPoint { get; set; } = KeybindList.Parse("N");
        public KeybindList FreeTravel_DeleteTravelPoint { get; set; } = KeybindList.Parse("Delete");
        public KeybindList FreeTravel_TravelPointUpRolling { get; set; } = KeybindList.Parse("W");
        public KeybindList FreeTravel_TravelPointDownRolling { get; set; } = KeybindList.Parse("S");
        public KeybindList FreeTravel_Warp { get; set; } = KeybindList.Parse("Enter");
        public KeybindList FreeTravel_TravelPointUpMove { get; set; } = KeybindList.Parse("LeftControl + Up");
        public KeybindList FreeTravel_TravelPointDownMove { get; set; } = KeybindList.Parse("LeftControl + Down");
        public KeybindList FreeTravel_SkipUp8 { get; set; } = KeybindList.Parse("PageUp");
        public KeybindList FreeTravel_SkipDown8 { get; set; } = KeybindList.Parse("PageDown");

        //导航面板
        public KeybindList NavigationOpenPanel { get; set; } = KeybindList.Parse("LeftAlt + P, RightAlt + P");
        public KeybindList NavigationItemUp { get; set; } = KeybindList.Parse("PageUp");
        public KeybindList NavigationItemDown { get; set; } = KeybindList.Parse("PageDown");
        public KeybindList NavigationOptionsUp { get; set; } = KeybindList.Parse("Home");
        public KeybindList NavigationOptionsDown { get; set; } = KeybindList.Parse("End");
        public KeybindList NavigationLaunchMove { get; set; } = KeybindList.Parse("Enter");
        public KeybindList NavigationStopMove { get; set; } = KeybindList.Parse("Escape");
        public KeybindList NavigationModifyItem { get; set; } = KeybindList.Parse("M");
        public KeybindList NavigationMonster { get; set; } = KeybindList.Parse("F6");
        public KeybindList NavigationMonsterUp { get; set; } = KeybindList.Parse("F7");
        public KeybindList NavigationMonsterDown { get; set; } = KeybindList.Parse("F8");
        public KeybindList NavigationMonsterDistanceKey { get; set; } = KeybindList.Parse("LeftAlt + F7, RightAlt + f7");
        public KeybindList NavigationMonsterAudioBrowserKey { get; set; } = KeybindList.Parse("LeftAlt + F8, RightAlt + f8");
        public bool NavigationMonsterDistance { get; set; } = true;
        public bool NavigationMonsterAudioBrowser { get; set; } = true;


    }
}
