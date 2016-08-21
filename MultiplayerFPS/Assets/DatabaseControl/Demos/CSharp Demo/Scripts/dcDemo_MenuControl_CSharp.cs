//This script controls the UI in the Database Control (Free) demo scene
//It uses database control to login, register and send and recieve data

using UnityEngine;
using System; //allows string.Split to be used with SplitStringOptions.none
using System.Collections;
using DatabaseControl;//This line is always needed for any C# script using the database control requests. See PDF documentation for more information
//use 'import DatabaseControl;' if you are using JS

public class dcDemo_MenuControl_CSharp : MonoBehaviour {
	////These variables are set in the Inspector:
	
	//they are enabled and disabled to show and hide the different parts of the UI
	public GameObject login_object;
	public GameObject register_object;
	public GameObject data_object;
	public GameObject loading_object;
	
	//these are the login input fields:
	public UnityEngine.UI.InputField input_login_username;
	public UnityEngine.UI.InputField input_login_password;
	
	//these are the register input fields:
	public UnityEngine.UI.InputField input_register_username;
	public UnityEngine.UI.InputField input_register_password;
	public UnityEngine.UI.InputField input_register_confirmPassword;
	
	//this is the send data input field:
	public UnityEngine.UI.InputField input_data_sendData;
	
	//red error UI Texts:
	public UnityEngine.UI.Text login_error;
	public UnityEngine.UI.Text register_error;
	public UnityEngine.UI.Text sendData_error;
	
	//logged in username and data text:
	public UnityEngine.UI.Text data_usernameText;
	public UnityEngine.UI.Text data_gotDataText;
	
	
	
	////These variables cannot be set in the Inspector:
	
	//the part of UI currently being shown
	// 0 = login, 1 = register, 2 = logged in, 3 = loading
	int part = 0;
	//scene starts showing login
	
	string loggedIn_Username = ""; //stores username once logged in
	string loggedIn_Password = ""; //stores password once logged in

	bool isDatabaseSetup = true;

	void Start () {

		//this checks whether the database is setup. It is used to prevent errors for users who try to use the demos
		//without having setup a database.
		//You don't need to use this bool as it will work without it as long as the database has been setup
		TextAsset datafile = Resources.Load ("data") as TextAsset;
		string[] splitdatafile = datafile.text.Split (new string[] { "-" }, StringSplitOptions.None);
		if (splitdatafile [0] == "0") {
			isDatabaseSetup = false;
			Debug.Log ("These demos will not work out of the box. You need to setup a database first for it to work. Please read the Setup section of the PDF for more information");
		} else {
			isDatabaseSetup = true;
		}

		//sets error Texts string to blank
		login_error.text = "";
		register_error.text = "";
		sendData_error.text = "";
		data_gotDataText.text = "";
	}

	void Update () {

		if (isDatabaseSetup == true) {

			//enables and disables the defferent objects to show correct part
			if (part == 0) {
				login_object.gameObject.SetActive (true);
				register_object.gameObject.SetActive (false);
				data_object.gameObject.SetActive (false);
				loading_object.gameObject.SetActive (false);
			}
			if (part == 1) {
				login_object.gameObject.SetActive (false);
				register_object.gameObject.SetActive (true);
				data_object.gameObject.SetActive (false);
				loading_object.gameObject.SetActive (false);
			}
			if (part == 2) {
				login_object.gameObject.SetActive (false);
				register_object.gameObject.SetActive (false);
				data_object.gameObject.SetActive (true);
				loading_object.gameObject.SetActive (false);
			}
			if (part == 3) {
				login_object.gameObject.SetActive (false);
				register_object.gameObject.SetActive (false);
				data_object.gameObject.SetActive (false);
				loading_object.gameObject.SetActive (true);
			}
		
			//sets logged in username text to the username
			data_usernameText.text = loggedIn_Username;
		
			if ((part == 0) || (part == 1)) { //blank the recieved data text if Login or Register UI is being diplayed
				data_gotDataText.text = "";
			}

		}
		
	}

	void blankErrors () {
		//blanks all error texts when part is changed e.g. login > Register
		login_error.text = "";
		register_error.text = "";
		sendData_error.text = "";
	}
	
	public void login_Register_Button () { //called when the 'Register' button on the login part is pressed
		part = 1; //show register UI
		blankErrors();
	}
	
	public void register_Back_Button () { //called when the 'Back' button on the register part is pressed
		part = 0; //goes back to showing login UI
		blankErrors();
	}
	
	public void data_LogOut_Button () { //called when the 'Log Out' button on the data part is pressed
		part = 0; //goes back to showing login UI
		loggedIn_Username = "";
		loggedIn_Password = "";
		blankErrors();
	}

	public void login_login_Button () { //called when the 'Login' button on the login part is pressed

		if (isDatabaseSetup == true) {
		
			//check fields aren't blank
			if ((input_login_username.text != "") && (input_login_password.text != "")) {
			
				//check fields don't contain '-' (if they do, login request will return with error and take longer)
				if ((input_login_username.text.Contains ("-")) || (input_login_password.text.Contains ("-"))) {
					//string contains "-" so return error
					login_error.text = "Unsupported Symbol '-'";
					input_login_password.text = ""; //blank password field
				} else {
					//ready to send request
					StartCoroutine (sendLoginRequest (input_login_username.text, input_login_password.text)); //calls function to send login request
					part = 3; //show 'loading...'
				}
			
			} else {
				//one of the fields is blank so return error
				login_error.text = "Field Blank!";
				input_login_password.text = ""; //blank password field
			}
		
		}
		
	}
	
	IEnumerator sendLoginRequest (string username, string password) {

		if (isDatabaseSetup == true) {
		
			IEnumerator e = DC.Login (username, password);
			while (e.MoveNext()) {
				yield return e.Current;
			}
			WWW returned = e.Current as WWW;
			if (returned.text == "Success") {
				//Password was correct
				blankErrors ();
				part = 2; //show logged in UI
			
				//blank username field
				input_login_username.text = ""; //password field is blanked at the end of this function, even when error is returned
			
				//set logged in username and password to variables
				loggedIn_Username = username;
				loggedIn_Password = password;
			}
			if (returned.text == "incorrectUser") {
				//Account with username not found in database
				login_error.text = "Username not found";
				part = 0; //back to login UI
			}
			if (returned.text == "incorrectPass") {
				//Account with username found, but password incorrect
				part = 0; //back to login UI
				login_error.text = "Incorrect Password";
			}
			if (returned.text == "ContainsUnsupportedSymbol") {
				//One of the parameters contained a - symbol
				part = 0; //back to login UI
				login_error.text = "Unsupported Symbol '-'";
			}
			if (returned.text == "Error") {
				//Account Not Created, another error occurred
				part = 0; //back to login UI
				login_error.text = "Database Error. Try again later.";
			}
		
			//blank password field
			input_login_password.text = "";

		}
	}

	public void register_register_Button () { //called when the 'Register' button on the register part is pressed

		if (isDatabaseSetup == true) {
		
			//check fields aren't blank
			if ((input_register_username.text != "") && (input_register_password.text != "") && (input_register_confirmPassword.text != "")) {
			
				//check username is longer than 4 characters
				if (input_register_username.text.Length > 4) {
				
					//check password is longer than 6 characters
					if (input_register_password.text.Length > 6) {
					
						//check passwords are the same
						if (input_register_password.text == input_register_confirmPassword.text) {
						
							if ((input_register_username.text.Contains ("-")) || (input_register_password.text.Contains ("-")) || (input_register_confirmPassword.text.Contains ("-"))) {
							
								//string contains "-" so return error
								register_error.text = "Unsupported Symbol '-'";
								input_login_password.text = ""; //blank password field
								input_register_confirmPassword.text = "";
							
							} else {
							
								//ready to send request
								StartCoroutine (sendRegisterRequest (input_register_username.text, input_register_password.text, "Hello World!")); //calls function to send register request
								part = 3; //show 'loading...'
							}
						
						} else {
							//return passwords don't match error
							register_error.text = "Passwords don't match!";
							input_register_password.text = ""; //blank password fields
							input_register_confirmPassword.text = "";
						}
					
					} else {
						//return password too short error
						register_error.text = "Password too Short";
						input_register_password.text = ""; //blank password fields
						input_register_confirmPassword.text = "";
					}
				
				} else {
					//return username too short error
					register_error.text = "Username too Short";
					input_register_password.text = ""; //blank password fields
					input_register_confirmPassword.text = "";
				}
			
			} else {
				//one of the fields is blank so return error
				register_error.text = "Field Blank!";
				input_register_password.text = ""; //blank password fields
				input_register_confirmPassword.text = "";
			}

		}
		
	}
	
	IEnumerator sendRegisterRequest (string username, string password, string data) {

		if (isDatabaseSetup == true) {
		
			IEnumerator ee = DC.RegisterUser(username, password, data);
			while(ee.MoveNext()) {
				yield return ee.Current;
			}
			WWW returnedd = ee.Current as WWW;
			
			if (returnedd.text == "Success") {
				//Account created successfully
				
				blankErrors();
				part = 2; //show logged in UI
				
				//blank username field
				input_register_username.text = ""; //password field is blanked at the end of this function, even when error is returned
				
				//set logged in username and password to variables
				loggedIn_Username = username;
				loggedIn_Password = password;
			}
			if (returnedd.text == "usernameInUse") {
				//Account Not Created due to username being used on another Account
				part = 1;
				register_error.text = "Username Unavailable. Try another.";
			}
			if (returnedd.text == "ContainsUnsupportedSymbol") {
				//Account Not Created as one of the parameters contained a - symbol
				part = 1;
				register_error.text = "Unsupported Symbol '-'";
			}
			if (returnedd.text == "Error") {
				//Account Not Created, another error occurred
				part = 1;
				login_error.text = "Database Error. Try again later.";
			}
			
			input_register_password.text = "";
			input_register_confirmPassword.text = "";

		}
	}

	public void data_sendData_Button () { //called when the 'Send Data' button on the data part is pressed
		if (isDatabaseSetup == true) {
			//ready to send request
			StartCoroutine (sendSendDataRequest (loggedIn_Username, loggedIn_Password, input_data_sendData.text)); //calls function to send: send data request
			part = 3; //show 'loading...'
		}
	}
	
	IEnumerator sendSendDataRequest(string username, string password, string data) {

		if (isDatabaseSetup == true) {
		
			IEnumerator eee = DC.SetUserData (username, password, data);
			while (eee.MoveNext()) {
				yield return eee.Current;
			}
			WWW returneddd = eee.Current as WWW;
			if (returneddd.text == "ContainsUnsupportedSymbol") {
				//One of the parameters contained a - symbol
				Debug.Log ("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
			}
			if (returneddd.text == "Error") {
				//Error occurred. For more information of the error, DC.Login could
				//be used with the same username and password
				Debug.Log ("Data Upload Error: Contains Unsupported Symbol '-'");
			}
			input_data_sendData.text = "";
			part = 2;

		}
	}

	public void data_getData_Button () { //called when the 'Get Data' button on the data part is pressed

		if (isDatabaseSetup == true) {
			//ready to send request
			StartCoroutine (sendGetDataRequest (loggedIn_Username, loggedIn_Password)); //calls function to send get data request
			part = 3; //show 'loading...'
		}
	}
	
	IEnumerator sendGetDataRequest(string username, string password) {

		if (isDatabaseSetup == true) {
		
			IEnumerator eeee = DC.GetUserData (username, password);
			while (eeee.MoveNext()) {
				yield return eeee.Current;
			}
			WWW returnedddd = eeee.Current as WWW;
			if (returnedddd.text == "Error") {
				//Error occurred. For more information of the error, DC.Login could
				//be used with the same username and password
				data_gotDataText.text = "Error";
				Debug.Log ("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
			} else {
				if (returnedddd.text == "ContainsUnsupportedSymbol") {
					//One of the parameters contained a - symbol
					data_gotDataText.text = "Error";
					Debug.Log ("Get Data Error: Contains Unsupported Symbol '-'");
				} else {
					//Data received in returned.text variable
					string DataRecieved = returnedddd.text;
					data_gotDataText.text = DataRecieved;
				}
			}
			part = 2;
		}
	}
}
