using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BF4_LoadoutChecker
{
    public partial class Information : Form
    {
        float opacity_disabled = 0.6F;
        public childLocation myLocation;
        public Boolean Docked;
        private int Height = -1;
        private int Width = -1;

        public Information()
        {
            InitializeComponent();
            myLocation = childLocation.LEFT;
            Docked = true;
        }

        public void AddInfo(Hashtable slot)
        {
            if (Width == -1) Width = this.Size.Width;
            if (Height == -1)  Height = this.Size.Height;
            int FLP_baseHeight = 123;
            #region Initialize
            LabelGradient.LabelGradient m_sid;
            LabelGradient.LabelGradient m_name;
            System.Windows.Forms.PictureBox m_imageConfig;
            System.Windows.Forms.PictureBox m_WARSAW_ID_P_CAT_OPTIC;
            System.Windows.Forms.PictureBox m_WARSAW_ID_P_CAT_ACCESSORY;
            System.Windows.Forms.PictureBox m_WARSAW_ID_P_CAT_BARREL;

            m_sid = new LabelGradient.LabelGradient();
            m_name = new LabelGradient.LabelGradient();
            m_imageConfig = new System.Windows.Forms.PictureBox();
            m_WARSAW_ID_P_CAT_OPTIC = new System.Windows.Forms.PictureBox();
            m_WARSAW_ID_P_CAT_ACCESSORY = new System.Windows.Forms.PictureBox();
            m_WARSAW_ID_P_CAT_BARREL = new System.Windows.Forms.PictureBox();
            #endregion

            // Parse Data
            Hashtable item = (Hashtable)slot["item"];

            // FlowLayoutPanel info
            FlowLayoutPanel FLP = new FlowLayoutPanel();
            FLP.BackColor = System.Drawing.Color.Transparent;
            FLP.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
            FLP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            FLP.Controls.Add(m_sid);
            FLP.Controls.Add(m_name);
            FLP.Controls.Add(m_imageConfig);

            FlowLayoutPanel fLP_slotInfo = AddStatsInfo(item);
            if (fLP_slotInfo != null)
            {
                FLP.Controls.Add(fLP_slotInfo);
                // Add 104 Height to FLP
                FLP_baseHeight += 104;
            } 

            FlowLayoutPanel fLP_extend = AddExtendedStats(item);
            if (fLP_extend != null)
            {
                FLP.Controls.Add(fLP_extend);
                // Add 84 Height to FLP
                FLP_baseHeight += 84;
            }

            if(item.ContainsKey("slots") && ((ArrayList)item["slots"]).Count > 2)
            {
                FLP.Controls.Add(m_WARSAW_ID_P_CAT_OPTIC);
                FLP.Controls.Add(m_WARSAW_ID_P_CAT_ACCESSORY);
                FLP.Controls.Add(m_WARSAW_ID_P_CAT_BARREL);

                // Parsing
                ArrayList slots = (ArrayList)item["slots"];

                // 
                // WARSAW_ID_P_CAT_OPTIC
                // 
                m_WARSAW_ID_P_CAT_OPTIC.Location = new System.Drawing.Point(1, 307);
                m_WARSAW_ID_P_CAT_OPTIC.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
                m_WARSAW_ID_P_CAT_OPTIC.Name = "WARSAW_ID_P_CAT_OPTIC";
                m_WARSAW_ID_P_CAT_OPTIC.Size = new System.Drawing.Size(78, 28);
                m_WARSAW_ID_P_CAT_OPTIC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                m_WARSAW_ID_P_CAT_OPTIC.TabStop = false;
                m_WARSAW_ID_P_CAT_OPTIC.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
                getimageConfig((Hashtable)((Hashtable)slots[0])["imageConfig"], m_WARSAW_ID_P_CAT_OPTIC, imageConfig.smallns, true);
                // 
                // WARSAW_ID_P_CAT_ACCESSORY
                // 
                m_WARSAW_ID_P_CAT_ACCESSORY.Location = new System.Drawing.Point(79, 307);
                m_WARSAW_ID_P_CAT_ACCESSORY.Margin = new System.Windows.Forms.Padding(0);
                m_WARSAW_ID_P_CAT_ACCESSORY.Name = "WARSAW_ID_P_CAT_ACCESSORY";
                m_WARSAW_ID_P_CAT_ACCESSORY.Size = new System.Drawing.Size(78, 28);
                m_WARSAW_ID_P_CAT_ACCESSORY.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                m_WARSAW_ID_P_CAT_ACCESSORY.TabStop = false;
                m_WARSAW_ID_P_CAT_ACCESSORY.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
                getimageConfig((Hashtable)((Hashtable)slots[1])["imageConfig"], m_WARSAW_ID_P_CAT_ACCESSORY, imageConfig.smallns, true);
                // 
                // WARSAW_ID_P_CAT_BARREL
                // 
                m_WARSAW_ID_P_CAT_BARREL.Location = new System.Drawing.Point(157, 307);
                m_WARSAW_ID_P_CAT_BARREL.Margin = new System.Windows.Forms.Padding(0);
                m_WARSAW_ID_P_CAT_BARREL.Name = "WARSAW_ID_P_CAT_BARREL";
                m_WARSAW_ID_P_CAT_BARREL.Size = new System.Drawing.Size(78, 28);
                m_WARSAW_ID_P_CAT_BARREL.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                m_WARSAW_ID_P_CAT_BARREL.TabStop = false;
                m_WARSAW_ID_P_CAT_BARREL.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
                getimageConfig((Hashtable)((Hashtable)slots[2])["imageConfig"], m_WARSAW_ID_P_CAT_BARREL, imageConfig.smallns, true);

                // Add 28 Height to FLP
                FLP_baseHeight += 28;
            }

            FLP.Location = new System.Drawing.Point(3, 3);
            FLP.Name = "fLP_info";
            FLP.Size = new System.Drawing.Size(240, FLP_baseHeight);
            // 
            // sid
            // 
            m_sid.BorderStyle = System.Windows.Forms.Border3DStyle.Adjust;
            m_sid.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_sid.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            m_sid.GradientColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(201)))));
            m_sid.GradientColorTwo = System.Drawing.Color.White;
            m_sid.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            m_sid.Location = new System.Drawing.Point(0, 0);
            m_sid.Margin = new System.Windows.Forms.Padding(0);
            m_sid.Name = "sid";
            m_sid.Size = new System.Drawing.Size(236, 23);
            m_sid.Text = String.Format(" {0}", Loadout_Backbone_Helper.getSID(slot["sid"].ToString()));
            m_sid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // name
            // 
            m_name.BorderStyle = System.Windows.Forms.Border3DStyle.Adjust;
            m_name.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_name.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            m_name.GradientColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(201)))));
            m_name.GradientColorTwo = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(122)))), ((int)(((byte)(124)))));
            m_name.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            m_name.Location = new System.Drawing.Point(0, 23);
            m_name.Margin = new System.Windows.Forms.Padding(0);
            m_name.Name = "name";
            m_name.Size = new System.Drawing.Size(236, 31);
            m_name.Text = String.Format(" {0}", Loadout_Backbone_Helper.getSID(item["name"].ToString()));
            m_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imageConfig
            // 
            m_imageConfig.Location = new System.Drawing.Point(0, 54);
            m_imageConfig.Margin = new System.Windows.Forms.Padding(0);
            m_imageConfig.Name = "imageConfig";
            m_imageConfig.Size = new System.Drawing.Size(236, 65);
            m_imageConfig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            m_imageConfig.TabStop = false;
            m_imageConfig.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
            getimageConfig((Hashtable)item["imageConfig"], m_imageConfig, imageConfig.mediumns);

            fLP_base.Controls.Add(FLP);

            this.Size = new System.Drawing.Size(Width, (FLP_baseHeight + (Height - fLP_info.Height)));
        }

        /// <summary>
        /// Retrieves the StatsInfo
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private FlowLayoutPanel AddStatsInfo(Hashtable item)
        {
            #region Initialize
            System.Windows.Forms.FlowLayoutPanel m_slotInfo_panel;

            System.Windows.Forms.Panel m_statDamage_panel;
            System.Windows.Forms.Label m_statDamage_name;
            ColorProgressBar.ColorProgressBar m_statDamage_cPB;
            System.Windows.Forms.Label m_statDamage_val;

            System.Windows.Forms.Panel m_statAccuracy_panel;
            System.Windows.Forms.Label m_statAccuracy_name;
            ColorProgressBar.ColorProgressBar m_statAccuracy_cPB;
            System.Windows.Forms.Label m_statAccuracy_val;

            System.Windows.Forms.Panel m_statMobility_panel;
            System.Windows.Forms.Label m_statMobility_name;
            ColorProgressBar.ColorProgressBar m_statMobility_cPB;
            System.Windows.Forms.Label m_statMobility_val;

            System.Windows.Forms.Panel m_statRange_panel;
            System.Windows.Forms.Label m_statRange_name;
            ColorProgressBar.ColorProgressBar m_statRange_cPB;
            System.Windows.Forms.Label m_statRange_val;

            System.Windows.Forms.Panel m_statHandling_panel;
            System.Windows.Forms.Label m_statHandling_name;
            ColorProgressBar.ColorProgressBar m_statHandling_cPB;
            System.Windows.Forms.Label m_statHandling_val;

            m_slotInfo_panel = new System.Windows.Forms.FlowLayoutPanel();
            m_statDamage_panel = new System.Windows.Forms.Panel();
            m_statDamage_name = new System.Windows.Forms.Label();
            m_statDamage_cPB = new ColorProgressBar.ColorProgressBar();
            m_statDamage_val = new System.Windows.Forms.Label();

            m_statAccuracy_panel = new System.Windows.Forms.Panel();
            m_statAccuracy_name = new System.Windows.Forms.Label();
            m_statAccuracy_cPB = new ColorProgressBar.ColorProgressBar();
            m_statAccuracy_val = new System.Windows.Forms.Label();

            m_statMobility_panel = new System.Windows.Forms.Panel();
            m_statMobility_name = new System.Windows.Forms.Label();
            m_statMobility_cPB = new ColorProgressBar.ColorProgressBar();
            m_statMobility_val = new System.Windows.Forms.Label();

            m_statRange_panel = new System.Windows.Forms.Panel();
            m_statRange_name = new System.Windows.Forms.Label();
            m_statRange_cPB = new ColorProgressBar.ColorProgressBar();
            m_statRange_val = new System.Windows.Forms.Label();

            m_statHandling_panel = new System.Windows.Forms.Panel();
            m_statHandling_name = new System.Windows.Forms.Label();
            m_statHandling_cPB = new ColorProgressBar.ColorProgressBar();
            m_statHandling_val = new System.Windows.Forms.Label();
            #endregion

            // Parse Data
            object[] Modifiers = (object[])Loadout_Backbone_Helper.calculateModifiers(item, null);
            if (Modifiers == null)
                return (null);

            var weaponDataRelative = ((KeyValuePair<String, Dictionary<String, double>>)Modifiers[0]).Value;
            int statRange = (int)Math.Round(Convert.ToSingle(weaponDataRelative["statRange"]) * 100, MidpointRounding.AwayFromZero);
            int statHandling = (int)Math.Round(Convert.ToSingle(weaponDataRelative["statHandling"]) * 100, MidpointRounding.AwayFromZero);
            int statMobility = (int)Math.Round(Convert.ToSingle(weaponDataRelative["statMobility"]) * 100, MidpointRounding.AwayFromZero);
            int statAccuracy = (int)Math.Round(Convert.ToSingle(weaponDataRelative["statAccuracy"]) * 100, MidpointRounding.AwayFromZero);
            int statDamage = (int)Math.Round(Convert.ToSingle(weaponDataRelative["statDamage"]) * 100, MidpointRounding.AwayFromZero);

            // 
            // slotInfo_panel
            // 
            m_slotInfo_panel.Controls.Add(m_statDamage_panel);
            m_slotInfo_panel.Controls.Add(m_statAccuracy_panel);
            m_slotInfo_panel.Controls.Add(m_statMobility_panel);
            m_slotInfo_panel.Controls.Add(m_statRange_panel);
            m_slotInfo_panel.Controls.Add(m_statHandling_panel);
            m_slotInfo_panel.Location = new System.Drawing.Point(0, 119);
            m_slotInfo_panel.Margin = new System.Windows.Forms.Padding(0);
            m_slotInfo_panel.Name = "slotInfo_panel";
            m_slotInfo_panel.Size = new System.Drawing.Size(236, 104);
            m_slotInfo_panel.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
            #region statDamage
            // 
            // statDamage_panel
            // 
            m_statDamage_panel.Controls.Add(m_statDamage_val);
            m_statDamage_panel.Controls.Add(m_statDamage_name);
            m_statDamage_panel.Controls.Add(m_statDamage_cPB);
            m_statDamage_panel.Location = new System.Drawing.Point(0, 119);
            m_statDamage_panel.Margin = new System.Windows.Forms.Padding(0);
            m_statDamage_panel.Name = "statDamage_panel";
            m_statDamage_panel.Size = new System.Drawing.Size(236, 16);
           // m_statDamage_panel.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
            // 
            // statDamage_val
            // 
            m_statDamage_val.Location = new System.Drawing.Point(195, 0);
            m_statDamage_val.Margin = new System.Windows.Forms.Padding(0);
            m_statDamage_val.Name = "statDamage_val";
            m_statDamage_val.Size = new System.Drawing.Size(30, 15);
            m_statDamage_val.Text = (statDamage).ToString();
            m_statDamage_val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statDamage_name
            // 
            m_statDamage_name.Location = new System.Drawing.Point(10, 0);
            m_statDamage_name.Margin = new System.Windows.Forms.Padding(0);
            m_statDamage_name.Name = "statDamage_name";
            m_statDamage_name.Size = new System.Drawing.Size(80, 15);
            m_statDamage_name.Text = "DAMAGE";
            m_statDamage_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statDamage_cPB
            // 
            m_statDamage_cPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            m_statDamage_cPB.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(176)))), ((int)(((byte)(177)))));
            m_statDamage_cPB.BorderColor = System.Drawing.Color.Black;
            m_statDamage_cPB.FillStyle = ColorProgressBar.ColorProgressBar.FillStyles.Dashed;
            m_statDamage_cPB.Location = new System.Drawing.Point(90, 1);
            m_statDamage_cPB.Margin = new System.Windows.Forms.Padding(0);
            m_statDamage_cPB.Maximum = 100;
            m_statDamage_cPB.Minimum = 0;
            m_statDamage_cPB.Name = "statDamage_cPB";
            m_statDamage_cPB.Size = new System.Drawing.Size(105, 13);
            m_statDamage_cPB.Step = 10;
            m_statDamage_cPB.Value = (statDamage); 
            #endregion
            #region statAccuracy
            // 
            // statAccuracy_panel
            // 
            m_statAccuracy_panel.Controls.Add(m_statAccuracy_val);
            m_statAccuracy_panel.Controls.Add(m_statAccuracy_name);
            m_statAccuracy_panel.Controls.Add(m_statAccuracy_cPB);
            m_statAccuracy_panel.Location = new System.Drawing.Point(0, 141);
            m_statAccuracy_panel.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            m_statAccuracy_panel.Name = "statAccuracy_panel";
            m_statAccuracy_panel.Size = new System.Drawing.Size(236, 16);
           // m_statAccuracy_panel.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
            // 
            // statAccuracy_val
            // 
            m_statAccuracy_val.Location = new System.Drawing.Point(195, 0);
            m_statAccuracy_val.Margin = new System.Windows.Forms.Padding(0);
            m_statAccuracy_val.Name = "statAccuracy_val";
            m_statAccuracy_val.Size = new System.Drawing.Size(30, 15);
            m_statAccuracy_val.Text = (statAccuracy).ToString();
            m_statAccuracy_val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statAccuracy_name
            // 
            m_statAccuracy_name.Location = new System.Drawing.Point(10, 0);
            m_statAccuracy_name.Margin = new System.Windows.Forms.Padding(0);
            m_statAccuracy_name.Name = "statAccuracy_name";
            m_statAccuracy_name.Size = new System.Drawing.Size(80, 15);
            m_statAccuracy_name.Text = "ACCURACY";
            m_statAccuracy_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statAccuracy_cPB
            // 
            m_statAccuracy_cPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            m_statAccuracy_cPB.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(176)))), ((int)(((byte)(177)))));
            m_statAccuracy_cPB.BorderColor = System.Drawing.Color.Black;
            m_statAccuracy_cPB.FillStyle = ColorProgressBar.ColorProgressBar.FillStyles.Dashed;
            m_statAccuracy_cPB.Location = new System.Drawing.Point(90, 1);
            m_statAccuracy_cPB.Margin = new System.Windows.Forms.Padding(0);
            m_statAccuracy_cPB.Maximum = 100;
            m_statAccuracy_cPB.Minimum = 0;
            m_statAccuracy_cPB.Name = "statAccuracy_cPB";
            m_statAccuracy_cPB.Size = new System.Drawing.Size(105, 13);
            m_statAccuracy_cPB.Step = 10;
            m_statAccuracy_cPB.Value = (statAccuracy); 
            #endregion
            #region statMobility
            // 
            // statMobility_panel
            // 
            m_statMobility_panel.Controls.Add(m_statMobility_val);
            m_statMobility_panel.Controls.Add(m_statMobility_name);
            m_statMobility_panel.Controls.Add(m_statMobility_cPB);
            m_statMobility_panel.Location = new System.Drawing.Point(0, 163);
            m_statMobility_panel.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            m_statMobility_panel.Name = "statMobility_panel";
            m_statMobility_panel.Size = new System.Drawing.Size(236, 16);
           // m_statMobility_panel.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
            // 
            // statMobility_val
            // 
            m_statMobility_val.Location = new System.Drawing.Point(195, 0);
            m_statMobility_val.Margin = new System.Windows.Forms.Padding(0);
            m_statMobility_val.Name = "statMobility_val";
            m_statMobility_val.Size = new System.Drawing.Size(30, 15);
            m_statMobility_val.Text = (statMobility).ToString();
            m_statMobility_val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statMobility_name
            // 
            m_statMobility_name.Location = new System.Drawing.Point(10, 0);
            m_statMobility_name.Margin = new System.Windows.Forms.Padding(0);
            m_statMobility_name.Name = "statMobility_name";
            m_statMobility_name.Size = new System.Drawing.Size(80, 15);
            m_statMobility_name.Text = "HIP FIRE";
            m_statMobility_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statMobility_cPB
            // 
            m_statMobility_cPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            m_statMobility_cPB.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(176)))), ((int)(((byte)(177)))));
            m_statMobility_cPB.BorderColor = System.Drawing.Color.Black;
            m_statMobility_cPB.FillStyle = ColorProgressBar.ColorProgressBar.FillStyles.Dashed;
            m_statMobility_cPB.Location = new System.Drawing.Point(90, 1);
            m_statMobility_cPB.Margin = new System.Windows.Forms.Padding(0);
            m_statMobility_cPB.Maximum = 100;
            m_statMobility_cPB.Minimum = 0;
            m_statMobility_cPB.Name = "statMobility_cPB";
            m_statMobility_cPB.Size = new System.Drawing.Size(105, 13);
            m_statMobility_cPB.Step = 10;
            m_statMobility_cPB.Value = (statMobility); 
            #endregion
            #region statRange
            // 
            // statRange_panel
            // 
            m_statRange_panel.Controls.Add(m_statRange_val);
            m_statRange_panel.Controls.Add(m_statRange_name);
            m_statRange_panel.Controls.Add(m_statRange_cPB);
            m_statRange_panel.Location = new System.Drawing.Point(0, 185);
            m_statRange_panel.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            m_statRange_panel.Name = "statRange_panel";
            m_statRange_panel.Size = new System.Drawing.Size(236, 16);
          //  m_statRange_panel.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
            // 
            // statRange_val
            // 
            m_statRange_val.Location = new System.Drawing.Point(195, 0);
            m_statRange_val.Margin = new System.Windows.Forms.Padding(0);
            m_statRange_val.Name = "statRange_val";
            m_statRange_val.Size = new System.Drawing.Size(30, 15);
            m_statRange_val.Text = (statRange).ToString();
            m_statRange_val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statRange_name
            // 
            m_statRange_name.Location = new System.Drawing.Point(10, 0);
            m_statRange_name.Margin = new System.Windows.Forms.Padding(0);
            m_statRange_name.Name = "statRange_name";
            m_statRange_name.Size = new System.Drawing.Size(80, 15);
            m_statRange_name.Text = "RANGE";
            m_statRange_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statRange_cPB
            // 
            m_statRange_cPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            m_statRange_cPB.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(176)))), ((int)(((byte)(177)))));
            m_statRange_cPB.BorderColor = System.Drawing.Color.Black;
            m_statRange_cPB.FillStyle = ColorProgressBar.ColorProgressBar.FillStyles.Dashed;
            m_statRange_cPB.Location = new System.Drawing.Point(90, 1);
            m_statRange_cPB.Margin = new System.Windows.Forms.Padding(0);
            m_statRange_cPB.Maximum = 100;
            m_statRange_cPB.Minimum = 0;
            m_statRange_cPB.Name = "statRange_cPB";
            m_statRange_cPB.Size = new System.Drawing.Size(105, 13);
            m_statRange_cPB.Step = 10;
            m_statRange_cPB.Value = (statRange); 
            #endregion
            #region statHandling
            // 
            // statHandling_panel
            // 
            m_statHandling_panel.Controls.Add(m_statHandling_val);
            m_statHandling_panel.Controls.Add(m_statHandling_name);
            m_statHandling_panel.Controls.Add(m_statHandling_cPB);
            m_statHandling_panel.Location = new System.Drawing.Point(0, 207);
            m_statHandling_panel.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            m_statHandling_panel.Name = "statHandling_panel";
            m_statHandling_panel.Size = new System.Drawing.Size(236, 16);
           // m_statHandling_panel.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
            // 
            // statHandling_val
            // 
            m_statHandling_val.Location = new System.Drawing.Point(195, 0);
            m_statHandling_val.Margin = new System.Windows.Forms.Padding(0);
            m_statHandling_val.Name = "statHandling_val";
            m_statHandling_val.Size = new System.Drawing.Size(30, 15);
            m_statHandling_val.Text = (statHandling).ToString();
            m_statHandling_val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statHandling_name
            // 
            m_statHandling_name.Location = new System.Drawing.Point(10, 0);
            m_statHandling_name.Margin = new System.Windows.Forms.Padding(0);
            m_statHandling_name.Name = "statHandling_name";
            m_statHandling_name.Size = new System.Drawing.Size(80, 15);
            m_statHandling_name.Text = "STABILITY";
            m_statHandling_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statHandling_cPB
            // 
            m_statHandling_cPB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            m_statHandling_cPB.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(176)))), ((int)(((byte)(177)))));
            m_statHandling_cPB.BorderColor = System.Drawing.Color.Black;
            m_statHandling_cPB.FillStyle = ColorProgressBar.ColorProgressBar.FillStyles.Dashed;
            m_statHandling_cPB.Location = new System.Drawing.Point(90, 1);
            m_statHandling_cPB.Margin = new System.Windows.Forms.Padding(0);
            m_statHandling_cPB.Maximum = 100;
            m_statHandling_cPB.Minimum = 0;
            m_statHandling_cPB.Name = "statHandling_cPB";
            m_statHandling_cPB.Size = new System.Drawing.Size(105, 13);
            m_statHandling_cPB.Step = 10;
            m_statHandling_cPB.Value = (statHandling); 
            #endregion

            return (m_slotInfo_panel);
        }

        /// <summary>
        /// Retrieves the ExtendedStats
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private FlowLayoutPanel AddExtendedStats(Hashtable item)
        {
            #region Initialize
            System.Windows.Forms.FlowLayoutPanel m_fLP_extended_stats;
            System.Windows.Forms.Panel m_rateOfFire_panel;
            System.Windows.Forms.Label m_rateOfFire;
            System.Windows.Forms.Label m_rateOfFire_name;
            System.Windows.Forms.Panel m_fireModes_panel;
            System.Windows.Forms.Label m_fireModes;
            System.Windows.Forms.Panel m_ammo_panel;
            System.Windows.Forms.Label m_ammo;
            System.Windows.Forms.Label m_ammo_name;
            System.Windows.Forms.Panel m_ammoType_panel;
            System.Windows.Forms.Label m_ammoType;
            System.Windows.Forms.Label m_ammoType_name;
            System.Windows.Forms.PictureBox m_fireModeAuto;
            System.Windows.Forms.PictureBox m_fireModeBurst;
            System.Windows.Forms.PictureBox m_fireModeSingle;

            m_fLP_extended_stats = new System.Windows.Forms.FlowLayoutPanel();
            m_rateOfFire_panel = new System.Windows.Forms.Panel();
            m_rateOfFire = new System.Windows.Forms.Label();
            m_rateOfFire_name = new System.Windows.Forms.Label();
            m_fireModes_panel = new System.Windows.Forms.Panel();
            m_fireModeAuto = new System.Windows.Forms.PictureBox();
            m_fireModeBurst = new System.Windows.Forms.PictureBox();
            m_fireModes = new System.Windows.Forms.Label();
            m_fireModeSingle = new System.Windows.Forms.PictureBox();
            m_ammo_panel = new System.Windows.Forms.Panel();
            m_ammo = new System.Windows.Forms.Label();
            m_ammo_name = new System.Windows.Forms.Label();
            m_ammoType_panel = new System.Windows.Forms.Panel();
            m_ammoType = new System.Windows.Forms.Label();
            m_ammoType_name = new System.Windows.Forms.Label();
            #endregion

            // Parse Data
            if (!item.ContainsKey("weaponData"))
                return (null);
            Hashtable weaponData = (Hashtable)item["weaponData"];
            int ammo = -1;
            object rateOfFire = null;
            Boolean fireModeSingle = false;
            Boolean fireModeBurst = false;
            Boolean fireModeAuto = false;
            String altAmmoName = null;
            String range = null;
            String ammoType = null;

            if (weaponData.ContainsKey("ammo"))
                ammo = Convert.ToInt32(weaponData["ammo"] ?? -1);
            if (weaponData.ContainsKey("rateOfFire"))
                rateOfFire = weaponData["rateOfFire"];
            if (weaponData.ContainsKey("fireModeSingle"))
                fireModeSingle = Convert.ToBoolean(weaponData["fireModeSingle"] ?? false);
            if (weaponData.ContainsKey("fireModeBurst"))
                fireModeBurst = Convert.ToBoolean(weaponData["fireModeBurst"] ?? false);
            if (weaponData.ContainsKey("fireModeAuto"))
                fireModeAuto = Convert.ToBoolean(weaponData["fireModeAuto"] ?? false);
            if (weaponData.ContainsKey("altAmmoName"))
                altAmmoName = Convert.ToString(weaponData["altAmmoName"] ?? null);
            if (weaponData.ContainsKey("range"))
                range = Convert.ToString(weaponData["range"] ?? null);
            if (weaponData.ContainsKey("ammoType"))
                ammoType = Convert.ToString(weaponData["ammoType"] ?? null);

            // 
            // fLP_extended_stats
            // 
            m_fLP_extended_stats.Controls.Add(m_rateOfFire_panel);
            m_fLP_extended_stats.Controls.Add(m_fireModes_panel);
            m_fLP_extended_stats.Controls.Add(m_ammo_panel);
            m_fLP_extended_stats.Controls.Add(m_ammoType_panel);
            m_fLP_extended_stats.Location = new System.Drawing.Point(10, 223);
            m_fLP_extended_stats.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            m_fLP_extended_stats.Name = "fLP_extended_stats";
            m_fLP_extended_stats.Size = new System.Drawing.Size(216, 84);
            m_fLP_extended_stats.BackgroundImage = global::BF4_LoadoutChecker.Properties.Resources._50_opacity;
            #region rateOfFire
            // 
            // rateOfFire_panel
            // 
            m_rateOfFire_panel.Controls.Add(m_rateOfFire);
            m_rateOfFire_panel.Controls.Add(m_rateOfFire_name);
            m_rateOfFire_panel.Location = new System.Drawing.Point(0, 0);
            m_rateOfFire_panel.Margin = new System.Windows.Forms.Padding(0);
            m_rateOfFire_panel.Name = "rateOfFire_panel";
            m_rateOfFire_panel.Size = new System.Drawing.Size(108, 42);
            // 
            // rateOfFire_name
            // 
            m_rateOfFire_name.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_rateOfFire_name.Location = new System.Drawing.Point(1, 0);
            m_rateOfFire_name.Margin = new System.Windows.Forms.Padding(0);
            m_rateOfFire_name.Name = "rateOfFire_name";
            m_rateOfFire_name.Size = new System.Drawing.Size(106, 15);
            m_rateOfFire_name.Text = "RATE OF FIRE";
            m_rateOfFire_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rateOfFire
            // 
            m_rateOfFire.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_rateOfFire.Location = new System.Drawing.Point(1, 15);
            m_rateOfFire.Margin = new System.Windows.Forms.Padding(0);
            m_rateOfFire.Name = "rateOfFire";
            m_rateOfFire.Size = new System.Drawing.Size(106, 26);
            m_rateOfFire.Text = (rateOfFire.GetType().Name == "String") ? Loadout_Backbone_Helper.getSID(rateOfFire.ToString()) : rateOfFire.ToString();
            m_rateOfFire.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            #endregion
            #region fireModes
            // 
            // fireModes_panel
            // 
            m_fireModes_panel.Controls.Add(m_fireModeAuto);
            m_fireModes_panel.Controls.Add(m_fireModeBurst);
            m_fireModes_panel.Controls.Add(m_fireModes);
            m_fireModes_panel.Controls.Add(m_fireModeSingle);
            m_fireModes_panel.Location = new System.Drawing.Point(108, 0);
            m_fireModes_panel.Margin = new System.Windows.Forms.Padding(0);
            m_fireModes_panel.Name = "fireModes_panel";
            m_fireModes_panel.Size = new System.Drawing.Size(108, 42);
            // 
            // fireModes
            // 
            m_fireModes.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_fireModes.Location = new System.Drawing.Point(1, 0);
            m_fireModes.Margin = new System.Windows.Forms.Padding(0);
            m_fireModes.Name = "fireModes";
            m_fireModes.Size = new System.Drawing.Size(106, 15);
            m_fireModes.Text = "FIRE MODES";
            m_fireModes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fireModeAuto
            // 
            m_fireModeAuto.Image = global::BF4_LoadoutChecker.Properties.Resources.firemode_auto;
            m_fireModeAuto.Location = new System.Drawing.Point(71, 15);
            m_fireModeAuto.Margin = new System.Windows.Forms.Padding(0);
            m_fireModeAuto.Name = "fireModeAuto";
            m_fireModeAuto.Size = new System.Drawing.Size(35, 26);
            m_fireModeAuto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            m_fireModeAuto.TabStop = false;
            if (!fireModeAuto)
                m_fireModeAuto.Image = ImageUtils.ImageTransparency.ChangeOpacity(m_fireModeAuto.Image, opacity_disabled);
            // 
            // fireModeBurst
            // 
            m_fireModeBurst.Image = global::BF4_LoadoutChecker.Properties.Resources.firemode_burst;
            m_fireModeBurst.Location = new System.Drawing.Point(36, 16);
            m_fireModeBurst.Margin = new System.Windows.Forms.Padding(0);
            m_fireModeBurst.Name = "fireModeBurst";
            m_fireModeBurst.Size = new System.Drawing.Size(35, 26);
            m_fireModeBurst.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            m_fireModeBurst.TabStop = false;
            if (!fireModeBurst)
                m_fireModeBurst.Image = ImageUtils.ImageTransparency.ChangeOpacity(m_fireModeBurst.Image, opacity_disabled);
            // 
            // fireModeSingle
            // 
            m_fireModeSingle.Image = global::BF4_LoadoutChecker.Properties.Resources.firemode_single;
            m_fireModeSingle.Location = new System.Drawing.Point(1, 16);
            m_fireModeSingle.Margin = new System.Windows.Forms.Padding(0);
            m_fireModeSingle.Name = "fireModeSingle";
            m_fireModeSingle.Size = new System.Drawing.Size(35, 26);
            m_fireModeSingle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            m_fireModeSingle.TabStop = false;
            if (!fireModeSingle)
                m_fireModeSingle.Image = ImageUtils.ImageTransparency.ChangeOpacity(m_fireModeSingle.Image, opacity_disabled);
            #endregion
            #region ammo
            // 
            // ammo_panel
            // 
            m_ammo_panel.Controls.Add(m_ammo);
            m_ammo_panel.Controls.Add(m_ammo_name);
            m_ammo_panel.Location = new System.Drawing.Point(0, 42);
            m_ammo_panel.Margin = new System.Windows.Forms.Padding(0);
            m_ammo_panel.Name = "ammo_panel";
            m_ammo_panel.Size = new System.Drawing.Size(108, 42);
            // 
            // ammo_name
            // 
            m_ammo_name.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_ammo_name.Location = new System.Drawing.Point(1, 0);
            m_ammo_name.Margin = new System.Windows.Forms.Padding(0);
            m_ammo_name.Name = "ammo_name";
            m_ammo_name.Size = new System.Drawing.Size(106, 15);
            m_ammo_name.Text = "MAGAZINE SIZE";
            m_ammo_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ammo
            // 
            m_ammo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_ammo.Location = new System.Drawing.Point(1, 15);
            m_ammo.Margin = new System.Windows.Forms.Padding(0);
            m_ammo.Name = "ammo";
            m_ammo.Size = new System.Drawing.Size(106, 26);
            m_ammo.Text = Loadout_Backbone_Helper.getSID(ammo.ToString());
            m_ammo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            #endregion
            #region ammoType
            // 
            // ammoType_panel
            // 
            m_ammoType_panel.Controls.Add(m_ammoType);
            m_ammoType_panel.Controls.Add(m_ammoType_name);
            m_ammoType_panel.Location = new System.Drawing.Point(108, 42);
            m_ammoType_panel.Margin = new System.Windows.Forms.Padding(0);
            m_ammoType_panel.Name = "ammoType_panel";
            m_ammoType_panel.Size = new System.Drawing.Size(108, 42);
            // 
            // ammoType_name
            // 
            m_ammoType_name.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_ammoType_name.Location = new System.Drawing.Point(1, 0);
            m_ammoType_name.Margin = new System.Windows.Forms.Padding(0);
            m_ammoType_name.Name = "ammoType_name";
            m_ammoType_name.Size = new System.Drawing.Size(106, 15);
            m_ammoType_name.Text = "AMMO TYPE";
            m_ammoType_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ammoType
            // 
            m_ammoType.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_ammoType.Location = new System.Drawing.Point(1, 15);
            m_ammoType.Margin = new System.Windows.Forms.Padding(0);
            m_ammoType.Name = "ammoType";
            m_ammoType.Size = new System.Drawing.Size(106, 26);
            m_ammoType.Text = Loadout_Backbone_Helper.getSID(ammoType);
            m_ammoType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; 
            #endregion

            return (m_fLP_extended_stats);
        }

        public void ClearfLPBase()
        {
            fLP_base.Controls.Clear();
        }

        private void getimageConfig(Hashtable imageConfig, PictureBox pBox, imageConfig image_type = BF4_LoadoutChecker.imageConfig.xsmall, bool async = false)
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
    }
}
