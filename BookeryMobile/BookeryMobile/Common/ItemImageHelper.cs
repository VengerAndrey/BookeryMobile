using System;
using Domain.Models;
using Xamarin.Forms;

namespace BookeryMobile.Common
{
    internal class ItemImageHelper
    {
        public static ImageSource GetImageSource(Item item)
        {
            ImageSource imageSource;

            if (item.IsDirectory)
            {
                imageSource = ImageSource.FromFile("folder.png");
            }
            else
            {
                var fileExtension = item.Path.Substring(item.Path.LastIndexOf(".") + 1);
                if (Enum.TryParse<FileExtension>(fileExtension, true, out var res))
                {
                    imageSource = ImageSource.FromFile($"{fileExtension}.png");
                }
                else
                {
                    imageSource = ImageSource.FromFile("unknown.png");
                }
            }

            return imageSource;
        }

        private enum FileExtension
        {
            Doc,
            Docx,
            Mp3,
            Pdf,
            Png,
            Ppt,
            Pptx,
            Txt,
            Xls,
            Xlsx,
            Zip
        }
    }
}