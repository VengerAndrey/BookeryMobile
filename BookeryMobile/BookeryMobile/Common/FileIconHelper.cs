using System;
using Domain.Models;
using Xamarin.Forms;

namespace BookeryMobile.Common
{
    internal static class FileIconHelper
    {
        public static ImageSource GetImageSource(Node node)
        {
            ImageSource imageSource;

            if (node.IsDirectory)
            {
                imageSource = ImageSource.FromFile("folder.png");
            }
            else
            {
                var fileExtension = node.Name.Substring(node.Name.LastIndexOf(".") + 1);
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