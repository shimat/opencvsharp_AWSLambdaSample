using System;
using Amazon.Lambda.Core;
using OpenCvSharp;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambda1
{
    public class Function
    {
        /// <summary>
        /// A simple function that takes a image data and returns a binarized image data.
        /// </summary>
        /// <param name="imageBase64"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(string imageBase64, ILambdaContext context)
        {
            var imageBytes = Convert.FromBase64String(imageBase64);

            using var src = Mat.FromImageData(imageBytes, ImreadModes.Grayscale);
            using var dst = Threshold(src);

            var dstImageBytes = dst.ImEncode(".png");
            var dstImageBase64 = Convert.ToBase64String(dstImageBytes);
            return dstImageBase64;
        }

        private static Mat Threshold(Mat src)
        {
            Mat dst = new ();
            Cv2.Threshold(src, dst, 0, 255, ThresholdTypes.Otsu);
            return dst;
        }
    }
}
