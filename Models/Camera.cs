// WebCameraをBitmapで作成するクラス
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
namespace PrismCameraSample.Models
{
    public class Camera
    {
        #region フィールド変数
        VideoCapture capture = null;
        Mat frame = null;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタでカメラを起動しframeに渡す
        /// </summary>
        public Camera()
        {
            capture = new VideoCapture(0);
            if (!capture.IsOpened())
                throw new Exception("Camera Initialize failed");
            frame = new Mat();
        }
        #endregion

        #region publicメソッド
        /// <summary>
        /// ViewModelに渡すようにWritalbleBitmapでカメラ映像を渡す
        /// </summary>
        /// <returns></returns>
        public WriteableBitmap Capture()
        {
            capture.Read(frame);
            if (frame.Empty()) return null;
            return frame.ToWriteableBitmap();
        }
        #endregion

    }
}
