using PRoCon.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BF4_LoadoutChecker
{
    public class allItemsCollection_
    {
        public Array models;
        public String name;
        public Boolean mySoldier;
    }
    public class model
    {
        public allItemsCollection_ items;
        public Boolean mySoldier;
        public int game;
        public Int64 personaId;
        public String personaName;
        public int platformInt;
        public String platformName;
        public String loadoutUrl;
        public Hashtable stats;
        public Hashtable licenses;
        public Hashtable presetLoadouts;
        public String premiumMaxPresetSlots;
        public String standardMaxPresetSlots;
        public Boolean isPremium;
        public ArrayList releasedXpacks;
        public Hashtable compatibility;
        public Hashtable structure;
    }
    public enum activeKit
    {
        ASSAULT = 0,
        ENGINEER = 1,
        SUPPORT = 2,
        RECON = 3,
        NOKIT = -1
    }

    public class Loadout_Backbone_Helper
    {
        public static Hashtable Loadout = null;
        public static String warsaw_loadout_js = null;
        public static activeKit activeKit = activeKit.NOKIT;
        public static Dictionary<String, String> Translations = null;

        private static Boolean game_Data_Loaded = false;
        private static String[] nonConfigurableItems = new String[] { "t62-cew" };
        public static allItemsCollection_ allItemsCollection = new allItemsCollection_() { models = null, name = "loadoutAllItemsCollection" };
        private static model model;

        /// <summary>
        /// Loads the warsaw.loadout.js
        /// </summary>
        /// <returns>Boolean (status)</returns>
        private static bool Initialize()
        {
            if (warsaw_loadout_js == null)
            {
                try
                {
                    // Check for cached version
                    if (File.Exists("warsaw.loadout.js"))
                    {
                        warsaw_loadout_js = File.ReadAllText("warsaw.loadout.js");
                    }
                    else // Download from the website
                    {
                        WebClient client = null;
                        if (client == null)
                            client = new WebClient();
                        warsaw_loadout_js = client.DownloadString("http://battlelog.battlefield.com/public/gamedatawarsaw/warsaw.loadout.js");
                        File.WriteAllText("warsaw.loadout.js", warsaw_loadout_js);
                    }
                    warsaw_loadout_js = warsaw_loadout_js.Split(new[] { "window.loadout.game_data = " }, StringSplitOptions.None)[1];
                    game_Data_Loaded = true;
                }
                catch (WebException e)
                {
                    if (e.Status.Equals(WebExceptionStatus.Timeout))
                        throw new Exception("HTTP request timed-out");
                    else
                        throw;
                }
            }
            return (true);
        }

        /// <summary>
        /// This initializes loadout and fetches all the data
        /// </summary>
        /// <param name="playerName"></param>
        public static object[] initialize(String playerName)
        {
            if (model == null)
                model = new model();
            Boolean loaded = false;

            // Items collection
            if (allItemsCollection.models != null && allItemsCollection.models.Length > 0) {
                loaded = true;
            }

            if (!playerName.Equals("initialize_loadot_backbone_helper"))
            {
                // Grab the fetch URL
                BattlelogClient bclient = new BattlelogClient();
                Loadout = bclient.getStats(playerName);
                if (Loadout == null)
                {
                    Debug.WriteLine("No such username!");
                    return (new object[] { new KeyValuePair<String, String>("type", "failed"), new KeyValuePair<String, String>("message", "No such username") });
                }

                //String loadoutUrl = "/bf4/loadout/get/[personaName]/[personaId]/[platformInt]/";

                model.items = allItemsCollection;
                model.mySoldier = true;
                allItemsCollection.mySoldier = true;
                if (model.mySoldier)
                {
                    model.personaId = Convert.ToInt64(Loadout["personaId"]);
                    model.personaName = (String)Loadout["personaName"];
                    model.platformInt = Convert.ToInt32(Loadout["platformInt"]);
                    model.platformName = (model.platformInt == 1) ? "pc" : "undefined";
                    model.game = Convert.ToInt32(Loadout["game"]);
                }
                model.loadoutUrl = String.Format("/bf4/loadout/get/{0}/{1}/{2}/", model.personaName, model.personaId, model.platformInt);
                model.stats = (Hashtable)Loadout["playerStats"];
                model.licenses = (Hashtable)Loadout["playerLicenses"];
                model.presetLoadouts = (Hashtable)Loadout["presets"];
                model.premiumMaxPresetSlots = Loadout["maxPresetsPremium"].ToString();
                model.standardMaxPresetSlots = Loadout["maxPresetsStandard"].ToString();
                model.isPremium = Convert.ToBoolean(Loadout["isPremium"]);
                model.releasedXpacks = (ArrayList)Loadout["releasedXpacks"];
                var currentLoadout = (Hashtable)Loadout["currentLoadout"];
            }

            // If we haven't loaded the items, add them. No need to load them again if we've previously visited Loadout
            if (!loaded)
            {
                // Load the game_Data
                Initialize();

                // Add loadout structure to the model
                model.structure = get_structure();
                // Add the item compatibility list
                model.compatibility = (Hashtable)model.structure["compatibility"];

                var json = (Hashtable)JSON.JsonDecode(warsaw_loadout_js);
                var data = (Hashtable)json["compact"];
                var structure = (Hashtable)json["loadout"];

                var items = new ArrayList();
                var k = 0;
                var kLen = ((ArrayList)structure["kits"]).Count;
                while (k < kLen)
                {
                    // Have to pop twice for BF4 to remove parachute and soldier camo
                    if (((ArrayList)((Hashtable)((ArrayList)structure["kits"])[k])["slots"]).Count > 8)
                    {
                        ((ArrayList)((Hashtable)((ArrayList)structure["kits"])[k])["slots"]).RemoveAt(((ArrayList)((Hashtable)((ArrayList)structure["kits"])[k])["slots"]).Count - 1);
                    }

                    ((ArrayList)((Hashtable)((ArrayList)structure["kits"])[k])["slots"]).RemoveAt(((ArrayList)((Hashtable)((ArrayList)structure["kits"])[k])["slots"]).Count - 1);
                    k++;
                }

                // Parse all the items
                Hashtable tmp = new Hashtable();
                tmp.Add("item", (Hashtable)data["kititems"]);
                tmp.Add("vehicle", (Hashtable)data["vehicles"]);
                tmp.Add("appearance", (Hashtable)data["appearances"]);
                tmp.Add("vehicleUnlock", (Hashtable)data["vehicleunlocks"]);
                tmp.Add("accessory", (Hashtable)data["weaponaccessory"]);
                tmp.Add("weapon", (Hashtable)data["weapons"]);
                data = tmp;

                foreach (DictionaryEntry categoryName in data)
                {
                    var category = (Hashtable)data[categoryName.Key];
                    foreach (DictionaryEntry guid in category)
                    {
                        Hashtable item = (Hashtable)category[guid.Key];
                        item.Add("itemType", categoryName.Key);
                        var see = (ArrayList)item["see"];
                        if (see != null) {
                            var i = 0;
                            var len = see.Count;
                            while (i < len) {
                                item = HashtableProcessor.Add(item, (Hashtable)category[see[i]]);
                                i++;
                            }
                            item.Add("guid", guid.Key);
                            if (categoryName.Key.Equals("weapon")) // UNTESTED
                            {
                                item.Add("configurable", false);
                                var weapon = (Hashtable)((Hashtable)structure["weapons"])[guid.Key];
                                if (weapon != null && (ArrayList)weapon["slots"] != null && !nonConfigurableItems.Contains(item["slug"].ToString())) {
                                    item["configurable"] = true;
                                }
                            }

                            if (model.compatibility[guid.Key] != null) {
                                item.Add("compatibility", (Hashtable)model.compatibility[guid.Key]);
                                if (((Hashtable)item["compatibility"])["items"] != null) {
                                    item.Add("compatibilityLocked", Convert.ToBoolean(((Hashtable)item["compatibility"])["exclusive"]));
                                } else {
                                    item["compatibility"] = false;
                                }
                            } else {
                                item["compatibility"] = false;
                            }

                            if (item["req"] != null) {
                                var skipItem = false;
                                var r = ((ArrayList)item["req"]).Count;
                                while (r-- > 0) {
                                    var req = (Hashtable)((ArrayList)item["req"])[r];
                                    if (!playerName.Equals("initialize_loadot_backbone_helper") && req["t"].Equals("l") && req["l"].Equals("dt_dev_team") && model.licenses[req["l"]] == null) {
                                        skipItem = true;
                                        break;
                                    }
                                }

                                if (skipItem) {
                                    // Work-around for the 3x scope on the M1911 being given to all players
                                    if (item["guid"].Equals("2852909715")) {
                                        item.Remove("req");
                                    } else {
                                        continue;
                                    }
                                }
                            }
                            items.Add(item);
                        }
                    }
                }

                // This adds all items to the massive global collection
                allItemsCollection.models = (items.ToArray(typeof(Hashtable)) as Hashtable[]);

                #region DEBUGGING
#if DEBUG
                //String[] itemTypes = new String[] { "appearance", "vehicleUnlock", "weapon", "item", "vehicle", "accessory" };
                //for (int j = 0; j < itemTypes.Length; j++)
                //    using (StreamWriter sw = new StreamWriter(itemTypes[j] + ".csv"))
                //    {
                //        sw.WriteLine("itemType;category;name;slug;guid");
                //        for (int i = 0; i < allItemsCollection.models.Length; i++)
                //            if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("itemType"))
                //                if (((Hashtable)allItemsCollection.models.GetValue(i))["itemType"].ToString().Equals(itemTypes[j]))
                //                {
                //                    sw.Write(String.Format("{0};", ((Hashtable)allItemsCollection.models.GetValue(i))["itemType"].ToString()));
                //                    if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("category"))
                //                        sw.Write(String.Format("{0};", getSID(((Hashtable)allItemsCollection.models.GetValue(i))["category"].ToString())));
                //                    else if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("categoryType"))
                //                        sw.Write(String.Format("{0};", ((Hashtable)allItemsCollection.models.GetValue(i))["categoryType"].ToString()));
                //                    else
                //                        sw.Write(";");
                //                    if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("name"))
                //                        sw.Write(String.Format("{0};", getSID(((Hashtable)allItemsCollection.models.GetValue(i))["name"].ToString())));
                //                    else
                //                        sw.Write(";");
                //                    if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("slug"))
                //                        sw.Write(String.Format("{0};", ((Hashtable)allItemsCollection.models.GetValue(i))["slug"].ToString()));
                //                    else
                //                        sw.Write(";");
                //                    if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("guid"))
                //                        sw.Write(String.Format("{0}", getSID(((Hashtable)allItemsCollection.models.GetValue(i))["guid"].ToString())));
                //                    else
                //                        sw.Write(";");
                //                    sw.Write(Environment.NewLine);
                //                }
                //    }
                //using (StreamWriter sw = new StreamWriter("combined.csv"))
                //{
                //    sw.WriteLine("itemType;category;name;slug;guid");
                //    for (int j = 0; j < itemTypes.Length; j++)
                //    {
                //        for (int i = 0; i < allItemsCollection.models.Length; i++)
                //        {
                //            if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("itemType"))
                //            {
                //                if (((Hashtable)allItemsCollection.models.GetValue(i))["itemType"].ToString().Equals(itemTypes[j]))
                //                {
                //                    sw.Write(String.Format("{0};", ((Hashtable)allItemsCollection.models.GetValue(i))["itemType"].ToString()));
                //                    if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("category"))
                //                        sw.Write(String.Format("{0};", getSID(((Hashtable)allItemsCollection.models.GetValue(i))["category"].ToString())));
                //                    else if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("categoryType"))
                //                        sw.Write(String.Format("{0};", ((Hashtable)allItemsCollection.models.GetValue(i))["categoryType"].ToString()));
                //                    else
                //                        sw.Write(";");
                //                    if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("name"))
                //                        sw.Write(String.Format("{0};", getSID(((Hashtable)allItemsCollection.models.GetValue(i))["name"].ToString())));
                //                    else
                //                        sw.Write(";");
                //                    if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("slug"))
                //                        sw.Write(String.Format("{0};", ((Hashtable)allItemsCollection.models.GetValue(i))["slug"].ToString()));
                //                    else
                //                        sw.Write(";");
                //                    if (((Hashtable)allItemsCollection.models.GetValue(i)).ContainsKey("guid"))
                //                        sw.Write(String.Format("{0}", getSID(((Hashtable)allItemsCollection.models.GetValue(i))["guid"].ToString())));
                //                    else
                //                        sw.Write(";");
                //                    sw.Write(Environment.NewLine);
                //                }
                //            }
                //        }
                //        sw.Write(Environment.NewLine);
                //    }
                //}
#endif 
                #endregion

                // Set our model
                model.structure = structure;

                // CHECK UNLOCK
                Hashtable result = checkUnlock("69312926");
            }
            return (new object[] { new KeyValuePair<String, String>("type", "success"), new KeyValuePair<String, String>("message", "OK") });
        }

        /// <summary>
        /// Gets all the unlocked GUIDs for a given Kit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static Hashtable getKitUnlocks(int kit, Boolean selectedKit)
        {
            List<String> unlockedGuids = new List<String>();
            var structure = get_structure();
            var items = get_items();
            var currentLoadout = get_loadout(); if (currentLoadout == null) return null;
            var slotAccessories = get_slotAccessories();
            var activeKit = (selectedKit) ? Convert.ToInt32(currentLoadout["selectedKit"]) : kit;
            var data = new Hashtable()
            {
                { 0, new ArrayList() }, // PRIMARY
                { 1, new ArrayList() }, // SECONDARY
                { 2, new ArrayList() }, // GADGET1
                { 3, new ArrayList() }, // GADGET2
                { 4, new ArrayList() }, // GRENADE
                { 5, new ArrayList() }, // KNIFE
                { 6, new ArrayList() }  // SPECIALIZATION
            };

            // Kits
            if (structure["kits"] != null)
            {
                var slots = (ArrayList)((Hashtable)((ArrayList)structure["kits"])[activeKit])["slots"];
                var j = 0;
                var slotLen = slots.Count;
                while (j < slotLen)
                {
                    var slot = (Hashtable)slots[j];
                    var sitems = (ArrayList)slot["items"];
                    var y = 0;
                    var itemLen = sitems.Count;
                    while (y < itemLen)
                    {
                        if (!(Boolean)(checkUnlock(sitems[y].ToString())["locked"]))
                            ((ArrayList)data[j]).Add(sitems[y].ToString());
                        y++;
                    }
                    j++;
                }

            }
            return (data);
        }

        /// <summary>
        /// Gets all the unlocked GUIDs for a given id and slot
        /// </summary>
        /// <param name="id"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static String[] getUnlockedItems(int id, int slot_id)
        {
            List<String> unlockedGuids = new List<String>();
            var structure = get_structure();
            if (structure["kits"] != null)
            {
                var slot = (Hashtable)((ArrayList)((Hashtable)((ArrayList)structure["kits"])[id])["slots"])[slot_id];
                var items = (ArrayList)slot["items"];
                for (int i = 0; i < items.Count; i++)
                {
                    if (!((Boolean)checkUnlock(items[i].ToString())["locked"]))
                        unlockedGuids.Add(items[i].ToString());
                }
                    
            }
            return (unlockedGuids.ToArray());
        }

        /// <summary>
        /// This creates the overview object that the view renders
        /// </summary>
        /// <param name="getPresets">boolean Include the kit/vehicle presets list</param>
        /// <returns>{{kits: Array, vehicles: Array}}</returns>
        public static Hashtable getCurrentOverview(Boolean getPresets)
        {
            var structure = get_structure();
            var items = get_items();
            var currentLoadout = get_loadout(); if (currentLoadout == null) return null;
            var slotAccessories = get_slotAccessories();
            var data = new Hashtable(); data.Add("kits", new ArrayList()); data.Add("vehicles", new ArrayList()); data.Add("presets", new Hashtable());
            data.Add("selectedKit", get_selectedKit());
            data.Add("activeKit", get_activeKit());
            data.Add("type", null);

            if (structure["kits"] != null)
            {
                // Loop through the kits first, generating all missing items/weapons as it goes along
                var kitLen = ((ArrayList)structure["kits"]).Count;
                var i = 0;
                while (i < kitLen)
                {
                    var kit = (Hashtable)((ArrayList)structure["kits"])[i];
                    ((ArrayList)data["kits"]).Add(new Hashtable());
                    ((Hashtable)((ArrayList)data["kits"])[i]).Add("sid", kit["sid"].ToString());
                    ((Hashtable)((ArrayList)data["kits"])[i]).Add("slots", new ArrayList(0));
                    ((Hashtable)((ArrayList)data["kits"])[i]).Add("iconClass", get_getClassInt(i));
                    if (((ArrayList)currentLoadout["kits"])[i] == null)
                    {
                        ((ArrayList)currentLoadout["kits"])[i] = new Array[((ArrayList)((Hashtable)((ArrayList)structure["kits"])[i])["slots"]).Count];
                    }
                    var j = 0;
                    var slotLen = ((ArrayList)kit["slots"]).Count;
                    while (j < slotLen)
                    {
                        var slot = (Hashtable)((ArrayList)kit["slots"])[j];
                        var guid = ((ArrayList)((ArrayList)currentLoadout["kits"])[i])[j] != null ? ((ArrayList)((ArrayList)currentLoadout["kits"])[i])[j].ToString() : 0.ToString();
                        var item = getItem("kits", i, j, guid);
                        if (item == null)
                        {
                            j++;
                            continue;
                        }
                        ((ArrayList)((ArrayList)currentLoadout["kits"])[i])[j] = item["guid"];
                        if (Array.IndexOf(slotAccessories, j) != -1)
                        {
                            item["slots"] = getAccessories(item["guid"].ToString());
                        }
                        ((ArrayList)((Hashtable)((ArrayList)data["kits"])[i])["slots"]).Add(new Hashtable() { { "sid", slot["sid"].ToString() }, { "item", item } });
                        j++;
                    }
                    i++;
                }

                // Now loop through the vehicles, generating any missing items
                var f = 0;
                var vehicleLen = ((ArrayList)structure["vehicles"]).Count;
                while (f < vehicleLen)
                {
                    var vehicle = (Hashtable)((ArrayList)structure["vehicles"])[f];
                    var vehicleImage = items_get(allItemsCollection, vehicle["sid"].ToString());
                    ((ArrayList)data["vehicles"]).Add(new Hashtable());
                    if (vehicleImage == null)
                    {
                        f++;
                        continue;
                    }
                    ((Hashtable)((ArrayList)data["vehicles"])[f]).Add("imageConfig", (Hashtable)vehicleImage["imageConfig"]);
                    ((Hashtable)((ArrayList)data["vehicles"])[f]).Add("sid", vehicle["sid"].ToString());
                    ((Hashtable)((ArrayList)data["vehicles"])[f]).Add("slots", new ArrayList(0));
                    if (((ArrayList)currentLoadout["vehicles"])[f] == null)
                    {
                        ((ArrayList)currentLoadout["vehicles"])[f] = new Array[((ArrayList)((Hashtable)((ArrayList)structure["vehicles"])[f])["slots"]).Count];
                    }
                    var vslotLen = ((ArrayList)vehicle["slots"]).Count;
                    var g = 0;
                    while (g < vslotLen)
                    {
                        var vslot = (Hashtable)((ArrayList)vehicle["slots"])[g];
                        if (((ArrayList)vslot["items"]).Count > 0)
                        {
                            var vguid = ((ArrayList)((ArrayList)currentLoadout["vehicles"])[f])[g].ToString() ?? "0";
                            var vitem = getItem("vehicles", f, g, vguid);
                            if (vitem != null)
                            {
                                ((ArrayList)((ArrayList)currentLoadout["vehicles"])[f])[g] = vitem["guid"];
                                ((ArrayList)((Hashtable)((ArrayList)data["vehicles"])[f])["slots"]).Add(new Hashtable() { { "sid", vslot["sid"].ToString() }, { "item", vitem } });
                            }
                        }
                        g++;
                    }
                    f++;
                }
                // Updates the current loadout in case we had to generate any items
                if (getPresets)
                {
                    data["presets"] = get_presetLoadouts();

                    // Find a preset that matches the currently selected items to see if it's "active"
                    foreach (DictionaryEntry type in (Hashtable)data["presets"])
                    {
                        var types = (Hashtable)((Hashtable)data["presets"])[type.Key];
                        foreach (DictionaryEntry id in (Hashtable)types)
                        {
                            var presets = (Hashtable)((Hashtable)types[id.Key])["presets"];
                            var current = String.Join("", ((ArrayList)((ArrayList)currentLoadout[type.Key])[Convert.ToInt32(id.Key)]).Cast<String>().ToArray());
                            foreach (DictionaryEntry key in (Hashtable)presets)
                            {
                                var preset = String.Join("", ((ArrayList)((Hashtable)presets[key.Key])["ids"]).Cast<String>().ToArray());
                                if (preset == current)
                                {
                                    ((Hashtable)types[id.Key]).Add("active", key.Key);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return (data);
        }

        /// <summary>
        /// Sets the unlock requirements for a locked item
        /// </summary>
        /// <param name="getPresets">boolean Include the kit/vehicle presets list</param>
        /// <returns>object { unlockCriterias, battlepack, award, locked }</returns>
        public static Hashtable checkUnlock(String guid)
        {
            var locked = false;
            var battlepack = false;
            var award = false;
            Hashtable tmp = items_get(get_items(), guid);
            var requirements = (tmp == null || !tmp.ContainsKey("req")) ? null : (ArrayList)tmp["req"];
            var criterias = new Hashtable(); criterias.Add("percentage", 100);

            var stats = model.stats;
            var licenses = model.licenses;
            if (requirements != null)
            {
                var i = requirements.Count;
                while (i-- > 0)
                {
                    var requirement = (Hashtable)requirements[i];
                    var key = (requirement.ContainsKey("c")) ? requirement["c"].ToString() : null;
                    var needed = (requirement.ContainsKey("v")) ? Int32.Parse(requirement["v"].ToString()) : -1;
                    var have = 0;
                    var unlockType = requirement["t"].ToString();

                    if (unlockType == "b")
                    {
                        unlockType = "bucket";
                        battlepack = false;
                        award = false;
                        have = (stats.ContainsKey(key)) ? Int32.Parse(stats[key].ToString()) : 0;
                    }
                    else if (unlockType == "w")
                    {
                        unlockType = "weapon";
                        battlepack = false;
                        award = false;
                        have = (stats.ContainsKey(key)) ? Int32.Parse(stats[key].ToString()) : 0;
                    }
                    else if (unlockType == "a")
                    {
                        unlockType = "award";
                        battlepack = false;
                        award = true;
                        have = (stats.ContainsKey(key)) ? Int32.Parse(stats[key].ToString()) : 0;
                    }
                    else if (unlockType == "l")
                    {
                        key = requirement["l"].ToString();
                        needed = 1;
                        unlockType = "license";
                        battlepack = true;
                        award = false;
                        have = licenses[key] != null ? 1 : 0;
                    }
                    else if (unlockType == "c")
                    {
                        key = requirement["c"].ToString();
                        needed = 1;
                        unlockType = "consumable";
                        battlepack = false;
                        award = false;
                        have = (stats.ContainsKey(key)) ? Int32.Parse(stats[key].ToString()) : 0;
                    }
                    else if (unlockType == "r")
                    {
                        key = requirement["c"].ToString();
                        unlockType = "rank";
                        have = (stats.ContainsKey(key)) ? Int32.Parse(stats[key].ToString()) : 0;
                    }

                    criterias = new Hashtable();
                    criterias.Add("codeNeeded", key);
                    criterias.Add("valueNeeded", needed);
                    criterias.Add("actualValue", have);
                    criterias.Add("unlockType", unlockType);
                    criterias.Add("license", false);
                    criterias.Add("award", false);

                    if (key.ToLower().Substring(0, 2) == "xp")
                    {
                        criterias.Add("expansion","xp" + key.Substring(2, 3));
                    }

                    if (have < needed || have < 1)
                    {
                        locked = true;
                        criterias.Add("percentage", have / needed * 100);
                    }
                    else
                    {
                        locked = false;
                        criterias.Add("percentage", 100);
                        break;
                    }

                }
            }
            return (new Hashtable() { { "unlockCriterias", criterias }, { "battlepack", battlepack }, { "award", award }, { "locked", locked } });
        }

        /**
         * Calculates the amount of unlocks that the soldier has unlocked versus the total available for a given item
         */
        public static void calculateUnlocks()
        {
            //var guid = this.id;
            //var model = BL.backbone.get({ model: this.collection.mySoldier ? 'loadoutModel' : 'friendLoadoutModel'});
            //var collection = model.get('items');
            //var structure = model.get('structure');

            //var slots = structure.weapons[guid].slots || false;
            //if (slots)
            //{
            //    var total = 0;
            //    var unlocked = 0;

            //    var i = 0;
            //    // Get all slots minus the last one, which is camos
            //    var len = slots.length - 1;
            //    while (i < len)
            //    {
            //        var items = slots[i].items;

            //        // Skip first item since it's the "no selection" item and thus not valid
            //        var j = 1;
            //        var len2 = items.length;
            //        while (j < len2)
            //        {
            //            var accessory = collection.get(items[j]);
            //            var locked = true;
            //            if (accessory)
            //            {
            //                locked = accessory.get('locked');
            //                if (!locked)
            //                {
            //                    unlocked++;
            //                }
            //                total++;
            //            }
            //            j++;
            //        }

            //        i++;
            //    }
            //    this.set({ unlockStats: { total: total,unlocked: unlocked} });
            //}
        }

        /// <summary>
        /// This will fetch a valid item for a given slot, type, and id.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="slot"></param>
        /// <param name="guid"></param>
        /// <returns>{}</returns>
        public static Hashtable getItem(string type, int id, int slot, string guid)
        {
            var items = allItemsCollection;
            var structure = get_structure();
            var item = items_get(items, guid);
            var guids = ((ArrayList)((Hashtable)((ArrayList)((Hashtable)((ArrayList)structure[type])[id])["slots"])[slot])["items"]);

            if (item == null || !guids.Contains(guid)) {
                if (guid.Equals("277773742")) guid = "27972285"; // C100 [KNIFE]
                else if (guid.Equals("3260690101")) guid = "312950893"; // BALLISTIC SHIELD [GADGET]
                //else if (guid.Equals("")) guid = ""; // TANTO [KNIFE]
                else guid = guids[0].ToString();
                item = items_get(items, guid);
            }
            return (item);
        }

        /// <summary>
        /// Get the accessories for a weapon, generating them if 0, undefined, or invalid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>{*}</returns>
        public static ArrayList getAccessories(String guid, Boolean asInt = false)
        {
            Hashtable loadout = (Hashtable)Loadout["currentLoadout"];
            var items = allItemsCollection;
            Hashtable structure = get_structure();
            ArrayList accessories = (ArrayList)((Hashtable)loadout["weapons"])[guid];

            if (!((Hashtable)structure["weapons"]).Contains(guid)) {
                Console.WriteLine("{0} does not exist in structure.weapons", guid);
                return null;
            }

            var slots = (ArrayList)((Hashtable)((Hashtable)structure["weapons"])[guid])["slots"];
            if (accessories == null) {
                ((Hashtable)loadout["weapons"])[guid] = new ArrayList();
                accessories = new ArrayList();
            }

            ArrayList fixedAccessories = new ArrayList();
            var i = 0;
            var len = slots.Count;
            while (i < len) {
                Hashtable item = null;
                if (i < slots.Count) {
                    if (Convert.ToInt64(accessories[i]) > 0) {
                        item = items_get(items, accessories[i].ToString());
                        if (item == null || !((ArrayList)((Hashtable)slots[i])["items"]).Contains(accessories[i].ToString())) {
                            item = items_get(items, generateSlot("weapons", guid, i));
                            if (!items_get(items, item["guid"].ToString(), true))
                                item = null;
                        }
                    } else {
                        item = items_get(items, generateSlot("weapons", guid, i));
                        if (!items_get(items, item["guid"].ToString(), true))
                            item = null;
                    }
                } else {
                    item = null;
                }

                if (item != null) {
                    if (asInt)
                    {
                        fixedAccessories.Add(item["guid"].ToString());
                    } else {
                        if (!item.ContainsKey("slotSid"))
                            item.Add("slotSid", ((Hashtable)slots[i])["sid"].ToString());
                        fixedAccessories.Add(item);
                    }
                    
                }
                i++;
            }
            return fixedAccessories;
        }

        /// <summary>
        /// Brings back the first item for the slot of type specified
        /// </summary>
        /// <param name="type">This is the type of the slot, can be weapons, vehicles, or kits</param>
        /// <param name="guid">This is the identifier for the type you're trying to change (kit 1, weapon 1234667, etc.)</param>
        /// <param name="slot">This is the slot number</param>
        /// <returns>{String} Returns the identifier of the item</returns>
        public static String generateSlot(String type, String guid, int slot)
        {
            var structure = get_structure();
            var loadout = (Hashtable)Loadout["currentLoadout"];
            var selectedSlot = (type.Equals("weapons")) ? ((ArrayList)((Hashtable)((Hashtable)structure[type])[guid.ToString()])["slots"])[slot] :
                (type.Equals("kits") || type.Equals("vehicles")) ? ((ArrayList)((Hashtable)((ArrayList)structure[type])[Convert.ToInt32(guid)])["slots"])[slot] : null;
            var item = "0";
            if (selectedSlot != null) {
                item = (type.Equals("weapons")) ? ((ArrayList)((Hashtable)((ArrayList)((Hashtable)((Hashtable)structure[type])[guid])["slots"])[slot])["items"])[0].ToString() :
                    (type.Equals("kits") || type.Equals("vehicles")) ? ((ArrayList)((Hashtable)((ArrayList)((Hashtable)((ArrayList)structure[type])[Convert.ToInt32(guid)])["slots"])[slot])["items"])[0].ToString() : "0";
                if (type.Equals("weapons"))
                    ((ArrayList)((Hashtable)loadout[type])[guid])[slot] = item;
                else
                    ((ArrayList)((ArrayList)loadout[type])[Convert.ToInt32(guid)])[slot] = item;
                Loadout["currentLoadout"] = loadout;
            }
            return item;
        }

        public static Boolean items_get(allItemsCollection_ items, string guid, Boolean ret = true)
        {
            if (items_get(items, guid) != null)
                return (true);
            else
                return (false);
        }
        public static Hashtable items_get(allItemsCollection_ items, string guid)
        {
            foreach (Hashtable hTable in items.models) {
                if (hTable["guid"].Equals(guid.ToString())) {
                    return (hTable);
                }
            }
            return (null);
        }

        private static Hashtable get_structure()
        {
            if (!game_Data_Loaded)
                if (Initialize())
                    return ((Hashtable)((Hashtable)JSON.JsonDecode(warsaw_loadout_js))["loadout"]);
                else
                    return (null);
            else if (model.structure != null)
                return (model.structure);
            else
                return ((Hashtable)((Hashtable)JSON.JsonDecode(warsaw_loadout_js))["loadout"]);
        }
        private static allItemsCollection_ get_items()
        {
            if (allItemsCollection.models == null || allItemsCollection.models.Length < 1)
                initialize("DICE");
            return (allItemsCollection);
        }
        private static Hashtable get_loadout()
        {
            if (Loadout == null)
                throw new Exception("Loadout is null!");
            if (Loadout.ContainsKey("error"))
                Debug.WriteLine(String.Format("Error: {0}", Loadout["error"].ToString()));
            if (!Loadout.ContainsKey("currentLoadout"))
                return (null);
            else
                return ((Hashtable)Loadout["currentLoadout"]);
        }
        private static Hashtable get_presetLoadouts()
        {
            if (Loadout == null)
                throw new Exception("Loadout is null!");
            if (Loadout.ContainsKey("error"))
                Debug.WriteLine(String.Format("Error: {0}", Loadout["error"].ToString()));
            if (!Loadout.ContainsKey("presets"))
                return (null);
            else
                return ((Hashtable)Loadout["presets"]);
        }
        private static Array get_slotAccessories()
        {
            return (new int[] { 0, 1 });
        }
        private static String get_selectedKit()
        {
            if (Loadout == null || Loadout["currentLoadout"] == null)
                throw new Exception("Loadout error!");
            else
                return (((Hashtable)Loadout["currentLoadout"])["selectedKit"].ToString());
        }
        private static int get_activeKit()
        {
            return ((int)activeKit);
        }
        private static String[] get_statKeys()
        {
            return (new String[] { "Damage", "Accuracy", "Mobility", "Range", "Handling" });
        }

        /// <summary>
        /// Get the translate string for the given sid, or return the sid untranslated if none found
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static String getSID(String sid)
        {
            if (Translations == null)
                Initialize_Translations();
            return ((Translations.ContainsKey(sid)) ? Translations[sid] : sid);
        }

        /// <summary>
        /// Convert the class int from 0-3 to what Battlelog uses
        /// </summary>
        /// <param name="kit"></param>
        /// <returns>{*}</returns>
        private static int get_getClassInt(int kit)
        {
            switch (kit)
            {
                // Assault
                case 0:
                    kit = 1;
                    break;
                // Engineer
                case 1:
                    kit = 2;
                    break;
                // Support
                case 2:
                    kit = 32;
                    break;
                // Recon
                case 3:
                    kit = 8;
                    break;
                // Default goes to Assault
                default:
                    kit = 1;
                    break;
            }
            return kit;
        }

        /// <summary>
        /// Creates and Dictionary with the weaponData
        /// </summary>
        /// <param name="weaponData"></param>
        /// <returns></returns>
        private static Dictionary<String, double> get_weaponDataRelative(Hashtable weaponData)
        {
            var weaponDataRelative = new Dictionary<String, double>();
            var statKeys = get_statKeys();
            var sLen = statKeys.Length;

            var s = 0;
            while (s < sLen)
            {
                weaponDataRelative.Add("stat" + statKeys[s], Convert.ToDouble(weaponData["stat" + statKeys[s]]));
                s++;
            }
            return (weaponDataRelative);
        }

        /// <summary>
        /// Calculates the weapon stats taking into account the equipped acessories for it, can also pass in a modifier object to get the new base values returned
        /// @param [modifierOverride] {*} {0:{statDamage: 1.33,statAccuracy: 1,...},...} Array of weaponData stats by slot index, you can provide some or all slots
        /// </summary>
        /// <param name="item"></param>
        /// <param name="modifierOverride"></param>
        /// <returns></returns>
        public static object calculateModifiers(Hashtable item, Hashtable modifierOverride)
        {
            if (item["itemType"].Equals("weapon"))
            {
                var weaponData = (Hashtable)item["weaponData"];
                if (weaponData != null)
                {
                    var items = get_items();
                    var loadout = get_loadout();
                    var weaponDataRelative = get_weaponDataRelative(weaponData);
                    var statKeys = get_statKeys();
                    var sLen = statKeys.Length;
                    var accessoryGuids = getAccessories(item["guid"].ToString(), true);
                    var slotModifiers = new Dictionary<String, List<double>>();

                    var s = 0;
                    while (s < sLen)
                    {
                        slotModifiers.Add(statKeys[s], new List<double>());
                        s++;
                    }

                    var a = 0;
                    var aLen = accessoryGuids.Count;
                    while (a < aLen)
                    {
                        var accessoryGuid = Convert.ToInt64(accessoryGuids[a]);
                        if (accessoryGuid != 0)
                        {
                            var accessoryData = new Hashtable();
                            if (modifierOverride != null && modifierOverride[a] != null) {
                                accessoryData = (Hashtable)modifierOverride[a];
                            } else {
                                accessoryData = (Hashtable)items_get(allItemsCollection, accessoryGuid.ToString())["weaponData"];
                            }
                            if (accessoryData != null)
                            {
                                s = 0;
                                while (s < sLen)
                                {
                                    var statKey = "stat" + statKeys[s];
                                    if (Convert.ToSingle(accessoryData[statKey]) != 1)
                                    {
                                        var statValue = Convert.ToDouble(accessoryData[statKey]);
                                        var baseStat = Convert.ToDouble(weaponData[statKey]);

                                        var difference = (double)0;
                                        if (statValue > 1)
                                        {
                                            difference = baseStat * (statValue - 1.0F);
                                        }
                                        else if (statValue < 0)
                                        {
                                            difference = baseStat * statValue;
                                        }
                                        else if (statValue < 1 && statValue != 0)
                                        {
                                            difference = baseStat * statValue - baseStat;
                                        }

                                        if (!weaponDataRelative.ContainsKey(statKey))
                                            throw new Exception(String.Format("weaponDataRelative doesn't contain key {0}", statKey));

                                        weaponDataRelative[statKey] += difference;
                                    }
                                    slotModifiers[statKeys[s]].Add(Convert.ToSingle(accessoryData[statKey]));
                                    s++;
                                }
                            }
                        }
                        a++;
                    }
                    if (modifierOverride != null)
                    {
                        return weaponDataRelative;
                    }
                    else
                    {
                        return (new object[] {
                            new KeyValuePair<String, Dictionary<String, double>>("weaponDataRelative", weaponDataRelative),
                            new KeyValuePair<String, Dictionary<String, List<double>>>("newslotModifiers", slotModifiers) });
                    }
                }
            }
            return (null);
        }

        /// <summary>
        /// Loads the en_US.js
        /// </summary>
        public static void Initialize_Translations()
        {
            if (Translations == null)
            {
                try
                {
                    // If there is no cached version, download
                    if (!File.Exists("en_US.js")) {
                        WebClient client = null;
                        if (client == null)
                            client = new WebClient();
                        String en_US_js = client.DownloadString("http://eaassets-a.akamaihd.net/bl-cdn/cdnprefix/production-253-adf-b/public/dynamic/bf4/en_US.js");
                        File.WriteAllText("en_US.js", en_US_js);
                    }
                    // Process
                    if (!File.Exists("en_US.js"))
                        return;

                    String[] translations = File.ReadAllLines("en_US.js");
                    Translations = new Dictionary<String, String>();
                    foreach (String line in translations)
                        if (line.StartsWith("t")) {
                            KeyValuePair<String, String> tmp = processTranslation(line);
                            Translations.Add(tmp.Key, tmp.Value);
                        }
                }
                catch (WebException e)
                {
                    if (e.Status.Equals(WebExceptionStatus.Timeout))
                        throw new Exception("HTTP request timed-out");
                    else
                        throw;
                }
            }
        }

        /// <summary>
        /// Processes one of the translation lines
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static KeyValuePair<String, String> processTranslation(String line)
        {
            // Extract the key
            int pFrom = line.IndexOf("t['") + "t['".Length;
            int pTo = line.LastIndexOf("']");
            String key = line.Substring(pFrom, pTo - pFrom);

            // Extract the value
            int _pFrom = line.IndexOf("']=\"") + "']=\"".Length;
            int _pTo = line.LastIndexOf("\";");
            String value = line.Substring(_pFrom, _pTo - _pFrom);

            return (new KeyValuePair<String, String>(key, value));
        }

        /// <summary>
        /// Merges two hashtables
        /// </summary>
        public sealed class HashtableProcessor
        {
            private HashtableProcessor() { }

            public static Hashtable Add(Hashtable first, Hashtable second)
            {
                Hashtable table = new Hashtable();

                foreach (DictionaryEntry e in first) {
                    table.Add(e.Key, e.Value);
                }

                foreach (DictionaryEntry e in second) {
                    if (!table.ContainsKey(e.Key))
                        table.Add(e.Key, e.Value);
                }

                return table;
            }
        }
    }
}
