using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Void.Hierarchies;

namespace Tobii
{
    /// <summary>
    /// There is an elephant in the room. A large problem with the way 
    /// that we write software. 
    /// 
    /// We are breaking the DRY and SRP principles of SOLID badly with 
    /// most every function we write. 
    /// 
    /// Would you create a new List data type every time
    /// you needed one? 
    /// No? 
    /// 
    /// Then why do you do it with algorithms?
    /// 
    /// We do it again and again, numb from years of doing it because we had 
    /// no good alternative. 
    /// 
    /// NOW WE DO!
    /// 
    /// </summary>
    [TestFixture]
    public class TheElephantInTheRoom
    {
        #region The wrong way

        /// <summary>
        /// This is about how I usually see code like this.
        /// Unless the coder is afraid of recursion and makes 
        /// it far uglier still that is!
        /// </summary>
        private static long SizeOfFolderClassic(string folder)
        {
            long result = 0;
            string[] subFolders = Directory.GetDirectories(folder);
            foreach (var subFolder in subFolders)
            {
                result += SizeOfFolderClassic(subFolder);
            }

            string[] files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                result += new FileInfo(file).Length;
            }

            return result;
        }

        #endregion

        #region And what's wrong with that?

        // Recursive tree walking, 
        // transforming data, and data aggregation 
        // is all baked into one big mess.
        //
        // Code should do one thing at a time in clear and easy to read steps.        
        //        
        // Wheels reinvented above: 
        //  Recursive tree walk
        //  Transforming data
        //  Flattening data
        //  Aggregating data
        //
        //In terms of SOLID this translates into violations of;
        //1.SRP since the function is supposed to calculate the size of 
        //a folder but takes upon itself to implement the algorithms above.
        //
        //2. DRY since you are reinventing the wheel time and time again.
        //

        #endregion

        #region A better way.

        private static long SizeOfFolderSane(string folder)
        {
            return folder.FlattenHierarchy(Directory.GetDirectories)
                .SelectMany(dir => Directory.GetFiles(dir))
                .Sum(file => new FileInfo(file).Length);
        }

        #endregion

        #region Yes it works

        [Test]
        public void BothReturnNonSeroSizesThatAreTheSame()
        {
            var folder = @"C:\temp";
            var sizeClassic = SizeOfFolderClassic(folder);
            var sizeSane = SizeOfFolderSane(folder);

            Assert.That(sizeClassic, Is.EqualTo(sizeSane));
            Console.WriteLine(sizeSane); //Compare with total commander...            
        }

        #endregion
    }
}