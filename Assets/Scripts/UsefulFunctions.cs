using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace LolilopGamesLibrary
{
    public class UsefulFunctions : MonoBehaviour
    {
        // Определяет максимальную длину строки в поле text 
        public static int GetMaxLineCount(Text text)
        {
            var textGenerator = new TextGenerator();
            var generationSettings = text.GetGenerationSettings(text.rectTransform.rect.size);
            var lineCount = 0;

            var s = new StringBuilder();
            while (true)
            {
                s.Append("\n");
                textGenerator.Populate(s.ToString(), generationSettings);
                var nextLineCount = textGenerator.lineCount;
                if (lineCount == nextLineCount) break;
                lineCount = nextLineCount;
            }
            return lineCount;
        }

        // Вычисляет максимальное количество символов в текстовом поле text для текста s с учетом пробелов и знаков препинания
        public static int GetMaxSymbolsCount(Text text, string s)
        {
            var textGenerator = new TextGenerator();
            var generationSettings = text.GetGenerationSettings(text.rectTransform.rect.size);

            textGenerator.Populate(s, generationSettings);            

            return textGenerator.characterCountVisible;
        }

        // Удаляет всех детей у переданного Transform'а
        public static void DestroyAllChild(Transform trans)
        {
            for (int i = trans.childCount - 1; i >= 0; i--)
            {
                Destroy(trans.GetChild(i).gameObject);
            }
        }

        public static void DestroyAllChildEditor(Transform trans)
        {
            for (int i = trans.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(trans.GetChild(i).gameObject);
            }
        }
    }
}