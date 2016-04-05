//using PRoCon.Core;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Xml.Serialization;

//namespace xfileFIN
//{

//    public enum kits
//    {
//        ASSAULT,
//        ENGINEER,
//        SUPPORT,
//        RECON,
//        NOKIT
//    }

//    public enum optics_close
//    {
//        IRON_SIGHTS,
//        F2000_16X, // Only F2000
//        REFLEX_RDS,
//        COYOTE_RDS,
//        KOBRA_RDS,
//        HOLO_1X,
//        HD33_1X,
//        PKAS_1X,
//        IRNV_IR_1X,
//        FLIR_IR_2X
//    }
//    public enum optics_medium
//    {
//        M145_34X,
//        PRISMA_34X,
//        PKA_34X,
//        ACOG_4X,
//        JGM4_4X,
//        PSO1_4X
//    }
//    public enum optics_long
//    {
//        CL6X_6X,
//        PKS07_7X,
//        RIFLE_SCOPE_8X,
//        HUNTER_20X,
//        BALLISTIC_40X
//    }

//    public enum category
//    {
//        PRIMARY,
//        SECONDARY,
//        GADGET1,
//        GADGET2,
//        GRENADE,
//        MELEE,
//        SPECIALIZATION
//    }

//    public class BLogWGC
//    {
//        public static Hashtable Loadout = null;
//        public static Hashtable currentLoadout = null;
//        public static Hashtable weapons = null;

//        public static Kit currentkit = new Kit();
//        public static kits ActiveKit = kits.NOKIT;

//        public static Int64 fixKey(Int64 key, category type)
//        {
//            #region OLD
//            //List<Replace> replaces = new List<Replace>();
//            //replaces.Add(new Replace() { name = "AK-12", type = category.PRIMARY, old_key = 1268485893, new_key = 3590299697 });
//            //replaces.Add(new Replace() { name = "CS-LR4", type = category.PRIMARY, old_key = 3109886683, new_key = 3458855537 });
//            //replaces.Add(new Replace() { name = "U-100 MK5", type = category.PRIMARY, old_key = 0, new_key = 3179658801 });

//            //replaces.Add(new Replace() { name = "P226", type = category.SECONDARY, old_key = 417382943, new_key = 944904529 });
//            //replaces.Add(new Replace() { name = "P226", type = category.SECONDARY, old_key = 3214146841, new_key = 944904529 });
//            //replaces.Add(new Replace() { name = "P226", type = category.SECONDARY, old_key = 4165557629, new_key = 944904529 });
//            //replaces.Add(new Replace() { name = "P226", type = category.SECONDARY, old_key = 3458855537, new_key = 944904529 });
//            //replaces.Add(new Replace() { name = "P226", type = category.SECONDARY, old_key = 2510389743, new_key = 944904529 });
//            //replaces.Add(new Replace() { name = "P226", type = category.SECONDARY, old_key = 162542510, new_key = 944904529 });

//            //replaces.Add(new Replace() { name = "NO GADGET", type = category.GADGET1, old_key = 1549533860, new_key = 1694579111 });
//            //replaces.Add(new Replace() { name = "NO GADGET", type = category.GADGET1, old_key = 3675115371, new_key = 1694579111 });
//            //replaces.Add(new Replace() { name = "NO GADGET", type = category.GADGET1, old_key = 944904529, new_key = 1694579111 });

//            //replaces.Add(new Replace() { name = "NO GADGET", type = category.GADGET2, old_key = 1694579111, new_key = 3164552276 });
//            //replaces.Add(new Replace() { name = "NO GADGET", type = category.GADGET2, old_key = 0, new_key = 3164552276 });
//            //replaces.Add(new Replace() { name = "NO GADGET", type = category.GADGET2, old_key = 3362096917, new_key = 3164552276 });

//            //replaces.Add(new Replace() { name = "M67 FRAG", type = category.GRENADE, old_key = 0, new_key = 2670747868 });
//            //replaces.Add(new Replace() { name = "M67 FRAG", type = category.GRENADE, old_key = 3164552276, new_key = 2670747868 });
//            //replaces.Add(new Replace() { name = "M67 FRAG", type = category.GRENADE, old_key = 1569055782, new_key = 2670747868 });

//            //replaces.Add(new Replace() { name = "BAYONET", type = category.MELEE, old_key = 302761745, new_key = 3214146841 });
//            //replaces.Add(new Replace() { name = "BAYONET", type = category.MELEE, old_key = 2670747868, new_key = 3214146841 });
//            //replaces.Add(new Replace() { name = "BAYONET", type = category.MELEE, old_key = 277773742, new_key = 3214146841 });
//            //replaces.Add(new Replace() { name = "BAYONET", type = category.MELEE, old_key = 2650800128, new_key = 3214146841 });

//            //replaces.Add(new Replace() { name = "DEFENSIVE", type = category.SPECIALIZATION, old_key = 3214146841, new_key = 1549533860 });
//            //replaces.Add(new Replace() { name = "DEFENSIVE", type = category.SPECIALIZATION, old_key = 944904529, new_key = 1549533860 });
//            //replaces.Add(new Replace() { name = "DEFENSIVE", type = category.SPECIALIZATION, old_key = 0, new_key = 1549533860 });

//            //XmlSerializer serializer = new XmlSerializer(typeof(List<Replace>));
//            //using (TextWriter writer = new StreamWriter("weapon_replaces.xml"))
//            //{
//            //    serializer.Serialize(writer, replaces);
//            //}

//            //int index = replaces.FindIndex(f => f.old_key == key && f.type == type);
//            //if(index > -1)
//            //    return (replaces[index].new_key);
//            //else
//            //    return (key); 
//            #endregion


//            return (key);
//        }

//        public static String warsaw_loadout_js = null;

//        private static bool Initialize()
//        {
//            if (warsaw_loadout_js == null)
//            {
//                try
//                {
//                    WebClient client = null;
//                    if (client == null)
//                        client = new WebClient();
//                    warsaw_loadout_js = client.DownloadString("http://battlelog.battlefield.com/public/gamedatawarsaw/warsaw.loadout.js");
//                    warsaw_loadout_js = warsaw_loadout_js.Split(new[] { "window.loadout.game_data = " }, StringSplitOptions.None)[1];
//                }
//                catch (WebException e)
//                {
//                    if (e.Status.Equals(WebExceptionStatus.Timeout))
//                        throw new Exception("HTTP request timed-out");
//                    else
//                        throw;
//                }
//            }
//            return (true);
//        }

//        public class allItemsCollection_
//        {
//            public Array models;
//            public String name;
//            public Boolean mySoldier;
//        }
//        public class model
//        {
//            public allItemsCollection_ items;
//            public Boolean mySoldier;
//            public int game;
//            public Int64 personaId;
//            public String personaName;
//            public int platformInt;
//            public String platformName;
//            public String loadoutUrl;
//            public Hashtable stats;
//            public Hashtable licenses;
//            public Hashtable presetLoadouts;
//            public String premiumMaxPresetSlots;
//            public String standardMaxPresetSlots;
//            public Boolean isPremium;
//            public ArrayList releasedXpacks;
//            public Hashtable compatibility;
//            public Hashtable structure;
//        }

//        static String[] nonConfigurableItems = new String[] { "t62-cew" };
//        static allItemsCollection_ allItemsCollection = new allItemsCollection_() { models = null, name = "loadoutAllItemsCollection" };

//        public static void init(String playerName)
//        {
//            var model = new model();

//            Boolean loaded = false;

//            // Items collection
            
//            if(allItemsCollection.models != null && allItemsCollection.models.Length > 0)
//            {
//                loaded = true;
//            }

//            // Grab the fetch URL
//            if (!fetchLoadout(playerName))
//                return;

//            String loadoutUrl = "/bf4/loadout/get/[personaName]/[personaId]/[platformInt]/";

//            if (!loaded)
//            {
//                // If we haven't loaded the items, add them. No need to load them again if we've previously visited Loadout
                
//                model.items = allItemsCollection;
//                model.mySoldier = true;
//                allItemsCollection.mySoldier = true;
//                if (model.mySoldier)
//                {
//                    model.personaId = Convert.ToInt64(Loadout["personaId"]);
//                    model.personaName = (String)Loadout["personaName"];
//                    model.platformInt = Convert.ToInt32(Loadout["platformInt"]);
//                    model.platformName = (model.platformInt == 1) ? "pc" : "undefined";
//                    model.game = Convert.ToInt32(Loadout["game"]);
//                }
//                model.loadoutUrl = String.Format("/bf4/loadout/get/{0}/{1}/{2}/", model.personaName, model.personaId, model.platformInt);
//                model.stats = (Hashtable)Loadout["playerStats"];
//                model.licenses = (Hashtable)Loadout["playerLicenses"];
//                model.presetLoadouts = (Hashtable)Loadout["presets"];
//                model.premiumMaxPresetSlots = Loadout["maxPresetsPremium"].ToString();
//                model.standardMaxPresetSlots = Loadout["maxPresetsStandard"].ToString();
//                model.isPremium = Convert.ToBoolean(Loadout["isPremium"]);
//                model.releasedXpacks = (ArrayList)Loadout["releasedXpacks"];
//                var currentLoadout = (Hashtable)Loadout["currentLoadout"];
//            }

//            // Add loadout structure to the model
//            model.structure = get_structure();
//            // Add the item compatibility list
//            model.compatibility = (Hashtable)model.structure["compatibility"];

//            var json = (Hashtable)JSON.JsonDecode(warsaw_loadout_js);
//            var data = (Hashtable)json["compact"];
//            var structure = (Hashtable)json["loadout"];

//            var items = new ArrayList();
//            var k = 0;
//            var kLen = ((ArrayList)structure["kits"]).Count;
//            while(k < kLen)
//            {
//                // Have to pop twice for BF4 to remove parachute and soldier camo
//                if(((ArrayList)((Hashtable)((ArrayList)structure["kits"])[k])["slots"]).Count > 8)
//                {
//                    ((ArrayList)((Hashtable)((ArrayList)structure["kits"])[k])["slots"]).RemoveAt(((ArrayList)((Hashtable)((ArrayList)structure["kits"])[k])["slots"]).Count - 1);
//                }

//                ((ArrayList)((Hashtable)((ArrayList)structure["kits"])[k])["slots"]).RemoveAt(((ArrayList)((Hashtable)((ArrayList)structure["kits"])[k])["slots"]).Count - 1);
//                k++;
//            }

//            // Parse all the items
//            Hashtable tmp = new Hashtable();
//            tmp.Add("item", (Hashtable)data["kititems"]);
//            tmp.Add("vehicle", (Hashtable)data["vehicles"]);
//            tmp.Add("appearance", (Hashtable)data["appearances"]);
//            tmp.Add("vehicleUnlock", (Hashtable)data["vehicleunlocks"]);
//            tmp.Add("accessory", (Hashtable)data["weaponaccessory"]);
//            tmp.Add("weapon", (Hashtable)data["weapons"]);
//            data = tmp;

//            foreach(DictionaryEntry categoryName in data) // item
//            {
//                var category = (Hashtable)data[categoryName.Key];
//                foreach(DictionaryEntry guid in category) // 53214369
//                {
//                    Hashtable item = (Hashtable)category[guid.Key];
//                    item.Add("itemType", categoryName.Key);
//                    var see = (ArrayList)item["see"];
//                    if(see != null)
//                    {
//                        var i = 0;
//                        var len = see.Count;
//                        while(i < len)
//                        {
//                            item = HashtableProcessor.Add(item, (Hashtable)category[see[i]]);
//                            i++;
//                        }
//                        item.Add("guid", guid.Key);
//                        if(categoryName.Key.Equals("weapon")) // UNTESTED
//                        {
//                            item.Add("configurable", false);
//                            var weapon = (Hashtable)((Hashtable)structure["weapons"])[guid.Key];
//                            if(weapon != null && (ArrayList)weapon["slots"] != null && !nonConfigurableItems.Contains(item["slug"].ToString())) {
//                                item["configurable"] = true;
//                            }
//                        }

//                        if(model.compatibility[guid.Key] != null)
//                        {
//                            item.Add("compatibility", (Hashtable)model.compatibility[guid.Key]);
//                            if(((Hashtable)item["compatibility"])["items"] != null) {
//                                item.Add("compatibilityLocked", Convert.ToBoolean(((Hashtable)item["compatibility"])["exclusive"]));
//                            } else {
//                                item["compatibility"] = false;
//                            }
//                        } else {
//                            item["compatibility"] = false;
//                        }

//                        if(item["req"] != null)
//                        {
//                            var skipItem = false;
//                            var r = ((ArrayList)item["req"]).Count;
//                            while (r-- > 0)
//                            {
//                                var req = (Hashtable)((ArrayList)item["req"])[r];
//                                if (req["t"].Equals("l") && req["l"].Equals("dt_dev_team") && model.licenses[req["l"]] == null) {
//                                    skipItem = true;
//                                    break;
//                                }
//                            }

//                            if (skipItem) {
//                                // Work-around for the 3x scope on the M1911 being given to all players
//                                if (item["guid"].Equals("2852909715")) {
//                                    item.Remove("req");
//                                } else {
//                                    continue;
//                                }
//                            }
//                        }
//                        items.Add(item);
//                    }
//                }
//            }

//            // This adds all items to the massive global collection
//            allItemsCollection.models = (items.ToArray(typeof(Hashtable)) as Hashtable[]);
//        }

//        private static Hashtable get_structure()
//        {
//            if (Initialize())
//                return ((Hashtable)((Hashtable)JSON.JsonDecode(warsaw_loadout_js))["loadout"]);
//            else
//                return (null);
//        }

//        public static Int64 fixKey(string type, int id, int slot, Int64 guid)
//        {
//            // Types: weapons, kits, compatibility, vehicles
//            //Hashtable items = ; TODO: Parse the warsaw.loadout.js like in the javascript initialize
//            Hashtable structure = get_structure();
//            Int64 item = guid; // TODO: check if the items collection contains this item

//            Int64[] guids = Array.ConvertAll((string[])((ArrayList)((Hashtable)((ArrayList)((Hashtable)((ArrayList)structure[type])[id])["slots"])[slot])["items"]).ToArray(typeof(string)), Int64.Parse);

//            if(item == 0 || !guids.Contains(guid))
//            {
//                guid = guids[0];
//                item = guid; // TODO: check if the items collection contains this item
//            }
//            return (item);
//        }
//        public static List<Hashtable> getAccessories(Int64 guid)
//        {
//            Hashtable loadout = (Hashtable)Loadout["currentLoadout"];
//            Hashtable items_ = (Hashtable)((Hashtable)((Hashtable)JSON.JsonDecode(warsaw_loadout_js))["compact"])["weaponaccessory"]; // TODO: Parse the warsaw.loadout.js like in the javascript initialize
//            var items = allItemsCollection;
//            Hashtable structure = get_structure();
//            ArrayList accessories = (ArrayList)((Hashtable)loadout["weapons"])[guid.ToString()];

//            if (!((Hashtable)structure["weapons"]).Contains(guid.ToString()))
//            {
//                Console.WriteLine("{0} does not exist in structure.weapons", guid);
//                return null;
//            }

//            var slots = (ArrayList)((Hashtable)((Hashtable)structure["weapons"])[guid.ToString()])["slots"];
//            if (accessories == null)
//            {
//                ((Hashtable)loadout["weapons"])[guid.ToString()] = new ArrayList();
//                accessories = new ArrayList();
//            }
//            List<Hashtable> fixedAccessories = new List<Hashtable>();
//            var i = 0;
//            var len = slots.Count;
//            while (i < len)
//            {
//                Hashtable item = null;
//                if (i < slots.Count)
//                {
//                    if (Convert.ToInt64(accessories[i]) > 0)
//                    {
//                        item = items_get(items, Convert.ToInt64(accessories[i]));
//                        if (item == null || !((ArrayList)((Hashtable)slots[i])["items"]).Contains(accessories[i].ToString()))
//                        {
//                            item.Add("guid", generateSlot("weapons", guid, i).ToString());
//                            if (!items_get(items, Convert.ToInt64(item["guid"]), true))
//                                item = null;
//                        }
//                    }
//                    else
//                    {
//                        item.Add("guid", generateSlot("weapons", guid, i).ToString());
//                        if (!items_get(items, Convert.ToInt64(item["guid"]), true))
//                            item = null;
//                    }
//                }
//                else
//                {
//                    item = null;
//                }

//                if (item != null)
//                {
//                    item.Add("slotSid", ((Hashtable)slots[i])["sid"].ToString());
//                    fixedAccessories.Add(item);
//                }
//                i++;
//            }
//            return fixedAccessories;
//        }
//        public static Int64 generateSlot(String type, Int64 guid, int slot)
//        {
//            var structure = get_structure();
//            var loadout = (Hashtable)Loadout["currentLoadout"];
//            var selectedSlot = ((ArrayList)((Hashtable)((Hashtable)structure[type])[guid.ToString()])["slots"])[slot];
//            Int64 item = 0;
//            if (selectedSlot != null)
//            {
//                item = Convert.ToInt64(((ArrayList)((Hashtable)((ArrayList)((Hashtable)((Hashtable)structure[type])[guid.ToString()])["slots"])[slot])["items"])[0]);
//                ((ArrayList)((Hashtable)loadout[type])[guid.ToString()])[slot] = item;
//                // TODO: Fix the value in the actual loadout
//            }
//            return item;
//        }

//        private static Boolean items_get(allItemsCollection_ items, Int64 id, Boolean ret = true)
//        {
//            if (items_get(items, id) != null)
//                return (true);
//            else
//                return (false);
//        }
//        private static Hashtable items_get(allItemsCollection_ items, Int64 id)
//        {
//            foreach(Hashtable hTable in items.models)
//            {
//                if(hTable["guid"].Equals(id.ToString())) {
//                    return (hTable);
//                }
//            }
//            return (null);
//        }

//        public static bool fetchLoadout(string playerName)
//        {
//            BattlelogClient bclient = new BattlelogClient();
//            Loadout = bclient.getStats(playerName);
//            if (Loadout == null)
//                return (false);
//            return (true);
//        }

//        public static Int64 personaID(Hashtable Loadout)
//        {
//            if(Loadout == null)
//                return (-1);
//            if (!Loadout.Contains("personaId"))
//                return (-1);
//            return (Convert.ToInt64(Loadout["personaId"]));
//        }

//        public static byte retrieveSelectedKit(Hashtable Loadout)
//        {
//            if (!Loadout.Contains("currentLoadout"))
//                return (4);
//            currentLoadout = (Hashtable)Loadout["currentLoadout"];

//            if (!currentLoadout.Contains("weapons"))
//                return (4);
//            weapons = (Hashtable)currentLoadout["weapons"];

//            if (!currentLoadout.Contains("selectedKit"))
//                return (4);

//            byte selectedKit = Convert.ToByte(currentLoadout["selectedKit"]);
//            ActiveKit = (kits)selectedKit; // Populate usage data
//            return ((byte)ActiveKit);
//        }

//        public static Kits retrieveKits()
//        {
//            //if (currentLoadout == null)
//                retrieveSelectedKit(Loadout);

//            Kits kits = new Kits();
//            kits.kits = new Kit[4];
//            kits.kits[0] = retrieveKit(0); // ASSAULT
//            kits.kits[1] = retrieveKit(1); // ENGINEER
//            kits.kits[2] = retrieveKit(2); // SUPPORT
//            kits.kits[3] = retrieveKit(3); // RECON
//            return (kits);
//        }
//        public static Kit retrieveKit(int kit_index)
//        {
//            Kit retrieve_kit = new Kit();

//            ArrayList kits = (ArrayList)currentLoadout["kits"];
//            object[] _kits = kits.ToArray();
//            string[] Kit = ((IEnumerable)kits[kit_index]).Cast<object>()
//                                 .Select(x => x.ToString())
//                                 .ToArray();

//            // KIT
//            retrieve_kit.primaryWeapon = new Weapon() { weaponGUID = fixKey("kits", kit_index, 0, Convert.ToInt64(Kit[0])) };
//            //Weapon tmp = getAccessories(retrieve_kit.primaryWeapon.weaponGUID);
//            //tmp.weaponGUID = retrieve_kit.primaryWeapon.weaponGUID;
//            //retrieve_kit.primaryWeapon = tmp;
//            //if (GetAttachments(retrieve_kit.primaryWeapon, Kit, 0) != null)
//            //    retrieve_kit.primaryWeapon = GetAttachments(retrieve_kit.primaryWeapon, Kit, 0);

//            retrieve_kit.secondaryWeapon = new Weapon() { weaponGUID = fixKey("kits", kit_index, 1, Convert.ToInt64(Kit[1])) };
//            //Weapon tmp2 = getAccessories(retrieve_kit.secondaryWeapon.weaponGUID);
//            //tmp2.weaponGUID = retrieve_kit.secondaryWeapon.weaponGUID;
//            //retrieve_kit.secondaryWeapon = tmp2;

//            retrieve_kit.gadgetOne = new Weapon() { weaponGUID = fixKey("kits", kit_index, 2, Convert.ToInt64(Kit[2])) };
//            retrieve_kit.gadgetTwo = new Weapon() { weaponGUID = fixKey("kits", kit_index, 3, Convert.ToInt64(Kit[3])) };
//            retrieve_kit.grenades = new Weapon() { weaponGUID = fixKey("kits", kit_index, 4, Convert.ToInt64(Kit[4])) };
//            retrieve_kit.knife = new Weapon() { weaponGUID = fixKey("kits", kit_index, 5, Convert.ToInt64(Kit[5])) };
//            retrieve_kit.fieldUpgrades = new Weapon() { weaponGUID = fixKey("kits", kit_index, 6, Convert.ToInt64(Kit[6])) };
//            retrieve_kit.unk7 = new Weapon() { weaponGUID = Convert.ToInt64(Kit[7]) };
//            retrieve_kit.unk8 = new Weapon() { weaponGUID = Convert.ToInt64(Kit[8]) };
//            retrieve_kit.soldierCamouflage = new Weapon() { weaponGUID = Convert.ToInt64(Kit[9]) };
//            retrieve_kit.parachuteCamouflage = new Weapon() { weaponGUID = Convert.ToInt64(Kit[10]) };
//            retrieve_kit.unk11 = new Weapon() { weaponGUID = Convert.ToInt64(Kit[11]) };
//            retrieve_kit.unk12 = new Weapon() { weaponGUID = Convert.ToInt64(Kit[12]) };
//            #region OLD
//            //retrieve_kit.primaryWeapon = new Weapon() { weaponGUID = fixKey(Convert.ToInt64(Kit[0]), category.PRIMARY) };
//            //if (GetAttachments(retrieve_kit.primaryWeapon, Kit, 0) != null)
//            //    retrieve_kit.primaryWeapon = GetAttachments(retrieve_kit.primaryWeapon, Kit, 0);

//            //retrieve_kit.secondaryWeapon = new Weapon() { weaponGUID = fixKey(Convert.ToInt64(Kit[1]), category.SECONDARY) };
//            //if (GetAttachments(retrieve_kit.secondaryWeapon, Kit, 1, true) != null) retrieve_kit.secondaryWeapon = GetAttachments(retrieve_kit.secondaryWeapon, Kit, 1, true);
//            //retrieve_kit.gadgetOne = new Weapon() { weaponGUID = fixKey(Convert.ToInt64(Kit[2]), category.GADGET1) };
//            //if (GetAttachments(retrieve_kit.gadgetOne, Kit, 2) != null) retrieve_kit.gadgetOne = GetAttachments(retrieve_kit.gadgetOne, Kit, 2);
//            //retrieve_kit.gadgetTwo = new Weapon() { weaponGUID = fixKey(Convert.ToInt64(Kit[3]), category.GADGET2) };
//            //if (GetAttachments(retrieve_kit.gadgetTwo, Kit, 3) != null) retrieve_kit.gadgetTwo = GetAttachments(retrieve_kit.gadgetTwo, Kit, 3);
//            //retrieve_kit.grenades = new Weapon() { weaponGUID = fixKey(Convert.ToInt64(Kit[4]), category.GRENADE) };
//            //if (GetAttachments(retrieve_kit.grenades, Kit, 4) != null) retrieve_kit.grenades = GetAttachments(retrieve_kit.grenades, Kit, 4);
//            //retrieve_kit.knife = new Weapon() { weaponGUID = fixKey(Convert.ToInt64(Kit[5]), category.MELEE) };
//            //if (GetAttachments(retrieve_kit.knife, Kit, 5) != null) retrieve_kit.knife = GetAttachments(retrieve_kit.knife, Kit, 5);
//            //retrieve_kit.fieldUpgrades = new Weapon() { weaponGUID = fixKey(Convert.ToInt64(Kit[6]), category.SPECIALIZATION) };
//            //if (GetAttachments(retrieve_kit.fieldUpgrades, Kit, 6) != null) retrieve_kit.fieldUpgrades = GetAttachments(retrieve_kit.fieldUpgrades, Kit, 6);
//            //retrieve_kit.unk7 = new Weapon() { weaponGUID = Convert.ToInt64(Kit[7]) };
//            //if (GetAttachments(retrieve_kit.unk7, Kit, 7) != null) retrieve_kit.unk7 = GetAttachments(retrieve_kit.unk7, Kit, 7);
//            //retrieve_kit.unk8 = new Weapon() { weaponGUID = Convert.ToInt64(Kit[8]) };
//            //if (GetAttachments(retrieve_kit.unk8, Kit, 8) != null) retrieve_kit.unk8 = GetAttachments(retrieve_kit.unk8, Kit, 8);
//            //retrieve_kit.soldierCamouflage = new Weapon() { weaponGUID = Convert.ToInt64(Kit[9]) };
//            //if (GetAttachments(retrieve_kit.soldierCamouflage, Kit, 9) != null) retrieve_kit.soldierCamouflage = GetAttachments(retrieve_kit.soldierCamouflage, Kit, 9);
//            //retrieve_kit.parachuteCamouflage = new Weapon() { weaponGUID = Convert.ToInt64(Kit[10]) };
//            //if (GetAttachments(retrieve_kit.parachuteCamouflage, Kit, 10) != null) retrieve_kit.parachuteCamouflage = GetAttachments(retrieve_kit.parachuteCamouflage, Kit, 10);
//            //retrieve_kit.unk11 = new Weapon() { weaponGUID = Convert.ToInt64(Kit[11]) };
//            //if (GetAttachments(retrieve_kit.unk11, Kit, 11) != null) retrieve_kit.unk11 = GetAttachments(retrieve_kit.unk11, Kit, 11);
//            //retrieve_kit.unk12 = new Weapon() { weaponGUID = Convert.ToInt64(Kit[12]) };
//            //if (GetAttachments(retrieve_kit.unk12, Kit, 12) != null) retrieve_kit.unk12 = GetAttachments(retrieve_kit.unk12, Kit, 12); 
//            #endregion

//            return (retrieve_kit);
//        }

//        public static Weapon GetAttachments(Weapon weapon, string[] Kit, int index, Boolean secondary = false)
//        {
//            if (weapons == null || !weapons.Contains(Kit[index]))
//                return null;

//            ArrayList arraylist = (ArrayList)weapons[Kit[index]];
//            string[] weapon_accessories = (string[])arraylist.ToArray(typeof(string));
//            weapon.weaponOPTIC = Convert.ToInt64(weapon_accessories[0]);
//            weapon.weaponACCESSORY = Convert.ToInt64(weapon_accessories[1]);
//            weapon.weaponBARREL = Convert.ToInt64(weapon_accessories[2]);
//            if (secondary)
//                weapon.weaponPAINT = Convert.ToInt64(weapon_accessories[3]);
//            else
//            {
//                weapon.weaponUNDERBARREL = Convert.ToInt64(weapon_accessories[3]);
//                weapon.weaponPAINT = Convert.ToInt64(weapon_accessories[4]);
//                weapon.weaponAMMO = Convert.ToInt64(weapon_accessories[5]);
//            }
//            return (weapon);
//        }

//        public static void GetAttachments(ref Weapon weapon, Int64 weaponGUID, Boolean secondary = false)
//        {
//            if (!weapons.Contains(weaponGUID.ToString()))
//                return;

//            ArrayList arraylist = (ArrayList)weapons[weaponGUID.ToString()];
//            string[] weapon_accessories = (string[])arraylist.ToArray(typeof(string));
//            if (secondary)
//                weapon = new Weapon()
//                {
//                    weaponGUID = weaponGUID,
//                    weaponOPTIC = Convert.ToInt64(weapon_accessories[0]),
//                    weaponACCESSORY = Convert.ToInt64(weapon_accessories[1]),
//                    weaponBARREL = Convert.ToInt64(weapon_accessories[2]),
//                    weaponPAINT = Convert.ToInt64(weapon_accessories[3])
//                };
//            else
//                weapon = new Weapon()
//                {
//                    weaponGUID = weaponGUID,
//                    weaponOPTIC = Convert.ToInt64(weapon_accessories[0]),
//                    weaponACCESSORY = Convert.ToInt64(weapon_accessories[1]),
//                    weaponBARREL = Convert.ToInt64(weapon_accessories[2]),
//                    weaponUNDERBARREL = Convert.ToInt64(weapon_accessories[3]),
//                    weaponPAINT = Convert.ToInt64(weapon_accessories[4]),
//                    weaponAMMO = Convert.ToInt64(weapon_accessories[5])
//                };
//        }
//    }

//    public class Kits
//    {
//        public byte selectedKit { get; set; }
//        public Kit[] kits { get; set; }
//        //public Kit Assault { get; set; }
//        //public Kit Engineer { get; set; }
//        //public Kit Support { get; set; }
//        //public Kit Recon { get; set; }
//    }

//    public class Kit
//    {
//        public Weapon primaryWeapon { get; set; }
//        public Weapon secondaryWeapon { get; set; }
//        public Weapon gadgetOne { get; set; }
//        public Weapon gadgetTwo { get; set; }
//        public Weapon grenades { get; set; }
//        public Weapon knife { get; set; }
//        public Weapon fieldUpgrades { get; set; }
//        public Weapon unk7 { get; set; }
//        public Weapon unk8 { get; set; }
//        public Weapon soldierCamouflage { get; set; }
//        public Weapon parachuteCamouflage { get; set; }
//        public Weapon unk11 { get; set; }
//        public Weapon unk12 { get; set; }
//    }

//    public class Weapon
//    {
//        public Int64 weaponGUID { get; set; }
//        public Int64 weaponOPTIC { get; set; }
//        public Int64 weaponACCESSORY { get; set; }
//        public Int64 weaponBARREL { get; set; }
//        public Int64 weaponUNDERBARREL { get; set; }
//        public Int64 weaponPAINT { get; set; }
//        public Int64 weaponAMMO { get; set; }
//    }

//    public class Replace
//    {
//        public String name { get; set; }
//        public category type { get; set; }
//        public Int64 old_key { get; set; }
//        public Int64 new_key { get; set; }
//    }

//    public sealed class HashtableProcessor
//    {
//        private HashtableProcessor() { }

//        public static Hashtable Add(Hashtable first, Hashtable second)
//        {
//            Hashtable table = new Hashtable();

//            foreach (DictionaryEntry e in first)
//            {
//                table.Add(e.Key, e.Value);
//            }

//            foreach (DictionaryEntry e in second)
//            {
//                if (!table.ContainsKey(e.Key))
//                    table.Add(e.Key, e.Value);
//            }

//            return table;
//        }
//    }
//}