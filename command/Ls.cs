﻿using Tree;
using static FileSystem.FileSystem;

namespace VirtualTerminal.Commands
{
    public class LsCommand : VirtualTerminal.ICommand
    {
        public void Execute(string[] args, VirtualTerminal VT)
        {
            Dictionary<string, bool> options = new(){
                { "l", false }
            };

            Tree<FileNode>? file;
            List<Tree<FileNode>>? fileChildren;
            string[]? path;
            string? absolutePath;

            foreach (string arg in args)
            {
                // -- 옵션을 위한 코드
                /*if(temp.Contains("--")) {
                    options[temp.Replace("--", "")] = true;
                }else */
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

            fileChildren = VT.pwdNode?.GetChildren();

            foreach (string arg in args)
            {
                if (arg != args[0] && !arg.Contains('-') && !arg.Contains("--"))
                {
                    absolutePath = VT.fileSystem.GetAbsolutePath(arg, VT.HOME, VT.PWD);
                    path = absolutePath.Split('/');

                    file = VT.fileSystem.FindFile(absolutePath, VT.root);

                    if (file == null)
                    {
                        Console.WriteLine($"ls: cannot access '{arg}': No such file or directory");
                        return;
                    }

                    if (file.Data.FileType != FileType.D)
                    {
                        Console.WriteLine($"{arg}: Not a directory");
                        return;
                    }

                    fileChildren = file.GetChildren();
                }
            }

            if (fileChildren == null)
            {
                return;
            }

            foreach (Tree<FileNode> fileChild in fileChildren)
            {
                if (options["l"])
                {
                    string permissions = VT.fileSystem.ConvertPermissionsToString(fileChild.Data.Permission);
                    Console.Write($"{Convert.ToChar(fileChild.Data.FileType)}{permissions} {fileChild.Data.UID} ");
                }
                
                
                if(fileChild?.Data.FileType == FileType.D)
                {
                    VT.WriteColoredText($"{fileChild?.Data.Name}", ConsoleColor.Blue);
                }
                else
                {
                    Console.Write(fileChild?.Data.Name);
                }

                Console.WriteLine();
            }
        }
    }
}