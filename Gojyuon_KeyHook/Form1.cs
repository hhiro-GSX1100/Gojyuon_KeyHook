//*************************************************************************************
//* 2022/09/26 hhiro-GSX1100
//* Special Thanks to AonaSuzutsuki:https://aonasuzutsuki.hatenablog.jp/entry/2018/10/15/170958
//*************************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gojyuon_KeyHook
{


    public partial class Form1 : Form
    {
        private static Form1 _form1Instance;
        public static Form1 Form1Instance
        {
            get
            {
                return _form1Instance;
            }
            set
            {
                _form1Instance = value;
            }
        }
        // タイマーでスキャンしたIMEの状態により、Hook対象であるかを格納
        public Boolean imeKanaMode;
        //IMEの値を定期的にスキャンするタイマー間隔
        const int MY_TIMER_INTERVAL = 300;

        //**************************************************************************************
        // キーマップ
        //**************************************************************************************
        static readonly Dictionary<Keys, Keys> Gojyuon = new Dictionary<Keys, Keys>() {                                             // Normal
            { Keys.D6,Keys.D3 },{ Keys.D7,Keys.E },{ Keys.D8,Keys.D4 },{ Keys.D9,Keys.D5 },{ Keys.D0,Keys.D6 },                     // 6 7 8 9 0
            { Keys.Y,Keys.T },{ Keys.U,Keys.G },{ Keys.I,Keys.H },{ Keys.O,Keys.OemSemicolon },{ Keys.P,Keys.B },                   // y u i o p
            { Keys.H,Keys.X },{ Keys.J,Keys.D },{ Keys.K,Keys.R },{ Keys.L,Keys.P },{ Keys.Oemplus,Keys.C },                        // h j k l ;
            { Keys.N,Keys.Q },{ Keys.M,Keys.A },{ Keys.Oemcomma,Keys.Z },{ Keys.OemPeriod,Keys.W },{ Keys.OemQuestion,Keys.S },     // n m , . /
            { Keys.D5,Keys.U },{ Keys.D4,Keys.I },{ Keys.D3,Keys.D1 },{ Keys.D2,Keys.Oemcomma },{ Keys.D1,Keys.K },                 // 5 4 3 2 1
            { Keys.T,Keys.F },{ Keys.R,Keys.V },{ Keys.E,Keys.D2 },{ Keys.W,Keys.Oem7 },{ Keys.Q,Keys.OemMinus },                   // t r e w q
            { Keys.G,Keys.J },{ Keys.F,Keys.N },{ Keys.D,Keys.Oem6 },{ Keys.S,Keys.OemQuestion },{ Keys.A,Keys.M },                 // g f d s a
            { Keys.B,Keys.O },{ Keys.V,Keys.L },{ Keys.C,Keys.OemPeriod },{ Keys.X,Keys.Oemplus },{ Keys.Z,Keys.OemBackslash },     // b v c x z
            { Keys.OemMinus,Keys.D7 },{ Keys.Oem7,Keys.D8 },{ Keys.Oem5,Keys.D9 },                                                  // - ^ \
            //{ Keys.Oemtilde,Keys.Oemtilde },{ Keys.OemOpenBrackets,Keys.OemOpenBrackets },                                        // @ [
            { Keys.Oem1,Keys.Y },{ Keys.Oem6,Keys.Oem5 },                                                                           //  : ]
            { Keys.OemBackslash,Keys.D0 }                                                                                           // \
        };
        static readonly Dictionary<Keys, Keys> Gojyuon_Shift = new Dictionary<Keys, Keys>() {                                                   // Shift
            { Keys.D6,Keys.D3 },{ Keys.D7,Keys.E },{ Keys.D8,Keys.D4 },{ Keys.D9,Keys.D5 },{ Keys.D0,Keys.D6 },                                 // 6 7 8 9 0
            { Keys.Y,Keys.T },{ Keys.U,Keys.G },{ Keys.I,Keys.H },{ Keys.O,Keys.OemSemicolon },{ Keys.P,Keys.B },                               // y u i o p
            { Keys.H,Keys.X },{ Keys.J,Keys.D },{ Keys.K,Keys.R },{ Keys.L,Keys.P },{ Keys.Oemplus,Keys.C },                                    // h j k l ;
            { Keys.N,Keys.Q },{ Keys.M,Keys.A },{ Keys.Oemcomma,Keys.Z },{ Keys.OemPeriod,Keys.Oemcomma },{ Keys.OemQuestion,Keys.OemPeriod },  // n m , . /
            { Keys.D5,Keys.U },{ Keys.D4,Keys.I },{ Keys.D3,Keys.D1 },{ Keys.D2,Keys.Oemcomma },{ Keys.D1,Keys.K },                             // 5 4 3 2 1
            { Keys.T,Keys.F },{ Keys.R,Keys.V },{ Keys.E,Keys.D2 },{ Keys.W,Keys.Oem7 },{ Keys.Q,Keys.OemMinus },                               //t r e w q
            { Keys.G,Keys.J },{ Keys.F,Keys.N },{ Keys.D,Keys.Oem6 },{ Keys.S,Keys.OemQuestion },{ Keys.A,Keys.M },                             //g f d s a
            { Keys.B,Keys.O },{ Keys.V,Keys.L },{ Keys.C,Keys.OemPeriod },{ Keys.X,Keys.Oemplus },{ Keys.Z,Keys.OemBackslash },                 // b v c x z
            { Keys.OemMinus,Keys.D7 },{ Keys.Oem7,Keys.D8 },{ Keys.Oem5,Keys.D9 },                                                              // - ^ \
            { Keys.Oemtilde,Keys.OemQuestion },//{ Keys.OemOpenBrackets,Keys.OemOpenBrackets },                                                 // @ [
            { Keys.Oem1,Keys.Y },//{ Keys.Oem6,Keys.Oem5 },                                                                                     // : ]
            { Keys.OemBackslash,Keys.D0 }                                                                                                       // \
        };


        Timer timer1;
        InterceptKeyboard interceptKeyboard;
        public Form1()
        {
            InitializeComponent();
            
            interceptKeyboard = new InterceptKeyboard();
            btnStop.Enabled = false;
            timer1 = new Timer();
            timer1.Interval = MY_TIMER_INTERVAL;
            timer1.Tick += timer_proc;
            btnStart_Click(this, new EventArgs());
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Start();
            interceptKeyboard.KeyDownEvent += InterceptKeyboard_KeyDownEvent;
            interceptKeyboard.KeyUpEvent += InterceptKeyboard_KeyUpEvent;
            interceptKeyboard.Hook();
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Dispose();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            lblModeChange(false);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Dispose();
            Application.Exit();
        }
        private void Dispose() {
            timer1.Stop();
            interceptKeyboard.KeyDownEvent -= InterceptKeyboard_KeyDownEvent;
            interceptKeyboard.KeyUpEvent -= InterceptKeyboard_KeyUpEvent;
            interceptKeyboard.UnHook();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        private void lblModeChange(bool mode) {
            if (mode)
            {
                this.lblCondition.Text = "有効";
                this.lblCondition.ForeColor = Color.Blue;
                this.lblCondition.BackColor = Color.White;
            }
            else {
                this.lblCondition.Text = "";
                //this.lblCondition.ForeColor = Color.Red;
                //this.lblCondition.BackColor = Color.Gray;
            }
        }

        static readonly InterceptInput interceptInput = new InterceptInput();

        private static void InterceptKeyboard_KeyUpEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {
            //Console.WriteLine("Keyup KeyCode {0} - {1}", e.KeyCode, (int)e.KeyCode);
        }

        private static void InterceptKeyboard_KeyDownEvent(object sender, InterceptKeyboard.OriginalKeyEventArg e)
        {
             if (e.IsVirtualInput)  //イベントにより発生させたものは処理しない。
                return;
            if (!Form1.Form1Instance.imeKanaMode)  //IMEがHook対象のモードではない場合は処理しない。
                return;
            // Shiftキー押下と通常でキーマップを切り替える。
            if ((Keys)(Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                if (Gojyuon_Shift.ContainsKey((Keys)e.KeyCode))
                {
                    var input = interceptInput.KeyDown((int)(short)Gojyuon_Shift[e.KeyCode]);
                    interceptInput.KeyUp(input);
                    e.IsCancel = true;
                }
            }
            else
            {
                if (Gojyuon.ContainsKey((Keys)e.KeyCode))
                {
                    var input = interceptInput.KeyDown((int)(short)Gojyuon[e.KeyCode]);
                    interceptInput.KeyUp(input);
                    e.IsCancel = true;
                }
            }
        }


        //**************************************************************************************
        // IME Modeの取得
        //**************************************************************************************
        [DllImport("User32.dll")]
        static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("imm32.dll")]
        static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool GetGUIThreadInfo(uint dwthreadid, ref GUITHREADINFO lpguithreadinfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct GUITHREADINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public System.Drawing.Rectangle rcCaret;
        }

        const int WM_IME_CONTROL = 0x283;
        const int IMC_GETCONVERSIONMODE = 1;
        const int IMC_SETCONVERSIONMODE = 2;
        const int IMC_GETOPENSTATUS = 5;
        const int IMC_SETOPENSTATUS = 6;

        const int IME_CMODE_NATIVE = 1;
        const int IME_CMODE_KATAKANA = 2;
        const int IME_CMODE_FULLSHAPE = 8;
        const int IME_CMODE_ROMAN = 16;

        //ローマ字入力
        const int CMode_Rom_HankakuKana = IME_CMODE_ROMAN | IME_CMODE_KATAKANA | IME_CMODE_NATIVE;
        const int CMode_Rom_ZenkakuEisu = IME_CMODE_ROMAN | IME_CMODE_FULLSHAPE;
        const int CMode_Rom_Hiragana = IME_CMODE_ROMAN | IME_CMODE_FULLSHAPE | IME_CMODE_NATIVE;
        const int CMode_Rom_ZenkakuKana = IME_CMODE_ROMAN | IME_CMODE_FULLSHAPE | IME_CMODE_KATAKANA | IME_CMODE_NATIVE;
        //かな字入力
        //           カナ,対象 / ローマ字
        // 半角カナ     3, *   /  19 
        // 全角英数     8,     /  24 
        // ひらがな     9, *   /  25 
        // 全角カナ    11, *   /  27
        const int CMode_Kana_HankakuKana = IME_CMODE_KATAKANA | IME_CMODE_NATIVE;
        const int CMode_Kana_ZenkakuEisu = IME_CMODE_FULLSHAPE;
        const int CMode_Kana_Hiragana = IME_CMODE_FULLSHAPE | IME_CMODE_NATIVE;
        const int CMode_Kana_ZenkakuKana = IME_CMODE_FULLSHAPE | IME_CMODE_KATAKANA | IME_CMODE_NATIVE;


        private void timer_proc(object sender, EventArgs e)
        {
            //IME状態の取得
            GUITHREADINFO guiThreadInfo = new GUITHREADINFO();
            guiThreadInfo.cbSize = Marshal.SizeOf(guiThreadInfo);

            if (!GetGUIThreadInfo(0, ref guiThreadInfo))
            {
                //Console.WriteLine("GetGUIThreadInfo");
                throw new ApplicationException("GetGUIThreadInfo failed on timer_proc", null); 
            }
            IntPtr imeWnd = ImmGetDefaultIMEWnd(guiThreadInfo.hwndFocus);

            int imeConversionMode = SendMessage(imeWnd, WM_IME_CONTROL, (IntPtr)IMC_GETCONVERSIONMODE, IntPtr.Zero);
            bool imeIsEnabled = (SendMessage(imeWnd, WM_IME_CONTROL, (IntPtr)IMC_GETOPENSTATUS, IntPtr.Zero) != 0);

            if (imeIsEnabled)
            {
                switch (imeConversionMode)
                {
                    case CMode_Kana_Hiragana:
                        if (!imeKanaMode)
                        {
                            imeKanaMode = !imeKanaMode;
                            //Console.WriteLine("ひらがな：" + imeKanaMode);
                        }
                        break;
                    case CMode_Kana_HankakuKana:
                         if (!imeKanaMode)
                        {
                            imeKanaMode = !imeKanaMode;
                            //Console.WriteLine("半角カナ：" + imeKanaMode);
                        }
                        break;
                    case CMode_Kana_ZenkakuKana:
                        if (!imeKanaMode)
                        {
                            imeKanaMode = !imeKanaMode;
                            //Console.WriteLine("全角カナ：" + imeKanaMode);
                        }
                        break;
                    default:
                        if (imeKanaMode)
                        {
                            imeKanaMode = !imeKanaMode;
                            //Console.WriteLine("OFF：" + imeKanaMode);
                        }
                        break;
                }
            }
            else {
                if (imeKanaMode)
                {
                    imeKanaMode = !imeKanaMode;
                    //Console.WriteLine("OFF-1：" + imeKanaMode);
                }
            }
            lblModeChange(imeKanaMode);
        }
    }
}
