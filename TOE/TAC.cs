
namespace TOE
{
    class TAC
    {

        private int[,] field = new int[3, 3];
        private bool gameover = false;

        public TAC()
        {
            Reset();
        }

        /// <summary>
        /// resets the field
        /// </summary>
        public void Reset()
        {
            gameover = false;
            for (int x = 0; x < field.GetLength(1); x++)
            {
                for (int y = 0; y < field.GetLength(0); y++)
                {
                    field[x, y] = 0;
                }
            }
        }

        public string ConvertPlayerData(int[] pos)
        {
            switch (field[pos[0],pos[1]])
            {
                case 0:
                    return "";
                case 1:
                    return "X";
                case 2:
                    return "O";
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="player"></param>
        /// <returns>Value to print on Button</returns>
        /// <exception cref="Exception"></exception>
        public void Place(int x, int y, int player)
        {
            if (!gameover)
            {
                if (field[x, y] == 0 && !gameover)
                {
                    field[x, y] = player;
                }
                else if (field[x, y] >= 1 && !gameover)
                {
                    throw new Exception("Field occupied");
                }
                else
                {
                    throw new Exception("Game is over");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>-1: Tie; 0: no one; 1: P1[X]; 2: P2[O]</returns>
        public int CheckWinner()
        {
            //vertical
            
            for (int y = 0; y < field.GetLength(1); y++)
            {
                if (field[y, 0] == field[y, 1]
                    && field[y, 2] == field[y, 0] && field[y,0] != 0)
                {
                    gameover = true;
                    return field[y, 0];
                }
            }

            // horizontal

            for (int x = 0; x < field.GetLength(0); x++)
            {
                if (field[0, x] == field[1, x] 
                    && field[2, x] == field[0, x] && field[0, x] != 0)
                {
                    gameover = true;
                    return field[0, x];
                }
            }

            // Cross
            if (field[1,1] != 0)
            {
                int temp = field[1,1];

                if (temp == field[0, 0]
                    && temp == field[2, 2])
                {
                    gameover = true;
                    return temp; 
                }
                if (temp == field[0,2]
                    && temp == field[2,0])
                {
                    gameover = true;
                    return temp;
                }
            }

            int c = 0;

            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int f = 0; f < field.GetLength(1); f++)
                {
                    if (field[i, f] != 0) c++;
                }
            }

            if (c == 9)
            {
                gameover = true;
                return -1;
            }

            gameover = false;
            return 0;
        }

        private struct PositionValue()
        {
            public int[] pos;
            public int value;
            public bool player;
        }

        private int ConvertGoodBad(int t, bool aiBegins = true)
        {
            switch (t)
            {
                case 1:
                    return aiBegins ? 1 : -1;
                case 2: 
                    return aiBegins ? -1 : 1;
                default:
                    return 0;
            }
        }

        public void AiMove(int player)
        {
            if (!gameover)
            {
                bool max = player == 1;
                PositionValue res = AiMoveCalculation(max);
                CheckWinner();
                Place(res.pos[0], res.pos[1], player);
            }
        }

        public void AiMove(int player, int depth)
        {
            if (!gameover)
            {
                bool max = player == 1;
                PositionValue res = AiMoveCalculation(max, depth);
                CheckWinner();
                Place(res.pos[0], res.pos[1], player);
            }
        }

        private PositionValue AiMoveCalculation(bool maximizingPlayer = true, int depth = int.MaxValue) 
        {
            PositionValue bestMove = new();
            PositionValue currentMove = new();
            bestMove.value = maximizingPlayer ? int.MinValue : int.MaxValue;
            if (depth  == 0 || CheckWinner() != 0)
            {
                return new PositionValue() { pos = [0,0], value = ConvertGoodBad(CheckWinner())};
            }
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (null == bestMove.pos) bestMove.pos = [x, y];
                    if (!IsOccupied([x, y]))
                    {
                        field[x, y] = maximizingPlayer ? 1 : 2;
                        currentMove = AiMoveCalculation(!maximizingPlayer, depth - 1);
                        field[x, y] = 0;

                        if ((currentMove.value > bestMove.value && maximizingPlayer) || (currentMove.value < bestMove.value && !maximizingPlayer))
                        {
                            bestMove = currentMove;
                            bestMove.pos = [x, y];
                        }
                    }
                }
            }
            return bestMove;
        }

        private bool IsOccupied(int[] pos)
        {
            return (field[pos[0],pos[1]] != 0);
        }

        public int GetPlayerInt(int x, int y)
        {
            return field[x,y];
        }
    }
}
