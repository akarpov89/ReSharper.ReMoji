using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Emoji
{
  internal class Downloader
  {
    private readonly HttpClient myClient = new HttpClient();

    public async Task<string> DownloadEmojiListAsync()
    {
      var request = new HttpRequestMessage
      {
        RequestUri = new Uri("https://api.github.com/emojis"),
        Headers =
        {
          {"Connection", "Keep-Alive"},
          {"Accept", "application/vnd.github.v3+json"},
          {"User-Agent", "EmojiVS"}
        }
      };

      var response = await myClient.SendAsync(request);
      return await response.Content.ReadAsStringAsync();
    }

    public async Task DownloadEmojiAsync([NotNull] Emoji emoji)
    {
      var request = new HttpRequestMessage
      {
        RequestUri = emoji.Uri,
        Headers =
        {
          {"Connection", "Keep-Alive"},
          {"User-Agent", "EmojiVS"},
        },
      };

      var response = await myClient.SendAsync(request);
      var stream = await response.Content.ReadAsStreamAsync();

      using (var file = File.OpenWrite(emoji.FileName))
      {
        await stream.CopyToAsync(file);
      }
    }
  }
}