using OpenAI_API.Embedding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAdjust.Services;

public static class Embeddings
{
    public static async Task<float[]> EmbedText(string text)
    {
        var result = await LLMServices.API.Embeddings.GetEmbeddingsAsync(text);
        return result;
    }

}
