using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using access_supplement;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using AccessSupplement;

namespace Freetravel
{
    public delegate void CallbackDelegate(string text);

    public class FreeTravel
    {
        private IModHelper Helper;
        private ModConfig Config;
        private ModHash TravelData;
        private API api;
        private FreeTravelPanel panel;
        private StardewValley.Menus.NamingMenu NameMenu;

        public void Initialize(IModHelper helper, ModConfig config, API api)
        {
            this.Helper = helper;
            this.Config = config;
            this.api = api;
            this.TravelData = helper.Data.ReadJsonFile<ModHash>("travelData.json") ?? new ModHash();
        }

public void OnKeyPressed(object sender, ButtonPressedEventArgs e)
        {

            //命名菜单内操作
            if (Game1.activeClickableMenu == this.NameMenu && this.NameMenu != null)
            {
                if (this.Helper.Input.IsDown(SButton.Escape))
                {
                    this.NamingMenuEscape();
                }
            }
            //命名框内操作结束
            //按下打开履行列表
            if (this.Config.FreeTravel_OpenTravelList.JustPressed() && Game1.activeClickableMenu == null)
            {
                if (this.TravelData.TravelData.Count > 0)
                {
                    this.OpenTravelList();
                } else
                {
                    this.AddTravelItem();
                }
            }
            //履行列表内操作
            if (Game1.activeClickableMenu is FreeTravelPanel p)
            {
                if (this.Config.FreeTravel_CreateTravelPoint.JustPressed())
                {
                    this.AddTravelItem();
                }
                if (this.Config.FreeTravel_Warp.JustPressed())
                {
                    this.ClickTravelPoint();
                }
                if (this.Config.FreeTravel_DeleteTravelPoint.JustPressed())
                {
                    this.DeleteTravelPoint();
                }
                if (this.Config.FreeTravel_TravelPointUpMove.JustPressed())
                {
                    this.TravelPointUpMove();
                }
                if (this.Config.FreeTravel_TravelPointDownMove.JustPressed())
                {
                    this.TravelPointDownMove();
                }
            }
            //履行列表内操作结束

        }

//回调朗读菜单项
        public void ReceiveCallback(string text)
        {
            this.api.Say(text, true);
        }

        //命名框内按下ESCAPE
        private void NamingMenuEscape()
        {
            if (this.NameMenu.textBox.Selected)
            {
                this.NameMenu.textBox.Selected = false;
            } else
            {
                this.Helper.Input.Suppress(SButton.Escape);
                if(this.TravelData.TravelData.Count !=0)
                {
                    this.OpenTravelList();
                } else
                {
                    Game1.exitActiveMenu();
                    Game1.playSound("bigDeSelect");
                }
            }
        }

        //打开履行列表
        private void OpenTravelList(int index=0)
        {
if(this.TravelData.TravelIndex.Count > 0)
            {
                this.Helper.Input.Suppress(SButton.Escape);
                this.Helper.Input.Suppress(SButton.Enter);
                List<string> travellist = this.GetTravelList();
                CallbackDelegate callback = this.ReceiveCallback;
                this.panel = new FreeTravelPanel(callback, travellist, this.Config);
                Game1.activeClickableMenu = this.panel;
                Game1.playSound("bigSelect");
                this.panel.PointIndex = index;
                this.api.Say(this.panel.PointIDS[this.panel.PointIndex], true);
            } else
            {
                Game1.activeClickableMenu = null;
                Game1.playSound("bigDeSelect");
            }
        }

        //获取履行列表数据
        private List<string> GetTravelList()
        {
            List<string> travellist = new List<string>();
            for (int i = 0; i < this.TravelData.TravelIndex.Count; i++)
            {
                string name = this.TravelData.TravelIndex[i];
                travellist.Add($"{i} {name}： {this.TravelData.TravelData[name]["location-name"]} （{this.TravelData.TravelData[name]["X"]}， {this.TravelData.TravelData[name]["Y"]}）");
            }
            return travellist;
        }

        //点击履行点
        private void ClickTravelPoint()
        {
            if (this.TravelData.TravelData.ContainsKey(this.TravelData.TravelIndex[this.panel.PointIndex]))
            {
                string name = this.TravelData.TravelIndex[this.panel.PointIndex];
                Game1.warpFarmer((string)this.TravelData.TravelData[name]["location-name"], (int)this.TravelData.TravelData[name]["X"], (int)this.TravelData.TravelData[name]["Y"], true);
            }
            Game1.activeClickableMenu = null;
            Game1.playSound("bigSelect");
        }

         //添加履行点
        private void AddTravelItem()
        {
            StardewValley.Menus.NamingMenu.doneNamingBehavior done = new StardewValley.Menus.NamingMenu.doneNamingBehavior(this.AddTravelItemCallback);
            this.NameMenu = new StardewValley.Menus.NamingMenu(done, "履行点名称", null);
            Game1.activeClickableMenu = this.NameMenu;
            Game1.playSound("bigSelect");
        }

        //添加履行点回调
        private void AddTravelItemCallback(string text)
        {
            if (!this.TravelData.TravelData.ContainsKey(text))
            {
                string localname = Game1.player.currentLocation.name;
                int x = (int)Game1.player.Tile.X;
                int y = (int)Game1.player.Tile.Y;
                Dictionary<string, dynamic> table = new Dictionary<string, dynamic>();
                table.Add("location-name", localname);
                table.Add("X", x);
                table.Add("Y", y);
                this.TravelData.TravelData.Add(text, table);
                this.TravelData.TravelIndex.Add(this.TravelData.TravelIndex.Count, text);
                this.Helper.Data.WriteJsonFile("travelData.json", this.TravelData);
            }
            else
            {
                this.api.Say("履行点名称已存在", true);
            }
            this.OpenTravelList();
        }

        //删除履行点
        private void DeleteTravelPoint()
        {
            if (this.TravelData.TravelData.ContainsKey(this.TravelData.TravelIndex[this.panel.PointIndex]))
            {
                this.TravelData.TravelData.Remove(this.TravelData.TravelIndex[this.panel.PointIndex]);
                this.TravelData.TravelIndex.Remove(this.panel.PointIndex);
                if(this.TravelData.TravelIndex.Count != 0)
                {
                    this.TravelData.TravelIndex = this.TravelData.TravelIndex.OrderBy(f => f.Key).ToDictionary(f => f.Key, o => o.Value);
                    Dictionary<int, string> dt = new Dictionary<int, string>();
                    int index = 0;
                    foreach (string s in this.TravelData.TravelIndex.Values)
                    {
                        dt.Add(index, s);
                        index++;
                    }
                    this.TravelData.TravelIndex = dt;
                }
                this.Helper.Data.WriteJsonFile("travelData.json", this.TravelData);
                List<string> travellist = this.GetTravelList();
                this.panel.PointIDS = travellist;
                if(this.panel.PointIndex >= travellist.Count)
                {
                    this.panel.PointIndex = travellist.Count - 1;
                }
                Game1.playSound("bigSelect");
                this.api.Say(this.panel.PointIDS[this.panel.PointIndex], true);
            }
        }

        //履行点上移
        private void TravelPointUpMove()
        {
            if(this.panel.PointIndex > 0)
            {
                string upTravelItem = this.TravelData.TravelIndex[this.panel.PointIndex - 1];
                string nowTravelItem = this.TravelData.TravelIndex[this.panel.PointIndex];
                this.TravelData.TravelIndex.Remove(this.panel.PointIndex - 1);
                this.TravelData.TravelIndex.Remove(this.panel.PointIndex);
                this.TravelData.TravelIndex.Add(this.panel.PointIndex - 1, nowTravelItem);
                this.TravelData.TravelIndex.Add(this.panel.PointIndex, upTravelItem);
                this.Helper.Data.WriteJsonFile("travelData.json", this.TravelData);
                this.OpenTravelList(this.panel.PointIndex - 1);
            }
        }

        //履行点下移
        private void TravelPointDownMove()
        {
            if (this.panel.PointIndex < this.TravelData.TravelIndex.Count -1)
            {
                string upTravelItem = this.TravelData.TravelIndex[this.panel.PointIndex + 1];
                string nowTravelItem = this.TravelData.TravelIndex[this.panel.PointIndex];
                this.TravelData.TravelIndex.Remove(this.panel.PointIndex + 1);
                this.TravelData.TravelIndex.Remove(this.panel.PointIndex);
                this.TravelData.TravelIndex.Add(this.panel.PointIndex + 1, nowTravelItem);
                this.TravelData.TravelIndex.Add(this.panel.PointIndex, upTravelItem);
                this.Helper.Data.WriteJsonFile("travelData.json", this.TravelData);
                this.OpenTravelList(this.panel.PointIndex + 1);
            }
        }




    }
}