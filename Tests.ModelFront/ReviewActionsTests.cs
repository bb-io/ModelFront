using Tests.ModelFront.Base;
using Apps.ModelFront.Actions;
using Apps.ModelFront.Models.Request.Review;
using Blackbird.Applications.Sdk.Common.Files;
using Newtonsoft.Json;

namespace Tests.ModelFront;

[TestClass]
public class ReviewActionsTests : TestBase
{
	[TestMethod]
    public async Task EstimateQuality_ReturnsScoreResponse()
    {
		// Arrange
		var actions = new ReviewActions(InvocationContext, FileManager);
		var request = new ScoreRequest { File = new FileReference { Name = "v22-sample (1).xlf" } };

		// Act
		var result = await actions.EstimateQuality(request);

        // Assert
        Console.WriteLine(JsonConvert.SerializeObject(result));
		Assert.IsNotNull(result);
	}
}
