using BepInEx;
using HarmonyLib;
using FistVR;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using BepInEx.Logging;

namespace H3VR_TrueGear
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static List<FVRQuickBeltSlot> slots = new List<FVRQuickBeltSlot>();
        private static float playerMaxHealth = 0f;
        private static bool canSendHeartBit = true;
        private static bool playerIsDeath = false;
        private static TrueGearMod _TrueGear = null;

        private static bool canReload = true;
        private static bool canPickup = true;

        private static string leftHandItem = null;
        private static string rightHandItem = null;

        private static bool canFlame = true;


        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(Plugin));
            _TrueGear = new TrueGearMod();
            // Plugin startup logic
            _TrueGear.Play("HeartBeat");
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }


        private static KeyValuePair<float, float> GetAngle(Transform player, Vector3 hitPoint)
        {
            
            Vector3 hitPos = hitPoint - player.position;
            float hitAngle = Mathf.Atan2(hitPos.x, hitPos.z) * Mathf.Rad2Deg;
            if (hitAngle < 0f)
            {
                hitAngle += 360f;
            }
            float verticalDifference = hitPoint.y - player.position.y;
            return new KeyValuePair<float, float>(hitAngle, verticalDifference);

        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRFireArm), "EjectMag")]
        public static void FVRFireArm_EjectMag_PostPatch(FVRFireArm __instance)
        {
            bool isRightHand = __instance.m_hand.IsThisTheRightHand;
            if (isRightHand)
            {
                //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("RightMagazineEjected");
                _TrueGear.Play("RightMagazineEjected");
            }
            else
            {
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("LeftMagazineEjected");
                _TrueGear.Play("LeftMagazineEjected");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRFireArm), "LoadMag")]
        public static void FVRFireArm_LoadMag_PostPatch(FVRFireArm __instance)
        {
            bool isRightHand = __instance.m_hand.IsThisTheRightHand;
            if (isRightHand)
            {
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("RightReloadAmmo");
                _TrueGear.Play("RightReloadAmmo");
            }
            else
            {
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("LeftReloadAmmo");
                _TrueGear.Play("LeftReloadAmmo");
            }
        }

        



        [HarmonyPostfix, HarmonyPatch(typeof(FVRFireArm), "Recoil")]
        public static void FVRFireArm_Recoil_PostPatch(FVRFireArm __instance, bool twoHandStabilized, bool foregripStabilized, bool shoulderStabilized)
        {
            string text = "";
            //string name = __instance.name;
            var roundType = __instance.RoundType;
            bool isRightHand = __instance.m_hand.IsThisTheRightHand;
             //Plugin.Log.LogInfo("---------------------------------------");
            if (twoHandStabilized)
            {
                text = "LeftHand" + RoundType.RoundTypeCheck(roundType) + "Shoot";
                 //Plugin.Log.LogInfo(text);
                _TrueGear.Play(text);
                text = "RightHand" + RoundType.RoundTypeCheck(roundType) + "Shoot";
                 //Plugin.Log.LogInfo(text);
                _TrueGear.Play(text);
                return;
            }
            else if (foregripStabilized)
            {
                text = "LeftHand" + RoundType.RoundTypeCheck(roundType) + "Shoot";
                 //Plugin.Log.LogInfo(text);
                _TrueGear.Play(text);
                text = "RightHand" + RoundType.RoundTypeCheck(roundType) + "Shoot";
                 //Plugin.Log.LogInfo(text);
                _TrueGear.Play(text);
                return;
            }
            else if (shoulderStabilized)
            {
                text = "LeftHand" + RoundType.RoundTypeCheck(roundType) + "Shoot";
                 //Plugin.Log.LogInfo(text);
                _TrueGear.Play(text);
                text = "RightHand" + RoundType.RoundTypeCheck(roundType) + "Shoot";
                 //Plugin.Log.LogInfo(text);
                _TrueGear.Play(text);
                return;
            }
            else if (isRightHand)
            {
                text = "RightHand" + RoundType.RoundTypeCheck(roundType) + "Shoot";
            }
            else
            {
                text = "LeftHand" + RoundType.RoundTypeCheck(roundType) + "Shoot";
            }
             //Plugin.Log.LogInfo(text);
            _TrueGear.Play(text);
            canReload = false;
            Timer ReloadTimer = new Timer(ReloadTimerCallBack, null, 100, Timeout.Infinite);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRSceneSettings), "OnPowerupUse")]
        public static void FVRSceneSettings_OnPowerupUse_PostPatch(PowerupType type)
        {
             //Plugin.Log.LogInfo("---------------------------------------");
            _TrueGear.Play("Healing");
            switch (type)
            {
                case PowerupType.Explosive:
                     //Plugin.Log.LogInfo("Powerup_Explosive");
                    return;
                case PowerupType.Uncooked:

                     //Plugin.Log.LogInfo("Powerup_ncooked");
                    return;
                case PowerupType.Health:
                     //Plugin.Log.LogInfo("Powerup_Health");      //吃东西加血
                    return;
                case PowerupType.QuadDamage:
                     //Plugin.Log.LogInfo("Powerup_QuadDamage");
                    return;
                case PowerupType.InfiniteAmmo:
                     //Plugin.Log.LogInfo("Powerup_InfiniteAmmo");
                    return;
                case PowerupType.Invincibility:
                     //Plugin.Log.LogInfo("Powerup_Invincibility");
                    return;
                case PowerupType.Ghosted:
                     //Plugin.Log.LogInfo("Powerup_Ghosted");
                    return;
                case PowerupType.FarOutMeat:
                     //Plugin.Log.LogInfo("Powerup_FarOutMeat");
                    return;
                case PowerupType.MuscleMeat:
                     //Plugin.Log.LogInfo("Powerup_MuscleMeat");
                    return;
                case PowerupType.HomeTown:
                     //Plugin.Log.LogInfo("Powerup_HomeTown");
                    return;
                case PowerupType.SnakeEye:
                     //Plugin.Log.LogInfo("Powerup_SnakeEye");
                    return;
                case PowerupType.Blort:
                     //Plugin.Log.LogInfo("Powerup_Blort");
                    return;
                case PowerupType.Regen:
                     //Plugin.Log.LogInfo("Powerup_Regen");
                    return;
                case PowerupType.Cyclops:
                     //Plugin.Log.LogInfo("Powerup_Cyclops");
                    return;
                case PowerupType.WheredIGo:
                     //Plugin.Log.LogInfo("Powerup_WheredIGo");
                    return;
                case PowerupType.ChillOut:
                     //Plugin.Log.LogInfo("Powerup_ChillOut");
                    return;
                case PowerupType.Debuff:
                     //Plugin.Log.LogInfo("Powerup_Debuff");
                    return;
                case PowerupType.Unfreeze:
                     //Plugin.Log.LogInfo("Powerup_Unfreeze");
                    return;
                case PowerupType.SpeedUp:
                     //Plugin.Log.LogInfo("Powerup_SpeedUp");
                    return;
                default:
                     //Plugin.Log.LogInfo("Healing");
                    return;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRPlayerHitbox), "Damage",new Type[] { typeof(Damage)})]
        public static void FVRPlayerHitbox_Damage_PostPatch(FVRPlayerHitbox __instance,Damage d)
        {
            string damageType = "";
            KeyValuePair<float, float> angle = GetAngle(__instance.Body.Head, d.point);

            switch (d.Class)
            {
                case Damage.DamageClass.Abstract:
                    damageType = "PlayerBulletDamage";
                    break;
                case Damage.DamageClass.Projectile:
                    damageType = "PlayerBulletDamage";
                    break;
                case Damage.DamageClass.Explosive:
                    angle = GetAngle(__instance.Body.TorsoTransform, d.point);
                    damageType = "DefaultDamage";
                    break;
                case Damage.DamageClass.Melee:
                    damageType = "DefaultDamage";
                    break;
                case Damage.DamageClass.Environment:
                    damageType = "DefaultDamage";
                    break;
            }
             //Plugin.Log.LogInfo("---------------------------------------");
             //Plugin.Log.LogInfo($"{damageType},{angle.Key},{angle.Value}");
            _TrueGear.PlayAngle(damageType, angle.Key, angle.Value);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRPlayerBody), "Update")]
        public static void FVRPlayerBody_Update_Postfix(FVRPlayerBody __instance)
        {
            float playerHealth = __instance.Health;
            if (playerHealth > 0 && Plugin.playerIsDeath) Plugin.playerIsDeath = false;
            if (!Plugin.playerIsDeath)
            {
                if (playerHealth > Plugin.playerMaxHealth)
                {
                    Plugin.playerMaxHealth = playerHealth;
                }
                else if ((playerHealth < Plugin.playerMaxHealth / 3f) && Plugin.canSendHeartBit)
                {
                    Plugin.canSendHeartBit = false;
                     //Plugin.Log.LogInfo("---------------------------------------");
                     //Plugin.Log.LogInfo("StartHeartBeat");
                    _TrueGear.StartHeartBeat();
                }
                else if (!(playerHealth < Plugin.playerMaxHealth / 3f) && !Plugin.canSendHeartBit)
                {
                    Plugin.canSendHeartBit = true;
                     //Plugin.Log.LogInfo("---------------------------------------");
                     //Plugin.Log.LogInfo("StopHeartBeat");
                    _TrueGear.StopHeartBeat();
                }
                if (playerHealth <= 0f)
                {
                    Plugin.playerIsDeath = true;
                    Plugin.playerMaxHealth = 0f;
                    Plugin.canSendHeartBit = true;
                     //Plugin.Log.LogInfo("---------------------------------------");
                     //Plugin.Log.LogInfo("PlayerDeath");
                    _TrueGear.Play("PlayerDeath");
                    _TrueGear.StopHeartBeat();
                }
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRMovementManager), "RocketJump")]
        public static void FVRMovementManager_RocketJump_PostPatch()
        {
             //Plugin.Log.LogInfo("---------------------------------------");
             //Plugin.Log.LogInfo("RocketJump");
            _TrueGear.Play("RocketJump");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ZosigGameManager), "EatBangerJunk")]
        public static void ZosigGameManager_EatBangerJunk_PostPatch()
        {
             //Plugin.Log.LogInfo("---------------------------------------");
             //Plugin.Log.LogInfo("Eating");
            _TrueGear.Play("Eating");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ZosigGameManager), "EatHerb")]
        public static void ZosigGameManager_EatHerb_PostPatch()
        {
             //Plugin.Log.LogInfo("---------------------------------------");
             //Plugin.Log.LogInfo("Eating");
            _TrueGear.Play("Eating");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ZosigGameManager), "EatMeatCore")]
        public static void ZosigGameManager_EatMeatCore_PostPatch()
        {
             //Plugin.Log.LogInfo("---------------------------------------");
             //Plugin.Log.LogInfo("Eating");
            _TrueGear.Play("Eating");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ZosigGameManager), "VomitObject")]
        public static void ZosigGameManager_VomitObject_PostPatch()
        {
             //Plugin.Log.LogInfo("---------------------------------------");
             //Plugin.Log.LogInfo("Vomit");
            _TrueGear.Play("Vomit");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRFireArmBeltSegment), "EndInteraction")]
        public static void FVRFireArmBeltSegment_EndInteraction_PostPatch(FVRViveHand hand)
        {
            if (hand.IsThisTheRightHand)
            {
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("RightHandPickupItem");
                _TrueGear.Play("RightHandPickupItem");
                return;
            }
             //Plugin.Log.LogInfo("---------------------------------------");
             //Plugin.Log.LogInfo("LeftHandPickupItem");
            _TrueGear.Play("LeftHandPickupItem");
        }


        [HarmonyPostfix, HarmonyPatch(typeof(FVRPhysicalObject), "OnCollisionEnter")]
        public static void FVRPhysicalObject_OnCollisionEnter_PostPatch(FVRPhysicalObject __instance, Collision col)
        {
            if (!__instance.IsHeld)
            {
                return;
            }
            if (!__instance.MP.IsMeleeWeapon)
            {
                return;
            }            
            string colliderName = col.collider.name;
            if (colliderName.Contains("Capsule") | colliderName.Contains("Mag"))
            {
                return;
            }
            if (!canPickup)
            {
                return;
            }
            canPickup = false;
            if (__instance.m_hand.IsThisTheRightHand)
            {
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("RightHandPickupItem");
                _TrueGear.Play("RightHandPickupItem");      //RightHandCollision
            }
            else
            {
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("LeftHandPickupItem");
                _TrueGear.Play("LeftHandPickupItem");
            }
            Timer PickupTimer = new Timer(PickupTimerCallBack,null,100,Timeout.Infinite);
        }

        private static void PickupTimerCallBack(object o)
        { 
            canPickup = true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRPhysicalObject), "EndInteractionIntoInventorySlot")]
        public static void FVRPhysicalObject_EndInteractionIntoInventorySlot_Postfix(FVRQuickBeltSlot slot)
        {
             //Plugin.Log.LogInfo("---------------------------------------");
            slots.Add(slot);
             //Plugin.Log.LogInfo($"Slot_Input_{slot}");
            _TrueGear.Play("ChestSlotInputItem");

        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRPhysicalObject), "BeginInteraction")]
        public static void FVRPhysicalObject_BeginInteraction_Postfix()
        {
            if (slots.Count != 0)
            {
                FVRQuickBeltSlot delSlot = new FVRQuickBeltSlot();
                foreach (FVRQuickBeltSlot slot in slots)
                {
                    if (slot.CurObject == null)
                    {
                        delSlot = slot;
                        break;
                    }
                }
                if (delSlot != null)
                {
                     //Plugin.Log.LogInfo("---------------------------------------");
                    slots.Remove(delSlot);
                     //Plugin.Log.LogInfo($"Slot_Output_{delSlot}");                    
                    _TrueGear.Play("ChestSlotOutputItem");
                }
            }
        }

        //[HarmonyPostfix, HarmonyPatch(typeof(FVRFireArmRound), "Awake")]
        //public static void FVRFireArmRound_Awake_PostPatch(FVRFireArmRound __instance)
        //{
        //     //Plugin.Log.LogInfo("---------------------------------------");
        //     //Plugin.Log.LogInfo("Awake");
        //}

        //[HarmonyPostfix, HarmonyPatch(typeof(FVRFireArmRound), "BeginInteraction")]
        //public static void FVRFireArmRound_BeginInteraction_PostPatch(FVRFireArmRound __instance)
        //{
        //     //Plugin.Log.LogInfo("---------------------------------------");
        //     //Plugin.Log.LogInfo("BeginInteraction");
        //}

        //[HarmonyPostfix, HarmonyPatch(typeof(FVRFireArmRound), "BeginAnimationFrom")]
        //public static void FVRFireArmRound_BeginAnimationFrom_PostPatch(FVRFireArmRound __instance)
        //{
        //     //Plugin.Log.LogInfo("---------------------------------------");
        //     //Plugin.Log.LogInfo("BeginAnimationFrom");
        //}

        [HarmonyPostfix, HarmonyPatch(typeof(FVRFireArm), "PlayAudioEvent")]
        public static void FVRFireArm_PlayAudioEvent_PostPatch(FVRFireArm __instance, FirearmAudioEventType eType)
        {
            //Plugin.Log.LogInfo("---------------------------------------");
            //Plugin.Log.LogInfo($"PlayAudio :{eType}");
            if (__instance == null)
            {
                return;
            }

            bool isRightHand = __instance.m_hand.IsThisTheRightHand;
            
            if (eType == FirearmAudioEventType.ChamberManual || eType == FirearmAudioEventType.MagazineInsertRound)
            {
                if (isRightHand)
                {
                     //Plugin.Log.LogInfo("RightReloadAmmo");
                    _TrueGear.Play("RightReloadAmmo");
                }
                else
                {
                     //Plugin.Log.LogInfo("LeftReloadAmmo");
                    _TrueGear.Play("LeftReloadAmmo");
                }
                return;
            }
            if (canReload)
            {
                switch (eType)
                {
                    case FirearmAudioEventType.BoltSlideBack:
                    case FirearmAudioEventType.BoltSlideBackLocked:
                    case FirearmAudioEventType.BoltSlideBackHeld:
                    case FirearmAudioEventType.HandleBack:
                    case FirearmAudioEventType.HandleBackEmpty:
                    case FirearmAudioEventType.BoltSlideForward:
                    case FirearmAudioEventType.BoltSlideForwardHeld:
                    case FirearmAudioEventType.HandleForward:
                    case FirearmAudioEventType.HandleForwardEmpty:
                        canReload = false;
                        if (isRightHand)
                        {
                             //Plugin.Log.LogInfo("RightDownReload");
                            _TrueGear.Play("RightDownReload");
                        }
                        else
                        {
                             //Plugin.Log.LogInfo("LeftDownReload");
                            _TrueGear.Play("LeftDownReload");
                        }
                        Timer ReloadTimer = new Timer(ReloadTimerCallBack, null, 200, Timeout.Infinite);
                        return;
                }
            }
                
        }

        private static void ReloadTimerCallBack(object o)
        { 
            canReload = true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRInteractiveObject), "PlayGrabSound")]
        public static void FVRInteractiveObject_PlayGrabSound_Postfix(FVRInteractiveObject __instance, bool isHard, FVRViveHand hand)
        {
            if (hand.HandSource == Valve.VR.SteamVR_Input_Sources.LeftHand)
            {
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("LeftHandPickupItem");
                _TrueGear.Play("LeftHandPickupItem");
                leftHandItem = __instance.GameObject.name;
            }
            else if (hand.HandSource == Valve.VR.SteamVR_Input_Sources.RightHand)
            {
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("RightHandPickupItem");
                _TrueGear.Play("RightHandPickupItem");
                rightHandItem = __instance.GameObject.name;
            }
             //Plugin.Log.LogInfo(__instance.GameObject.name);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FVRInteractiveObject), "PlayReleaseSound")]
        public static void FVRInteractiveObject_PlayReleaseSound_Postfix(FVRInteractiveObject __instance,FVRViveHand hand)
        {
            if (hand.HandSource == Valve.VR.SteamVR_Input_Sources.LeftHand)
            {                                
                Timer LeftHandItemTimer = new Timer(LeftHandItemTimerCallBack,null,5,Timeout.Infinite);
            }
            else if (hand.HandSource == Valve.VR.SteamVR_Input_Sources.RightHand)
            {
                Timer rightHandItemTimer = new Timer(RightHandItemTimerCallBack,null,5,Timeout.Infinite);
            }
        }

        private static void LeftHandItemTimerCallBack(object o)
        {
            leftHandItem = null;
        }

        private static void RightHandItemTimerCallBack(object o)
        {
            rightHandItem = null;
        }


        [HarmonyPostfix, HarmonyPatch(typeof(FVRInteractiveObject), "OnDestroy")]
        public static void FVRInteractiveObject_OnDestroy_Postfix(FVRInteractiveObject __instance)
        {
            if (!__instance.GameObject.name.Contains("HotDog"))
            {
                return;
            }
            if (__instance.GameObject.name == leftHandItem || __instance.GameObject.name == rightHandItem)
            {                
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("Eating");
                _TrueGear.Play("Eating");
            }
        }


        [HarmonyPostfix, HarmonyPatch(typeof(FVRSceneSettings), "OnSosiggunFired")]
        public static void FVRSceneSettings_OnSosiggunFired_Postfix(FVRSceneSettings __instance, SosigWeapon weapon)
        {
            if (weapon.gameObject.name == rightHandItem)
            {
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("RightHandRifleShoot");
                _TrueGear.Play("RightHandRifleShoot");
            }
            if (weapon.gameObject.name == leftHandItem)
            {
                 //Plugin.Log.LogInfo("---------------------------------------");
                 //Plugin.Log.LogInfo("LeftHandRifleShoot");
                _TrueGear.Play("LeftHandRifleShoot");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Minigun), "Fire")]
        public static void Minigun_Fire_Postfix(Minigun __instance)
        {
             //Plugin.Log.LogInfo("---------------------------------------");
             //Plugin.Log.LogInfo("RightHandShotgunShoot");
            _TrueGear.Play("RightHandShotgunShoot");
             //Plugin.Log.LogInfo("LeftHandShotgunShoot");
            _TrueGear.Play("LeftHandShotgunShoot");
        }


        [HarmonyPostfix, HarmonyPatch(typeof(RemoteMissileLauncher), "FireShot")]
        public static void RemoteMissileLauncher_FireShot_Postfix(RemoteMissileLauncher __instance)
        {
            if (__instance.gameObject.name == rightHandItem)
            {
                //Plugin.Log.LogInfo("---------------------------------------");
                //Plugin.Log.LogInfo("RightHandShotgunShoot");
                _TrueGear.Play("RightHandShotgunShoot");
            }
            if (__instance.gameObject.name == leftHandItem)
            {
                //Plugin.Log.LogInfo("---------------------------------------");
                //Plugin.Log.LogInfo("LeftHandShotgunShoot");
                _TrueGear.Play("LeftHandShotgunShoot");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(FlameThrower), "UpdateFire")]
        public static void FlameThrower_UpdateFire_Postfix(FlameThrower __instance)
        {
            if (!__instance.m_isFiring)
            {
                canFlame = true;
                return;
            }
            if (!canFlame)
            {
                return;
            }
            canFlame = false;
            if (__instance.gameObject.name == rightHandItem)
            {
                //Plugin.Log.LogInfo("---------------------------------------");
                //Plugin.Log.LogInfo("RightHandRifleShoot");
                _TrueGear.Play("RightHandRifleShoot");
            }
            if (__instance.gameObject.name == leftHandItem)
            {
                //Plugin.Log.LogInfo("---------------------------------------");
                //Plugin.Log.LogInfo("LeftHandRifleShoot");
                _TrueGear.Play("LeftHandRifleShoot");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(sblp), "TryToEngageShot")]             //开始激光
        public static void sblp_TryToEngageShot_Postfix(sblp __instance)
        {

            if (__instance.gameObject.name == rightHandItem)
            {
                //Plugin.Log.LogInfo("---------------------------------------");
                //Plugin.Log.LogInfo("RightHandPistolShoot");
                _TrueGear.Play("RightHandPistolShoot");
            }
            if (__instance.gameObject.name == leftHandItem)
            {
                //Plugin.Log.LogInfo("---------------------------------------");
                //Plugin.Log.LogInfo("LeftHandPistolShoot");
                _TrueGear.Play("LeftHandPistolShoot");
            }
        }

        //[HarmonyPostfix, HarmonyPatch(typeof(sblp), "TryToDisengageShot")]            结束激光
        //public static void sblp_TryToDisengageShot_Postfix(sblp __instance)
        //{

        //    if (__instance.gameObject.name == rightHandItem)
        //    {
        //        //Plugin.Log.LogInfo("---------------------------------------");
        //        //Plugin.Log.LogInfo("TryToDisengageShot");
        //        _TrueGear.Play("RightHandRifleShoot");
        //    }
        //    if (__instance.gameObject.name == leftHandItem)
        //    {
        //        //Plugin.Log.LogInfo("---------------------------------------");
        //        //Plugin.Log.LogInfo("TryToDisengageShot");
        //        _TrueGear.Play("LeftHandRifleShoot");
        //    }

        //}


        [HarmonyPostfix, HarmonyPatch(typeof(HCB), "ReleaseSled")] 
        public static void HCB_ReleaseSled_Postfix(HCB __instance)
        {

            if (__instance.gameObject.name == rightHandItem)
            {
                //Plugin.Log.LogInfo("---------------------------------------");
                //Plugin.Log.LogInfo("RightHandPistolShoot");
                _TrueGear.Play("RightHandPistolShoot");
            }
            if (__instance.gameObject.name == leftHandItem)
            {
                //Plugin.Log.LogInfo("---------------------------------------");
                //Plugin.Log.LogInfo("LeftHandPistolShoot");
                _TrueGear.Play("LeftHandPistolShoot");
            }
        }

    }
}