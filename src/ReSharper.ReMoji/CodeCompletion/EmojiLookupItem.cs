using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems.Impl;
using JetBrains.UI.Icons;
using ReSharper.ReMoji.Icons;

namespace ReSharper.ReMoji.CodeCompletion
{
  internal sealed class EmojiLookupItem : TextLookupItemBase
  {
    public override IconId Image { get; }

    public EmojiLookupItem([NotNull] Emoji.Emoji emoji) : base(true)
    {
      Text = $":{emoji.Name}:";
      Image = new EmojiIconId(emoji);
    }
  }
}