﻿using Tree;
using VirtualTerminal.Commands;
using static FileSystem.FileSystem;

namespace VirtualTerminal
{
    public partial class VirtualTerminal
    {
        internal FileSystem.FileSystem fileSystem = new();
        internal Tree<FileNode> root;
        internal Tree<FileNode>? pwdNode;
        internal Tree<FileNode>? homeNode;

        internal string PWD;
        // private List<string> PWD;
        // private List<FileNode> PWD;
        internal string HOME;
        internal string USER;

        internal Dictionary<string, ICommand> commandMap;

        public VirtualTerminal()
        {
            USER = "user";
            PWD = $"/home/{USER}";
            HOME = $"/home/{USER}";

            root = new Tree<FileNode>(new FileNode("/", "root", 0b111101, FileType.D));
            fileSystem.CreateFile("/", new FileNode("home", "root", 0b111101, FileType.D), root);
            fileSystem.CreateFile("/", new FileNode("root", "root", 0b111000, FileType.D), root);

            homeNode = fileSystem.CreateFile("/home", new FileNode(USER, USER, 0b111101, FileType.D), root);
            
            fileSystem.CreateFile(HOME, new FileNode("Item", "root", 0b111101, FileType.D), root);
            fileSystem.CreateFile(HOME, new FileNode($"Hello_{USER}.txt", "root", 0b111111, FileType.F, $"Hello, {USER}!"), root);

            pwdNode = fileSystem.FindFile(PWD, root);

            if (homeNode == null)
            {
                Console.WriteLine($"homeNode err");
                Environment.Exit(0);
            }

            if (pwdNode == null)
            {
                Console.WriteLine("pwdNode err");
                Environment.Exit(0);
            }

            // 명령어와 메서드를 매핑하는 사전 초기화
            commandMap = new Dictionary<string, ICommand>
            {
                { "cat", new CatCommand() },
                { "cd", new CdCommand() },
                { "chmod", new ChmodCommand() }, // 제작 전
                { "clear", new ClearCommand() },
                { "cp", new CpCommand() }, // 제작 전
                { "date", new DateCommand() },
                { "echo", new EchoCommand() },
                { "exit", new ExitCommand() },
                { "help", new HelpCommand() },
                { "ls", new LsCommand() },
                { "man", new ManCommand() },
                { "mkdir", new MkdirCommand() },
                { "mv", new MvCommand() }, // 제작 전
                { "pwd", new PwdCommand() },
                { "rm", new RmCommand() }, // 제작 전
                { "rmdir", new RmdirCommand() },
                { "whoami", new WhoamiCommand() }
            };
        }
        
        public void Run()
        {
            while (true)
            {
                DisplayPrompt();
                string? command = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }

                ProcessCommand(command);
            }
        }

        internal interface ICommand
        {
            void Execute(string[] args, VirtualTerminal VT);
            //String Description();
        }

        private void DisplayPrompt()
        {
            WriteColoredText("\x1b[1muser\x1b[22m", ConsoleColor.Green);
            WriteColoredText(":", Console.ForegroundColor);
            WriteColoredText($"\x1b[1m{PWD}\x1b[22m", ConsoleColor.Blue);
            WriteColoredText("$ ", Console.ForegroundColor);
        }

        private void ProcessCommand(string command)
        {
            string[] args = command.Split(' ');

            if (commandMap.TryGetValue(args[0], out var action))
            {
                action.Execute(args, this);
            }
            else
            {
                Console.WriteLine($"Command not found: {args[0]}");
            }
        }

        internal string ReadMultiLineInput()
        {
            string content = string.Empty;
            string? line;
            while ((line = Console.ReadLine()) != ".")
            {
                content += line + Environment.NewLine;
            }
            return content.TrimEnd('\n');
        }

        internal void WriteColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
