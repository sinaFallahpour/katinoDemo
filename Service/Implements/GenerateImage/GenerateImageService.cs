

using Domain;
using Microsoft.AspNetCore.Http;
using Service.Interfaces.GenerateImage;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Service.Implements.GenerateImage
{
    public class GenerateImageService : IGenerateImageService
    {

        public string GetDefaultBase64Image(JobAdvertisement adver, string defaultImageAddress, Font font, Color textColor, Color backColor, int height, int width, string imagePath)
        {
            try
            {


                // get first text
                //text = text.Trim().ToUpper()[0].ToString();

                // get image
                Image img = GetDefaultImage2(adver, defaultImageAddress, font, textColor, backColor, height, width);


                // convert to image array
                //////////////var converter = new ImageConverter();
                //////////////byte[] imageArray = (byte[])converter.ConvertTo(img, typeof(byte[]));



                byte[] imageArray;
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, ImageFormat.Jpeg);
                    //img.Save(ms, img.RawFormat,imagef);
                    imageArray = ms.ToArray();
                }

                //using (MemoryStream ms = new MemoryStream())
                //{
                //    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                //    result.Content = new ByteArrayContent(ms.ToArray());
                //    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                //    return result;
                //}


                using (MemoryStream ms = new MemoryStream(imageArray))
                {
                    //MemoryStream ms = new MemoryStream(imageArray);
                    System.Drawing.Image img2 = System.Drawing.Image.FromStream(ms);
                    img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }



                // return base64image
                return @"data:image/jpg;base64," + Convert.ToBase64String(imageArray);
            }
            catch (Exception ex)
            {

                return "";

            }
        }

        public Image GetDefaultImage2(JobAdvertisement adver, string defaultImageAddress, Font font, Color textColor, Color backColor, int height, int width)
        {
            var title = adver.Title;
            var salary = adver.Salary.GetDisplayAttributeFrom().ToString();
            var degreeOfEducation = adver.DegreeOfEducation.GetDisplayAttributeFrom().ToString();
            var typeOfCooperation = adver.TypeOfCooperation.GetDisplayAttributeFrom().ToString();
            var phone = adver.Company?.PhoneNumber;


            // first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(defaultImageAddress, true);

            //Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            // measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(adver.Title, font);

            // free up the dummy image and old graphics object
            ////////img.Dispose();
            ////////drawing.Dispose();

            //////////// create a new image of the right size
            //////////img = new Bitmap(width, height);

            //////////drawing = Graphics.FromImage(img);

            //////////// paint the background
            //////////drawing.Clear(backColor);

            // create a brush for the text
            Brush brush = new SolidBrush(textColor);
            Brush BlackBrush = new SolidBrush(Color.Black);
            Brush OrangeBrush = new SolidBrush(Color.Orange);
            Brush RedBrush = new SolidBrush(Color.Red);

            // string alignment
            StringFormat stringFormat = new StringFormat();
            //stringFormat.LineAlignment = StringAlignment.Far;
            stringFormat.Alignment = StringAlignment.Far;

            //// rectangular
            //RectangleF rectangleF = new RectangleF(0, 0, img.Width, img.Height);

            //// draw text on image
            //drawing.DrawString(text, font, brush, rectangleF, stringFormat);




            var fontnew = new Font(FontFamily.GenericSansSerif, 54, FontStyle.Bold);


            StringFormat stringFormat2 = new StringFormat();
            stringFormat2.Alignment = StringAlignment.Center;




            /*       ************************ in miare vasat ************************      */
            // rectangular
            RectangleF rectangleF0 = new RectangleF(-290, 200,img.Width, img.Width);
            // draw text on image
            drawing.DrawString(title, fontnew, BlackBrush, rectangleF0, stringFormat);



            RectangleF rectangleF6 = new RectangleF(-350, 340, img.Width, img.Width);
            // draw text on image
            drawing.DrawString(adver.Id.ToString(), fontnew, BlackBrush, rectangleF6, stringFormat);
            
            RectangleF rectangleF7 = new RectangleF(-460, 480, img.Width, img.Width);
            // draw text on image
            drawing.DrawString(adver.Category.Name.ToString(), fontnew, BlackBrush, rectangleF7, stringFormat);

            //if (adver.IsImmediate)
            //{
            //    /*       ************************ in miare vasat ************************      */
            //    // rectangular
            //    RectangleF rectangleF15 = new RectangleF(0, 200, img.Width, img.Width);
            //    // draw text on image
            //    drawing.DrawString("فوری", fontnew, RedBrush, rectangleF15, stringFormat2);
            //}



            //rectangular
            RectangleF rectangleF1 = new RectangleF(-430, 625, img.Width, img.Width);
            // draw text on image
            drawing.DrawString(salary, fontnew, BlackBrush, rectangleF1, stringFormat);

            
            //rectangular
            RectangleF rectangleF8 = new RectangleF(-250, 760, img.Width, img.Width);
            // draw text on image
            drawing.DrawString(adver.City, fontnew, BlackBrush, rectangleF8, stringFormat);
            
            //rectangular
            RectangleF rectangleF9 = new RectangleF(-360, 900, img.Width, img.Width);
            // draw text on image
            drawing.DrawString(adver.Gender.GetDisplayAttributeFrom(), fontnew, BlackBrush, rectangleF9, stringFormat);


            //// rectangular
            //RectangleF rectangleF2 = new RectangleF(600, 490, img.Width, img.Height);
            //// draw text on image
            //drawing.DrawString(degreeOfEducation, font, brush, rectangleF2, stringFormat);

            // rectangular
            //RectangleF rectangleF3 = new RectangleF(500, 640, img.Width, img.Height);
            //// draw text on image
            //drawing.DrawString(typeOfCooperation, font, brush, rectangleF3, stringFormat);


            // rectangular
            RectangleF rectangleF4 = new RectangleF(440, 800, img.Width, img.Height);
            // draw text on image
            drawing.DrawString("01142038808", font, BlackBrush, rectangleF4, stringFormat);
            //drawing.DrawString(phone, font, BlackBrush, rectangleF4, stringFormat);



            // save drawing
            drawing.Save();

            // dispose
            brush.Dispose();
            drawing.Dispose();

            // return image
            return img;
        }


        public Image GetDefaultImage(JobAdvertisement adver, string defaultImageAddress, Font font, Color textColor, Color backColor, int height, int width)
        {

            //var str1 = $"عنوان : {adver.Title}ASD ";
            var str1 = $"عنوان: { adver.Title}";
            var str2 = $"نوع همکاری : { adver.AdverStatus.GetDisplayAttributeFrom().ToString()}";
            var str3 = $"حقوق : { adver.Salary.GetDisplayAttributeFrom().ToString()}";
            var str4 = $"سابقه کار : { adver.WorkExperience.GetDisplayAttributeFrom().ToString()}";
            var str5 = $"مدرک تحصیلی : { adver.DegreeOfEducation.GetDisplayAttributeFrom().ToString()}";
            var str6 = $"خدمت سربازی : { adver.Military.ToString()}";
            var str7 = $"نوع آگهی: { adver.AdverStatus.GetDisplayAttributeFrom()}";
            var str8 = $"وضعیت آگهی: { adver.AdverCreatationStatus.GetDisplayAttributeFrom()}";


            // first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            // measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(adver.Title, font);

            // free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            // create a new image of the right size
            img = new Bitmap(width, height);

            drawing = Graphics.FromImage(img);

            // paint the background
            drawing.Clear(backColor);

            // create a brush for the text
            Brush brush = new SolidBrush(textColor);

            // string alignment
            StringFormat stringFormat = new StringFormat();
            //stringFormat.LineAlignment = StringAlignment.Far;
            stringFormat.Alignment = StringAlignment.Near;

            //// rectangular
            //RectangleF rectangleF = new RectangleF(0, 0, img.Width, img.Height);

            //// draw text on image
            //drawing.DrawString(text, font, brush, rectangleF, stringFormat);



            // rectangular
            RectangleF rectangleF1 = new RectangleF(200, 50, img.Width, img.Width);
            // draw text on image
            drawing.DrawString(str1, font, brush, rectangleF1, stringFormat);


            // rectangular
            RectangleF rectangleF2 = new RectangleF(200, 120, img.Width, img.Height);
            // draw text on image
            drawing.DrawString(str2, font, brush, rectangleF2, stringFormat);

            // rectangular
            RectangleF rectangleF3 = new RectangleF(200, 190, img.Width, img.Height);
            // draw text on image
            drawing.DrawString(str3, font, brush, rectangleF3, stringFormat);


            // rectangular
            RectangleF rectangleF4 = new RectangleF(200, 260, img.Width, img.Height);
            // draw text on image
            drawing.DrawString(str4, font, brush, rectangleF4, stringFormat);



            // rectangular
            RectangleF rectangleF5 = new RectangleF(200, 330, img.Width, img.Height);
            // draw text on image
            drawing.DrawString(str5, font, brush, rectangleF5, stringFormat);

            // rectangular
            RectangleF rectangleF6 = new RectangleF(200, 400, img.Width, img.Height);
            // draw text on image
            drawing.DrawString(str6, font, brush, rectangleF6, stringFormat);


            // rectangular325
            RectangleF rectangleF7 = new RectangleF(200, 470, img.Width, img.Height);
            // draw text on image
            drawing.DrawString(str7, font, brush, rectangleF7, stringFormat);

            // rectangular
            RectangleF rectangleF8 = new RectangleF(200, 550, img.Width, img.Height);
            // draw text on image
            drawing.DrawString(str8, font, brush, rectangleF8, stringFormat);




            //// rectangular
            //RectangleF rectangleF2 = new RectangleF(470, 30, img.Width, img.Width);
            //// draw text on image
            //drawing.DrawString(str1, font, brush, rectangleF2);


            //// rectangular
            //RectangleF rectangleF3 = new RectangleF(60, 30, 200, 200);
            //// draw text on image
            //drawing.DrawString(str2, font, brush, rectangleF3);













            //// rectangular
            //RectangleF rectangleF4 = new RectangleF(470, 150, img.Width, img.Height);
            //// draw text on image
            //drawing.DrawString(str1, font, brush, rectangleF4);


            //// rectangular
            //RectangleF rectangleF5 = new RectangleF(60, 150, img.Width, img.Height);
            //// draw text on image
            //drawing.DrawString(str1, font, brush, rectangleF5);



            //// rectangular
            //RectangleF rectangleF6 = new RectangleF(470, 250, 200, 200);
            //// draw text on image
            //drawing.DrawString(str1, font, brush, rectangleF6);


            //// rectangular
            //RectangleF rectangleF7 = new RectangleF(60, 250, 100, 100);
            //// draw text on image
            //drawing.DrawString(str1, font, brush, rectangleF7);




            //////////// rectangular
            //////////RectangleF rectangleF3 = new RectangleF(100, 100, img.Width, img.Height);
            //////////// draw text on image
            //////////drawing.DrawString(str2, font, brush, rectangleF3, stringFormat);


            //////////// rectangular
            //////////RectangleF rectangleF4 = new RectangleF(300, 300, img.Width, img.Height);
            //////////// draw text on image
            //////////drawing.DrawString(str3, font, brush, rectangleF4, stringFormat);

            //////////// rectangular
            //////////RectangleF rectangleF5 = new RectangleF(300, 300, img.Width, img.Height);
            //////////// draw text on image
            //////////drawing.DrawString(str4, font, brush, rectangleF5, stringFormat);

            //////////// rectangular
            //////////RectangleF rectangleF6 = new RectangleF(300, 300, img.Width, img.Height);
            //////////// draw text on image
            //////////drawing.DrawString(str5, font, brush, rectangleF6, stringFormat);



            // save drawing
            drawing.Save();

            // dispose
            brush.Dispose();
            drawing.Dispose();

            // return image
            return img;
        }

    }




    public static class EnumExtentions
    {
        public static string GetDisplayAttributeFrom(this Enum enumValue)
        {
            MemberInfo info = enumValue
                .GetType()
                .GetMember(enumValue.ToString())
                .First();
            if (info != null && info.CustomAttributes.Any())
            {
                DisplayAttribute nameAttr = info.GetCustomAttribute<DisplayAttribute>();
                return nameAttr != null ? nameAttr.Name : enumValue.ToString();
            }
            return enumValue.ToString();
        }
    }
}
