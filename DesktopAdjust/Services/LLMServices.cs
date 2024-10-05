using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAdjust.Services;
public static class LLMServices
{
    public static OpenAIAPI API = new OpenAIAPI
    {
        ApiUrlFormat = "http://127.0.0.1:1234/{0}/{1}",
        Auth = "Bearer YOUR"
    };
    public static string AssistantName => "Avocado";
}
