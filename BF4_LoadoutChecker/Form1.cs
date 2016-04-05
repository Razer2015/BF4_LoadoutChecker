﻿using Newtonsoft.Json;
using PRoCon.Core;
using System;
using System.Collections;
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
                    PictureBox pBox = getPictureBox("weapons", i, j);
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
                            pBox = getPictureBox("accessory", i, k);
                            getimageConfig((Hashtable)((Hashtable)_slots[k])["imageConfig"], pBox, imageConfig.smallns);
                        }
                    }
                }
            }

            // Active Kit
            int activeKit = Convert.ToInt32(currentOverview["selectedKit"]);
            for (int i = 0; i < 4; i++)
            {
                Label lbl = getLabel(i, 7);
                if (i == activeKit)
                    lbl.Text = "CURRENTLY ACTIVE KIT";
                else
                    lbl.Text = "SET AS ACTIVE KIT";
            }
        }

        /// <summary>
        /// Get the corresponding PictureBox in the Form
        /// </summary>
        /// <param name="kit"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        private PictureBox getPictureBox(String type, int kit, int slot)
        {
            String[] kit_chars = new String[] { "a", "e", "s", "r" };
            String[] endings = new String[] {
                "_pwep_lineart",
                "_swep_lineart",
                "_gadget1_lineart",
                "_gadget2_lineart",
                "_grenade_lineart",
                "_knife_lineart",
                "_specialization_lineart"
            };

            if (type == "accessory")
                endings = new String[] {
                "_pwep_a1_lineart",
                "_pwep_a2_lineart",
                "_pwep_a3_lineart"
            };

            if (kit > (kit_chars.Length - 1) || slot > (endings.Length - 1))
                throw new Exception(String.Format("Error: getPictureBox| kit = {0}, slot = {1}", kit, slot));

            return ((PictureBox)this.Controls.Find("pBox_" + kit_chars[kit] + endings[slot], true)[0]);
        }

        /// <summary>
        /// Get the corresponding Label in the Form
        /// </summary>
        /// <param name="kit"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        private Label getLabel(int kit, int slot)
        {
            String[] kit_chars = new String[] { "a", "e", "s", "r" };
            String[] endings = new String[] {
                "_pwep_name",
                "_swep_name",
                "_gadget1_name",
                "_gadget2_name",
                "_grenade_name",
                "_knife_name",
                "_specialization_name",
                "_activity"
            };

            if (kit > (kit_chars.Length - 1) || slot > (endings.Length - 1))
                throw new Exception(String.Format("Error: Label| kit = {0}, slot = {1}", kit, slot));

            return ((Label)this.Controls.Find("lbl_" + kit_chars[kit] + endings[slot], true)[0]);
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
    }
}