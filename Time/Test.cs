using NUnit.Framework;
using Tobii.Points;
using Tobii.SpaceTime;

namespace Tobii.Time
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void Test()
        {
            IPixelPositionEvent test = new PixelPositionEvent(new PixelPosition(5, 5), new Time(150));
            

            var ten = test.Position.Move(test.Position);

            Assert.That(ten.X, Is.EqualTo(10));
            Assert.That(ten.Y, Is.EqualTo(10));
        }
    }
}