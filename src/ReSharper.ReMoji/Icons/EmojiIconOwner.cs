using System;
using System.Windows.Media;
using JetBrains.Application;
using JetBrains.Application.Icons;
using JetBrains.Application.UI.Icons;
using JetBrains.UI.Icons;
using JetBrains.Util;

namespace ReSharper.ReMoji.Icons
{
  [ShellComponent]
  internal class EmojiIconOwner : IIconIdOwner
  {
    public ImageSource TryGetImage(IconId iconId, IconTheme theme, IThemedIconManagerPerThemeCache cache, OnError onerror)
    {
      try
      {
        var emojiIconId = iconId as EmojiIconId;
        var image = emojiIconId?.Emoji.Bitmap();

        return image;
      }
      catch(Exception ex)
      {
        onerror.Handle(ex);
        return null;
      }
    }

    public Type IconIdType => typeof(EmojiIconId);
  }
}