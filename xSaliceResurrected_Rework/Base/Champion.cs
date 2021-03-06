﻿namespace xSaliceResurrected_Rework.Base
{
    using LeagueSharp;
    using LeagueSharp.Common;
    using System;
    using Managers;

    public class Champion : SpellBase
    {
        protected readonly Obj_AI_Hero Player = ObjectManager.Player;

        protected Champion()
        {
            //Events
            Game.OnUpdate += Game_OnGameUpdateEvent;
            Drawing.OnDraw += Drawing_OnDrawEvent;
            Interrupter2.OnInterruptableTarget += Interrupter_OnPosibleToInterruptEvent;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloserEvent;
            AttackableUnit.OnDamage += ObjAiHeroOnOnDamage;

            GameObject.OnCreate += GameObject_OnCreateEvent;
            GameObject.OnDelete += GameObject_OnDeleteEvent;

            Spellbook.OnUpdateChargedSpell += Spellbook_OnUpdateChargedSpellEvent;
            Spellbook.OnCastSpell += SpellbookOnOnCastSpell;
            Spellbook.OnStopCast += SpellbookOnOnStopCast;

            xSaliceResurrected_Rework.Orbwalking.AfterAttack += AfterAttackEvent;
            xSaliceResurrected_Rework.Orbwalking.BeforeAttack += BeforeAttackEvent;
            xSaliceResurrected_Rework.Orbwalking.OnAttack += OnAttack;

            Obj_AI_Base.OnDoCast += Obj_AI_Base_OnDoCast;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCastEvent;
            Obj_AI_Base.OnIssueOrder += ObjAiHeroOnOnIssueOrderEvent;
            Obj_AI_Base.OnBuffAdd += ObjAiBaseOnOnBuffAdd;
            Obj_AI_Base.OnBuffRemove += ObjAiBaseOnOnBuffRemove;
        }

        public Champion(bool load)
        {
            if (load)
                GameOnLoad();
        }

        public static xSaliceResurrected_Rework.Orbwalking.Orbwalker Orbwalker;
        public static Menu Menu;

        private void GameOnLoad()
        {
            Game.PrintChat("<font color = \"#FFB6C1\">xSalice's Ressurected AIO</font> by <font color = \"#00FFFF\">xSalice</font>, imsosharp Fix, NightMoon Rework!");
            Game.PrintChat("---------------------------------");
            Game.PrintChat("Change Log: Rewrite Done! Now just need to fix the question.(Prediction Change in Next Update)");

            Menu = new Menu("xSalice's " + Player.ChampionName, Player.ChampionName, true);

            Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new xSaliceResurrected_Rework.Orbwalking.Orbwalker(Menu.SubMenu("Orbwalker"));

            var itemMenu = new Menu("Activator", "Items");
            ItemManager.AddToMenu(itemMenu);
            Menu.AddSubMenu(itemMenu);

            var predMenu = new Menu("Prediction", "Prediction");
            predMenu.AddItem(new MenuItem("(Now Not Work)", "Now Not Work!!!!!"));
            predMenu.AddItem(new MenuItem("SelectPred", "Select Prediction: ", true).SetValue(new StringList(new[] { "Common Prediction", "OKTW Prediction", "SDK Prediction", "SPrediction(Need F5 Reload)", "xcsoft AIO Prediction" }, 1)));
            predMenu.AddItem(new MenuItem("AboutCommonPred", "Common Prediction -> LeagueSharp.Commmon Prediction"));
            predMenu.AddItem(new MenuItem("AboutOKTWPred", "OKTW Prediction -> Sebby' Prediction"));
            predMenu.AddItem(new MenuItem("AboutSDKPred", "SDK Prediction -> LeagueSharp.SDKEx Prediction"));
            predMenu.AddItem(new MenuItem("AboutSPred", "SPrediction -> Shine' Prediction"));
            predMenu.AddItem(new MenuItem("AboutxcsoftAIOPred", "xcsoft AIO Prediction -> xcsoft ALL In One Prediction"));
            Menu.AddSubMenu(predMenu);

            Menu.AddToMainMenu();

            var pluginLoader = new PluginLoader();
        }

        #region 
        private void Drawing_OnDrawEvent(EventArgs args)
        {
            if (Player.IsDead) return;

            Drawing_OnDraw(args);
        }

        protected virtual void Drawing_OnDraw(EventArgs args)
        {
      
        }

        private void Obj_AI_Base_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            OnDoCast(sender, args);
        }

        protected virtual void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            
        }

        private void AntiGapcloser_OnEnemyGapcloserEvent(ActiveGapcloser gapcloser)
        {
            AntiGapcloser_OnEnemyGapcloser(gapcloser);
        }

        protected virtual void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
       
        }

        private void Interrupter_OnPosibleToInterruptEvent(Obj_AI_Hero unit, Interrupter2.InterruptableTargetEventArgs spell)
        {
            Interrupter_OnPosibleToInterrupt(unit, spell);
        }

        protected virtual void Interrupter_OnPosibleToInterrupt(Obj_AI_Hero unit, Interrupter2.InterruptableTargetEventArgs spell)
        {

        }

        private void Game_OnGameUpdateEvent(EventArgs args)
        {
            if (Player.IsDead && Player.ChampionName.ToLower() != "karthus") return;

            Game_OnGameUpdate(args);
        }

        protected virtual void Game_OnGameUpdate(EventArgs args)
        {

        }

        private void GameObject_OnCreateEvent(GameObject sender, EventArgs args)
        {
            GameObject_OnCreate(sender, args);
        }

        protected virtual void GameObject_OnCreate(GameObject sender, EventArgs args)
        {

        }

        private void GameObject_OnDeleteEvent(GameObject sender, EventArgs args)
        {
            GameObject_OnDelete(sender, args);
        }

        protected virtual void GameObject_OnDelete(GameObject sender, EventArgs args)
        {

        }

        private void Obj_AI_Base_OnProcessSpellCastEvent(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs args)
        {
            Obj_AI_Base_OnProcessSpellCast(unit, args);
        }

        protected virtual void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs args)
        {

        }

        private void AfterAttackEvent(AttackableUnit unit, AttackableUnit target)
        {
            AfterAttack(unit, target);
        }

        protected virtual void AfterAttack(AttackableUnit unit, AttackableUnit target)
        {

        }

        private void BeforeAttackEvent(xSaliceResurrected_Rework.Orbwalking.BeforeAttackEventArgs args)
        {
            BeforeAttack(args);
        }

        protected virtual void BeforeAttack(xSaliceResurrected_Rework.Orbwalking.BeforeAttackEventArgs args)
        {
           
        }

        protected virtual void OnAttack(AttackableUnit unit, AttackableUnit target)
        {
            
        }

        private void ObjAiHeroOnOnIssueOrderEvent(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            ObjAiHeroOnOnIssueOrder(sender, args);
        }

        protected virtual void ObjAiHeroOnOnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
           
        }

        private void Spellbook_OnUpdateChargedSpellEvent(Spellbook sender, SpellbookUpdateChargedSpellEventArgs args)
        {
            Spellbook_OnUpdateChargedSpell(sender, args);
        }

        protected virtual void Spellbook_OnUpdateChargedSpell(Spellbook sender, SpellbookUpdateChargedSpellEventArgs args)
        {
            
        }

        protected virtual void SpellbookOnOnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            
        }

        protected virtual void SpellbookOnOnStopCast(Spellbook sender, SpellbookStopCastEventArgs args)
        {
          
        }

        protected virtual void ObjAiBaseOnOnBuffAdd(Obj_AI_Base sender, Obj_AI_BaseBuffAddEventArgs args)
        {

        }

        protected virtual void ObjAiBaseOnOnBuffRemove(Obj_AI_Base sender, Obj_AI_BaseBuffRemoveEventArgs args)
        {

        }

        protected virtual void ObjAiHeroOnOnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {

        }
        #endregion
    }
}