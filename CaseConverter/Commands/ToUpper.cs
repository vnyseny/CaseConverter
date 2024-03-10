using EnvDTE;
using Microsoft.VisualStudio.VCProjectEngine;

namespace CaseConverter;

[Command(PackageIds.ToUpper)]
internal sealed class ToUpper
    : BaseCommand<ToUpper>
{
    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
    {
        await TextConvertHelper.ConvertAsync(TextCase.ToUpperCase);
    }
}

[Command(PackageIds.ToLower)]
internal sealed class ToLower
    : BaseCommand<ToLower>
{
    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
    {
        await TextConvertHelper.ConvertAsync(TextCase.ToLowerCase);
    }
}

[Command(PackageIds.ToCamel)]
internal sealed class ToCamel
    : BaseCommand<ToCamel>
{
    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
    {
        await TextConvertHelper.ConvertAsync(TextCase.ToCammelCase);
    }
}

[Command(PackageIds.ToPascal)]
internal sealed class ToPascal
    : BaseCommand<ToPascal>
{
    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
    {
        await TextConvertHelper.ConvertAsync(TextCase.ToPascelCase);
    }
}

internal static class TextConvertHelper
{
    public static async Task ConvertAsync(TextCase textCase)
    {
        var documnent = await VS.Documents.GetActiveDocumentViewAsync();
        if (documnent is not null)
        {
            var spans = documnent.TextView.Selection.SelectedSpans;
            using (var edit = documnent.TextBuffer.CreateEdit())
            {
                foreach (var span in spans)
                {
                    edit.Replace(span, ConvertText(span.GetText(), textCase));
                }
                edit.Apply();

            }
        }
    }

    private static string ConvertText(string text, TextCase textCase)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;
        return textCase switch
        {
            TextCase.ToUpperCase => text.ToUpper(),
            TextCase.ToLowerCase => text.ToLower(),
            TextCase.ToCammelCase => $"{char.ToLower(text[0])}{(text.Length > 1 ? text.Substring(1) : string.Empty)}",
            TextCase.ToPascelCase => $"{char.ToUpper(text[0])}{(text.Length > 1 ? text.Substring(1) : string.Empty)}",
            _ => text
        };
    }
}

public enum TextCase
{
    ToUpperCase,
    ToLowerCase,
    ToCammelCase,
    ToPascelCase,
    ToSnakeCase,
    ToKebabCase
}