﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SkyCar.Framework.WindowUI
{
    /// <summary>
    /// 主题色彩：面朝大海，春暖花开
    /// </summary>
    /// User:Ryan  CreateTime:2012-8-7 22:17.
    internal class SkinThemeBlueSea : SkinTheme
    {
        public SkinThemeBlueSea()
            : base()
        {
        }

        /// <summary>
        /// 初始化主题色彩方案
        /// </summary>
        /// User:Ryan  CreateTime:2012-8-7 22:11.
        /// User:Ryan  CreateTime:2012-8-7 22:18.
        public override void IniSkinTheme()
        {
            this.ThemeStyle = EnumTheme.BlueSea;
            this.ThemeName = "面朝大海，春暖花开";
            this.BackGroundImage = Properties.Resources.bg06;
            this.BackGroundImageEnable = false;
            this.BackGroundImageOpacity = 0.8F;
            this.BaseColor = Color.FromArgb(238, 247, 252);
            this.BorderColor = Color.FromArgb(65, 157, 212);
            this.InnerBorderColor = Color.FromArgb(196, 214, 230);
            this.OuterBorderColor = Color.FromArgb(30, 111, 201);
            this.DefaultControlColor = new GradientColor(Color.FromArgb(248, 245, 251), Color.FromArgb(227, 226, 227),
                new float[] { 0.0f, 0.15f, 0.05f, 0.2f, 0.6f, 0.8f, 0.85f },
                new float[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 0.9f, 1.0f });
            this.HeightLightControlColor = new GradientColor(Color.FromArgb(1, 185, 246), Color.FromArgb(31, 112, 202),
                new float[] { 0.0F, 0.7F, 1.5F }, new float[] { 0.0F, 0.6F, 1F });
            this.FocusedControlColor = new GradientColor(Color.FromArgb(30, 111, 201), Color.FromArgb(60, 206, 238),
                new float[] { 0.2F, 0.6F, 0.8F, 0.4F, 0.2F }, new float[] { 0.0F, 0.3F, 0.6F, 0.8F, 1F });
            this.UselessColor = Color.FromArgb(159, 159, 159);
            this.CaptionColor = new GradientColor(Color.FromArgb(131, 196, 241), Color.FromArgb(131, 196, 241),
                new float[] {  }, new float[] {  });
            this.ThemeColor = Color.FromArgb(255, 255, 255);
            this.CaptionFontColor = Color.FromArgb(255, 255, 255);
            ////control color
            this.ControlBoxDefaultColor = new GradientColor(Color.FromArgb(131, 196, 241), Color.FromArgb(131, 196, 241),
                new float[] {  }, new float[] {  });
            this.ControlBoxHeightLightColor = new GradientColor(Color.FromArgb(49, 157, 232), Color.FromArgb(49, 157, 232),
                new float[] {  }, new float[] {  });
            //this.ControlBoxPressedColor = new GradientColor(Color.FromArgb(33, 154, 202), Color.FromArgb(0, 33, 154, 202),
            //    new float[] { 0f, .7f, .2f, 0f }, new float[] { 0f, .3f, .6f, 1f });
            this.CloseBoxHeightLightColor = new GradientColor(Color.FromArgb(49, 157, 232), Color.FromArgb(49, 157, 232),
                new float[] {  }, new float[] {  });
            //this.CloseBoxPressedColor = new GradientColor(Color.FromArgb(167, 78, 58), Color.FromArgb(0, 167, 78, 58),
            //    new float[] { 0f, .7f, .2f, 0f }, new float[] { 0f, .3f, .6f, 1f });
            this.ControlBoxFlagColor = Color.White;
        }
    }
}
