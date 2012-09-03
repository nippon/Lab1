using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab1.Helpers;
using Lab1.Model.Repository.Abstract;
using Lab1.Model.Repository;

namespace Lab1.Model
{
    /// <summary>
    /// Input Parser ansvarar för att tolka och utföra de kommandon användaren matar in
    /// </summary>
    public class InputParser
    {
       
        private enum State 
        { 
            Exit, Default 
        }
        private IRepository Repo;
        State ParserState = State.Default;
        private Logger LogList = new Logger();
        private bool IsUserAuthenticated = false;

        public InputParser( IRepository Repo ) 
        {
            this.Repo = Repo;
        }

        

        /// <summary>
        /// Sätter ParserState till Exit
        /// </summary>
        private void SetExitParserState()
        {
            ParserState = State.Exit;
        }

        /// <summary>
        /// Returnerar true om ParserState är Exit (eller rättare sagt -1)
        /// </summary>
        public bool IsStateExit
        {
            get
            {
                return ParserState == State.Exit;
            }
        }
        public string ListUsers( List<User> users )
        {
            string result = "";
            foreach ( var user in users )
            {
                result += user.ToString() + "\n";
            }
            return result;
        }
        /// <summary>
        /// Tolka input baserat på vilket tillstånd (ParserState) InputParser-objektet befinner sig i.
        /// </summary>
        /// <param name="input">Input sträng som kommer från användaren.</param>
        /// <returns></returns>
        public string ParseInput(string input)
        {
           
            input = input.ToLower();
            LogList.Log( input );
            if (ParserState == State.Default)
            {
                return ParseDefaultStateInput(input);
            }
            else if (ParserState == State.Exit)
            {
                // Do nothing - program should exit
                return "";
            }
            else
            {
                
                return OutputHelper.ErrorLostState;
            }
        }

        /// <summary>
        /// Tolka och utför de kommandon som ges när InputParser är i Default State
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string ParseDefaultStateInput(string input)
        {
            string result;
            switch (input)
            {
                case "login admin":
                    if (!IsUserAuthenticated)
                    {
                        IsUserAuthenticated = true;
                        result = "You have been logged in as admin.";
                        break;
                    }
                    else 
                    {
                        result = "You are already logged in as admin.";
                    }
                    break;
                case "logout":
                    if (IsUserAuthenticated)
                    {
                        IsUserAuthenticated = false;
                        result = "You have been logged out.";
                        break;
                    }
                    else
                    {
                        result = "You are not logged in.";
                    }
                    break;
                case "list":
                    result = ListUsers( Repo.GetUsers().Take(10).ToList() );
                    break;
                case "dictionary":
                    result = "ICollection<KeyValuePair<TKey, TValue>>";
                    break;
                case "interface": 
                    result = OutputHelper.InterfaceMessage;
                    break;
                case "log":
                    result = LogList.ToString();
                    break;
                case "?": // Inget break; eller return; => ramlar igenom till nästa case (dvs. ?/help hanteras likadant)
                case "help":
                    result = OutputHelper.RootCommandList;
                    break;
                case "exit":
                    SetExitParserState(); // Lägg märke till att vi utför en Action här.
                    result = OutputHelper.ExitMessage("Bye!"); // Det går bra att skicka parametrar
                    break;
                default:
                    result = OutputHelper.ErrorInvalidInput;
                    break;
            }
            return result + OutputHelper.EnterCommand;
        }  
    }
}
