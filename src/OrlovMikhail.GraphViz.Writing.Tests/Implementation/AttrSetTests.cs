using Xunit;

namespace OrlovMikhail.GraphViz.Writing.Tests
{
    public class AttrSetTests
    {
        [Fact]
        public void GivenAttributeWithNullValue_ThenDoesntEnumerateIt()
        {
            UrlAttribute a1 = new UrlAttribute("abc");
            LabelAttribute a2 = new LabelAttribute("xyz");

            IAttrSet set = AttrSet.Empty.Add(a1).Add(a2);

            Assert.Equal(new IAttribute[] {a1, a2}, set);

            UrlAttribute a3 = new UrlAttribute(null);

            set = set.Add(a3);

            Assert.Equal(new IAttribute[] {a2}, set);
        }
    }
}