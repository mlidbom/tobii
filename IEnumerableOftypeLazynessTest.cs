using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

namespace Tobii
{
    interface IMediaEvent{}
    interface IFixation : IMediaEvent{}
    interface IMouseClick : IMediaEvent{}

    [TestFixture]
    public class IEnumerableOftypeLazynessTest
    {
        private bool HasEnumeratedFixations { get; set; }
        IEnumerable<IFixation> _Fixations
        {
            get
            {
                for (int i = 0; i < 5; i++)
                {
                    HasEnumeratedFixations = true;
                    yield return null;
                }
            }
        }

        IEnumerable<IFixation> Fixations
        {
            get
            {
                return _Fixations;
            }
        }

        IEnumerable<IMediaEvent> MediaEvents
        {
            get
            {
                HasEnumeratedFixations = true;
                yield return null;
            }
        }


        [Test]
        public void ShouldNotEnumerateEnumeratorOfWrongType()
        {
            //var onlyClicks = MediaEvents.OfType<IMouseClick>().ToList();
            //Assert.IsFalse(HasEnumeratedFixations);
        }
    }
}