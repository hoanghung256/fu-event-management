using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Common.FaceRecognization
{
    public class FacePreprocessor
    {
        public static Mat CropAndResize(Mat inputImage)
        {
            // Chuyển xám
            var gray = new Mat();
            Cv2.CvtColor(inputImage, gray, ColorConversionCodes.BGR2GRAY);

            // Load cascade face detect
            var cascade = new CascadeClassifier(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "models", "haarcascade_frontalface_default.xml"));
            var faces = cascade.DetectMultiScale(gray, 1.1, 3);

            if (faces.Length == 0)
                throw new Exception("No face detected!");

            var faceRect = faces[0];
            var face = new Mat(inputImage, faceRect);

            // Resize
            Mat resizedFace = new Mat();
            Cv2.Resize(face, resizedFace, new OpenCvSharp.Size(112, 112));

            return resizedFace;
        }

        public static float[] Normalize(Mat image)
        {
            Cv2.CvtColor(image, image, ColorConversionCodes.BGR2RGB);

            int channels = 3, height = image.Rows, width = image.Cols;
            float[] result = new float[channels * height * width];
            int idx = 0;

            for (int c = 0; c < channels; c++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Vec3b pixel = image.At<Vec3b>(y, x);
                        float value = pixel[c] / 255.0f;
                        value = (value - 0.5f) / 0.5f; // [-1,1]
                        result[idx++] = value;
                    }
                }
            }

            return result;
        }
    }
}
