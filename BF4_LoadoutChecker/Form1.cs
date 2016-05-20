using Newtonsoft.Json;
using PRoCon.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace BF4_LoadoutChecker
{
    public enum unlockImageConfig
    {
        smallinv,
        medium,
        squarelarge,
        mobile,
        small,
        xsmall,
        mediumns,
        smallns
    }
    public enum imageConfig
    {
        smallinv,
        medium,
        mobile,
        large,
        small,
        xsmall,
        mediumns,
        xsmallinv,
        smallns
    }

    public enum childLocation
    {
        LEFT,
        RIGHT,
        BOTTOM,
        TOP
    }

    public partial class bf4_loadoutchecker : Form
    {
        private Hashtable currentOverview = null;
        private Details dForm;
        private Information iForm;
        private Form closer;

        public bf4_loadoutchecker()
        {
            InitializeComponent();

            // Add the dark background
            pBox_kit_assault.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            lbl_0_ClassName.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            pBox_kit_engineer.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            lbl_1_ClassName.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            pBox_kit_support.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            lbl_2_ClassName.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            pBox_kit_recon.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            lbl_3_ClassName.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;

            Loadout_Backbone_Helper.Initialize_Translations();
        }

        private void bf4_loadoutchecker_Load(object sender, EventArgs e)
        {
            CoS_dForm();
            CoS_iForm();
        }

        /// <summary>
        /// Sets the location for Details and Information forms
        /// </summary>
        private void SetChildLocations()
        {
            Boolean dForm_active = false;
            Boolean iForm_active = false;
            if (dForm != null) dForm_active = !dForm_active;
            if (iForm != null) iForm_active = !iForm_active;

            if (dForm_active && iForm_active && dForm.myLocation == iForm.myLocation && dForm.Docked && iForm.Docked)
            {
                if (dForm == closer)
                {
                    if (dForm.myLocation == childLocation.RIGHT && iForm.myLocation == childLocation.RIGHT)
                    {
                        if (dForm.Docked)
                            dForm.Location = new Point((this.Location.X + this.Size.Width) - 10, this.Location.Y);
                        if (iForm.Docked)
                            iForm.Location = new Point((dForm.Location.X + dForm.Size.Width) - 15, dForm.Location.Y);
                    }
                    if (dForm.myLocation == childLocation.LEFT && iForm.myLocation == childLocation.LEFT)
                    {
                        if (dForm.Docked)
                            dForm.Location = new Point((this.Location.X - dForm.Size.Width) + 10, this.Location.Y);
                        if (iForm.Docked)
                            iForm.Location = new Point((dForm.Location.X - iForm.Size.Width) + 15, dForm.Location.Y);
                    }
                    if (dForm.myLocation == childLocation.BOTTOM && iForm.myLocation == childLocation.BOTTOM)
                    {
                        if (dForm.Docked)
                            dForm.Location = new Point((this.Location.X + ((this.Size.Width / 2) - dForm.Size.Width)) + 7, (this.Location.Y + this.Size.Height) - 3);
                        if (iForm.Docked)
                            iForm.Location = new Point((this.Location.X + (this.Size.Width / 2)) - 7, (this.Location.Y + this.Size.Height) - 3);
                    }
                    if (dForm.myLocation == childLocation.TOP && iForm.myLocation == childLocation.TOP)
                    {
                        if (dForm.Docked)
                            dForm.Location = new Point((this.Location.X + ((this.Size.Width / 2) - dForm.Size.Width)) + 7, (this.Location.Y - dForm.Size.Height) + 8);
                        if (iForm.Docked)
                            iForm.Location = new Point((this.Location.X + (this.Size.Width / 2)) - 7, (this.Location.Y - iForm.Size.Height) + 8);
                    }
                }
                else
                {
                    if (iForm.myLocation == childLocation.RIGHT && dForm.myLocation == childLocation.RIGHT)
                    {
                        if (iForm.Docked)
                            iForm.Location = new Point((this.Location.X + this.Size.Width) - 10, this.Location.Y);
                        if (dForm.Docked)
                            dForm.Location = new Point((iForm.Location.X + iForm.Size.Width) - 15, iForm.Location.Y);
                    }
                    if (iForm.myLocation == childLocation.LEFT && dForm.myLocation == childLocation.LEFT)
                    {
                        if (iForm.Docked)
                            iForm.Location = new Point((this.Location.X - iForm.Size.Width) + 10, this.Location.Y);
                        if (dForm.Docked)
                            dForm.Location = new Point((iForm.Location.X - dForm.Size.Width) + 15, iForm.Location.Y);
                    }
                    if (iForm.myLocation == childLocation.BOTTOM && dForm.myLocation == childLocation.BOTTOM)
                    {
                        if (iForm.Docked)
                            iForm.Location = new Point((this.Location.X + ((this.Size.Width / 2) - iForm.Size.Width)) + 7, (this.Location.Y + this.Size.Height) - 3);
                        if (dForm.Docked)
                            dForm.Location = new Point((this.Location.X + (this.Size.Width / 2)) - 7, (this.Location.Y + this.Size.Height) - 3);
                    }
                    if (iForm.myLocation == childLocation.TOP && dForm.myLocation == childLocation.TOP)
                    {
                        if (iForm.Docked)
                            iForm.Location = new Point((this.Location.X + ((this.Size.Width / 2) - iForm.Size.Width)) + 7, (this.Location.Y - iForm.Size.Height) + 8);
                        if (dForm.Docked)
                            dForm.Location = new Point((this.Location.X + (this.Size.Width / 2)) - 7, (this.Location.Y - dForm.Size.Height) + 8);
                    }
                }
                return;
            }

            // If not, set location for either one
            if (dForm_active)
                if (dForm.Docked)
                {
                    if (dForm.myLocation == childLocation.RIGHT)
                        dForm.Location = new Point((this.Location.X + this.Size.Width) - 10, this.Location.Y);
                    if (dForm.myLocation == childLocation.LEFT)
                        dForm.Location = new Point((this.Location.X - dForm.Size.Width) + 10, this.Location.Y);
                    if (dForm.myLocation == childLocation.BOTTOM)
                        dForm.Location = new Point((this.Location.X + ((this.Size.Width / 2) - (dForm.Size.Width / 2))), (this.Location.Y + this.Size.Height) - 3);
                    if (dForm.myLocation == childLocation.TOP)
                        dForm.Location = new Point((this.Location.X + ((this.Size.Width / 2) - (dForm.Size.Width / 2))), (this.Location.Y - dForm.Size.Height) + 8);
                }
                    
            if (iForm_active)
                if (iForm.Docked)
                {
                    if (iForm.myLocation == childLocation.RIGHT)
                        iForm.Location = new Point((this.Location.X + this.Size.Width) - 10, this.Location.Y);
                    if (iForm.myLocation == childLocation.LEFT)
                        iForm.Location = new Point((this.Location.X - iForm.Size.Width) + 10, this.Location.Y);
                    if (iForm.myLocation == childLocation.BOTTOM)
                        iForm.Location = new Point((this.Location.X + ((this.Size.Width / 2) - (iForm.Size.Width / 2))), (this.Location.Y + this.Size.Height) - 3);
                    if (iForm.myLocation == childLocation.TOP)
                        iForm.Location = new Point((this.Location.X + ((this.Size.Width / 2) - (iForm.Size.Width / 2))), (this.Location.Y - iForm.Size.Height) + 8);
                }
        }

        private void bf4_loadoutchecker_LocationChanged(object sender, EventArgs e)
        {
            SetChildLocations();
        }

        private void btn_fetch_immediate_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            object[] result = Loadout_Backbone_Helper.initialize(tBox_soldierName.Text.Trim());
            if (((KeyValuePair<String, String>)result[0]).Key == "type" && ((KeyValuePair<String, String>)result[0]).Value == "success")
                currentOverview = Loadout_Backbone_Helper.getCurrentOverview(true);
            else
            {
                toolStripStatusLabel1.Text = String.Format("{0} : {1}",
                    ((KeyValuePair<String, String>)result[1]).Key.ToString(),
                    ((KeyValuePair<String, String>)result[1]).Value.ToString());

                Debug.WriteLine(String.Format("{0} : {1}", 
                    ((KeyValuePair<String, String>)result[0]).Key.ToString(), 
                    ((KeyValuePair<String, String>)result[0]).Value.ToString()));
                Debug.WriteLine(String.Format("{0} : {1}",
                    ((KeyValuePair<String, String>)result[1]).Key.ToString(),
                    ((KeyValuePair<String, String>)result[1]).Value.ToString()));
                return;
            }

            if(currentOverview == null)
            {
                Debug.WriteLine(String.Format("currentOverview is null"));
                toolStripStatusLabel1.Text = String.Format("currentOverview is null");
                return;
            }
#if DEBUG
            //Loadout_Backbone_Helper.generateSlot("kits", "0", 0);
            //Loadout_Backbone_Helper.getKitUnlocks(-1, true);
            //String[] results = Loadout_Backbone_Helper.getUnlockedItems(0, 0);
            File.WriteAllText("currentOverview.json", JsonConvert.SerializeObject(currentOverview));
#endif
            visualize();
            sw.Stop();
            TimeSpan timeSpan = sw.Elapsed;
            Debug.WriteLine(String.Format("Updated in: {0}m {1}s {2}ms", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds));
            toolStripStatusLabel1.Text = String.Format("Updated in: {0}m {1}s {2}ms", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);

            Hashtable tmp = Loadout_Backbone_Helper.items_get(Loadout_Backbone_Helper.allItemsCollection, "2670747868");
        }

        private void getunlockImageConfig(String weapon_key, PictureBox pBox, unlockImageConfig image_type = unlockImageConfig.mediumns)
        {
        }
        private void getimageConfig(Hashtable imageConfig, PictureBox pBox, imageConfig image_type = imageConfig.xsmall, bool async = false)
        {
            Hashtable version = (Hashtable)((Hashtable)imageConfig["versions"])[image_type.ToString()];
            if (version != null)
            {
                string image_url = version["path"].ToString();
                if (!String.IsNullOrEmpty(image_url))
                    if (!async)
                        pBox.Load("http://eaassets-a.akamaihd.net/bl-cdn/cdnprefix/9b764b983931ba3a6435e21c2fd56eddc76b813a/public/profile/" + image_url);
                    else
                        pBox.LoadAsync("http://eaassets-a.akamaihd.net/bl-cdn/cdnprefix/9b764b983931ba3a6435e21c2fd56eddc76b813a/public/profile/" + image_url);
                else
                    pBox.Image = null;
            }
            else
                if (!async)
                pBox.Load("http://eaassets-a.akamaihd.net/bl-cdn/cdnprefix/9b764b983931ba3a6435e21c2fd56eddc76b813a/public/profile/warsaw/gamedata/weaponaccessory/" + image_type.ToString() + "/_noselection_lineart.png");
            else
                pBox.LoadAsync("http://eaassets-a.akamaihd.net/bl-cdn/cdnprefix/9b764b983931ba3a6435e21c2fd56eddc76b813a/public/profile/warsaw/gamedata/weaponaccessory/" + image_type.ToString() + "/_noselection_lineart.png");
        }

        private void visualize()
        {
            var kits = ((ArrayList)currentOverview["kits"]);
            for (int i = 0; i < kits.Count; i++)
            {
                var slots = ((ArrayList)((Hashtable)kits[i])["slots"]);
                for (int j = 0; j < slots.Count; j++)
                {
                    // Basic Information
                    PictureBox pBox = getPictureBox(i, j);
                    Label lbl = getLabel(i, j);
                    var item = (Hashtable)((Hashtable)slots[j])["item"];
                    getimageConfig((Hashtable)item["imageConfig"], pBox, (j == 0) ? imageConfig.mediumns : imageConfig.smallns, (j == 0) ? false : true);
                    lbl.Text = Loadout_Backbone_Helper.getSID(item["name"].ToString());

                    // Accessories for primary weapon
                    if (j == 0)
                    {
                        var _slots = (ArrayList)item["slots"];
                        for (int k = 0; k < 3; k++) // Only OPTIC, ACCESSORY and BARREL
                        {
                            pBox = getPictureBox(i, j, k);
                            getimageConfig((Hashtable)((Hashtable)_slots[k])["imageConfig"], pBox, imageConfig.smallns);
                        }
                    }
                }
            }

            // Active Kit
            int activeKit = Convert.ToInt32(currentOverview["selectedKit"]);
            for (int i = 0; i < 4; i++)
            {
                Label lbl = getLabel(i, -1);
                Panel panel = ((Panel)this.Controls.Find(get_getClassString(i), true)[0]);
                if (i == activeKit){
                    lbl.Text = "CURRENTLY ACTIVE KIT";
                    lbl.BackColor = SystemColors.ControlLightLight;
                    lbl.ForeColor = SystemColors.ControlText;
                    panel.BorderStyle = BorderStyle.Fixed3D;
                } else{
                    lbl.Text = "SET AS ACTIVE KIT";
                    lbl.BackColor = Color.Transparent;
                    lbl.ForeColor = SystemColors.ControlLightLight;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        /// <summary>
        /// Get the corresponding PictureBox in the Form
        /// </summary>
        /// <param name="kit"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        private PictureBox getPictureBox(int kit, int slot, int accessory = -1)
        {
            return ((PictureBox)this.Controls.Find(String.Format("pBox_{0}_{1}{2}", kit, slot, (accessory == -1) ? String.Empty : "_" + accessory), true)[0]);
        }
        /// <summary>
        /// Get the corresponding Label in the Form
        /// </summary>
        /// <param name="kit"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        private Label getLabel(int kit, int slot)
        {
            return ((Label)this.Controls.Find(String.Format("lbl_{0}_{1}", kit, (slot == -1) ? "active" : slot.ToString()), true)[0]);
        }
        /// <summary>
        /// Get the corresponding class name to a int 0 - 3
        /// </summary>
        /// <param name="kit"></param>
        /// <returns>{*}</returns>
        private static String get_getClassString(int kit)
        {
            String className = "WARSAW_ID_M_ASSAULT";
            switch (kit)
            {
                // Assault
                case 0:
                    className = "WARSAW_ID_M_ASSAULT";
                    break;
                // Engineer
                case 1:
                    className = "WARSAW_ID_M_ENGINEER";
                    break;
                // Support
                case 2:
                    className = "WARSAW_ID_M_SUPPORT";
                    break;
                // Recon
                case 3:
                    className = "WARSAW_ID_M_RECON";
                    break;
                // Default goes to Assault
                default:
                    className = "WARSAW_ID_M_ASSAULT";
                    break;
            }
            return className;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private void pBox_image_resizeOn_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            PictureBox pBox = (PictureBox)sender;
            Bitmap default_image = new Bitmap(pBox.Image);
            pBox.Image = ResizeImage(default_image, 64, 16);
        }

        private void pBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pBox = (PictureBox)sender;
            pBox.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
        }
        private void pBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pBox = (PictureBox)sender;
            pBox.BackgroundImage = null;
        }
        private void pBox_customize_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pBox = (PictureBox)sender;
            pBox.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            pBox.Image = BF4_LoadoutChecker.Properties.Resources.loadout_cog_white; 
        }
        private void pBox_customize_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pBox = (PictureBox)sender;
            pBox.BackgroundImage = null;
            //pBox.Image = BF4_LoadoutChecker.Properties.Resources.loadout_cog_black;
        }
        private void label_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
        }
        private void label_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BackgroundImage = null;
        }

        private void bf4_loadoutchecker_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
        private void bf4_loadoutchecker_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void customization_Click(object sender, EventArgs e)
        {
            var kit = Convert.ToInt32(((PictureBox)sender).Name.Substring(5, 1));
            var slot = Convert.ToInt32(((PictureBox)sender).Name.Substring(7, 1));

            // Show the Details form
            CoS_dForm();

            // Clear contents
            dForm.ClearfLPBase();

            if (currentOverview == null)
                return;

            // Populate data
            var kits = ((ArrayList)currentOverview["kits"]);
            var slots = ((ArrayList)((Hashtable)kits[kit])["slots"]);

            // Basic Information
            var sid = ((Hashtable)kits[kit])["sid"].ToString();
            var item = (Hashtable)((Hashtable)slots[slot])["item"];

            // Accessories
            var _slots = (ArrayList)item["slots"];
            for (int k = 0; k < (_slots.Count - 1); k++) // No PAINT yet
            {
                var _slot = ((Hashtable)_slots[k]);
                dForm.AddDescription(_slot);
            }

            dForm.BringToFront();

            #region OLD CODE
            //var kit = Convert.ToInt32(((PictureBox)sender).Name.Substring(5, 1));
            //var slot = Convert.ToInt32(((PictureBox)sender).Name.Substring(7, 1));
            //primary_indepth_info.Visible = true;
            //visualize_panel.Visible = false;

            //// Populate data
            //var kits = ((ArrayList)currentOverview["kits"]);
            //var slots = ((ArrayList)((Hashtable)kits[kit])["slots"]);

            //// Basic Information
            //var sid = ((Hashtable)kits[kit])["sid"].ToString();
            //var item = (Hashtable)((Hashtable)slots[slot])["item"];
            //btn_back.Text = String.Format("< {0}", Loadout_Backbone_Helper.getSID(sid).ToUpper());

            //// Accessories
            //var _slots = (ArrayList)item["slots"];
            //Boolean AMMO = false;
            //Boolean AUXILIARY = false;
            //for (int k = 0; k < (_slots.Count - 1); k++) // No PAINT yet
            //{
            //    var _slot = ((Hashtable)_slots[k]);
            //    var slotSid = _slot["slotSid"].ToString();
            //    if (slotSid.Equals("WARSAW_ID_P_CAT_AMMO"))
            //        AMMO = true;
            //    if (slotSid.Equals("WARSAW_ID_P_CAT_AUXILIARY"))
            //        AUXILIARY = true;

            //    PictureBox pBox = getPictureBox2(slotSid);
            //    Label lbl_1 = getLabel2("name", slotSid);
            //    Label lbl_2 = getLabel2("desc", slotSid);
            //    getimageConfig((Hashtable)_slot["imageConfig"], pBox, imageConfig.smallns);
            //    lbl_1.Text = Loadout_Backbone_Helper.getSID(_slot["name"].ToString());
            //    lbl_2.Text = Loadout_Backbone_Helper.getSID(_slot["desc"].ToString());

            //    pBox.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            //    lbl_1.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            //    lbl_2.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            //}

            //// Hide unnecessary
            //FlowLayoutPanel flp = ((FlowLayoutPanel)this.Controls.Find("WARSAW_ID_P_CAT_AMMO", true)[0]);
            //FlowLayoutPanel flp2 = ((FlowLayoutPanel)this.Controls.Find("WARSAW_ID_P_CAT_UNDERBARREL", true)[0]);
            //FlowLayoutPanel flp3 = ((FlowLayoutPanel)this.Controls.Find("WARSAW_ID_P_CAT_AUXILIARY", true)[0]);
            //flp.Visible = AMMO && !AUXILIARY;
            //flp2.Visible = !AMMO && !AUXILIARY;
            //flp3.Visible = AUXILIARY && !AMMO;
            //if (_slots.Count < 5)
            //    flp2.Visible = false;

            //// Visual effects
            //lbl_OPTIC.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
            //lbl_ACCESSORY.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
            //lbl_BARREL.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
            //lbl_UNDERBARREL.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
            //lbl_AUXILIARY.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
            //lbl_AMMO.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity; 
            #endregion
        }

        private void information_Click(object sender, EventArgs e)
        {
            String sender_Name = ((Control)sender).Name;
            sender_Name = sender_Name.Replace("lbl_", String.Empty).Replace("pBox_", String.Empty); // Trim the start
            
            // Parse the values
            List<int> values = new List<int>();
            for (int i = 0; i < sender_Name.Length; i+=2) {
                values.Add((int)Char.GetNumericValue(sender_Name[i]));
            }

            if (values.Count < 2)
                return;

            // Populate data
            var kits = ((ArrayList)currentOverview["kits"]);
            var slots = ((ArrayList)((Hashtable)kits[values[0]])["slots"]);

            // Basic Information
            var item = (Hashtable)((Hashtable)slots[values[1]])["item"];
            if (values.Count == 2)
            {
                CoS_iForm();
                iForm.ClearfLPBase();
                iForm.AddInfo((Hashtable)slots[values[1]]);
            }
                
            if (values.Count > 2) // Falls here if it's a attachment
                item = ((Hashtable)((ArrayList)item["slots"])[values[2]]);

            // Print out the result
            toolStripStatusLabel1.Text = String.Format("GUID for {0} is {1}", 
                Loadout_Backbone_Helper.getSID(item["name"].ToString()),
                item["guid"].ToString()
                );
        }

        /// <summary>
        /// Get the corresponding PictureBox in the Form
        /// </summary>
        /// <param name="accessory"></param>
        /// <returns></returns>
        private PictureBox getPictureBox2(String accessory)
        {
            return ((PictureBox)this.Controls.Find(String.Format("pBox_{0}", accessory), true)[0]);
        }
        /// <summary>
        /// Get the corresponding Label in the Form
        /// </summary>
        /// <param name="type"></param>
        /// <param name="accessory"></param>
        /// <returns></returns>
        private Label getLabel2(String type, String accessory)
        {
            return ((Label)this.Controls.Find(String.Format("{0}_{1}", type, accessory), true)[0]);
        }

        /// <summary>
        /// Reduce Flickering
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        /// <summary>
        /// Get back to loadout window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_back_Click(object sender, EventArgs e)
        {
            primary_indepth_info.Visible = false;
            visualize_panel.Visible = true;
        }

        /// <summary>
        /// Create a new Details form
        /// </summary>
        private void CoS_dForm()
        {
            // if the form is not closed, show it
            if (dForm == null || dForm.IsDisposed)
            {
                dForm = new Details();

                // Set closer status
                if (iForm == null)
                    closer = dForm;

                // attach the handler
                dForm.FormClosed += CFC_dForm;
            }

            // show it
            dForm.Show();
            SetChildLocations();
            dForm.Owner = this;
        }

        /// <summary>
        /// when the Details form closes, detach the handler and clear the field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void CFC_dForm(object sender, FormClosedEventArgs args)
        {
            // detach the handler
            dForm.FormClosed -= CFC_dForm;

            // let GC collect it (and this way we can tell if it's closed)
            dForm = null;

            if (iForm != null)
                closer = iForm;

            SetChildLocations();
        }

        /// <summary>
        /// Create a new Information form
        /// </summary>
        private void CoS_iForm()
        {
            // if the form is not closed, show it
            if (iForm == null || iForm.IsDisposed)
            {
                iForm = new Information();

                // Set closer status
                if (dForm == null)
                    closer = iForm;

                // attach the handler
                iForm.FormClosed += CFC_iForm;
            }

            // show it
            iForm.Show();
            SetChildLocations();
            iForm.Owner = this;
        }

        /// <summary>
        /// when the Information form closes, detach the handler and clear the field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void CFC_iForm(object sender, FormClosedEventArgs args)
        {
            // detach the handler
            iForm.FormClosed -= CFC_iForm;

            // let GC collect it (and this way we can tell if it's closed)
            iForm = null;

            if (dForm != null)
                closer = dForm;

            SetChildLocations();
        }
    }
}