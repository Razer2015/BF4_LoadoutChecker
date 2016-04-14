using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BF4_LoadoutChecker
{
    public partial class Details : Form
    {
        public childLocation myLocation;
        public Boolean Docked;
        private int box_count;

        public Details()
        {
            InitializeComponent();

            myLocation = childLocation.RIGHT;
            Docked = true;
        }

        public void AddDescription(Hashtable slot)
        {
            // Base FlowLayoutPanel
            FlowLayoutPanel FLP = new FlowLayoutPanel();
            FLP.Width = 236;
            FLP.Height = 133;
            FLP.Margin = new Padding(0, 0, 0, 0);

            // Label which holds the category
            Label lbl_category = new Label();
            lbl_category.Width = 235;
            lbl_category.Height = 23;
            lbl_category.Location = new Point(0,0);
            lbl_category.TextAlign = ContentAlignment.MiddleLeft;
            lbl_category.Margin = new Padding(0, 0, 0, 1);
            lbl_category.Text = String.Format(" {0}", Loadout_Backbone_Helper.getSID(slot["slotSid"].ToString()));
            lbl_category.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._99_opacity;

            // PictureBox which holds the image
            PictureBox pBox = new PictureBox();
            pBox.Width = 64;
            pBox.Height = 28;
            pBox.Location = new Point(0, 1);
            pBox.Margin = new Padding(0, 0, 0, 0);
            pBox.SizeMode = PictureBoxSizeMode.CenterImage;
            pBox.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;
            getimageConfig((Hashtable)slot["imageConfig"], pBox, imageConfig.smallns);

            // Label which holds the name
            Label lbl_name = new Label();
            lbl_name.Width = 171;
            lbl_name.Height = 28;
            lbl_name.Location = new Point(64, 1);
            lbl_name.TextAlign = ContentAlignment.MiddleLeft;
            lbl_name.Margin = new Padding(0, 0, 0, 0);
            lbl_name.Text = String.Format("{0}", Loadout_Backbone_Helper.getSID(slot["name"].ToString()));
            lbl_name.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;

            // Label which holds the description
            Label lbl_desc = new Label();
            lbl_desc.Width = 235;
            lbl_desc.Height = 76;
            lbl_desc.Location = new Point(0, 54);
            lbl_desc.TextAlign = ContentAlignment.MiddleCenter;
            lbl_desc.Margin = new Padding(0, 0, 0, 1);
            lbl_desc.Text = String.Format("{0}", Loadout_Backbone_Helper.getSID(slot["desc"].ToString()));
            lbl_desc.BackgroundImage = BF4_LoadoutChecker.Properties.Resources._50_opacity;

            FLP.Controls.Add(lbl_category);
            FLP.Controls.Add(pBox);
            FLP.Controls.Add(lbl_name);
            FLP.Controls.Add(lbl_desc);
            fLP_base.Controls.Add(FLP);

            // Resize the form Height
            box_count++;
            this.Size = new System.Drawing.Size(Width, (FLP.Height * box_count) + 63);
        }

        public void ClearfLPBase()
        {
            fLP_base.Controls.Clear();
            box_count = 0;
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

        /// <summary>
        /// Change the docking location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String[] enums = Enum.GetNames(typeof(childLocation));

            ToolStripMenuItem TSMI = (ToolStripMenuItem)sender;
            if (TSMI.Checked)
                return;

            foreach (ToolStripMenuItem C in dockingLocationToolStripMenuItem.DropDownItems)
            {
                if (C.Checked)
                    C.Checked = !C.Checked;
            }

            TSMI.Checked = !TSMI.Checked;
            myLocation = (childLocation)Array.IndexOf(enums, TSMI.Text);
        }

        /// <summary>
        /// Change the docking status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void keepDockingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            keepDockingToolStripMenuItem.Checked = !keepDockingToolStripMenuItem.Checked;
            Docked = keepDockingToolStripMenuItem.Checked;
        }

        /// <summary>
        /// Report resizing information if debug build
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Details_Resize(object sender, EventArgs e)
        {
#if DEBUG
            Debug.WriteLine(String.Format("Width: {0}", this.Size.Width));
            Debug.WriteLine(String.Format("Height: {0}", this.Size.Height));
#endif
        }
    }
}
