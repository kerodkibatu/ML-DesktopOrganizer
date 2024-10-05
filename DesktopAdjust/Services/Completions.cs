using OpenAI_API.Chat;

namespace DesktopAdjust.Services;
public static class Completions
{
    public static string GetDefaultSystemMessage()
    {
        var sm =
$"""
You are {LLMServices.AssistantName}. A very intelligent assistant that will respond to your queries with the utmost accuracy and precision.
""";
        return sm;
    }
    public static Conversation NewConversation(int maxTokens = 4096)
    {
        var conv = LLMServices.API.Chat.CreateConversation(
            new ChatRequest
            {
                MaxTokens = maxTokens,
            });
        return conv;
    }
    public static async Task<string> Query(string query, string imageBase64 = "", int maxTokens = -1)
    {
        var conv = NewConversation(maxTokens);
        conv.AppendUserInput(query, !string.IsNullOrEmpty(imageBase64) ? [new ChatMessage.ImageInput(Convert.FromBase64String(imageBase64))] : []);
        return await conv.GetResponseFromChatbotAsync();
    }
    // StreamQuery
    public static async Task StreamQuery(string query, Action<string> onMessage, string imageBase64 = "", int maxTokens = -1)
    {
        var conv = NewConversation(maxTokens);
        conv.AppendUserInput(query, !string.IsNullOrEmpty(imageBase64) ? [new ChatMessage.ImageInput(Convert.FromBase64String(imageBase64))] : []);
        await conv.StreamResponseFromChatbotAsync(onMessage);
    }
}
