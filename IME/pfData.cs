using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IME
{
    class pfData
    {
        public List<string> PaletteColors { get; set; } // パレットの色 (HTML形式)
        public string CurrentColor { get; set; }       // 現在の色 (HTML形式)
        public string PenWidth { get; set; }           // 線の太さ
        public string BackgroundImage { get; set; }    // 背景画像 (Base64形式)
        public string DrawingImage { get; set; }       // 描画画像 (Base64形式)
    }
}
