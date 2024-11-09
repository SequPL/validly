using System.Text;

namespace Valigator.SourceGenerator.Utils.FileBuilders;

public class FilePart
{
	private readonly List<StringBuilder> _lines = [new StringBuilder()];

	private StringBuilder CurrentLine => _lines[_lines.Count - 1];

	public List<StringBuilder> Lines => _lines;

	public FilePart Append(string text)
	{
		CurrentLine.Append(text);
		return this;
	}

	public FilePart AppendIf(string text, bool condition)
	{
		return condition ? Append(text) : this;
	}

	public FilePart AppendLine(string text)
	{
		CurrentLine.Append(text);
		return AppendLine();
	}

	public FilePart AppendLineIf(string text, bool condition)
	{
		return condition ? AppendLine(text) : this;
	}

	public FilePart AppendLine()
	{
		_lines.Add(new StringBuilder());
		return this;
	}

	public FilePart Indent(int count = 1)
	{
		foreach (StringBuilder line in _lines)
		{
			line.Insert(0, new string('\t', count));
		}

		return this;
	}

	public override string ToString() => string.Join(Environment.NewLine, _lines.Select(x => x.ToString()));
}
