﻿namespace VirtualTerminal.Command
{
    public class EchoCommand : VirtualTerminal.ICommand
    {
        public void Execute(int argc, string[] argv, VirtualTerminal VT)
        {
            foreach (string arg in argv.Skip(1))
            {
                Console.Write(arg + " ");
            }

            Console.WriteLine();
        }

        public string Description(bool detail)
        {
            if (detail)
            {
                return "echo - 입력한 테스트 출력\n";
            }

            return "echo - 입력한 테스트 출력";
        }
    }
}