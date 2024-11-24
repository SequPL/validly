using System.Text;

namespace Validly.SourceGenerator.Utils.SourceTexts.FileBuilders;

public class SourceTextSectionBuilder
{
	private readonly List<StringBuilder> _lines = new() { new StringBuilder() };

	private StringBuilder CurrentLine => _lines[_lines.Count - 1];

	public List<StringBuilder> Lines => _lines;

	public SourceTextSectionBuilder Append(string text)
	{
		CurrentLine.Append(text);
		return this;
	}

	public SourceTextSectionBuilder AppendIf(string text, bool condition)
	{
		return condition ? Append(text) : this;
	}

	public SourceTextSectionBuilder AppendLine(string text)
	{
		CurrentLine.Append(text);
		return AppendLine();
	}

	public SourceTextSectionBuilder AppendLineIf(string text, bool condition)
	{
		return condition ? AppendLine(text) : this;
	}

	public SourceTextSectionBuilder AppendLine()
	{
		_lines.Add(new StringBuilder());
		return this;
	}

	public SourceTextSectionBuilder Indent(int count = 1)
	{
		foreach (StringBuilder line in _lines)
		{
			line.Insert(0, new string('\t', count));
		}

		return this;
	}

	public override string ToString() => string.Join(Environment.NewLine, _lines.Select(x => x.ToString()));
}
