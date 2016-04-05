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
        private static allItemsCollection_ allItemsCollection = new allItemsCollection_() { models = null, name = "loadoutAllItemsCollection" };
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
        public static void initialize(String playerName)
        {
            if (model == null)
                model = new model();
            Boolean loaded = false;

            // Items collection
            if (allItemsCollection.models != null && allItemsCollection.models.Length > 0) {
                loaded = true;
            }

            // Grab the fetch URL
            BattlelogClient bclient = new BattlelogClient();
            Loadout = bclient.getStats(playerName);
            if (Loadout == null)
                return;

            //String loadoutUrl = "/bf4/loadout/get/[personaName]/[personaId]/[platformInt]/";

            model.items = allItemsCollection;
            model.mySoldier = true;
            allItemsCollection.mySoldier = true;
            if (model.mySoldier) {
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
                                    if (req["t"].Equals("l") && req["l"].Equals("dt_dev_team") && model.licenses[req["l"]] == null) {
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

                // Set our model
                model.structure = structure;
            }
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
            var currentLoadout = get_loadout();
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
                        ((ArrayList)currentLoadout["kits"])[i] = new Array[((ArrayList)((Hashtable)((ArrayList)structure["kits"])[i])["slots"]).Count]; // Might need to minus 1 or 2
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

                //    // Now loop through the vehicles, generating any missing items
                //    var f = 0;
                //    var vehicleLen = structure.vehicles.length;
                //    while (f < vehicleLen)
                //    {
                //        var vehicle = structure.vehicles[f];
                //        var vehicleImage = items.get(vehicle.sid);
                //        data.vehicles[f] = [];
                //        if (typeof vehicleImage === 'undefined')
                //        {
                //            f++;
                //            continue;
                //        }
                //        data.vehicles[f].imageConfig = vehicleImage.get('imageConfig');
                //        data.vehicles[f].sid = vehicle.sid;
                //        data.vehicles[f].slots = [];
                //        if (typeof currentLoadout.vehicles[f] === 'undefined')
                //        {
                //            currentLoadout.vehicles[f] = new Array(structure.vehicles[f].slots.length);
                //        }
                //        var vslotLen = vehicle.slots.length;
                //        var g = 0;
                //        while (g < vslotLen)
                //        {
                //            var vslot = vehicle.slots[g];
                //            if (vslot.items.length)
                //            {
                //                var vguid = currentLoadout.vehicles[f][g] || 0;
                //                var vitem = model.getItem('vehicles', f, g, vguid);
                //                if (typeof vitem !== 'undefined')
                //                {
                //                    vitem = vitem.toJSON();
                //                    currentLoadout.vehicles[f][g] = vitem.guid;
                //                    data.vehicles[f].slots[g] = { sid: vslot.sid,item: vitem};
                //                }
                //            }
                //            g++;
                //        }
                //        f++;
                //    }
                //    data.vehicles = data.vehicles.filter(function(e){ return e});
                //    // Updates the current loadout in case we had to generate any items
                //    this.set({ loadout: currentLoadout});
                //    if (getPresets)
                //    {
                //        data.presets = this.get('presetLoadouts');

                //        // Find a preset that matches the currently selected items to see if it's "active"
                //        for(var type in data.presets)
                //        {
                //            var types = data.presets[type];
                //            for(var id in types)
                //            {
                //                var presets = types[id].presets;
                //                var current = currentLoadout[type][id].join('');
                //                for(var key in presets)
                //                {
                //                    var preset = presets[key].ids.join('');
                //                    if (preset === current)
                //                    {
                //                        types[id].active = key;
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }

            return (data);
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
            //var guids = Array.ConvertAll((string[])((ArrayList)((Hashtable)((ArrayList)((Hashtable)((ArrayList)structure[type])[id])["slots"])[slot])["items"]).ToArray(typeof(string)), Int64.Parse);

            if (item == null || !guids.Contains(guid))
            {
                guid = guids[0].ToString();
                item = items_get(items, guid);
            }
            return (item);
        }

        /// <summary>
        /// Get the accessories for a weapon, generating them if 0, undefined, or invalid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>{*}</returns>
        public static ArrayList getAccessories(String guid)
        {
            Hashtable loadout = (Hashtable)Loadout["currentLoadout"];
            var items = allItemsCollection;
            Hashtable structure = get_structure();
            ArrayList accessories = (ArrayList)((Hashtable)loadout["weapons"])[guid];

            if (!((Hashtable)structure["weapons"]).Contains(guid))
            {
                Console.WriteLine("{0} does not exist in structure.weapons", guid);
                return null;
            }

            var slots = (ArrayList)((Hashtable)((Hashtable)structure["weapons"])[guid])["slots"];
            if (accessories == null)
            {
                ((Hashtable)loadout["weapons"])[guid] = new ArrayList();
                accessories = new ArrayList();
            }

            ArrayList fixedAccessories = new ArrayList();
            var i = 0;
            var len = slots.Count;
            while (i < len)
            {
                Hashtable item = null;
                if (i < slots.Count)
                {
                    if (Convert.ToInt64(accessories[i]) > 0)
                    {
                        item = items_get(items, accessories[i].ToString());
                        if (item == null || !((ArrayList)((Hashtable)slots[i])["items"]).Contains(accessories[i].ToString()))
                        {
                            item = items_get(items, generateSlot("weapons", guid, i));
                            if (!items_get(items, item["guid"].ToString(), true))
                                item = null;
                        }
                    }
                    else
                    {
                        item = items_get(items, generateSlot("weapons", guid, i));
                        if (!items_get(items, item["guid"].ToString(), true))
                            item = null;
                    }
                } else {
                    item = null;
                }

                if (item != null)
                {
                    if (!item.ContainsKey("slotSid"))
                        item.Add("slotSid", ((Hashtable)slots[i])["sid"].ToString());
                    fixedAccessories.Add(item);
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
            var selectedSlot = ((ArrayList)((Hashtable)((Hashtable)structure[type])[guid.ToString()])["slots"])[slot];
            var item = "0";
            if (selectedSlot != null)
            {
                item = ((ArrayList)((Hashtable)((ArrayList)((Hashtable)((Hashtable)structure[type])[guid])["slots"])[slot])["items"])[0].ToString();
                ((ArrayList)((Hashtable)loadout[type])[guid])[slot] = item;
                Loadout["currentLoadout"] = loadout;
            }
            return item;
        }

        private static Boolean items_get(allItemsCollection_ items, string guid, Boolean ret = true)
        {
            if (items_get(items, guid) != null)
                return (true);
            else
                return (false);
        }
        private static Hashtable items_get(allItemsCollection_ items, string guid)
        {
            foreach (Hashtable hTable in items.models)
            {
                if (hTable["guid"].Equals(guid.ToString()))
                {
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
            if(Loadout["currentLoadout"] == null)
                throw new Exception("Loadout doesn't contain currentLoadout");
            return ((Hashtable)Loadout["currentLoadout"]);
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
        /// Loads the en_US.js
        /// </summary>
        public static void Initialize_Translations()
        {
            if (Translations == null)
            {
                try
                {
                    // If there is no cached version, download
                    if (!File.Exists("en_US.js"))
                    {
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
                        if (line.StartsWith("t"))
                        {
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

        public sealed class HashtableProcessor
        {
            private HashtableProcessor() { }

            public static Hashtable Add(Hashtable first, Hashtable second)
            {
                Hashtable table = new Hashtable();

                foreach (DictionaryEntry e in first)
                {
                    table.Add(e.Key, e.Value);
                }

                foreach (DictionaryEntry e in second)
                {
                    if (!table.ContainsKey(e.Key))
                        table.Add(e.Key, e.Value);
                }

                return table;
            }
        }
    }
}
