using UnityEngine;

namespace Utils
{
    public static class Logger
    {
        public static void Log(string tag, Color color, string message)
        {
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>[{tag}] {message}</color>");
        }
    }
}