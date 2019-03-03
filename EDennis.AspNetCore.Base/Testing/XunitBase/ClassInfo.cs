using System.IO;
using System.Linq;
using System.Reflection;

namespace EDennis.AspNetCore.Base.Testing {

    public class ClassInfo<TClass>
        where TClass : class {

        public string ClassName { get; }
        public string Namespace { get; }
        public string ProjectName { get; }
        public string SolutionName { get; private set; }

        public string ProjectDirectory { get; }
        public string SolutionDirectory { get; private set; }

        private FileInfo _solutionFile;

        public ClassInfo() {
            ClassName = typeof(TClass).Name;
            Namespace = typeof(TClass).Namespace;

            _solutionFile = GetSolutionFile();
            SolutionName = _solutionFile.Name.Replace(".sln", "");
            SolutionDirectory = _solutionFile.DirectoryName;

            ProjectName = GetProjectName();
            ProjectDirectory = GetProjectDirectory();
        }


        private string GetProjectDirectory() {

            var solutionFileText = File.ReadAllText(_solutionFile.FullName);


            var index = solutionFileText.IndexOf($"\"{ProjectName}\",");
            var start = index + $"\"{ProjectName}\", \"".Length;
            var end = solutionFileText.IndexOf(".csproj\",", start);
            var length = solutionFileText.Substring(start, end - start).LastIndexOf("\\");

            var projectDirectory = solutionFileText.Substring(start, length).Trim();

            projectDirectory = SolutionDirectory.Replace("/", "\\") + "\\" + projectDirectory;

            return projectDirectory;
        }


        private string GetProjectName() {
            var projectName = Assembly.GetAssembly(typeof(TClass)).FullName;
            projectName = projectName.Substring(0, projectName.IndexOf(',')).TrimEnd();
            return projectName;
        }



        /// <summary>
        /// Gets the solution file in a parent (or ancestor) directory
        /// </summary>
        /// <param name="currentPath">optional current path</param>
        /// <returns></returns>
        ///from https://stackoverflow.com/a/35824406/10896865
        private FileInfo GetSolutionFile([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "") {

            var thisFile = new FileInfo(sourceFilePath);
            var directory = thisFile.Directory;
            while (directory != null && !directory.GetFiles("*.sln").Any()) {
                directory = directory.Parent;
            }
            return directory.GetFiles("*.sln").FirstOrDefault();
        }


    }

}