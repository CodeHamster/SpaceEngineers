﻿
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Gui;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Diagnostics;
using System.Text;
using VRage;
using VRage.Library.Utils;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Gui
{
    class MyGuiScreenEncounterSettings : MyGuiScreenBase
    {
        MyGuiScreenAdvancedWorldSettings m_parent;
        bool m_isNewGame;

        bool m_isConfirmed;   
        
        MyGuiControlButton m_okButton, m_cancelButton;               

        internal MyGuiScreenEncounterSettings EncounterConfiguration;        

        public bool IsConfirmed
        {
            get
            {
                return m_isConfirmed;
            }
        }

        public MyGuiScreenEncounterSettings(MyGuiScreenAdvancedWorldSettings parent)
            : base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, CalcSize(parent.Checkpoint))
        {
            MySandboxGame.Log.WriteLine("MyGuiScreenEncounterSettings.ctor START");

            m_parent = parent;
            EnabledBackgroundFade = true;

            m_isNewGame = (parent.Checkpoint == null);
            m_isConfirmed = false;

            RecreateControls(true);

            MySandboxGame.Log.WriteLine("MyGuiScreenEncounterSettings.ctor END");
        }

        public static Vector2 CalcSize(MyObjectBuilder_Checkpoint checkpoint)
        {
            float width = 0.9f;
            float height = 1.24f;

            if (MyFakes.OCTOBER_RELEASE_HIDE_WORLD_PARAMS)
                height -= 0.27f;

            return new Vector2(width, height);
        }

        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);

            BuildControls();

            LoadValues();
        }

        public void BuildControls()
        {
            MyGuiControlParent parent = new MyGuiControlParent(size: new Vector2(Size.Value.X - 0.05f, Size.Value.Y));
            MyGuiControlScrollablePanel scrollPanel = new MyGuiControlScrollablePanel(parent);
            scrollPanel.ScrollbarVEnabled = true;
            scrollPanel.Size = new Vector2(Size.Value.X - 0.05f, 0.8f);

            Vector2 buttonSize = MyGuiConstants.BACK_BUTTON_SIZE;
            Vector2 buttonsOrigin = m_size.Value / 2 - new Vector2(0.23f, 0.03f);

            AddCaption(MySpaceTexts.ScreenCaptionEncountersConfiguration);

            int numControls = 0;           

            float width = 0.284375f + 0.025f;           

            // Ok/Cancel
            m_okButton = new MyGuiControlButton(position: buttonsOrigin - new Vector2(0.01f, 0f), size: buttonSize, text: MyTexts.Get(MySpaceTexts.Ok), onButtonClick: OkButtonClicked, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
            m_cancelButton = new MyGuiControlButton(position: buttonsOrigin + new Vector2(0.01f, 0f), size: buttonSize, text: MyTexts.Get(MySpaceTexts.Cancel), onButtonClick: CancelButtonClicked, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);                     

            float labelSize = 0.21f;

            float MARGIN_TOP = 0.03f;
            
            Controls.Add(m_okButton);
            Controls.Add(m_cancelButton);

            Controls.Add(scrollPanel);
            CloseButtonEnabled = true;
        }

        private MyGuiControlLabel MakeLabel(MyStringId textEnum)
        {
            return new MyGuiControlLabel(text: MyTexts.GetString(textEnum), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
        }

        private void LoadValues()
        {            
        }

        private void CheckButton(float value, params MyGuiControlButton[] allButtons)
        {
            bool any = false;
            foreach (var btn in allButtons)
            {
                if (btn.UserData is float)
                {
                    if ((float)btn.UserData == value && !btn.Checked)
                    {
                        any = true;
                        btn.Checked = true;
                    }
                    else if ((float)btn.UserData != value && btn.Checked)
                        btn.Checked = false;
                }
            }

            if (!any)
                allButtons[0].Checked = true;
        }

        private void CheckButton(MyGuiControlButton active, params MyGuiControlButton[] allButtons)
        {
            foreach (var btn in allButtons)
            {
                if (btn == active && !btn.Checked)
                    btn.Checked = true;
                else if (btn != active && btn.Checked)
                    btn.Checked = false;
            }
        }

        public void UpdateSurvivalState(bool survivalEnabled)
        {           
        }       

        public void GetSettings(MyObjectBuilder_SessionSettings output)
        {
        }

        public void SetSettings(MyObjectBuilder_SessionSettings settings)
        {           
        }

        public override string GetFriendlyName()
        {
            return "MyGuiScreenEncounterSettings";
        }

        private void CancelButtonClicked(object sender)
        {
            this.CloseScreen();
        }

        private void OkButtonClicked(object sender)
        {
            m_isConfirmed = true;

            if (OnOkButtonClicked != null)
            {
                OnOkButtonClicked();
            }

            this.CloseScreen();
        }

        public event System.Action OnOkButtonClicked;
    }
}
