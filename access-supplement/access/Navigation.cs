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
using StardewValley.Pathfinding;
using StardewValley.TerrainFeatures;
using StardewValley.Locations;
using xTile.Dimensions;
using StardewValley.Monsters;
using FMOD;

namespace AccessSupplement
{
    public class NavigationPoint
    {
        public string DisplayName;
        public string Name;
        public Vector2 Position;
        public string PointType;
        public bool IsFestival = false;
        public string FestivalId = null;
        public string LocationName = null;
    }

    public class Navigation
    {
        private List<string> Options = new List<string>() {"交互点", "传送点"};
        private int OptionsIndex = 0;
        private int CurrentIndex = 0;
        private Vector2 CurrentPositionOption = new Vector2(0, 0);
        private int PlayerX = 0;
        private int PlayerY = 0;
        private bool IsMove = false;
        private ModNavigation NavigationData;
        private NamingMenu NameMenu;
        private int MonsterIndex = 0;
        private IModHelper Helper;
        private ModConfig Config;
        private API api;
        //FMOD
        private FMOD.RESULT FmodResult;
        private FMOD.System FmodSystem;
        private FMOD.Channel ChannelEffect;
        private FMOD.ChannelGroup ChannelGroup;
        private FMOD.Sound SoundEffect1;
        private FMOD.Sound SoundEffect2;



        public Navigation(IModHelper helper, ModConfig config, API api)
        {
            this.NavigationData = helper.Data.ReadJsonFile<ModNavigation>("assets/NavigationData.json") ?? new ModNavigation();
            this.Helper = helper;
            this.Config = config;
            this.api=api;
            this.FmodResult = FMOD.Factory.System_Create(out this.FmodSystem);
            this.FmodResult = this.FmodSystem.init(32, INITFLAGS.NORMAL, IntPtr.Zero);
            this.FmodSystem.set3DSettings(1.0f, 1.0f, 0.3f);
            this.ChannelGroup = new FMOD.ChannelGroup();
            this.FmodSystem.createSound(Path.Combine(this.Helper.DirectoryPath, "assets", "sounds", $"1.wav"), FMOD.MODE.LOOP_OFF | FMOD.MODE._3D, out this.SoundEffect1);
            this.SoundEffect1.set3DMinMaxDistance(0.5f, 30.0f);
        }



        //时时监听
        public void OnGameLoop(API api)
        {
            if(Game1.player.controller!=null && this.IsMove)
            {
                if(Game1.activeClickableMenu != null)
                {
                    Game1.player.controller = null;
                    api.Say("寻路结束", true);
                    return;
                }
                if(this.PlayerX != Game1.player.Tile.X || this.PlayerY != Game1.player.Tile.Y)
                {
                    this.PlayerX = (int)Game1.player.Tile.X;
                    this.PlayerY = (int)Game1.player.Tile.Y;
                    Game1.player.currentLocation.playTerrainSound(new Vector2(this.PlayerX, this.PlayerY));
                }
if(Math.Sqrt((this.PlayerX-Game1.player.controller.endPoint.X)*(this.PlayerX-Game1.player.controller.endPoint.X)+(this.PlayerY-Game1.player.controller.endPoint.Y)*(this.PlayerY-Game1.player.controller.endPoint.Y)) <=1.5)
                {
                    api.Say($"到达目标附近，方向{this.GetDirection(new Vector2(Game1.player.controller.endPoint.X, Game1.player.controller.endPoint.Y))}", true);
                    Game1.player.controller = null;
                }
            } else
            {
                this.IsMove = false;
            }
            this.FmodSystem.update();
        }


//键盘监听
        public void OnKeyPressed(object sender, ButtonPressedEventArgs e,IModHelper helper ,ModConfig config, API api)
        {
            if(Game1.activeClickableMenu == null)
            {
                if (config.NavigationMonsterDistanceKey.JustPressed())
                {
                    this.Config.NavigationMonsterDistance = !this.Config.NavigationMonsterDistance;
                    if (this.Config.NavigationMonsterDistance)
                    {
                        api.Say($"距离排序：开", true);
                    }
                    else
                    {
                        api.Say($"距离排序：关", true);
                    }
                    this.Helper.WriteConfig<ModConfig>(this.Config);
                }
                else if (config.NavigationMonsterAudioBrowserKey.JustPressed())
                {
                    this.Config.NavigationMonsterAudioBrowser = !this.Config.NavigationMonsterAudioBrowser;
                    if (this.Config.NavigationMonsterAudioBrowser)
                    {
                        api.Say("音频指向：开", true);
                    }
                    else
                    {
                        api.Say("音频指向：关", true);
                    }
                    this.Helper.WriteConfig<ModConfig>(this.Config);
                }
                else if (config.NavigationMonsterUp.JustPressed())
                {
                    this.ReadMonsterUp(api);
                }
                else if (config.NavigationMonsterDown.JustPressed())
                {
                    this.ReadMonsterDown(api);
                }
                else if (config.NavigationMonster.JustPressed())
                {
                    this.LaunchMonsterMove(api);
                }
                if (config.NavigationOpenPanel.JustPressed())
                {
                    this.OpenPanel(config, api);
                }
                if(config.NavigationStopMove.JustPressed() && Game1.player.controller!=null)
                {
                    Game1.player.controller = null;
                    api.Say("寻路结束", true);
                    helper.Input.Suppress(SButton.Escape);
                }
            }
            if (Game1.activeClickableMenu == this.NameMenu && this.NameMenu != null)
            {
                if (this.Helper.Input.IsDown(SButton.Escape))
                {
                    this.NameMenuEscape();
                }
            }
            if (Game1.activeClickableMenu is NavigationPanel p)
            {
                if(config.NavigationOptionsUp.JustPressed())
                {
                    this.OptionsUp(api);
                } else if(config.NavigationOptionsDown.JustPressed())
                {
                    this.OptionsDown(api);
                } else if (config.NavigationItemUp.JustPressed())
                {
                    this.ItemUp(api);
                } else if (config.NavigationItemDown.JustPressed())
                {
                    this.ItemDown(api);
                } else if (config.NavigationLaunchMove.JustPressed())
                {
                    this.LaunchWarp(this.CurrentPositionOption, api);
                } else if(config.NavigationModifyItem.JustPressed())
                {
                    this.ModifyItem();
                }
            }

        }

        //切换选项向上
        private void ItemUp(API api)
        {
            this.CurrentIndex--;
            //传送点
            if (this.Options[this.OptionsIndex] == "传送点")
            {
                List<NavigationPoint> warps = this.GetWarps();
                if (this.CurrentIndex < 0) this.CurrentIndex = warps.Count - 1;
                this.ReadCurrentItem(warps, api);
            } else if(this.Options[this.OptionsIndex] =="交互点")
            {
                List<NavigationPoint> objects = this.GetInteractives();
                if (this.CurrentIndex < 0) this.CurrentIndex = objects.Count - 1;
                this.ReadCurrentItem(objects, api);
            }
            Game1.playSound("shwip");
        }

        //选项向下
        private void ItemDown(API api)
        {
            this.CurrentIndex++;
            //传送点
            if (this.Options[this.OptionsIndex] == "传送点")
            {
                List<NavigationPoint> warps = this.GetWarps();
                if (this.CurrentIndex >= warps.Count) this.CurrentIndex = 0;
                this.ReadCurrentItem(warps, api);
            } else if(this.Options[this.OptionsIndex] =="交互点")
            {
                List<NavigationPoint> objects = this.GetInteractives();
                if (this.CurrentIndex >= objects.Count) this.CurrentIndex = 0;
                this.ReadCurrentItem(objects, api);
            }
            Game1.playSound("shwip");
        }


        //打开导航面板
        private void OpenPanel(ModConfig config,API api)
        {
            Game1.activeClickableMenu = new NavigationPanel(config);
            api.Say("导航面板", true);
            this.CurrentIndex = 0;
            Game1.playSound("bigSelect");
        }


        //寻路
        private void LaunchWarp(Vector2 position, API api)
        {
            if (position.X==0 && position.Y ==0) return;
            Game1.exitActiveMenu();
            this.CurrentIndex = 0;
            List<Vector2> result= this.GetMoveEndPoint(position);
            foreach (Vector2 p in result)
            {
                PathFindController controller = new(Game1.player, Game1.currentLocation, new Point((int)p.X, (int)p.Y), Game1.player.FacingDirection)
                {
                    allowPlayerPathingInEvent = true
                };
                if (controller.pathToEndPoint != null && controller.pathToEndPoint.Count > 0)
                {
                    Game1.player.controller = controller;
                    this.PlayerX = (int)Game1.player.Tile.X;
                    this.PlayerY = (int)Game1.player.Tile.Y;
                    Game1.player.controller.endPoint = new Point((int)p.X, (int)p.Y);
                }
                if (Game1.player.controller != null)
                {
                    this.IsMove = true;
                    api.Say("开始寻路", true);
                    return;
                }
            }
            if(result.Count==0 || this.IsMove==false)
            {
                api.Say($"未找到可用路径", true);
            }
        }

        //切换选择栏向上
        private void OptionsUp(API api)
        {
            this.OptionsIndex--;
            if (this.OptionsIndex < 0) this.OptionsIndex = this.Options.Count - 1;
            this.CurrentIndex = 0;
            api.Say($"{this.Options[this.OptionsIndex]}", true);
        }

        //切换选择栏向下
        private void OptionsDown(API api)
        {
            this.OptionsIndex++;
            if (this.OptionsIndex >= this.Options.Count) this.OptionsIndex =0;
            this.CurrentIndex = 0;
            api.Say($"{this.Options[this.OptionsIndex]}", true);
        }


        //获取扭曲点列表
        private List<NavigationPoint> GetWarps()
        {
            var warps =Game1.player.currentLocation.warps;
            List<NavigationPoint> result = new List<NavigationPoint>();
            foreach(Warp w in warps)
            {
                    NavigationPoint p = new NavigationPoint();
                    p.Name = w.TargetName;
                    p.DisplayName = Game1.getLocationFromName(p.Name).GetDisplayName();
                    p.PointType = "传送点";
                    p.Position = new Vector2(w.X, w.Y);
                p = this.ReplaceWarp(p);
                result = this.IsSavePoint(p, result);
            }

            foreach(Point key in Game1.player.currentLocation.doors.Keys)
            {
                NavigationPoint p = new NavigationPoint();
                p.DisplayName = Game1.getLocationFromName(Game1.player.currentLocation.doors[key]).GetDisplayName();
                p.Name = (string)Game1.player.currentLocation.doors[key];
                p.PointType = "传送点";
                p.Position = new Vector2(key.X, key.Y+1);
                p = this.ReplaceWarp(p);
                result = this.IsSavePoint(p, result);
            }

            return result;
        }

        //查重
        private List<NavigationPoint> IsSavePoint(NavigationPoint p, List<NavigationPoint> result)
        {
            List<NavigationPoint> returnResult = new List<NavigationPoint>();
            bool isAdd = false;
            foreach(NavigationPoint r in result)
            {
                if ((p.Name == r.Name && p.Position.X == r.Position.X) || (p.Name == r.Name && p.Position.Y == r.Position.Y))
                {
                    returnResult.Add(r);
                    isAdd = true;
                } else if (p.Name == r.Name && p.PointType == "传送点")
                {
                    returnResult.Add(r);
                    isAdd = true;
                }
                else
                {
                    returnResult.Add(r);
                }
            }
if(returnResult.Count==0 || !isAdd)
            {
                returnResult.Add(p);
            }
            return returnResult;
        }


        //朗读当前项目
        private void ReadCurrentItem(List<NavigationPoint> points, API api)
        {
            if (points.Count == 0) return;
            string name = points[this.CurrentIndex].DisplayName;
            Vector2 position = points[this.CurrentIndex].Position;
            this.CurrentPositionOption = position;
            api.Say($"{this.CurrentIndex}：{name}， {points[this.CurrentIndex].Name}（{position.X}，{position.Y}）", true);
        }

        //获取方向
        private string GetDirection(Vector2 target)
        {
            int px = (int)Game1.player.Tile.X;
            int py = (int)Game1.player.Tile.Y;
            if (px == target.X)
            {
                if (py < target.Y) return "南";
                if (py > target.Y) return "北";
            }
            if (py == target.Y)
            {
                if (px < target.X) return "东";
                if (px > target.X) return "西";
            }
            if (px < target.X)
            {
                if (py < target.Y) return "东南";
                if (py > target.Y) return "东北";
            }
            if (px > target.X)
            {
                if (py < target.Y) return "西南";
                if (py > target.Y) return "西北";
            }
            return "原地";
        }

        //获取可交互列表
        private List<NavigationPoint> GetInteractives()
        {
            List<NavigationPoint> result = new List<NavigationPoint>();
            int tileX = (int)(Game1.player.currentLocation.Map.DisplayHeight / 64);
            int tileY = (int)(Game1.player.currentLocation.Map.DisplayWidth / 64);
            for(int i =0;i<=tileX;i++)
            {
                for(int j=0;j<=tileY;j++)
                {
                    string[] array = ArgUtility.SplitBySpace(Game1.player.currentLocation.doesTileHaveProperty(i, j, "Action", "Buildings"));
                    if (Game1.player.currentLocation.ShouldIgnoreAction(array, Game1.player, new Location(i, j))) continue;
                    if (array[0] == "LockedDoorWarp" || array[0] == "EnterSewer") continue;
                    NavigationPoint p = new NavigationPoint();
                    p.DisplayName = "交互点";
                    p.Name = array[0];
                    p.Position = new Vector2(i, j);
                    p.PointType = "交互点";
                    p.IsFestival = Game1.isFestival();
                    if (p.IsFestival) p.FestivalId = $"{Game1.dayOfMonth}{Game1.Date.SeasonKey}";
                    p.LocationName = Game1.player.currentLocation.name;
                    p = this.ReplaceInteractive(p); 
                        result = this.IsSavePoint(p, result);
                }
            }
            return result;
        }

        //交互点替换
        private NavigationPoint ReplaceInteractive(NavigationPoint p)
        {
foreach(string key in this.NavigationData.NavigationData["交互点"].Keys)
            {
                NavigationPoint r = this.NavigationData.NavigationData["交互点"][key];
                if(p.Name==r.Name && p.Position == r.Position && p.FestivalId == r.FestivalId && p.LocationName == r.LocationName)
                {
                    p.DisplayName = r.DisplayName;
                }
            }
            return p;
        }

        //传送点替换
        private NavigationPoint ReplaceWarp(NavigationPoint p)
        {
            foreach (string key in this.NavigationData.NavigationData["传送点"].Keys)
            {
                NavigationPoint r = this.NavigationData.NavigationData["传送点"][key];
                if (p.Name == r.Name) p.DisplayName = r.DisplayName;
            }
            return p;
        }

        //命名框回调
        private void NavigationModifyBack(string text)
        {
            NavigationPoint p = new NavigationPoint();
            p.DisplayName = text;
            if (this.Options[this.OptionsIndex]=="交互点")
            {
                List<NavigationPoint> result = this.GetInteractives();
                if (result.Count == 0 || this.CurrentIndex == -1)
                {
                    Game1.exitActiveMenu();
                    Game1.playSound("bigDeSelect");
                }
                NavigationPoint r = result[this.CurrentIndex];
                p.Name = r.Name;
                p.PointType = r.PointType;
                p.Position = r.Position;
                p.IsFestival = r.IsFestival;
                p.FestivalId = r.FestivalId;
                p.LocationName = r.LocationName;
                string key = $"{p.Name}-{p.Position}-{p.LocationName}-{p.FestivalId}";
                if (this.NavigationData.NavigationData["交互点"].ContainsKey(key))
                {
                    this.NavigationData.NavigationData["交互点"][key] = p;
                }
                else
                {
                    this.NavigationData.NavigationData["交互点"].Add(key, p);
                }
                this.Helper.Data.WriteJsonFile("assets/NavigationData.json", this.NavigationData);
            }
            if (this.Options[this.OptionsIndex] == "传送点")
            {
                List<NavigationPoint> result = this.GetWarps();
                if (result.Count == 0 || this.CurrentIndex == -1)
                {
                    Game1.exitActiveMenu();
                    Game1.playSound("bigDeSelect");
                }
                NavigationPoint r = result[this.CurrentIndex];
                p.Name = r.Name;
                p.PointType = r.PointType;
                p.Position = r.Position;
                p.IsFestival = r.IsFestival;
                p.FestivalId = r.FestivalId;
                p.LocationName = r.LocationName;
                string key = $"{p.Name}-{p.Position}-{p.LocationName}-{p.FestivalId}";
                if (this.NavigationData.NavigationData["交互点"].ContainsKey(key))
                {
                    this.NavigationData.NavigationData["传送点"][key] = p;
                } else
                {
                    this.NavigationData.NavigationData["传送点"].Add(key, p);
                }
                this.Helper.Data.WriteJsonFile("assets/NavigationData.json", this.NavigationData);
            }
            this.Helper.Input.Suppress(SButton.Enter);
            this.OpenPanel(this.Config, this.api);
        }

        //命名项目
        private void ModifyItem()
        {
            StardewValley.Menus.NamingMenu.doneNamingBehavior done = new StardewValley.Menus.NamingMenu.doneNamingBehavior(this.NavigationModifyBack);
            this.NameMenu = new StardewValley.Menus.NamingMenu(done, "新名称", null);
            Game1.activeClickableMenu = this.NameMenu;
            Game1.playSound("bigSelect");
        }

        //命名框内按下ESC
        private void NameMenuEscape()
        {
            if (this.NameMenu.textBox.Selected)
            {
                this.NameMenu.textBox.Selected = false;
            } else {
                this.Helper.Input.Suppress(SButton.Escape);
                    Game1.exitActiveMenu();
                    Game1.playSound("bigDeSelect");
                }
            }

        //计算导航终点
        private List<Vector2> GetMoveEndPoint(Vector2 position)
        {
            List<Vector2> result = new List<Vector2>();
            if (Game1.player.currentLocation.isTilePassable(new Vector2(position.X, position.Y + 1))) result.Add(new Vector2(position.X, position.Y + 1));
            if (Game1.player.currentLocation.isTilePassable(new Vector2(position.X, position.Y - 1))) result.Add(new Vector2(position.X, position.Y - 1));
            if (Game1.player.currentLocation.isTilePassable(new Vector2(position.X + 1, position.Y))) result.Add(new Vector2(position.X + 1, position.Y));
            if (Game1.player.currentLocation.isTilePassable(new Vector2(position.X - 1, position.Y))) result.Add(new Vector2(position.X - 1, position.Y));
            if (Game1.player.currentLocation.isTilePassable(new Vector2(position.X + 1, position.Y + 1))) result.Add(new Vector2(position.X + 1, position.Y + 1));
            if (Game1.player.currentLocation.isTilePassable(new Vector2(position.X + 1, position.Y - 1))) result.Add(new Vector2(position.X + 1, position.Y - 1));
            if (Game1.player.currentLocation.isTilePassable(new Vector2(position.X - 1, position.Y + 1))) result.Add(new Vector2(position.X - 1, position.Y + 1));
            if (Game1.player.currentLocation.isTilePassable(new Vector2(position.X - 1, position.Y - 1))) result.Add(new Vector2(position.X - 1, position.Y - 1));
            return result;
        }

        //获取怪物列表
        private List<NPC> GetMonsters()
        {
            Dictionary<NPC, int> resultdict = new Dictionary<NPC, int>();
            foreach(var key in Game1.player.currentLocation.characters)
            {
                if (key.IsMonster)
                {
                    float mx = key.Tile.X;
                    float my = key.Tile.Y;
                    float px = Game1.player.Tile.X;
                    float py = Game1.player.Tile.Y;
                    int distance = (int)Math.Sqrt(Math.Pow(px - mx, 2) + Math.Pow(py - my, 2));
                    resultdict.Add(key,distance);
                }
            }
            if (this.Config.NavigationMonsterDistance)
            {
                resultdict = resultdict.OrderBy(o => o.Value).ToDictionary(f => f.Key, o => o.Value);
            }
            List<NPC> result = new List<NPC>();
            foreach(NPC n in resultdict.Keys)
            {
                result.Add(n);
            }
            return result;
        }

        //朗读上一个怪物
        private void ReadMonsterUp(API api)
        {
            Game1.playSound("shwip");
            List<NPC> result = this.GetMonsters();
            if (result.Count == 0) return;
            this.MonsterIndex--;
            if (this.MonsterIndex < 0) this.MonsterIndex = result.Count - 1;
            NPC monster = result[this.MonsterIndex];
            string name = monster.displayName;
            float mx = monster.Tile.X;
            float my = monster.Tile.Y;
            float px = Game1.player.Tile.X;
            float py = Game1.player.Tile.Y;
            int distance = (int)Math.Sqrt(Math.Pow(px - mx, 2) + Math.Pow(py - my, 2));
            int health = ((Monster)monster).Health;
            int maxHealth = ((Monster)monster).MaxHealth;
            api.Say($"{this.MonsterIndex} {name}，健康{health}/{maxHealth}，位于（{mx}，{my}），{this.GetDirection(new Vector2(mx, my))}方{distance}块瓷砖", true);
            if(this.Config.NavigationMonsterAudioBrowser) this.PlayMonster3D(new Vector2(mx, my));
        }

        //朗读下一个怪物
        private void ReadMonsterDown(API api)
        {
            Game1.playSound("shwip");
            List<NPC> result = this.GetMonsters();
            if (result.Count == 0) return;
            this.MonsterIndex++;
            if (this.MonsterIndex >= result.Count) this.MonsterIndex = 0;
            NPC monster = result[this.MonsterIndex];
            string name = monster.displayName;
            float mx = monster.Tile.X;
            float my = monster.Tile.Y;
            float px = Game1.player.Tile.X;
            float py = Game1.player.Tile.Y;
            int distance = (int)Math.Sqrt(Math.Pow(px - mx, 2) + Math.Pow(py - my, 2));
            int health = ((Monster)monster).Health;
            int maxHealth = ((Monster)monster).MaxHealth;
            api.Say($"{this.MonsterIndex} {name}，健康{health}/{maxHealth}，位于（{mx}，{my}），{this.GetDirection(new Vector2(mx, my))}方{distance}块瓷砖", true);
            if (this.Config.NavigationMonsterAudioBrowser) this.PlayMonster3D(new Vector2(mx, my));
        }

        //寻路怪物
        private void LaunchMonsterMove(API api)
        {
            List<NPC> result = this.GetMonsters();
            if(result.Count==0)
            {
                api.Say("无可导航目标", true);
                return;
            }
            if (this.MonsterIndex >= result.Count) this.MonsterIndex = 0;
            NPC monster = result[this.MonsterIndex];
            int mx = (int)monster.Tile.X;
            int my = (int)monster.Tile.Y;
            this.LaunchWarp(new Vector2(mx, my), api);
        }

        //播放怪物3D音频指向
        private void PlayMonster3D(Vector2 position)
        {
            this.FmodSystem.playSound(this.SoundEffect1, this.ChannelGroup, true, out this.ChannelEffect);
            FMOD.VECTOR soundPos = new FMOD.VECTOR() { x = position.X, y = 0.0f, z = position.Y };
            FMOD.VECTOR SoundVel = new FMOD.VECTOR() { x = 0.0f, y = 0.0f, z = 0.0f };
            this.ChannelEffect.set3DAttributes(ref soundPos, ref SoundVel);
            FMOD.VECTOR pos = new FMOD.VECTOR() { x = Game1.player.Tile.X, y = 0.0f, z = Game1.player.Tile.Y };
            FMOD.VECTOR forward = new FMOD.VECTOR { x = 0.0f, y = 0.0f, z = 1.0f };
            FMOD.VECTOR up = new FMOD.VECTOR { x = 0.0f, y = 1.0f, z = 0.0f };
            FMOD.VECTOR lvel = new FMOD.VECTOR { x = 0.0f, y = 0.0f, z = 0.0f };
            this.FmodSystem.set3DListenerAttributes(0, ref pos, ref lvel, ref forward, ref up);
            this.ChannelEffect.setPaused(false);
        }



    }
}