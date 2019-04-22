using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace flickrFix
{
    class Program
    {
        public static void Main()
        {
            // Path to Folder
            const string startFolder = @"F:\wdrive\z";
            const string path = @"C:\Users\daron\Desktop\flickrList.txt";

            // Take a snapshot of the file system.  
            IEnumerable<FileInfo> fileList = GetFiles(startFolder);

            List<string> flickrFileMatches = new List<string>();

            // Create the regular expression to find all things "flickr".  
            System.Text.RegularExpressions.Regex searchTerm =
                new System.Text.RegularExpressions.Regex(@"(?i)(flickr)(?-i)");

            
                var queryMatchingFiles =
                    from file in fileList
                    let fName = file.FullName
                    where (file.Extension == ".php" ||
                            file.Extension == ".html" ||
                            file.Extension == ".htm") &&
                            (!fName.Contains("wp-"))

                    let fileText = File.ReadAllText(file.FullName) // HERE 
                    let matches = searchTerm.Matches(fileText)
                    where matches.Count > 0
                    select new
                    {
                        fileLoc = file
                    };

                // Execute the query.  


                foreach (var val in queryMatchingFiles)
                {

                    // Gets file location as string
                    string fn = val.fileLoc.ToString();

                    // Change to a, b, etc.
                    string folderToFindFlickrIn = "\\z";

                    // trims to just the website name
                    string folder = fn.Substring(fn.IndexOf(folderToFindFlickrIn) + 1);
                
                    string withoutSubFolder = folder.Substring(0, folder.LastIndexOf("\\"));

                    flickrFileMatches.Add(withoutSubFolder);
                }

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e + ":::: " + e.Message + " END MSG!");
            //}

            string[] flickrSites = flickrFileMatches.Distinct().ToArray();


            for (int i = 0; i <flickrSites.Length; i++)
            {
                string appendText = flickrSites[i] + Environment.NewLine;
                File.AppendAllText(path, appendText);
            }

            // Keep the console window open in debug mode  
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        // This method assumes that the application has discovery   
        // permissions for all folders under the specified path.  
        static IEnumerable<FileInfo> GetFiles(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException();

            string[] fileNames = null;
            List<FileInfo> files = new List<FileInfo>();

            fileNames = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (string name in fileNames)
            {
                files.Add(new FileInfo(name));
            }
            return files;
        }
    }
}
