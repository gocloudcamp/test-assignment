using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaylistClass;

namespace UnitTestProject
{
    [TestClass]
    public class ModuleInterface
    {
        public void ContaintInList(SetMusic<Music> set, Music music, bool expectedResult)
        {
            var result = set.Contains(music);
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        public void UnitTest1()
        {
            ContaintInList(new SetMusic<Music>() { }, new Music("C", "ds"), false);
        }
    }
}
