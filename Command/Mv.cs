﻿using Tree;
using VirtualTerminal.Error;
using VirtualTerminal.FileSystem;

namespace VirtualTerminal.Command
{
    public class MvCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            if (argc < 3)
            {
                Console.WriteLine(ErrorMessage.ArgLack(argv[0]));
                return;
            }

            Tree<FileNode>?[] file = new Tree<FileNode>?[2];
            byte fileCounter = 0;
            string?[] absolutePath = new string?[2];
            string? fileName;
            bool[] permission;

            Dictionary<string, bool> options = new() { { "r", false }, { "f", false } };

            VT.OptionCheck(ref options, in argv);

            foreach (string arg in argv)
            {
                if (arg == argv[0] || arg.Contains('-') || arg.Contains("--"))
                {
                    continue;
                }

                if (fileCounter + 1 > file.Length)
                {
                    Console.WriteLine(ErrorMessage.ArgLack(argv[0]));
                    return;
                }

                absolutePath[fileCounter] = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);

                if (fileCounter == 0)
                {
                }

                fileCounter++;
            }
        }

        public string Description(bool detail)
        {
            return "mv - 파일이나 폴더 위치를 옮기거나 이름 제지정";
        }
    }
}