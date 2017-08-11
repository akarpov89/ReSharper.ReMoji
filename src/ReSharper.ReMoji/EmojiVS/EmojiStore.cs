using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JetBrains.Util.Collections;
using SimpleJson;
using Json = SimpleJson.SimpleJson;

namespace Emoji
{
  internal class EmojiStore
  {
    [NotNull] private readonly string myStoreDirectory;
    [NotNull] private readonly Downloader myDownloader;
    [NotNull] private readonly HashMap<string, Emoji> myEmojis = new HashMap<string, Emoji>();

    public EmojiStore()
    {
      myStoreDirectory = StoreDirectory();
      myDownloader = new Downloader();

      if (!Directory.Exists(myStoreDirectory))
        Directory.CreateDirectory(myStoreDirectory);

      InitializeStore();
    }

    private void InitializeStore()
    {
      Task.Run(async () =>
      {
        try
        {
          var json = await myDownloader.DownloadEmojiListAsync();
          InitializeFromJson(json);
          await WriteLocalJsonAsync(json);
        }
        catch
        {
          InitializeFromJson(await ReadLocalJsonAsync());
        }
      });
    }

    private async Task WriteLocalJsonAsync([NotNull] string json)
    {
      var localJsonFile = Path.Combine(myStoreDirectory, "emojis.json");
      var fileMode = File.Exists(localJsonFile) ? FileMode.Truncate : FileMode.OpenOrCreate;

      using (var file = new FileStream(localJsonFile, fileMode, FileAccess.Write))
      using (var writer = new StreamWriter(file) {AutoFlush = true})
      {
        await writer.WriteAsync(json);
      }
    }

    private async Task<string> ReadLocalJsonAsync()
    {
      var localJsonFile = Path.Combine(myStoreDirectory, "emojis.json");

      using (var file = File.OpenText(localJsonFile))
      {
        return await file.ReadToEndAsync();
      }
    }

    private void InitializeFromJson(string json)
    {
      foreach (var pair in Json.DeserializeObject<JsonObject>(json))
      {
        var name = pair.Key;
        var uri = new Uri((string) pair.Value);

        var emoji = new Emoji(name, uri, EmojiFileName(uri));
        if (!emoji.IsRetrieved)
          DownloadEmoji(emoji);

        myEmojis.Add(name, emoji);
      }
    }

    private void DownloadEmoji(Emoji emoji)
    {
      Task.Run(async () => await myDownloader.DownloadEmojiAsync(emoji));
    }

    [NotNull, Pure]
    private string EmojiFileName([NotNull] Uri uri)
    {
      return Path.Combine(myStoreDirectory, Path.GetFileName(uri.LocalPath));
    }

    [NotNull, MustUseReturnValue]
    private static string StoreDirectory()
    {
      return Path.Combine(Path.GetTempPath(), "EmojiStore");
    }

    public IEnumerable<Emoji> Emojis()
    {
      return myEmojis.Values;
    }
  }
}