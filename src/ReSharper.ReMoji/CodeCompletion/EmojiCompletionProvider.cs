using System.Collections.Generic;
using System.Linq;
using Emoji;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.CompletionInDocComments;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;

namespace ReSharper.ReMoji.CodeCompletion
{
  [Language(typeof(CSharpLanguage))]
  class EmojiCompletionProvider : ItemsProviderOfSpecificContext<ContextInDocComment>
  {
    [NotNull] private readonly IEnumerable<EmojiLookupItem> myLookupItems;

    public EmojiCompletionProvider()
    {
      var emojiStore = new EmojiStore();
      myLookupItems = emojiStore.Emojis().Select(e => new EmojiLookupItem(e));
    }

    protected override bool IsAvailable(ContextInDocComment context)
    {
      if (AutomaticCodeCompletionStrategyEx.MatchTokenType(
        null,
        context.BasicContext.File,
        context.BasicContext.TextControl,
        token => token == CSharpTokenType.END_OF_LINE_COMMENT))
        return true;

      return base.IsAvailable(context);
    }

    protected override bool AddLookupItems(ContextInDocComment context, IItemsCollector collector)
    {
      foreach (var lookupItem in myLookupItems)
      {
        lookupItem.InitializeRanges(context.TextLookupRanges, context.BasicContext);
        collector.Add(lookupItem);
      }

      return true;
    }
  }
}