using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cship_8
{
    class Interpreter
    {
        int t;
        byte[] V, memory;
        public ushort[] screen;
        public int[] keys = new int[16];
        ushort I, pc, opcode;
        Stack<ushort> stack;
        uint stimer, dtimer;
        bool playsound = false;
        public enum ScreenState { CLEAR, DRAW, IDLE }
        public ScreenState screenState;

        public Interpreter()
        {
            pc = 0x200;
            opcode = 0;
            I = (char)0;
            memory = new byte[0x1000];
            V = new byte[16];
            stack = new Stack<ushort>();
            screen = new ushort[64 * 32];
            screenState = ScreenState.IDLE;
            LoadFile("../../Games/PONG");

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = (char)0;
            }
        }

        public void Cycle(chip8Form F)
        {
            opcode = (ushort)(memory[pc] << 8 | memory[pc + 1]);
            Debug.WriteLine("0x" + opcode.ToString("X"));
            // Opcode instruction interpreting
            switch (opcode & 0xF000)
            {
                case 0x0000:
                    switch (opcode & 0x00FF)
                    {
                        case 0x00E0:
                            screenState = ScreenState.CLEAR;
                            F.Invalidate();
                            break;
                        case 0x00EE:
                            pc = stack.Pop();
                            return;
                    }
                    break;
                case 0x1000:
                    pc = (ushort)(opcode & 0x0FFF);
                    break;
                case 0x2000:
                    stack.Push(pc);
                    pc = (ushort)(opcode & 0x0FFF);
                    break;
                case 0x3000:
                    if ((opcode & 0x0F00) >> 8 == (opcode & 0x00FF))
                    {
                        pc += 2;
                    }
                    break;
                case 0x4000:
                    if ((opcode & 0x0F00) >> 8 != (opcode & 0x00FF))
                    {
                        pc += 2;
                    }
                    break;
                case 0x5000:
                    if (V[opcode & 0x0F00] >> 8 == V[opcode & 0x00F0] >> 4)
                    {
                        pc += 2;
                    }
                    break;
                case 0x6000:
                    V[(opcode & 0x0F00) >> 8] = (byte)(opcode & 0x00FF);
                    break;
                case 0x7000:
                    V[(opcode & 0x0F00) >> 8] = (byte)((V[(opcode & 0x0F00) >> 8] + opcode) & 0x00FF);
                    break;
                case 0x8000:
                    switch (opcode & 0x000F)
                    {
                        case 0x0000:
                            V[(opcode & 0x0F00) >> 8] = V[(opcode & 0x00F0) >> 4];
                            break;
                        case 0x0001:
                            V[(opcode & 0x0F00) >> 8] |= V[(opcode & 0x00F0) >> 4];
                            break;
                        case 0x0002:
                            V[(opcode & 0x0F00) >> 8] &= V[(opcode & 0x00F0) >> 4];
                            break;
                        case 0x0003:
                            V[(opcode & 0x0F00) >> 8] ^= V[(opcode & 0x00F0) >> 4];
                            break;
                        case 0x0004:
                            t = V[(opcode & 0x0F00) >> 8] + V[(opcode & 0x00F0) >> 4];
                            V[(opcode & 0x0F00) >> 8] = (byte)(t & 0x00FF);
                            V[0x000F] = (byte)(t & 0x0F00);
                            break;
                        case 0x0005:
                            t = V[(opcode & 0x0F00) >> 8] - V[(opcode & 0x00F0) >> 4];
                            if (t < 0)
                            {
                                t += 0x00FF;
                                V[0x000F] = (byte)0x1;
                            }
                            V[(opcode & 0x0F00) >> 8] = (byte)(t & 0x00FF);
                            break;
                        case 0x0006:
                            V[0x000F] = (byte)(V[(opcode & 0x0F00) >> 8] & 0x0001);
                            V[(opcode & 0x0F00) >> 8] >>= 1;
                            break;
                        case 0x0007:
                            t = V[(opcode & 0x00F0) >> 4] - V[(opcode & 0x0F00) >> 8];
                            if (t < 0)
                            {
                                t += 0x00FF;
                                V[0x000F] = (byte)0x0001;
                            }
                            V[(opcode & 0x0F00) >> 8] = (byte)(t & 0x00FF);
                            break;
                        case 0x000E:
                            V[0x000F] = (byte)((V[(opcode & 0x0F00) >> 8] & 0x0080) >> 7);
                            V[(opcode & 0x0F00) >> 8] <<= 1;
                            break;
                    }
                    break;
                case 0x9000:
                    if (V[opcode & 0x0F00] >> 8 != V[opcode & 0x00F0] >> 4)
                    {
                        pc += 2;
                    }
                    break;
                case 0xA000:
                    I = (char)(opcode & 0x0FFF);
                    break;
                case 0xB000:
                    pc = (ushort)(opcode & 0x0FFF + V[0]);
                    return;
                case 0xC000:
                    int r = new Random().Next(255);
                    V[(opcode & 0x0F00) >> 8] = (byte)(new Random().Next(255) & (opcode & 0x00FF));
                    break;
                case 0xD000:
                    ushort X = (ushort)((opcode & 0x0F00) >> 8);
                    ushort Y = (ushort)((opcode & 0x00F0) >> 4);
                    ushort data;

                    for (int row = 0; row < (opcode & 0x000F); row++)
                    {
                        data = memory[I + row];
                        for (int col = 0; col < 8; col++)
                        {
                            if ((data & (0x80 >> col)) != 0)
                            {
                                if (screen[col + V[X] + ((row + V[Y]) * 64)] == 1)
                                {
                                    V[0xF] = 1;
                                }
                                screen[col + V[X] + ((row + V[Y]) * 64)] ^= (char)1;
                            }
                        }
                    }
                    screenState = ScreenState.DRAW;
                    F.Invalidate();
                    break;
                case 0xE000:
                    switch (opcode & 0x00FF)
                    {
                        case 0x009E:
                            if (keys[(opcode & 0x0F00) >> 8] == 1)
                            {
                                pc += 2;
                            }
                            break;
                        case 0x00A1:
                            if (keys[(opcode & 0x0F00) >> 8] == 0)
                            {
                                pc += 2;
                            }
                            break;
                    }
                    break;
                case 0xF000:
                    switch (opcode & 0x00FF)
                    {
                        case 0x0007:
                            V[(opcode & 0x0F00) >> 8] = (byte)dtimer;
                            break;
                        case 0x000A:
                            bool wait = true;
                            while (wait)
                            {
                                for (int i = 0; i < keys.Length; i++)
                                {
                                    if (keys[i] == 1)
                                    {
                                        wait = false;
                                        V[(opcode & 0x0F00) >> 8] = (byte)i;
                                        break;
                                    }
                                }
                            }
                            break;
                        case 0x0015:
                            dtimer = V[(opcode & 0x0F00) >> 8];
                            break;
                        case 0x0018:
                            stimer = V[(opcode & 0x0F00) >> 8];
                            break;
                        case 0x001E:
                            I += V[(opcode & 0x0F00) >> 8];
                            break;
                        case 0x0029:
                            I = (char)(V[(opcode & 0x0F00) >> 8] * 0x5);
                            break;
                        case 0x0033:
                            t = V[(opcode & 0x0F00) >> 8];
                            memory[I] = (byte)(t / 100);
                            memory[I + 1] = (byte)(t / 10);
                            memory[I + 2] = (byte)((t % 100) % 10);
                            break;
                        case 0x0055:
                            for (int i = 0; i < ((opcode & 0x0F00) >> 8); i++)
                            {
                                memory[I + i] = V[i];
                            }
                            break;
                        case 0x0065:
                            for (int i = 0; i < ((opcode & 0x0F00) >> 8); i++)
                            {
                                V[i] = memory[I + i];
                            }
                            break;
                    }
                    break;
                default:
                    throw new System.ArgumentException("Unknown opcode: 0x" + opcode.ToString("X"));
            }
            // Each opcode is 2 bytes
            pc += 2;

            // Delay timer
            if (dtimer > 0)
            {
                dtimer--;
            }

            // Sound timer
            if (stimer > 0)
            {
                if (stimer == 1)
                {
                    playsound = true;
                }
                stimer--;
            }
        }

        public void LoadFile(string filename)
        {
            byte[] loadedFile = File.ReadAllBytes(filename);
            loadedFile.CopyTo(memory, 0x200);
        }
    }
}
