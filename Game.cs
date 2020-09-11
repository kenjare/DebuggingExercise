using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld
{
    //Create a turn based PvP game. It should have a battle loop where both players
    //must fight until one is dead. The game should allow players to upgrade their stats
    //using items. Both players and items should be defined as structs. 

    struct Player
    {
        public string _playerName;
        public int _playerHealth;
        public int _playerDamage;
        public int _playerDefense;
    }

    struct item
    {
        public string name;
        public int damage;
        public int health;
        public int statBoost;
    }
    class Game
    {
        bool _gameOver = false;
        Player player1;
        Player player2;
        public int levelScaleMax;
        item Chair;

        //Run the game
        public void Run()
        {
            Start();

            while (_gameOver == false)
            {
                Update();
            }

            End();

        }
        //This function handles the battles for our ladder. roomNum is used to update the our opponent to be the enemy in the current room. 
        //turnCount is used to keep track of how many turns it took the player to beat the enemy
        bool StartBattle(int roomNum, ref int turnCount)
        {
            //initialize default enemy stats
            int enemyHealth = 0;
            int enemyAttack = 0;
            int enemyDefense = 0;
            string enemyName = "";

            //Changes the enemy's default stats based on our current room number. 
            //This is how we make it seem as if the player is fighting different enemies
            switch (roomNum)
            {
                case 0:
                    {
                        enemyHealth = 100;
                        enemyAttack = 15;
                        enemyDefense = 76;
                        enemyName = " Mother Spider Chair";
                        break;
                    }
                case 1:
                    {
                        enemyHealth = 80;
                        enemyAttack = 30;
                        enemyDefense = 5;
                        enemyName = " Father Lawn Chair";
                        break;
                    }
                case 2:
                    {

                        enemyHealth = 200;
                        enemyAttack = 40;
                        enemyDefense = 10;
                        enemyName = "Plastic green Chair";
                        break;
                    }
            }

            //Loops until the player or the enemy is dead
            while (player1._playerHealth > 0 && enemyHealth > 0)
            {
                //Displays the stats for both charactersa to the screen before the player takes their turn
                PrintStats(player1._playerName, player1._playerHealth, player1._playerDamage, player1._playerDefense);
                PrintStats(enemyName, enemyHealth, enemyAttack, enemyDefense);

                //Get input from the player
                char input;
                GetInput(out input, "Swing chair leg", "Fold");
                //If input is 1, the player wants to attack. By default the enemy blocks any incoming attack
                if (input == '1')
                {
                    BlockAttack(ref enemyHealth, player1._playerDamage, enemyDefense);
                    Console.Clear();
                    Console.WriteLine("You have swung your chair leg dealing" + (player1._playerDamage - enemyDefense) + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    Console.Clear();

                    //After the player attacks, the enemy takes its turn. Since the player decided not to defend, the block attack function is not called.
                    player1._playerHealth -= enemyAttack;
                    Console.WriteLine(enemyName + " Front Flip kicks with both legs dealing " + enemyAttack + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    turnCount++;
                }
                //If the player decides to defend the enemy just takes their turn. However this time the block attack function is
                //called instead of simply decrementing the health by the enemy's attack value.
                else
                {
                    BlockAttack(ref player1._playerHealth, enemyAttack, player1._playerDefense);
                    Console.WriteLine(enemyName + " dealt " + (enemyAttack - player1._playerDefense) + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    turnCount++;
                    Console.Clear();
                }


            }
            //Return whether or not our player died
            return player1._playerHealth != 0;

        }
        //Decrements the health of a character. The attack value is subtracted by that character's defense
        void BlockAttack(ref int opponentHealth, int attackVal, int opponentDefense)
        {
            int damage = attackVal - opponentDefense;
            if (damage < 0)
            {
                damage = 0;
            }
            opponentHealth -= damage;
        }
        //Scales up the player's stats based on the amount of turns it took in the last battle


        void UpgradeStats(int turnCount)
        {
            //Subtract the amount of turns from our maximum level scale to get our current level scale
            int scale = levelScaleMax - turnCount;
            if (scale <= 0)
            {
                scale = 1;
            }
            player1._playerHealth += 10 * scale;
            player1._playerDamage *= scale;
            player1._playerDefense *= scale;
        }
        //Gets input from the player
        //Out's the char variable given. This variables stores the player's input choice.
        //The parameters option1 and option 2 displays the players current chpices to the screen
        void GetInput(out char input, string option1, string option2, string query)
        {
            Console.WriteLine(query);
            //Initialize input
            input = ' ';
            //Loop until the player enters a valid input
            while (input != '1' && input != '2')
            {
                Console.WriteLine("1." + option1);
                Console.WriteLine("2." + option2);
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
            }

        }

        void GetInput(out char input, string option1, string option2)
        {
            //Initialize input
            input = ' ';
            //Loop until the player enters a valid input
            while (input != '1' && input != '2')
            {
                Console.WriteLine("1." + option1);
                Console.WriteLine("2." + option2);
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
            }
        }
        //Prints the stats given in the parameter list to the console
        void PrintStats(string name, int health, int damage, int defense)
        {
            Console.WriteLine("\n" + name);
            Console.WriteLine("Health: " + health);
            Console.WriteLine("Damage: " + damage);
            Console.WriteLine("Defense: " + defense);
        }

        void PrintStats(Player player)
        {
            Console.WriteLine("\n" + player._playerHealth);
            Console.WriteLine("Health: " + player._playerHealth);
            Console.WriteLine("Damage: " + player._playerDamage);
            Console.WriteLine("Defense: " + player._playerDefense);
        }

        //This is used to progress through our game. A recursive function meant to switch the rooms and start the battles inside them.
        void ClimbLadder(int roomNum)
        {
            //Displays context based on which room the player is in
            switch (roomNum)
            {
                case 0:
                    {
                        Console.WriteLine("A Mother Spider Chair blocks your path");
                        break;
                    }
                case 1:
                    {
                        Console.WriteLine("A Father Lawn Chair stands before you");
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("A giant has appeared!");
                        break;
                    }
                default:
                    {
                        _gameOver = true;
                        return;
                    }
            }
            int turnCount = 0;
            //Starts a battle. If the player survived the battle, level them up and then proceed to the next room.
            if (StartBattle(roomNum, ref turnCount))
            {
                UpgradeStats(turnCount);
                ClimbLadder(roomNum + 1);
                Console.Clear();
            }
            _gameOver = true;

        }

        //Displays the character selection menu. 
        void SelectCharacter()
        {
            char input = ' ';
            //Loops until a valid option is choosen
            while (input != '1' && input != '2' && input != '3')
            {
                //Prints options
                Console.WriteLine("Welcome! Please select a character.");
                Console.WriteLine("1.Sir Kibble");
                Console.WriteLine("2.Gnojoel");
                Console.WriteLine("3.Joedazz");
                Console.Write("> ");
                input = Console.ReadKey().KeyChar;
                //Sets the players default stats based on which character was picked
                switch (input)
                {
                    case '1':
                        {
                            player1._playerName = "Sir Kibble";
                            player1._playerHealth = 120;
                            player1._playerDefense = 10;
                            player1._playerDamage = 2000000;
                            break;
                        }
                    case '2':
                        {
                            player1._playerName = "Gnojoel";
                            player1._playerHealth = 40;
                            player1._playerDefense = 2;
                            player1._playerDamage = 70;
                            break;
                        }
                    case '3':
                        {
                            player1._playerName = "Joedazz";
                            player1._playerHealth = 200;
                            player1._playerDefense = 5;
                            player1._playerDamage = 25;
                            break;
                        }
                    //If an invalid input is selected display and input message and input over again.
                    default:
                        {
                            Console.WriteLine("Invalid input. Press any key to continue.");
                            Console.Write("> ");
                            Console.ReadKey();
                            break;
                        }
                }
                Console.Clear();
            }
            //Prints the stats of the choosen character to the screen before the game begins to give the player visual feedback
            PrintStats(player1._playerName, player1._playerHealth, player1._playerDamage, player1._playerDefense);
            Console.WriteLine("Press any key to continue.");
            Console.Write("> ");
            Console.ReadKey();
            Console.Clear();
        }

        void EquipWeapon()
        {
            Console.WriteLine("Welcome to the Chair 1v1 Super Tournament");
            Console.WriteLine("Press space to contiue");
            Console.ReadKey();

            char input;
            GetInput(out input, "lawn Chair", "Folding Chair", "Player one choose Choose your Chair");
            if (input == '1')
            {
                player1._playerDamage += 76;
            }
            else
            {
                player1._playerDamage = 85;
            }
            Console.Clear();
            Console.WriteLine("Welcome Player two ");
            GetInput(out input, "Lawn Chair", "Folding Chair", "Player two choose a chair");
            if (input == '1')
            {
                player2._playerDamage = 87;
            }
            else
            {
                player2._playerDamage = 110;
            }
        }

        void BeginMultiplayerBattle()
        {
            //Loops until the player or the enemy is dead
            while (player1._playerHealth > 0 && player2._playerHealth > 0)
            {
                //Displays the stats for both charactersa to the screen before the player takes their turn
                PrintStats(player1);
                PrintStats(player2);

                //Get input from the player
                char input;
                GetInput(out input, "Swing Chair", "Defend", "Player one turn");
                //If input is 1, the player wants to attack. By default the enemy blocks any incoming attack
                if (input == '1')
                {
                    BlockAttack(ref player2._playerHealth, player1._playerDamage, player2._playerDefense);
                    Console.Clear();
                    Console.WriteLine("You dealt " + (player1._playerDamage - player2._playerDefense) + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    Console.Clear();
                }
                //If the player decides to defend the enemy just takes their turn. However this time the block attack function is
                //called instead of simply decrementing the health by the enemy's attack value.
                else
                {
                    BlockAttack(ref player1._playerHealth, player2._playerDamage, player1._playerDefense);
                    Console.WriteLine(player2._playerName + " dealt " + (player2._playerDamage - player1._playerDefense) + " damage.");
                    Console.Write("> ");
                    Console.WriteLine("Player one it is your turn");
                    Console.ReadKey();
                    Console.Clear();
                }
                Console.Clear();
                if (player2._playerHealth <= 0)
                {
                    break;
                }
                //Displays the stats for both characters to the screen before the player takes their turn
                PrintStats(player1);
                PrintStats(player2);

                GetInput(out input, "Attack", "Defend", "Player two turn");
                //If input is 1, the player wants to attack. By default the enemy blocks any incoming attack
                if (input == '1')
                {
                    BlockAttack(ref player1._playerHealth, player2._playerDamage, player1._playerDefense);
                    Console.Clear();
                    Console.WriteLine("You dealt " + (player2._playerDamage - player1._playerDefense) + " damage.");
                    Console.Write("> ");
                    Console.WriteLine("Player to its your turn");
                    Console.ReadKey();
                    Console.Clear();
                }
                //If the player decides to defend the enemy just takes their turn. However this time the block attack function is
                //called instead of simply decrementing the health by the enemy's attack value.
                else
                {
                    BlockAttack(ref player2._playerHealth, player1._playerDamage, player2._playerDefense);
                    Console.WriteLine(player2._playerName + " dealt " + (player1._playerDamage - player2._playerDefense) + " damage.");
                    Console.Write("> ");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            _gameOver = true;
        }
        void InitializeCharacters()
        {
            player1._playerName = "Player1";
            player1._playerDefense = 10;
            player1._playerHealth = 100;
            player2._playerName = "Player2";
            player2._playerDefense = 10;
            player2._playerHealth = 100;
        }

        //Performed once when the game begins
        public void Start()
        {
            InitializeCharacters();
        }

        public void GetModeChoice()
        {
            char input;
            GetInput(out input, "Single Player", "Multiplayer", "Choose a mode");
            if (input == '1')
            {
                Console.Clear();
                SelectCharacter();
                ClimbLadder(0);
            }
            else
            {
                Console.Clear();
                EquipWeapon();
                BeginMultiplayerBattle();
            }
        }

        //Repeated until the game ends
        public void Update()
        {
            GetModeChoice();
        }

        //Performed once when the game ends
        public void End()
        {
            //If the player died print death message
            if (player1._playerHealth <= 0)
            {
                Console.WriteLine("You Are the supreme Chair MAy all who stand sit in your presence ");
                return;
            }
            //Print game over message
            Console.WriteLine("I mean good job for ruining player 2's day 1");
        }
    }
}