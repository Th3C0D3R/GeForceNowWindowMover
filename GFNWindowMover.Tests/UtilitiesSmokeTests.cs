using GFNWindowMover.Utilities;

namespace GFNWindowMover.Tests;

public sealed class UtilitiesSmokeTests
{
	private static void ResetArgsState()
	{
		Utils.NoFixedWindow = false;
		Utils.ProcessName = string.Empty;
		Utils.PreDefHeight = 720;
		Utils.PreDefWidth = 1280;
		Utils.ResizeOnly = false;
	}

	[Fact]
	public void ArgHelper_ParsesKnownFlags()
	{
		ResetArgsState();

		object result = Utils.ArgHelper(["--nofixed", "--process", "GeForceNOW", "--height", "1080", "--width", "1920", "--resizeOnly"]);

		Assert.True(Utils.TestObject(result, true));
		Assert.True(Utils.NoFixedWindow);
		Assert.True(Utils.ResizeOnly);
		Assert.Equal("GeForceNOW", Utils.ProcessName);
		Assert.Equal(1080, Utils.PreDefHeight);
		Assert.Equal(1920, Utils.PreDefWidth);
	}

	[Fact]
	public void ArgHelper_ReturnsFalseForHelp()
	{
		ResetArgsState();

		object result = Utils.ArgHelper(["--help"]);

		Assert.True(Utils.TestObject(result, false));
	}

	[Fact]
	public void ArgHelper_IgnoresInvalidMissingValue()
	{
		ResetArgsState();
		Utils.ProcessName = "ExistingProcess";

		object result = Utils.ArgHelper(["--process"]);

		Assert.True(Utils.TestObject(result, false));
		Assert.Equal("ExistingProcess", Utils.ProcessName);
	}
}
