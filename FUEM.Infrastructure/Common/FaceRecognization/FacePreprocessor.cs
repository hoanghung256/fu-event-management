using OpenCvSharp;
using SharpCompress.Common;
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
            // Convert to grayscale
            var gray = new Mat();
            Cv2.CvtColor(inputImage, gray, ColorConversionCodes.BGR2GRAY);

            // Construct full path to cascade file
            string path = Path.Combine(AppContext.BaseDirectory, "wwwroot", "models", "haarcascade_frontalface_default.xml");

            if (!File.Exists(path))
            {
                Console.WriteLine("❌ File does not exist: " + path);
                throw new FileNotFoundException($"Haarcascade XML file not found at: {path}");
            }

            Console.WriteLine("✅ File found at: " + path);

            // Optional: Try reading manually to verify
            try
            {
                var xmlContent = File.ReadAllText(path);
                Console.WriteLine("✅ File read OK.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Failed to read file: " + ex.Message);
            }

            // Now load the cascade
            CascadeClassifier cascade = null;
            try
            {
                cascade = new CascadeClassifier(path);
                Console.WriteLine("✅ Cascade loaded successfully.");
            }
            catch (Exception ex)
            {
                throw new Exception("❌ Failed to load CascadeClassifier: " + ex.Message);
            }

            var faces = cascade.DetectMultiScale(gray, 1.1, 3);

            if (faces.Length == 0)
            {
                Console.WriteLine("❌ No faces detected.");
                throw new Exception("No face detected!");
            }

            var faceRect = faces[0];
            var face = new Mat(inputImage, faceRect);

            // Resize face
            Mat resizedFace = new Mat();
            Cv2.Resize(face, resizedFace, new OpenCvSharp.Size(112, 112));

            Console.WriteLine("✅ Face cropped and resized successfully.");

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
