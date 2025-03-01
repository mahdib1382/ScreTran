namespace ScreTran;

public interface ITranslationService
{
    /// <summary>
    /// Translate input to target language.
    /// </summary>
    /// <param name="input">Input text.</param>
    /// <param name="translator">Translator type.</param>
    /// <param name="targetLanguage">Target language code.</param>
    /// <returns>Translated text.</returns>
    string Translate(string input, Enumerations.Translator translator, string targetLanguage = "fa");
}
