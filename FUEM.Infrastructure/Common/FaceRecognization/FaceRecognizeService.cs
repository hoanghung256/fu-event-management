using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUEM.Domain.Entities;
using System.Text.Json;

namespace FUEM.Infrastructure.Common.FaceRecognization
{
    public class FaceRecognizeService
    {
        private readonly InferenceSession _session;

        public FaceRecognizeService()
        {
            try
            {
                var modelPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "models", "arc_face_r50.onnx");
                Console.WriteLine($"MODEL PATH: {modelPath}");

                if (!System.IO.File.Exists(modelPath))
                    throw new FileNotFoundException($"ONNX model not found at: {modelPath}");

                _session = new InferenceSession(modelPath);
                Console.WriteLine("=== ONNX INPUT ===");
                foreach (var input in _session.InputMetadata)
                    Console.WriteLine($"Name: {input.Key}");

                Console.WriteLine("=== ONNX OUTPUT ===");
                foreach (var output in _session.OutputMetadata)
                    Console.WriteLine($"Name: {output.Key}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to initialize ONNX model: " + ex.Message);
                throw;
            }
        }

        public float[] ExtractEmbedding(float[] inputData)
        {
            var inputTensor = new DenseTensor<float>(inputData, new[] { 1, 3, 112, 112 });
            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("input.1", inputTensor)
            };

            using var results = _session.Run(inputs, new[] { "683" });
            return results.First().AsEnumerable<float>().ToArray();
        }

        public bool VerifyEmbedding(List<FaceEmbedding> storedFaceEmbeddings, float[] inputEmb)
        {
            foreach (var storedEmbedding in storedFaceEmbeddings)
            {
                var storedEmb = JsonSerializer.Deserialize<float[]>(storedEmbedding.EmbeddingJson);
                double similarity = CosineSimilarity(inputEmb, storedEmb);
                if (similarity >= 0.6)
                    return true;
            }
            return false;
        }

        private double CosineSimilarity(float[] v1, float[] v2)
        {
            double dot = 0.0, mag1 = 0.0, mag2 = 0.0;

            for (int i = 0; i < v1.Length; i++)
            {
                dot += v1[i] * v2[i];
                mag1 += Math.Pow(v1[i], 2);
                mag2 += Math.Pow(v2[i], 2);
            }

            if (mag1 == 0 || mag2 == 0) return 0.0;

            return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }
    }
}
