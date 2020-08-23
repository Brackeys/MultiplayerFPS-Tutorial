#pragma strict
import DatabaseControl; // << Remember to add this reference to your scripts which use DatabaseControl

//All public variables bellow are assigned in the Inspector

//These are the GameObjects which are parents of groups of UI elements. The objects are enabled and disabled to show and hide the UI elements.
public var loginParent : GameObject;
public var registerParent : GameObject;;
public var loggedInParent : GameObject;;
public var loadingParent : GameObject;;

//These are all the InputFields which we need in order to get the entered usernames, passwords, etc
public var Login_UsernameField : UI.InputField;
public var Login_PasswordField : UI.InputField;
public var Register_UsernameField : UI.InputField;
public var Register_PasswordField : UI.InputField;
public var Register_ConfirmPasswordField : UI.InputField;
public var LoggedIn_DataInputField : UI.InputField;
public var LoggedIn_DataOutputField : UI.InputField;

//These are the UI Texts which display errors
public var Login_ErrorText : UI.Text;
public var Register_ErrorText : UI.Text;

//This UI Text displays the username once logged in. It shows it in the form "Logged In As: " + username
public var LoggedIn_DisplayUsernameText : UI.Text;

//These store the username and password of the player when they have logged in
private var playerUsername = "";
private var playerPassword = "";

//Called at the very start of the game
function Awake()
{
    ResetAllUIElements();
}

//Called by Button Pressed Methods to Reset UI Fields
function ResetAllUIElements ()
{
    //This resets all of the UI elements. It clears all the strings in the input fields and any errors being displayed
    Login_UsernameField.text = "";
    Login_PasswordField.text = "";
    Register_UsernameField.text = "";
    Register_PasswordField.text = "";
    Register_ConfirmPasswordField.text = "";
    LoggedIn_DataInputField.text = "";
    LoggedIn_DataOutputField.text = "";
    Login_ErrorText.text = "";
    Register_ErrorText.text = "";
    LoggedIn_DisplayUsernameText.text = "";
}

//Called by Button Pressed Methods. These use DatabaseControl namespace to communicate with server.
function LoginUser ()
{
    var e = DCF.Login(playerUsername, playerPassword); // << Send request to login, providing username and password
    while (e.MoveNext()) {
        yield e.Current;
    }
    var response : String = e.Current.ToString(); // << The returned string from the request

    if (response == "Success")
    {
        //Username and Password were correct. Stop showing 'Loading...' and show the LoggedIn UI. And set the text to display the username.
        ResetAllUIElements();
        loadingParent.gameObject.SetActive(false);
        loggedInParent.gameObject.SetActive(true);
        LoggedIn_DisplayUsernameText.text = "Logged In As: " + playerUsername;
    } else
    {
        //Something went wrong logging in. Stop showing 'Loading...' and go back to LoginUI
        loadingParent.gameObject.SetActive(false);
        loginParent.gameObject.SetActive(true);
        if (response == "UserError")
        {
            //The Username was wrong so display relevent error message
            Login_ErrorText.text = "Error: Username not Found";
        } else
        {
            if (response == "PassError")
            {
                //The Password was wrong so display relevent error message
                Login_ErrorText.text = "Error: Password Incorrect";
            } else
            {
                //There was another error. This error message should never appear, but is here just in case.
                Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
            }
        }
    }
}
function RegisterUser()
{
    var e = DCF.RegisterUser(playerUsername, playerPassword, "Hello World"); // << Send request to register a new user, providing submitted username and password. It also provides an initial value for the data string on the account, which is "Hello World".
    while (e.MoveNext())
    {
        yield e.Current;
    }
    var response : String = e.Current.ToString(); // << The returned string from the request

    if (response == "Success")
    {
        //Username and Password were valid. Account has been created. Stop showing 'Loading...' and show the loggedIn UI and set text to display the username.
        ResetAllUIElements();
        loadingParent.gameObject.SetActive(false);
        loggedInParent.gameObject.SetActive(true);
        LoggedIn_DisplayUsernameText.text = "Logged In As: " + playerUsername;
    } else
    {
        //Something went wrong logging in. Stop showing 'Loading...' and go back to RegisterUI
        loadingParent.gameObject.SetActive(false);
        registerParent.gameObject.SetActive(true);
        if (response == "UserError")
        {
            //The username has already been taken. Player needs to choose another. Shows error message.
            Register_ErrorText.text = "Error: Username Already Taken";
        } else
        {
            //There was another error. This error message should never appear, but is here just in case.
            Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
        }
    }
}
function GetData ()
{
    var e = DCF.GetUserData(playerUsername, playerPassword); // << Send request to get the player's data string. Provides the username and password
    while (e.MoveNext())
    {
        yield e.Current;
    }
    var response : String = e.Current.ToString(); // << The returned string from the request

    if (response == "Error")
    {
        //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
        ResetAllUIElements();
        playerUsername = "";
        playerPassword = "";
        loginParent.gameObject.SetActive(true);
        loadingParent.gameObject.SetActive(false);
        Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
    }
    else
    {
        //The player's data was retrieved. Goes back to loggedIn UI and displays the retrieved data in the InputField
        loadingParent.gameObject.SetActive(false);
        loggedInParent.gameObject.SetActive(true);
        LoggedIn_DataOutputField.text = response;
    }
}
function SetData (data)
{
    var e = DCF.SetUserData(playerUsername, playerPassword, data.ToString()); // << Send request to set the player's data string. Provides the username, password and new data string
	while (e.MoveNext())
	{
		yield e.Current;
	}
	var response : String = e.Current.ToString(); // << The returned string from the request

	if (response == "Success")
	{
		//The data string was set correctly. Goes back to LoggedIn UI
		loadingParent.gameObject.SetActive(false);
		loggedInParent.gameObject.SetActive(true);
	}
	else
	{
		//There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
		ResetAllUIElements();
		playerUsername = "";
		playerPassword = "";
		loginParent.gameObject.SetActive(true);
		loadingParent.gameObject.SetActive(false);
		Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
	}
}

//UI Button Pressed Methods
public function Login_LoginButtonPressed ()
{
    //Called when player presses button to Login

    //Get the username and password the player entered
    playerUsername = Login_UsernameField.text;
    playerPassword = Login_PasswordField.text;

    //Check the lengths of the username and password. (If they are wrong, we might as well show an error now instead of waiting for the request to the server)
    if (playerUsername.Length > 3)
    {
        if (playerPassword.Length > 5)
        {
            //Username and password seem reasonable. Change UI to 'Loading...'. Start the Coroutine which tries to log the player in.
            loginParent.gameObject.SetActive(false);
            loadingParent.gameObject.SetActive(true);
            LoginUser();
        }
        else
        {
            //Password too short so it must be wrong
            Login_ErrorText.text = "Error: Password Incorrect";
        }
    } else
    {
        //Username too short so it must be wrong
        Login_ErrorText.text = "Error: Username Incorrect";
    }
}
public function Login_RegisterButtonPressed ()
{
    //Called when the player hits register on the Login UI, so switches to the Register UI
    ResetAllUIElements();
    loginParent.gameObject.SetActive(false);
    registerParent.gameObject.SetActive(true);
}
public function Register_RegisterButtonPressed ()
{
    //Called when the player presses the button to register

    //Get the username and password and repeated password the player entered
    playerUsername = Register_UsernameField.text;
    playerPassword = Register_PasswordField.text;
    var confirmedPassword : String = Register_ConfirmPasswordField.text;

    //Make sure username and password are long enough
    if (playerUsername.Length > 3)
    {
        if (playerPassword.Length > 5)
        {
            //Check the two passwords entered match
            if (playerPassword == confirmedPassword)
            {
                //Username and passwords seem reasonable. Switch to 'Loading...' and start the coroutine to try and register an account on the server
                registerParent.gameObject.SetActive(false);
                loadingParent.gameObject.SetActive(true);
                RegisterUser();
            }
            else
            {
                //Passwords don't match, show error
                Register_ErrorText.text = "Error: Password's don't Match";
            }
        }
        else
        {
            //Password too short so show error
            Register_ErrorText.text = "Error: Password too Short";
        }
    }
    else
    {
        //Username too short so show error
        Register_ErrorText.text = "Error: Username too Short";
    }
}
public function Register_BackButtonPressed ()
{
    //Called when the player presses the 'Back' button on the register UI. Switches back to the Login UI
    ResetAllUIElements();
    loginParent.gameObject.SetActive(true);
    registerParent.gameObject.SetActive(false);
}
public function LoggedIn_SaveDataButtonPressed ()
{
    //Called when the player hits 'Set Data' to change the data string on their account. Switches UI to 'Loading...' and starts coroutine to set the players data string on the server
    loadingParent.gameObject.SetActive(true);
    loggedInParent.gameObject.SetActive(false);
    SetData(LoggedIn_DataInputField.text);
}
public function LoggedIn_LoadDataButtonPressed ()
{
    //Called when the player hits 'Get Data' to retrieve the data string on their account. Switches UI to 'Loading...' and starts coroutine to get the players data string from the server
    loadingParent.gameObject.SetActive(true);
    loggedInParent.gameObject.SetActive(false);
    GetData();
}
public function LoggedIn_LogoutButtonPressed ()
{
    //Called when the player hits the 'Logout' button. Switches back to Login UI and forgets the player's username and password.
    //Note: Database Control doesn't use sessions, so no request to the server is needed here to end a session.
    ResetAllUIElements();
    playerUsername = "";
    playerPassword = "";
    loginParent.gameObject.SetActive(true);
    loggedInParent.gameObject.SetActive(false);
}