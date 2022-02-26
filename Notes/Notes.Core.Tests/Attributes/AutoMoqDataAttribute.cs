using AutoFixture;
using AutoFixture.Xunit2;
using Notes.Core.Tests.Customizations;

namespace Notes.Core.Tests.Attributes
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() => new Fixture().Customize(new DomainCustomization()))
        { }
    }
}
