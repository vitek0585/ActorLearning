using System.Collections.Generic;
using AkkaTextAnalizatorCommon.Messages;

namespace AkkaTextAnalizatorCommon.Utils
{
    public class TextMessageStorage
    {
        public IEnumerable<TextMessage> Get(int total)
        {
            for (int i = 0; i < total; i++)
            {
                yield return new TextMessage(i.ToString(), i);
            }
        }
    }
}