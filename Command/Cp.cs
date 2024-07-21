using System.Security.Principal;
using Tree;
using VirtualTerminal.Error;
using static VirtualTerminal.FileSystem.FileSystem;

namespace VirtualTerminal.Command
{
    public class CpCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if(argc < 3){
                Console.WriteLine(ErrorMessage.ArgLack(argv[0]));
                return;
            }

            Tree<FileNode>?[] file = new Tree<FileNode>?[2];
            List<Tree<FileNode>?> tempFile = [];
            byte fileCounter = 0;
            string?[] absolutePath = new string?[2];
            string? fileName;
            bool[] permission;

            Dictionary<string, bool> options = new(){
                { "r", false },
                { "f", false}
            };

            VT.OptionCheck(ref options, in argv);

            foreach (string arg in argv)
            {
                if (arg == argv[0] || arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                if(fileCounter + 1 > file.Length){
                    Console.WriteLine(ErrorMessage.ArgLack(argv[0]));
                    return;
                }

                absolutePath[fileCounter] = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                if(fileCounter == 0)
                {
                    file[fileCounter] = VT.fileSystem.FindFile(absolutePath[fileCounter], VT.root);
                }
                else
                {
                    fileName = absolutePath[fileCounter]?.Split('/')[^1];
                    absolutePath[fileCounter] = absolutePath[fileCounter]?.Replace('/' + fileName, "");
                    file[fileCounter] = VT.fileSystem.FindFile(absolutePath[fileCounter], VT.root);
                }

                fileCounter++;
            }
        }

        public string Description(bool detail)
        {
            return "cp - 파일과 디렉터리 복사";
        }
    }
}