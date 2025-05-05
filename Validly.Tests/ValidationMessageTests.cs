namespace Validly.Tests;

public class ValidationMessageTests
{
	[Theory]
	[MemberData(nameof(ArgsJsonData))]
	public void ArgsJson_Test(ValidationMessage message, string expected)
	{
		Assert.Equal(message.ArgsJson, expected);
	}

	public static TheoryData<ValidationMessage, string> ArgsJsonData = new()
	{
		{
			new ValidationMessage("Test error {0} {1}", "message.key", null, null),
			"null, null"
		},
		{
			new ValidationMessage("Test error {0}", "message.key", 1.1m),
			"1.1"
		},
		{
			new ValidationMessage("Test error {0}", "message.key", 1),
			"1"
		},
		{
			new ValidationMessage("Test error {0}", "message.key", "string"),
			"\"string\""
		},
		{
			new ValidationMessage("Test error {0} {1} {2} {3}", "message.key", 1.1m, 1, "string", null),
			"1.1, 1, \"string\", null"
		},
	};
}