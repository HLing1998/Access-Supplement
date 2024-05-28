using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using access_supplement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Menus;

namespace AccessSupplement
{

    public class CarpenterShop
    {
        private IModHelper Helper;
        private ModConfig Config;
        private API api;
        private MarkPanel Markpanel;
        private ModMark MarkData;
        private int BuildIndex = 0;
        private Building choseBuild = null;
        private bool building = false;


        public void Initialize(IModHelper helper, ModConfig config, API api)
        {
            this.Helper = helper;
            this.Config = config;
            this.api = api;
            this.MarkData = helper.Data.ReadJsonFile<ModMark>("assets/MarkData.json") ?? new ModMark();
            if (this.MarkData.MarkData.Count != 9)
            {
                this.InitializeMark();
            }
        }

        //初始化mark数据
        private void InitializeMark()
        {
            this.MarkData.MarkData = new Dictionary<int, Dictionary<string, dynamic>>();
for(int i = 1; i < 10; i++)
            {
                this.MarkData.MarkData.Add(i, new Dictionary<string, dynamic>());
            }
        }

        public void OnKeyPressed(object sender, ButtonPressedEventArgs e)
        {
            //无菜单时操作
            if(this.Config.OpenMarkMenu.JustPressed() && Game1.activeClickableMenu == null)
            {
                this.Markpanel = new MarkPanel(this.Config);
                Game1.activeClickableMenu = this.Markpanel;
                Game1.playSound("bigSelect");
            }
            //处于Mark菜单内操作
            if(Game1.activeClickableMenu is MarkPanel markpanel)
            {
                //设定mark1
                if (this.Config.SetAndUseMark1.JustPressed())
                {
                    this.SetMarkPoint(1);
                } else if (this.Config.SetAndUseMark2.JustPressed())
                {
                    this.SetMarkPoint(2);
                } else if (this.Config.SetAndUseMark3.JustPressed())
                {
                    this.SetMarkPoint(3);
                } else if (this.Config.SetAndUseMark4.JustPressed())
                {
                    this.SetMarkPoint(4);
                } else if (this.Config.SetAndUseMark5.JustPressed())
                {
                    this.SetMarkPoint(5);
                } else if (this.Config.SetAndUseMark6.JustPressed())
                {
                    this.SetMarkPoint(6);
                } else if (this.Config.SetAndUseMark7.JustPressed())
                {
                    this.SetMarkPoint(7);
                } else if (this.Config.SetAndUseMark8.JustPressed())
                {
                    this.SetMarkPoint(8);
                } else if (this.Config.SetAndUseMark9.JustPressed())
                {
                    this.SetMarkPoint(9);
                } else if (this.Config.ReadMarkPoint1.JustPressed())
                {
                    this.ReadMarkPoint(1);
                } else if (this.Config.ReadMarkPoint2.JustPressed())
                {
                    this.ReadMarkPoint(2);
                } else if (this.Config.ReadMarkPoint3.JustPressed())
                {
                    this.ReadMarkPoint(3);
                } else if (this.Config.ReadMarkPoint4.JustPressed())
                {
                    this.ReadMarkPoint(4);
                } else if (this.Config.ReadMarkPoint5.JustPressed())
                {
                    this.ReadMarkPoint(5);
                } else if (this.Config.ReadMarkPoint6.JustPressed())
                {
                    this.ReadMarkPoint(6);
                } else if (this.Config.ReadMarkPoint7.JustPressed())
                {
                    this.ReadMarkPoint(7);
                } else if (this.Config.ReadMarkPoint8.JustPressed())
                {
                    this.ReadMarkPoint(8);
                } else if (this.Config.ReadMarkPoint9.JustPressed())
                {
                    this.ReadMarkPoint(9);
                }
            }
//在木匠菜单内操作
if((Game1.activeClickableMenu is CarpenterMenu carpenter && this.Helper.Reflection.GetField<bool>(Game1.activeClickableMenu, "onFarm").GetValue()))
            {
                if (this.Config.SetAtCarpenterMark1.JustPressed())
                {
                    this.SetMarkCursor(1);
                }
                else if (this.Config.SetAtCarpenterMark2.JustPressed())
                {
                    this.SetMarkCursor(2);
                }
                else if (this.Config.SetAtCarpenterMark3.JustPressed())
                {
                    this.SetMarkCursor(3);
                }
                else if (this.Config.SetAtCarpenterMark4.JustPressed())
                {
                    this.SetMarkCursor(4);
                }
                else if (this.Config.SetAtCarpenterMark5.JustPressed())
                {
                    this.SetMarkCursor(5);
                }
                else if (this.Config.SetAtCarpenterMark6.JustPressed())
                {
                    this.SetMarkCursor(6);
                }
                else if (this.Config.SetAtCarpenterMark7.JustPressed())
                {
                    this.SetMarkCursor(7);
                }
                else if (this.Config.SetAtCarpenterMark8.JustPressed())
                {
                    this.SetMarkCursor(8);
                }
                else if (this.Config.SetAtCarpenterMark9.JustPressed())
                {
                    this.SetMarkCursor(9);
                } else if (this.Config.SetAndUseMark1.JustPressed())
                {
                    this.ClickMarkPoint(1);
                }
                else if (this.Config.SetAndUseMark2.JustPressed())
                {
                    this.ClickMarkPoint(2);
                }
                else if (this.Config.SetAndUseMark3.JustPressed())
                {
                    this.ClickMarkPoint(3);
                }
                else if (this.Config.SetAndUseMark4.JustPressed())
                {
                    this.ClickMarkPoint(4);
                }
                else if (this.Config.SetAndUseMark5.JustPressed())
                {
                    this.ClickMarkPoint(5);
                }
                else if (this.Config.SetAndUseMark6.JustPressed())
                {
                    this.ClickMarkPoint(6);
                }
                else if (this.Config.SetAndUseMark7.JustPressed())
                {
                    this.ClickMarkPoint(7);
                }
                else if (this.Config.SetAndUseMark8.JustPressed())
                {
                    this.ClickMarkPoint(8);
                }
                else if (this.Config.SetAndUseMark9.JustPressed())
                {
                    this.ClickMarkPoint(9);
                }
                else if (this.Config.ReadMarkPoint1.JustPressed())
                {
                    this.ReadMarkPoint(1);
                }
                else if (this.Config.ReadMarkPoint2.JustPressed())
                {
                    this.ReadMarkPoint(2);
                }
                else if (this.Config.ReadMarkPoint3.JustPressed())
                {
                    this.ReadMarkPoint(3);
                }
                else if (this.Config.ReadMarkPoint4.JustPressed())
                {
                    this.ReadMarkPoint(4);
                }
                else if (this.Config.ReadMarkPoint5.JustPressed())
                {
                    this.ReadMarkPoint(5);
                }
                else if (this.Config.ReadMarkPoint6.JustPressed())
                {
                    this.ReadMarkPoint(6);
                }
                else if (this.Config.ReadMarkPoint7.JustPressed())
                {
                    this.ReadMarkPoint(7);
                }
                else if (this.Config.ReadMarkPoint8.JustPressed())
                {
                    this.ReadMarkPoint(8);
                }
                else if (this.Config.ReadMarkPoint9.JustPressed())
                {
                    this.ReadMarkPoint(9);
                }
                //建筑列表上翻页
                if (this.Config.CarpenterBuildUp.JustPressed())
                {
                    this.BuildIndexUp();
                }
                //建筑列表下翻页
                if (this.Config.CarpenterBuildDown.JustPressed())
                {
                    this.BuildIndexDown();
                }
                //建筑列表项点击
                if (this.Config.CarpenterBuildEnter.JustPressed())
                {
                    this.ClickBuild();
                }
                if(this.Config.CarpenterReadMousePosition.JustPressed())
                {
                    this.readMouse();
                }
                } else if(Game1.activeClickableMenu is CarpenterMenu carpenteri && this.Helper.Reflection.GetField<bool>(Game1.activeClickableMenu, "onFarm").GetValue() == false)
                {
                    this.BuildIndex = 0;
                this.choseBuild = null;
                }

        }

        //收录当前光标
        private void SetMarkCursor(int index)
        {
            if (!(Game1.activeClickableMenu is CarpenterMenu car)) return;
            int x = (int)((Game1.viewport.X + Game1.getOldMouseX(ui_scale: false)) / 64);
            int y = (int)((Game1.viewport.Y + Game1.getOldMouseY(ui_scale: false)) / 64);
            Dictionary<string, dynamic> dt = new Dictionary<string, dynamic>();
            dt.Add("X", x);
            dt.Add("Y", y);
            this.MarkData.MarkData[index] = dt;
            this.Helper.Data.WriteJsonFile("assets/MarkData.json", this.MarkData);
            this.api.Say("收录成功！", true);
        }


        //朗读鼠标位置
        private void readMouse()
        {
            this.api.Say($"{Game1.getOldMouseX(ui_scale:false)} ， {Game1.getOldMouseY(ui_scale:false)}", true);
        }


        //浏览mark
        private void ReadMarkPoint(int index)
        {
            if (this.MarkData.MarkData[index].Count == 0)
            {
                this.api.Say($"{index}： 空", true);
            } else
            {
                int x = (int)this.MarkData.MarkData[index]["X"];
                int y = (int)this.MarkData.MarkData[index]["Y"];
                string locationname = "Farm";
                this.api.Say($"{index}： {locationname} {x}， {y}", true);
            }
        }

        //设定mark
        private void SetMarkPoint(int index)
        {
            if(Game1.player.currentLocation.name != "Farm")
            {
                this.api.Say("只能收录农场内的坐标", true);
            } else
            {
                int x = (int)Game1.player.Tile.X;
                int y = (int)Game1.player.Tile.Y;
                Dictionary<string, dynamic> dt = new Dictionary<string, dynamic>();
                dt.Add("X", x);
                dt.Add("Y", y);
                this.MarkData.MarkData[index] = dt;
                this.Helper.Data.WriteJsonFile("assets/MarkData.json", this.MarkData);
                this.api.Say("收录成功！", true);
            }
       }

        //点击mark
        private void ClickMarkPoint(int index)
        {
            if (this.MarkData.MarkData[index].Count == 0)
            {
                this.api.Say("无收录内容", true);
            } else
            {
                int x = (int)this.MarkData.MarkData[index]["X"];
                int y = (int)this.MarkData.MarkData[index]["Y"];
                this.ClickPoint(x, y);
            }
        }

        //建筑索引上移
        private void BuildIndexUp()
        {
            if (Game1.locations[0].buildings.Count <= 1) return;
            this.BuildIndex--;
            if (this.BuildIndex < 1) this.BuildIndex = Game1.locations[0].buildings.Count - 1;
            this.ReadBuildName();
            Game1.playSound("shwip");
        }

        //建筑索引下移
        private void BuildIndexDown()
        {
            if (Game1.locations[0].buildings.Count <= 1) return;
            this.BuildIndex++;
            if (this.BuildIndex >= Game1.locations[0].buildings.Count) this.BuildIndex = 1;
            this.ReadBuildName();
            Game1.playSound("shwip");
        }

        //朗读建筑名称
        private void ReadBuildName()
        {
            string name = Game1.locations[0].buildings[this.BuildIndex].GetData().Name;
            MatchCollection match = Regex.Matches(name, @"(?<=\s)(\S+)(?=\])");
            string text = Game1.content.LoadString(match[0].Value);
            this.api.Say($"{this.BuildIndex}：{text}", true);
        }

        //点击建筑
        private void ClickBuild()
        {
            if (this.BuildIndex == 0) return;
            this.ClickPoint(Game1.locations[0].buildings[this.BuildIndex].tileX.Value, Game1.locations[0].buildings[this.BuildIndex].tileY.Value);
        }

        //获取建筑
        private Building GetBuild(int x, int y)
        {
            foreach(Building build in Game1.locations[0].buildings)
            {
                if((build.tileX.Value <= x && x<= build.tileX.Value + build.tilesWide -1) && (build.tileY.Value <= y && y<= build.tileY.Value+build.tilesHigh -1))
                {
                    return build;
                }
            }
            return null;
        }

        //点击坐标
        private void ClickPoint(int x, int y)
        {
            //移动建筑
            if (this.Helper.Reflection.GetField<bool>(Game1.activeClickableMenu, "moving").GetValue())
            {
                if (this.choseBuild == null)
                {
                    this.choseBuild = this.GetBuild(x, y);
                    if (this.choseBuild == null) return;
                    this.api.Say("已选中", true);
                } else
                {
                    this.MoveBuild(this.choseBuild, new Vector2((float)x, (float)y));
                }
            } else if(this.Helper.Reflection.GetField<bool>(Game1.activeClickableMenu, "demolishing").GetValue())
            {
                //拆毁建筑
                Building build = this.GetBuild(x, y);
                    if (build == null) return;
                this.DemolishBuild(build);
                } else if (this.Helper.Reflection.GetField<bool>(Game1.activeClickableMenu, "upgrading").GetValue())
            {
                //升级建筑
                Building build = this.GetBuild(x, y);
                if (build == null) return;
                this.UpgradingBuild(build);
            } else if (this.Helper.Reflection.GetField<bool>(Game1.activeClickableMenu, "painting").GetValue())
            {
                //油漆建筑
            }
            else
            {
                    //建造建筑
                    Building bd = this.GetBuild(x, y);
                    if (bd == null) this.ConstructBuild(new Vector2((float)x, (float)y));
            }

        }

        //建造建筑
        private void ConstructBuild(Vector2 position)
        {
            this.building = true;
            Game1.viewport.X = (int)((position.X * 64) - Game1.getOldMouseX(ui_scale: false));
            Game1.viewport.Y = (int)((position.Y * 64) - Game1.getOldMouseY(ui_scale: false));

            Game1.player.team.buildLock.RequestLock(delegate
            {
                if(Game1.locationRequest == null)
                {
                    if(this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "tryToBuild").Invoke<bool>())
                    {
                        this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "ConsumeResources").Invoke();
                        this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "returnToCarpentryMenuAfterSuccessfulBuild").Invoke();
                        this.Helper.Reflection.GetField<bool>(Game1.activeClickableMenu, "freeze").SetValue(true);
                    } else
                    {
                        this.api.Say("建造失败", true);
                    }
                }
                Game1.player.team.buildLock.ReleaseLock();
            });
            this.building = false;
        }


        //升级建筑
        private void UpgradingBuild(Building build)
        {
            if (build == null) return;

            if(build.buildingType.Value == this.Helper.Reflection.GetField<CarpenterMenu.BlueprintEntry>(Game1.activeClickableMenu, "Blueprint").GetValue().UpgradeFrom)
            {
                this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "ConsumeResources").Invoke();
                build.upgradeName.Value = this.Helper.Reflection.GetField<CarpenterMenu.BlueprintEntry>(Game1.activeClickableMenu, "Blueprint").GetValue().Id;
                build.daysUntilUpgrade.Value = Math.Max(this.Helper.Reflection.GetField<CarpenterMenu.BlueprintEntry>(Game1.activeClickableMenu, "Blueprint").GetValue().BuildDays, 1);
                build.showUpgradeAnimation(Game1.locations[0]);
                Game1.playSound("axe");
                this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "returnToCarpentryMenuAfterSuccessfulBuild").Invoke();
                if (this.Helper.Reflection.GetField<CarpenterMenu.BlueprintEntry>(Game1.activeClickableMenu, "Blueprint").GetValue().BuildDays < 1)
                {
                    build.FinishConstruction();
                } else
                {
                    Game1.netWorldState.Value.MarkUnderConstruction(this.Helper.Reflection.GetField<string>(Game1.activeClickableMenu, "Builder").GetValue(), build);
                }
            }
            else {
                this.api.Say("升级失败", true);
            }
        }


        //拆毁建筑
        private void DemolishBuild(Building build)
        {
            if (build == null) return;
            if (Game1.locations[0].destroyStructure(build))
            {
                Game1.flashAlpha = 1f;
                build.showDestroyedAnimation(Game1.locations[0]);
                Game1.playSound("explosion");
                Utility.spreadAnimalsAround(build, Game1.locations[0]);
            } else
            {
                this.api.Say("拆毁失败", true);
            }
        }

//移动建筑
private void MoveBuild(Building build , Vector2 position)
        {
            if (build == null) return;
            if (!Game1.locations[0].buildStructure(build, position, Game1.player))
            {
                this.api.Say("移动失败", true);
                Game1.playSound("cancel");
            }
            build.performActionOnBuildingPlacement();
            this.choseBuild = null;
            Game1.playSound("axchop");
        }



    }
}