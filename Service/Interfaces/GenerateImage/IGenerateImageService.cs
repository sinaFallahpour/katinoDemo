using Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Service.Interfaces.GenerateImage
{
    public interface IGenerateImageService
    {
        //Task<(bool isSuccess, List<string> errors)> SendEmail(string to, string subject, string content);
        string GetDefaultBase64Image(JobAdvertisement adver, string defaultImageAddress, Font font, Color textColor, Color backColor, int height, int width, string imagePath);
        Image GetDefaultImage(JobAdvertisement adver, string defaultImageAddress, Font font, Color textColor, Color backColor, int height, int width);
    }
}
