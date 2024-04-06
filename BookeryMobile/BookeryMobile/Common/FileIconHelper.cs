using System;
using BookeryMobile.Data.DTOs.Node.Output;
using Xamarin.Forms;

namespace BookeryMobile.Common
{
    internal static class FileIconHelper
    {
        public static ImageSource GetImageSource(NodeDto node)
        {
            ImageSource imageSource;

            if (node.IsDirectory)
            {
                imageSource = ImageSource.FromFile("folder.png");
            }
            else
            {
                var fileExtension = node.Name.Substring(node.Name.LastIndexOf(".", StringComparison.Ordinal) + 1);
                // if (Enum.TryParse<FileExtension>(fileExtension, true, out var res))
                // {
                //     imageSource = ImageSource.FromFile($"{fileExtension}.png");
                // }
                // else
                // {
                //     imageSource = ImageSource.FromFile("unknown.png");
                // }
                imageSource = ImageSource.FromFile("document.png");
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