using System;
using System.Globalization;

namespace ChatGPTHistory
{
    /// <summary>
    /// Small class to estimate token counts. Will be used in the future when uploading summaries to retain session history.
    /// </summary>
    public class TokenEstimator
    {
        public static int EstimateTokenCount(string prompt)
        {
            /*
             Based upon:
             Here are some helpful rules of thumb for understanding tokens in terms of lengths:
                1 token ~= 4 chars in English
                1 token ~= ¾ words
                100 tokens ~= 75 words
                    Or 
                1-2 sentence ~= 30 tokens
                1 paragraph ~= 100 tokens
                1,500 words ~= 2048 tokens
             */
            int tokenCount = 0;
            tokenCount = (int)Math.Ceiling(prompt.Length / 4.0);
            return tokenCount;
        }
    }

}