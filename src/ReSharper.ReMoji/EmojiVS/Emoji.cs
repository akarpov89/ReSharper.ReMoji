using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JetBrains.Annotations;

namespace Emoji
{
  internal class Emoji
  {
    [NotNull] public string Name { get; }
    [NotNull] public string FileName { get; }
    [NotNull] public Uri Uri { get; }

    public bool IsRetrieved => File.Exists(FileName);

    [CanBeNull] private BitmapImage myBitmap;

    public Emoji([NotNull] string name, [NotNull] Uri uri, [NotNull] string fileName)
    {
      Name = name;
      Uri = uri;
      FileName = fileName;
    }

    [NotNull, MustUseReturnValue]
    public ImageSource Bitmap()
    {
      if (myBitmap == null)
      {
        myBitmap = new BitmapImage();
        myBitmap.BeginInit();
        myBitmap.DecodePixelHeight = 16;
        myBitmap.DecodePixelWidth = 16;
        myBitmap.UriSource = new Uri(FileName, UriKind.Absolute);
        myBitmap.EndInit();
        myBitmap.Freeze();
      }

      return myBitmap;
    }

    public override string ToString() => Name;
  }
}