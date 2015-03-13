using System;
using xpf.Http.Original;

namespace xpf.Http.Extensions
{
    public static class ShrinkTheWeb
    {
        public static string GetThumbnailUrl(this Url uri, ThumbnailSize size = ThumbnailSize.Large200x150)
        {
            string sizeCode = "";
            switch (size)
            {
                case ThumbnailSize.ExtraLarge320x240:
                    sizeCode = "xlg";
                    break;
                case ThumbnailSize.Large200x150:
                    sizeCode = "lg";
                    break;
                case ThumbnailSize.Micro75x56:
                    sizeCode = "mcr";
                    break;
                case ThumbnailSize.Small120x90:
                    sizeCode = "sm";
                    break;
                case ThumbnailSize.Tiny90x68:
                    sizeCode = "tny";
                    break;
                case ThumbnailSize.VerySmall100x75:
                    sizeCode = "vsm";
                    break;
            }
            return string.Format("http://images.shrinktheweb.com/xino.php?stwembed=1&stwaccesskeyid=3ab2d75e4b4621d&stwhash=22817fa5af&stwinside=1&stwsize={1}&stwurl={0}",
                uri.Model.Url, sizeCode);
            //
        }

    }
}
