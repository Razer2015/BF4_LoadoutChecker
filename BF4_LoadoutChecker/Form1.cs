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

    public partial class bf4_loadoutchecker : Form
    {
        private Hashtable currentOverview = null;

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

        private void btn_fetch_immediate_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Loadout_Backbone_Helper.initialize(tBox_soldierName.Text.Trim());
            currentOverview = Loadout_Backbone_Helper.getCurrentOverview(true);
#if DEBUG
            File.WriteAllText("currentOverview.json", JsonConvert.SerializeObject(currentOverview));
#endif
            visualize();
            sw.Stop();
            TimeSpan timeSpan = sw.Elapsed;
            Debug.WriteLine(String.Format("Updated in: {0}m {1}s {2}ms", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds));
            toolStripStatusLabel1.Text = String.Format("Updated in: {0}m {1}s {2}ms", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
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
            primary_indepth_info.Visible = true;
            visualize_panel.Visible = false;

            // Populate data
            var kits = ((ArrayList)currentOverview["kits"]);
            var slots = ((ArrayList)((Hashtable)kits[kit])["slots"]);

            // Basic Information
            var sid = ((Hashtable)kits[kit])["sid"].ToString();
            var item = (Hashtable)((Hashtable)slots[slot])["item"];
            btn_back.Text = String.Format("< {0}", Loadout_Backbone_Helper.getSID(sid).ToUpper());

            // Accessories
            var _slots = (ArrayList)item["slots"];
            Boolean AMMO = false;
            Boolean AUXILIARY = false;
            for (int k = 0; k < (_slots.Count - 1); k++) // No PAINT yet
            {
                var _slot = ((Hashtable)_slots[k]);
                var slotSid = _slot["slotSid"].ToString();
                if (slotSid.Equals("WARSAW_ID_P_CAT_AMMO"))
                    AMMO = true;
                if (slotSid.Equals("WARSAW_ID_P_CAT_AUXILIARY"))
                    AUXILIARY = true;

                PictureBox pBox = getPictureBox2(slotSid);
                Label lbl_1 = getLabel2("name", slotSid);
                Label lbl_2 = getLabel2("desc", slotSid);
                getimageConfig((Hashtable)_slot["imageConfig"], pBox, imageConfig.smallns);
                lbl_1.Text = Loadout_Backbone_Helper.getSID(_slot["name"].ToString());
                lbl_2.Text = Loadout_Backbone_Helper.getSID(_slot["desc"].ToString());

                pBox.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
                lbl_1.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
                lbl_2.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            }

            // Hide unnecessary
            FlowLayoutPanel flp = ((FlowLayoutPanel)this.Controls.Find("WARSAW_ID_P_CAT_AMMO", true)[0]);
            FlowLayoutPanel flp2 = ((FlowLayoutPanel)this.Controls.Find("WARSAW_ID_P_CAT_UNDERBARREL", true)[0]);
            FlowLayoutPanel flp3 = ((FlowLayoutPanel)this.Controls.Find("WARSAW_ID_P_CAT_AUXILIARY", true)[0]);
            flp.Visible = AMMO && !AUXILIARY;
            flp2.Visible = !AMMO && !AUXILIARY;
            flp3.Visible = AUXILIARY && !AMMO;
            if (_slots.Count < 5)
                flp2.Visible = false;

            // Visual effects
            lbl_OPTIC.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
            lbl_ACCESSORY.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
            lbl_BARREL.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
            lbl_UNDERBARREL.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
            lbl_AUXILIARY.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
            lbl_AMMO.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;
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
    }
}