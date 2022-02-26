using AutoFixture;
using AutoFixture.AutoMoq;

namespace Notes.Core.Tests.Customizations
{
    public class DomainCustomization : CompositeCustomization
    {
        public DomainCustomization() : base(new AutoMoqCustomization(), new RecursionBehaviourCustomization())
        { }
    }
}
