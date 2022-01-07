using TechCommunityCalendar.Concretions;
using Xunit;

namespace TechCommunityCalendar.UnitTests
{
    public class GeneralTests
    {
        [Fact]
        public void WhenEventNameHasSpaces_SpacesAreRemovedFromBranchName()
        {
            // Arrange
            var originalEventName = "Some event with spaces";
            var expectedEventName = "some-event-with-spaces";

            // Act
            var actualEventName = TechEventCleaner.MakeFriendlyBranchName(originalEventName);

            // Assert
            Assert.Equal(expectedEventName, actualEventName);
        }

        [Fact]
        public void WhenEventNameHasInvalidCharacters_InvalidCharactersAreRemovedFromBranchName()
        {
            // Arrange
            var originalEventName = "Some / ~event  ;with, ^invalid-characters. ";
            var expectedEventName = "some-event-with-invalid-characters";

            // Act
            var actualEventName = TechEventCleaner.MakeFriendlyBranchName(originalEventName);

            // Assert
            Assert.Equal(expectedEventName, actualEventName);
        }
    }
}
