using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Shared.Extensions
{
    public static class ConvertToImageBase64Extension
    {
      public  static string ConvertImageToBase64(this string value)
        {
            // Read image file as bytes
            byte[] imageBytes = File.ReadAllBytes(value);

            // Convert bytes to Base64 string
            string base64Image = Convert.ToBase64String(imageBytes);

            return base64Image;
        }
    }
}
