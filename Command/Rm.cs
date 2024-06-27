﻿using Tree;
using VirtualTerminal.Error;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Command
{
    public class RmCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Tree<FileNode>? file;

            string[]? splitPath;
            string? absolutePath;

            bool[] permission;
            // bool[] parentPermission;

            Dictionary<string, bool> options = new(){
                { "r", false },
                // { "f", false } // 사용 용도 고민중
            };

            foreach (string arg in args)
            {
                if (arg.Contains('-'))
                {
                    foreach (char c in arg)
                    {
                        if (c != '-')
                        {
                            options[c.ToString()] = true;
                        }
                    }
                }
            }

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    splitPath = absolutePath.Split('/');

                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if(file == null){
                        Console.WriteLine(ErrorMessage.NoSuchForD(args[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    permission = VT.fileSystem.CheckFilePermission(VT.USER, file, VT.root);

                    if(!permission[0] || !permission[1] || !permission[2]){
                        Console.WriteLine(ErrorMessage.PermissionDenied(args[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    if(!options["r"] && file.Data.FileType == FileType.D){
                        Console.WriteLine(ErrorMessage.NotF(args[0], ErrorMessage.DefaultErrorComment(arg)));
                        return;
                    }

                    if(options["r"]){
                        VT.fileSystem.RemoveFile(absolutePath, VT.root, 'r');
                        return;
                    }

                    VT.fileSystem.RemoveFile(absolutePath, VT.root, null);
                }
            }
        }

        public string Description(bool detail)
        {
            return "rm - 파일이나 디렉터리 삭제";
        }
    }
}