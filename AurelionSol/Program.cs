namespace Flowers_AurelionSol
{
    using Common;
    using System;
    using System.Linq;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Color = System.Drawing.Color;
    using SharpDX;

    internal class Program
    {
        public static Menu Menu;
        private static Obj_AI_Hero Me;
        private static int SkinID;
        private static Spell Q, W, E, R;
        private static Orbwalking.Orbwalker Orbwalker;
        private static MissileClient qMillile;
        private static HpBarDraw HpBarDraw = new HpBarDraw();

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "AurelionSol")
            {
                return;
            }

            Me = ObjectManager.Player;

            SkinID = Me.BaseSkinId;

            Q = new Spell(SpellSlot.Q, 700f);
            W = new Spell(SpellSlot.W, 675f);
            E = new Spell(SpellSlot.E, 400f);
            R = new Spell(SpellSlot.R, 1550f);

            Q.SetSkillshot(0.40f, 180, 800, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.25f, 180, 1750, false, SkillshotType.SkillshotLine);

            Menu = new Menu("Flowers' AurelionSol", "NightMoon", true);

            var OrbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            {
                Orbwalker = new Orbwalking.Orbwalker(OrbwalkerMenu);
            }

            var Combo = Menu.AddSubMenu(new Menu("Combo", "Combo"));
            {
                Combo.AddItem(new MenuItem("ComboQ", "Use Q", true).SetValue(true));
                Combo.AddItem(new MenuItem("ComboQFollow", "Auto Follow Q", true).SetValue(true));
                Combo.AddItem(new MenuItem("ComboW", "Use W", true).SetValue(true));
                Combo.AddItem(new MenuItem("ComboR", "Use R", true).SetValue(true));
                Combo.AddItem(new MenuItem("ComboRHit", "R Min HitChance Counts", true).SetValue(new Slider(2, 1, 5)));
            }

            var Harass = Menu.AddSubMenu(new Menu("Harass", "Harass"));
            {
                Harass.AddItem(new MenuItem("HarassQ", "Use Q", true).SetValue(true));
                Harass.AddItem(new MenuItem("HarassW", "Use W", true).SetValue(true));
                Harass.AddItem(new MenuItem("HarassMana", "Min Harass Mana < %", true).SetValue(new Slider(40)));
            }

            var LaneClear = Menu.AddSubMenu(new Menu("LaneClear", "LaneClear"));
            {
                LaneClear.AddItem(new MenuItem("LaneClearQ", "Use Q", true).SetValue(true));
                LaneClear.AddItem(new MenuItem("LaneClearW", "Use W", true).SetValue(true));
                LaneClear.AddItem(
                    new MenuItem("LaneClearMana", "When Player ManaPercent >= x%", true).SetValue(new Slider(40)));
            }

            var JungleClear = Menu.AddSubMenu(new Menu("[FL] Mana Control", "nightmoon.Mana.Menu"));
            {
                JungleClear.AddItem(new MenuItem("JungleClearQ", "Use Q", true).SetValue(true));
                JungleClear.AddItem(new MenuItem("JungleClearW", "Use W", true).SetValue(true));
                JungleClear.AddItem(
                    new MenuItem("JungleClearMana", "When Player ManaPercent >= x%", true).SetValue(new Slider(40)));
            }

            var KillSteal = Menu.AddSubMenu(new Menu("KillSteal", "KillSteal"));
            {
                KillSteal.AddItem(new MenuItem("KillStealQ", "Use Q", true).SetValue(true));
                KillSteal.AddItem(new MenuItem("KillStealR", "Use R", true).SetValue(true));
            }

            var Misc = Menu.AddSubMenu(new Menu("Misc", "Misc"));
            {
                Misc.AddItem(new MenuItem("Interrupt", "Interrupt Spell", true));
                Misc.AddItem(new MenuItem("InterruptQ", "Use Q", true).SetValue(true));
                Misc.AddItem(new MenuItem("InterruptR", "Use R", true).SetValue(false));
                Misc.AddItem(new MenuItem("GapCloser", "Anti GapCloser", true));
                Misc.AddItem(new MenuItem("GapCloserQ", "Use Q", true).SetValue(true));
                Misc.AddItem(new MenuItem("GapCloserR", "Use R", true).SetValue(false));
            }

            var PredMenu = Menu.AddSubMenu(new Menu("Prediction", "Prediction"));
            {
                PredMenu.AddItem(new MenuItem("SelectPred", "Select Prediction: ", true).SetValue(new StringList(new[]
                {
                    "Common Prediction", "OKTW Prediction", "SDK Prediction", "SPrediction(Need F5 Reload)",
                    "xcsoft AIO Prediction"
                }, 1)));
                PredMenu.AddItem(
                    new MenuItem("SetHitchance", "HitChance: ", true).SetValue(
                        new StringList(new[] { "VeryHigh", "High", "Medium", "Low" })));
                PredMenu.AddItem(new MenuItem("AboutCommonPred", "Common Prediction -> LeagueSharp.Commmon Prediction"));
                PredMenu.AddItem(new MenuItem("AboutOKTWPred", "OKTW Prediction -> Sebby' Prediction"));
                PredMenu.AddItem(new MenuItem("AboutSDKPred", "SDK Prediction -> LeagueSharp.SDKEx Prediction"));
                PredMenu.AddItem(new MenuItem("AboutSPred", "SPrediction -> Shine' Prediction"));
                PredMenu.AddItem(new MenuItem("AboutxcsoftAIOPred", "xcsoft AIO Prediction -> xcsoft ALL In One Prediction"));
            }

            var SkinMenu = Menu.AddSubMenu(new Menu("SkinChance", "SkinChance"));
            {
                SkinMenu.AddItem(new MenuItem("EnableSkin", "Enabled", true).SetValue(false)).ValueChanged += EnbaleSkin;
                SkinMenu.AddItem(
                    new MenuItem("SelectSkin", "Select Skin: ", true).SetValue(
                        new StringList(new[]
                        {
                            "Classic", "Ashen Lord Aurelion Sol"
                        })));
            }

            var Draw = Menu.AddSubMenu(new Menu("[FL] Draw Menu", "nightmoon.Draw.Menu"));
            {
                Draw.AddItem(new MenuItem("DrawQ", "Q Range", true).SetValue(new Circle(false, Color.Azure)));
                Draw.AddItem(new MenuItem("DrawW", "W Range", true).SetValue(new Circle(false, Color.Blue)));
                Draw.AddItem(new MenuItem("DrawE", "E Range", true).SetValue(new Circle(false, Color.DarkSalmon)));
                Draw.AddItem(new MenuItem("DrawR", "R Range", true).SetValue(new Circle(false, Color.Red)));
                Draw.AddItem(new MenuItem("DrawDamage", "Draw Combo Damage", true).SetValue(true));
            }

            if (Menu.Item("SelectPred", true).GetValue<StringList>().SelectedIndex == 3)
            {
                SPrediction.Prediction.Initialize(PredMenu);
            }

            Menu.AddToMainMenu();

            Game.PrintChat(
                "<font color='#2848c9'>Flowers' AurelionSol</font> --> <font color='#b756c5'>Load! </font> <font size='30'><font color='#d949d4'>Good Luck!</font></font>");

            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Game.OnUpdate += OnUpdate;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDelete += OnDelete;
            Drawing.OnDraw += OnDraw;
        }

        private static void EnbaleSkin(object obj, OnValueChangeEventArgs Args)
        {
            if (!Args.GetNewValue<bool>())
            {
                ObjectManager.Player.SetSkin(ObjectManager.Player.ChampionName, SkinID);
            }
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser Args)
        {
            var sender = Args.Sender;

            if (!sender.IsEnemy)
            {
                return;
            }

            if (Q.IsReady() && Menu.GetBool("GapCloserQ"))
            {
                var QPred = Q.GetPrediction(sender);

                if (sender.IsValidTarget(Q.Range))
                {
                    if (QPred.Hitchance >= HitChance.VeryHigh)
                    {
                        Q.Cast(QPred.CastPosition);
                    }
                }
            }

            if (R.IsReady() && Menu.GetBool("GapCloserR"))
            {
                var RPred = R.GetPrediction(sender);

                if (sender.IsValidTarget(R.Range))
                {
                    if (RPred.Hitchance >= HitChance.VeryHigh)
                    {
                        R.Cast(RPred.CastPosition);
                    }
                }
            }
        }

        private static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs Args)
        {
            if (sender.IsMe || sender.IsAlly)
            {
                return;
            }

            if (Args.DangerLevel >= Interrupter2.DangerLevel.Medium)
            {
                if (Q.IsReady() && Menu.GetBool("GapCloserQ"))
                {
                    var QPred = Q.GetPrediction(sender);

                    if (sender.IsValidTarget(Q.Range))
                    {
                        if (QPred.Hitchance >= HitChance.VeryHigh)
                        {
                            Q.Cast(QPred.CastPosition);
                        }
                    }
                }

                if (Args.DangerLevel == Interrupter2.DangerLevel.High)
                {
                    if (R.IsReady() && Menu.GetBool("GapCloserR"))
                    {
                        var RPred = R.GetPrediction(sender);

                        if (sender.IsValidTarget(R.Range))
                        {
                            if (RPred.Hitchance >= HitChance.VeryHigh)
                            {
                                R.Cast(RPred.CastPosition);
                            }
                        }
                    }
                }
            }
        }

        private static void OnUpdate(EventArgs Args)
        {
            if (Me.IsDead || Me.IsRecalling())
            {
                return;
            }

            if (Menu.Item("EnableSkin", true).GetValue<bool>())
            {
                ObjectManager.Player.SetSkin(ObjectManager.Player.ChampionName,
                    Menu.Item("SelectSkin", true).GetValue<StringList>().SelectedIndex);
            }

            AutoKillStealLogic();

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    ComboLogic();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    HarassLogic();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClearLogic();
                    JungleClearLogic();
                    break;
            }
        }

        private static void AutoKillStealLogic()
        {
            foreach(var e in HeroManager.Enemies.Where(em => em.IsValidTarget() && !em.IsZombie && !em.IsDead))
            {
                if (Q.IsReady() && Menu.GetBool("KillStealQ"))
                {
                    if (e.Health + e.MagicalShield + 50 < GetQDamage(e))
                    {
                        Q.CastTo(e);
                    }
                }

                if (R.IsReady() && Menu.GetBool("KillStealR"))
                {
                    if (e.Health + e.MagicalShield + 50 < GetRDamage(e))
                    {
                        R.CastTo(e);
                    }
                }
            }
        }

        private static void JungleClearLogic()
        {
            if (Me.ManaPercent > Menu.GetSlider("JungleClearMana"))
            {
                if (Q.IsReady() && Menu.GetBool("JungleClearQ"))
                {
                    var QMob = MinionManager.GetMinions(Me.Position, Q.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

                    if (QMob != null)
                    {
                        foreach(var mob in QMob)
                        {
                            if (mob.IsValidTarget(Q.Range))
                                Q.Cast(mob);
                        }
                    }
                }

                if (HavePassive && Menu.GetBool("JungleClearW"))
                {
                    var WMob = MinionManager.GetMinions(Me.Position, W.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

                    if (WMob != null)
                    {
                        foreach (var mob in WMob)
                        {
                            if (mob.IsValidTarget(W.Range) && !mob.IsValidTarget(420) && !IsWActive)
                            {
                                W.Cast();
                            }
                            else if (IsWActive && mob.IsValidTarget(420))
                            {
                                W.Cast();
                            }
                        }
                    }
                }
            }
        }

        private static void LaneClearLogic()
        {
            if (Me.ManaPercent > Menu.GetSlider("LaneClearMana"))
            {
                if (Menu.GetBool("LaneClearQ") && Q.IsReady())
                {
                    var QMin = MinionManager.GetMinions(Me.Position, Q.Range);
                    var FarmLocation = Q.GetCircularFarmLocation(QMin, Q.Width);

                    if (QMin != null)
                    {
                        if (FarmLocation.MinionsHit >= 3)
                        {
                            Q.Cast(FarmLocation.Position);
                        }
                    }
                }

                if (Menu.GetBool("LaneClearW") && HavePassive)
                {
                    var WMin = MinionManager.GetMinions(Me.Position, W.Range);

                    if (WMin?.Count >= 2)
                    {
                        W.Cast();
                    }
                }
            }
        }

        private static void HarassLogic()
        {
            if (Me.ManaPercent > Menu.GetSlider("HarassMana"))
            {
                if (Menu.GetBool("HarassQ"))
                {
                    var target = TargetSelector.GetTarget(Q.Range * 2, TargetSelector.DamageType.Magical);

                    if (Q.IsReady() && target.IsValidTarget(Q.Range) && !SecondQ)
                    {
                        Q.CastTo(target);
                    }

                    if (qMillile != null && SecondQ)
                    {
                        var QSize = qMillile.StartPosition.Distance(qMillile.Position);
                        var QRange = (QSize + Q.Width) / 15 * ((QSize + Q.Width) / 15);

                        if (targetInRange(target.ServerPosition, QRange))
                        {
                            Q.Cast();
                        }
                    }
                }

                if (Menu.GetBool("HarassW") && HavePassive && W.IsReady())
                {
                    var WTTT = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);

                    if (!IsWActive)
                    {
                        if (WTTT != null && WTTT.IsValidTarget(W.Range) && !WTTT.IsValidTarget(420))
                        {
                            W.Cast();
                        }
                    }
                    else if (IsWActive)
                    {
                        if (!(WTTT.Distance(Me.ServerPosition) < 840) || WTTT.IsValidTarget(420))
                        {
                            W.Cast();
                        }
                    }
                }
            }
        }

        private static void ComboLogic()
        {
            if (Menu.GetBool("ComboQ"))
            {
                var target = TargetSelector.GetTarget(Q.Range * 2, TargetSelector.DamageType.Magical);

                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {
                    Q.CastTo(target);
                }
            }

            if (Menu.GetBool("ComboQFollow"))
            {
                if (SecondQ && qMillile != null)
                {
                    Orbwalker.SetMovement(false);
                    Orbwalker.SetAttack(false);
                    Me.IssueOrder(GameObjectOrder.MoveTo, qMillile.Position);
                }
                else if (!SecondQ && qMillile == null)
                {
                    Orbwalker.SetAttack(true);
                    Orbwalker.SetMovement(true);
                }
            }

            if (Menu.GetBool("ComboW") && HavePassive && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);

                if (!IsWActive)
                {
                    if (target != null && target.IsValidTarget(W.Range) && !target.IsValidTarget(420))
                    {
                        W.Cast();
                    }
                }
                else if (IsWActive)
                {
                    if (!target.IsValidTarget(800) || target.IsValidTarget(420))
                    {
                        W.Cast();
                    }
                }
            }
            
            if (Menu.GetBool("ComboR") && R.IsReady())
            {
                foreach (var enemy in from enemy in HeroManager.Enemies
                                      let startPos = enemy.ServerPosition
                                      let endPos = Me.ServerPosition.Extend(startPos, Me.Distance(enemy) + R.Range)
                                      let rectangle = new Geometry.Polygon.Rectangle(startPos, endPos, R.Width)
                                      where HeroManager.Enemies.Count(rectangle.IsInside) >= Menu.GetSlider("ComboRHit")
                                      select enemy)
                {
                    R.CastTo(enemy);
                }
            }
        }

        private static bool targetInRange(Vector3 TargetPos, float range)
        {
            return qMillile.Position.To2D().Distance(TargetPos.To2D(), true) < range;
        }

        private static void OnCreate(GameObject sender, EventArgs args)
        {
            var SpellQ = sender as MissileClient;

            if (SpellQ != null)
            {
                if (SpellQ.IsValid && SpellQ.SpellCaster.IsMe && SpellQ.SpellCaster.IsValid &&
                    SpellQ.SData.Name.Contains("AurelionSolQMissile"))
                {
                    qMillile = SpellQ;
                }
            }
        }

        private static void OnDelete(GameObject sender, EventArgs args)
        {
            var SpellQ = sender as MissileClient;

            if (SpellQ != null)
            {
                if (SpellQ.IsValid && SpellQ.SpellCaster is Obj_AI_Hero)
                {
                    if (SpellQ.SpellCaster.IsMe && SpellQ.SpellCaster.IsValid &&
                        SpellQ.SData.Name.Contains("AurelionSolQMissile"))
                    {
                        qMillile = null;
                    }
                }
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (Me.IsDead)
            {
                return;
            }

            if (Menu.GetDraw("DrawQ") && Q.IsReady())
            {
                Render.Circle.DrawCircle(Me.Position, Q.Range, Menu.GetColor("DrawQ"));
            }

            if (Menu.GetDraw("DrawW") && HavePassive)
            {
                Render.Circle.DrawCircle(Me.Position, IsWActive ? 675f : 420f, Menu.GetColor("DrawW"));
            }

            if (Menu.GetDraw("DrawE") && E.IsReady())
            {
                Render.Circle.DrawCircle(Me.Position, E.Range, Menu.GetColor("DrawE"));
            }

            if (Menu.GetDraw("DrawR") && R.IsReady())
            {
                Render.Circle.DrawCircle(Me.Position, R.Range, Menu.GetColor("DrawR"));
            }

            if (Menu.Item("DrawDamage", true).GetValue<bool>())
            {
                foreach (var e in ObjectManager.Get<Obj_AI_Hero>().Where(e => e.IsValidTarget() && !e.IsDead && !e.IsZombie))
                {
                    HpBarDraw.Unit = e;
                    HpBarDraw.DrawDmg(GetComboDamage(e), new ColorBGRA(255, 204, 0, 170));
                }
            }
        }

        private static double GetQDamage(Obj_AI_Base t)
        {
            return Me.CalcDamage
            (t, Damage.DamageType.Magical,
                (float) new double[] {70, 110, 150, 190, 230}[Q.Level - 1] + 0.65f*
                Me.TotalMagicalDamage);
        }

        private static double GetRDamage(Obj_AI_Base t)
        {
            return Me.CalcDamage
            (t, Damage.DamageType.Magical,
                (float) new double[] {200, 400, 600}[R.Level - 1] + 0.70f*
                Me.TotalMagicalDamage);
        }

        private static bool IsWActive => Me.HasBuff("AurelionSolWActive");

        private static bool HavePassive => Me.HasBuff("AurelionSolPassive");

        private static bool SecondQ => Me.HasBuff("AurelionSolQHaste");

        private static float GetComboDamage(Obj_AI_Hero target)
        {
            double Damage = 0;

            if (Q.IsReady())
                Damage += GetQDamage(target);

            if (W.IsReady())
                Damage += Me.GetSpellDamage(target, SpellSlot.W);

            if (E.IsReady())
                Damage += Me.GetSpellDamage(target, SpellSlot.E);

            if (R.IsReady())
                Damage += GetRDamage(target);

            if (target.HasBuff("KindredRNoDeathBuff"))
            {
                return 0;
            }

            if (target.HasBuff("UndyingRage") && target.GetBuff("UndyingRage").EndTime - Game.Time > 0.3)
            {
                return 0;
            }

            if (target.HasBuff("JudicatorIntervention"))
            {
                return 0;
            }

            if (target.HasBuff("ChronoShift") && target.GetBuff("ChronoShift").EndTime - Game.Time > 0.3)
            {
                return 0;
            }

            if (target.HasBuff("FioraW"))
            {
                return 0;
            }

            return (float)Damage;
        }
    }
}
