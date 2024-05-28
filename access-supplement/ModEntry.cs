using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using access_supplement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using Freetravel;
using xTile.Tiles;
using StardewValley.GameData.WorldMaps;
using StardewValley.GameData.Locations;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AccessSupplement
{
    public interface API
    {
        bool Say(String text, Boolean interrupt);
        Dictionary<Vector2, (string name, string category)> SearchLocation();
    }

    //入口点
    public class ModEntry : Mod
    {
        private API api;
        private ModConfig Config;
        private AnimalShop Animalshop;
        private CarpenterShop Carpentershop;
        private Renovate Renovatemenu;
        private Club21D Club21d;
        private ClubSlots Clubslots;
        private FreeTravel Travel;
        private GameInfo gameInfo;
        private Navigation NavigationHand;
        private AudioListener Listener;
        private AudioEmitter Emitter;



        //入口点方法
        public override void Entry(IModHelper helper)
        {
            //实例初始化以及加载配置
            this.Listener = new AudioListener();
            this.Emitter = new AudioEmitter();
            this.Config = this.Helper.ReadConfig<ModConfig>();
            this.Animalshop = new AnimalShop();
            this.Carpentershop = new CarpenterShop();
            this.Renovatemenu = new Renovate();
            this.Club21d = new Club21D();
            this.Clubslots = new ClubSlots();
            this.Travel = new FreeTravel();
            this.gameInfo = new GameInfo();
            //事件绑定
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.GameLoop.OneSecondUpdateTicked += this.OnGameLoop;
            helper.Events.Input.ButtonPressed += this.OnKeyPressed;
            helper.Events.GameLoop.UpdateTicked += this.OnGameUpdate;
        }

        //时时监听
        private void OnGameUpdate(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            this.NavigationHand.OnGameLoop(this.api);
        }


        //启动时加载
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            //this.LoadSound();
            this.api = this.Helper.ModRegistry.GetApi<API>("shoaib.stardewaccess");
            this.Travel.Initialize(this.Helper, this.Config, this.api);
            this.Carpentershop.Initialize(this.Helper, this.Config, this.api);
            this.NavigationHand = new Navigation(this.Helper, this.Config, this.api);
        }

        //每秒循环监听
        private void OnGameLoop(object sender, OneSecondUpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            this.Club21d.OnGameLoop(this.api);
        }

        //按键监听
private void OnKeyPressed(object sender, ButtonPressedEventArgs e)
        {
            // 如果玩家还没有进入存档，则取消执行
            if (!Context.IsWorldReady)
                return;

            //自由履行
            if(Game1.activeClickableMenu == null || Game1.activeClickableMenu is FreeTravelPanel p || Game1.activeClickableMenu is StardewValley.Menus.NamingMenu n)
            {
                this.Travel.OnKeyPressed(sender, e);
            }

            //导航面板
            if(Game1.activeClickableMenu == null || Game1.activeClickableMenu is NavigationPanel navigation)
            {
                this.NavigationHand.OnKeyPressed(sender, e,this.Helper ,this.Config, this.api);
            }

            //木匠商店
            if(Game1.activeClickableMenu == null || Game1.activeClickableMenu is MarkPanel markpanel || Game1.activeClickableMenu is CarpenterMenu carpenter)
            {
                this.Carpentershop.OnKeyPressed(sender, e);
            }

            //装修房屋
            if(Game1.activeClickableMenu is RenovateMenu ren)
            {
                this.Renovatemenu.OnKeyPressed(this.Helper, this.Config, this.api);
            }

            //动物商店
            if(Game1.activeClickableMenu is PurchaseAnimalsMenu animalmenu)
            {
                this.Animalshop.OnKeyPressed(this.Helper, this.Config, this.api);
            }

            //21点游戏
            if (Game1.currentMinigame is StardewValley.Minigames.CalicoJack gam)
            {
                this.Club21d.OnKeyPressed(sender , e, this.Helper, this.Config, this.api);
            }

            //老虎机游戏
            if (Game1.currentMinigame is StardewValley.Minigames.Slots slots)
            {
                this.Clubslots.OnKeyPressed(sender, e, this.Helper, this.Config, this.api);
            }

                //游戏菜单内操作
                if (Game1.activeClickableMenu is StardewValley.Menus.GameMenu m)
            {
                //阅读技能经验
                if (this.Config.ReadXPAndFriendship.JustPressed() && this.Helper.Reflection.GetField<int>(Game1.activeClickableMenu, "currentTab").GetValue() == 1)
                {
                    this.gameInfo.ReadXP(this.Helper, this.api);
                }
                //阅读友好度
                if (this.Config.ReadXPAndFriendship.JustPressed() && this.Helper.Reflection.GetField<int>(Game1.activeClickableMenu, "currentTab").GetValue() == 2)
                {
                    this.gameInfo.ReadFriendship(this.Helper, this.api);
                }
                //阅读动物友好度
                if (this.Config.ReadXPAndFriendship.JustPressed() && this.Helper.Reflection.GetField<int>(Game1.activeClickableMenu, "currentTab").GetValue() == 5)
                {
                    this.gameInfo.ReadFriendshipAtAnimal(this.Helper, this.api);
                }
                    //阅读幸运值
                    if (this.Config.ReadLuck.JustPressed() && this.Helper.Reflection.GetField<int>(Game1.activeClickableMenu, "currentTab").GetValue() == 0)
                {
                    this.gameInfo.ReadLuck(this.Helper, this.api);
                }
            }


                //if(this.Helper.Input.IsDown(SButton.O))
            //{
//SButton.
            //}


        }

        //音频资源加载
        private void LoadSound()
        {
            List<string> soundlist = new List<string>() { "dog", "cat", "pig", "chicken" };
            foreach(string key in new ModSound().Soundeffect.Keys)
            {
                CueDefinition cuedefition = new()
                {
                    name = key
                };
                cuedefition.instanceLimit = 1;
                cuedefition.limitBehavior = CueDefinition.LimitBehavior.ReplaceOldest;
                SoundEffect effect;
                string filePath = Path.Combine(this.Helper.DirectoryPath, "assets", "sounds", $"{key}.wav");
                FileStream soundstream = new(filePath, FileMode.Open);
                    effect = SoundEffect.FromStream(soundstream);
                    cuedefition.SetSound(effect, Game1.audioEngine.GetCategoryIndex("Sound"), false);
                    Game1.soundBank.AddCue(cuedefition);
            }
            //Game1.player.currentLocation.DisplayName


        }



    }
}
