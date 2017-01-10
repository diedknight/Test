//using System;
//using System.Collections.Generic;
//using System.Web;
//using System.IO;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Text;
//using Gif.Components;


//namespace PriceMeCommon {
//    /// <summary>
//    ///Catchpa 的摘要说明
//    /// </summary>
//    public class CaptchaTool {
//        private AnimatedGifEncoder coder = new AnimatedGifEncoder();
//        private Stream stream = new MemoryStream();
//        private Random random = new Random();

//        private StringBuilder _identifyingCode = new StringBuilder();
//        private int _defaultIdentifyingCodeLen = 4;
//        private string _availableLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//        private int _frameCount = 4;
//        private int _delay = 900;
//        private int _noiseCount = 15;
//        private int _width = 150, _height = 60;
//        private int _fontSize = 30;
//        private FontFamily _fontFamily = null;
//        private HatchStyle _backgroundHatchStyle = HatchStyle.SmallConfetti;
//        private HatchStyle _frontHatchStyle = HatchStyle.Shingle;

//        public int Width {
//            get { return _width; }
//        }

//        public int Height {
//            get { return _height; }
//        }

//        public string IdentifyingCode {
//            get { return _identifyingCode.ToString(); }
//        }

//        /// <summary>
//        /// millisecond , min : 100 , default: 900
//        /// </summary>
//        public int Delay {
//            get { return _delay; }
//            set { _delay = value < 100 ? 100 : value; }
//        }

//        /// <summary>
//        /// Min : 1 , default : 4
//        /// </summary>
//        public int FrameCount {
//            get { return _frameCount; }
//            set { _frameCount = value < 1 ? 1 : value; }
//        }


//        /// <summary>
//        /// Min : 0 ,default :15
//        /// </summary>
//        public int NoiseCount {
//            get { return _noiseCount; }
//            set { _noiseCount = value < 0 ? 0 : value; }
//        }


//        /// <summary>
//        /// Default FontFamily.GenericSansSerif
//        /// </summary>
//        public FontFamily FontFamily {
//            get { return _fontFamily == null ? FontFamily.GenericSansSerif : _fontFamily; }
//            set { _fontFamily = value; }
//        }

//        /// <summary>
//        /// Between 1 and 50, Default 30
//        /// </summary>
//        public int FontSize {
//            get { return _fontSize; }
//            set { _fontSize = value > 50 ? 50 : (value < 1 ? 1 : value); }
//        }


//        public CaptchaTool(int width, int height) {
//            _width = width < 1 ? 1 : width;
//            _height = height < 1 ? 1 : height;
//            coder.SetSize(Width, Height);
//            coder.SetRepeat(0);
//        }

//        private void GenerateIdentifyingCode(int codeLength) {
//            if (codeLength < 1)
//                codeLength = _defaultIdentifyingCodeLen;

//            _identifyingCode = new StringBuilder();
//            for (int i = 0; i < codeLength; i++) {
//                _identifyingCode.Append(_availableLetters[random.Next(0, _availableLetters.Length)]);
//            }
//        }

//        public void Create(Stream stream) {
//            if (FrameCount > 1) {
//                coder.Start(stream);
//                coder.SetDelay(_delay);
//            }
//            Process(stream);
//        }

//        private void Process(Stream stream) {

//            GenerateIdentifyingCode(_defaultIdentifyingCodeLen);

//            Rectangle rect = new Rectangle(0, 0, Width, Height);

//            HatchBrush br = new HatchBrush(_backgroundHatchStyle, Color.DarkGray, Color.White);
//            HatchBrush brString = new HatchBrush(_frontHatchStyle, Color.White, Color.Transparent);

//            LinearGradientBrush brGString = new LinearGradientBrush(rect, Color.Green, Color.LightGray, LinearGradientMode.BackwardDiagonal);

//            Font f = new Font(this.FontFamily, this.FontSize, FontStyle.Bold);
//            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
//            #region old
//            /*
//        float v = 4f;
//        GraphicsPath gp;
//        */
//            #endregion

//            //Matrix ma = new Matrix();
//            //ma.Translate(0f, 0f);

//            //RectangleF gpRectangle;

//            if (FrameCount > 1) {
//                for (int i = 0; i < _frameCount; i++) {
//                    #region old
//                    /*
//            Image im = new Bitmap(Width, Height);
//            Graphics ga = Graphics.FromImage(im);
//            //这两句没什么作用,大小并没有因此而改变.
//            //ga.SmoothingMode = SmoothingMode.HighSpeed;
//            //ga.InterpolationMode = InterpolationMode.Low;
            
//            ga.FillRectangle(br, rect);

//            PointF[] pfs = { 
//                    new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
//                    new PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
//                    new PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v),
//                    new PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v)
//                };

//            gp = new GraphicsPath();
//            sf.Alignment =(StringAlignment)random.Next(0, 3);//不取上界,即只有 0 - 2 near center far
//            gp.AddString(IdentifyingCode, FontFamily, (int)f.Style, f.Size, rect, sf);
//            //gp.Warp(pfs, rect, ma, WarpMode.Perspective);
//            gp.Warp(pfs, rect);

//            //gpRectangle = gp.GetBounds(ma);
//            //brGString = new LinearGradientBrush(gpRectangle, Color.Green, Color.LightGray, LinearGradientMode.BackwardDiagonal);

//            ga.FillPath(brGString, gp);

//            ga.FillPath(brString, gp);

//            //int fH = (int)f.GetHeight();
//            //int fW = (int)ga.MeasureString(IdentifyingCode, f).Width;
//            //AddNoise(ga);
//            //ga.DrawString(IdentifyingCode, f, brString  , new PointF(random.Next(1, Width - 1 - fW), random.Next(1, Height - 1 - fH)));

//            ga.Flush();
//             */
//                    #endregion
//                    Image im = AddFrame(br, brString, brGString, rect, f, sf);
//                    coder.AddFrame(im);
//                    im.Dispose();
//                }
//                coder.Finish();
//            } else {
//                Image im = AddFrame(br, brString, brGString, rect, f, sf);
//                im.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
//                im.Dispose();
//            }
//        }

//        private Image AddFrame(HatchBrush br, HatchBrush brString, LinearGradientBrush brGString, Rectangle rect, Font f, StringFormat sf) {
//            float v = 4f;
//            Image im = new Bitmap(Width, Height);
//            Graphics ga = Graphics.FromImage(im);
//            ga.FillRectangle(br, rect);

//            PointF[] pfs = { 
//                    new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
//                    new PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
//                    new PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v),
//                    new PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v)
//                };

//            GraphicsPath gp = new GraphicsPath();
//            sf.Alignment = (StringAlignment)random.Next(0, 3);//不取上界,即只有 0 - 2 near center far
//            gp.AddString(IdentifyingCode, FontFamily, (int)f.Style, f.Size, rect, sf);
//            gp.Warp(pfs, rect);
//            ga.FillPath(brGString, gp);
//            ga.FillPath(brString, gp);
//            ga.Flush();
//            return im;
//        }

//        //private void AddNoise(Graphics ga) {

//        //    Pen pen = new Pen(SystemColors.GrayText);

//        //    Point[] ps = new Point[_noiseCount];
//        //    for (int i = 0; i < _noiseCount; i++) {
//        //        ps[i] = new Point(random.Next(1, Width - 1), random.Next(1, Height - 1));
//        //    }

//        //    ga.DrawLines(pen, ps);
//        //}

//        public void Create(string path) {
//            //coder.Start(path);用它的这个方法,比用 stream 生成的要大!
//            FileStream fs = new FileStream(path, FileMode.Create);
//            coder.Start(fs);
//            Process(fs);
//            fs.Close();
//        }
//    }
//}