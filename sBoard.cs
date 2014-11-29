using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class sBoard : MonoBehaviour 
{
	//Starting sequence count that simon creates
	public int startingSequenceCount = 4;
	//Simon's sequence
	public List<int> simonSequence;
	//User's sequence
	public List<int> userSequence;
	//Reference to the current sequence text GUI
	public Text sequenceCountText;
	//Is Simon currently animating his sequence
	public bool animatingSequence = false;

	//Reference to Simon's buttons
	private sButton[] buttons;
	
	void Start () 
	{
		//Initialise a new List containing ints (button ID's)
		simonSequence = new List<int> ();
		userSequence = new List<int> ();

		//Initialise Simon's button array reference
		buttons = FindObjectsOfType <sButton> ();

		//Reset game to its defaults
		ResetGame ();

		//Start the gameloop coroutine
		StartCoroutine (GameLoop ());
	}

	public void ResetGame ()
	{
		//Clears Simon's and User's sequence Lists
		simonSequence.Clear ();
		userSequence.Clear ();

		//Sets the GUI text to the starting sequence count
		sequenceCountText.text = startingSequenceCount.ToString ();

		//Adds a new sequence ID to Simon's sequence List
		for (int i = 0; i < startingSequenceCount; i++)
		{
			AddToSimonSequence ();
		}

		//Start Simon's sequence animation
		StartCoroutine (AnimateButtonSequence ());
	}

	public IEnumerator AnimateButtonSequence ()
	{
		//Sets Simon's sequence to animating
		animatingSequence = true;
		foreach (int id in simonSequence)
		{
			//Wait 1 second before animating each button animation
			yield return new WaitForSeconds (1.0f);
			//Get the button that corresponds to the id in Simon's sequence
			sButton button = GetButtonFromID (id);
			//Start the button animation
			StartCoroutine (button.AnimateButton ());
		}
		//Sets Simon's sequence to not animating
		animatingSequence = false;
	}

	public void AddToSimonSequence ()
	{
		//Generate a random ID
		int randomColour = Random.Range (0, buttons.Length);
		//Add the ID to Simon's sequence List
		simonSequence.Add (randomColour);
	}

	public void AddToUserSequence (int id)
	{
		//Checks if we have set the button ID. If not then do NOT add it to the User's sequence
		if (id == -1) return;
		//Add ID to the User's sequence
		userSequence.Add (id);
	}
	
	void Update () 
	{
		//Woo nothing in here
	}

	IEnumerator GameLoop ()
	{
		do
		{
			//Checks if Simon's sequence length is the same as the User's sequence length
			if (EqualSequenceLength ())
			{
				//Checks if the User's sequence is exactly the same as Simon's
				if (CompareSequences ())
				{
					Debug.LogWarning ("Congratulations! You successfully copied simons sequence!");

					//Clear the User's sequence because we have the correct sequence
					ClearUserSequence ();
					//Add another random ID to Simon's sequence
					AddToSimonSequence ();

					//Update the GUI text to correspond to the amount of ID's in Simon's sequence
					sequenceCountText.text = simonSequence.Count.ToString ();

					//Wait 1.5 seconds
					yield return new WaitForSeconds (1.5f);

					//Start animating Simon's new squence
					StartCoroutine (AnimateButtonSequence ());
				}
				//The User's sequence doesn't match Simon's sequence
				else
				{
					Debug.LogWarning ("Epic Fail! You are fucking retarded!");

					//Reset the game values
					ResetGame ();
					//Wait 1.5 seconds
					yield return new WaitForSeconds (1.5f);
				}
			}
			//Check if the user has clicked more buttons than Simon's sequence has in it
			else if (userSequence.Count > simonSequence.Count)
			{
				//Wait 1.5 seconds
				yield return new WaitForSeconds (1.5f);
				//Reset the game values
				ResetGame ();
			}

			//Wait 1 second
			yield return new WaitForSeconds (1.0f);

			//Never ending loop. Does everything above while true (Can be changed to a bool variable to sets to true if the user reaches a certain level
		} while (true);
	}

	public bool CompareSequences ()
	{
		//Iterate through Simon's sequence
		for (int i = 0; i < simonSequence.Count; i++)
		{
			//Get Simon's and User's ID
			int simonID = simonSequence[i];
			int userID = userSequence[i];

			//Check if they are the same
			if (simonID != userID)
				//If not then return false
				return false;
		}
		//If yes then return true
		return true;
	}

	public bool EqualSequenceLength ()
	{
		//Check whether Simon's sequence count is exactly the same as the User's sequence count
		return (simonSequence.Count == userSequence.Count);
	}

	public sButton GetButtonFromID (int id)
	{
		//Iterate over each button in button array
		foreach (sButton b in buttons)
		{
			//If the button ID is the same as the sought ID
			if (b.ID == id)
				//Return the button reference
				return b;
		}
		//If can't find it then return null
		return null;
	}

	public void ClearUserSequence ()
	{
		//Clear the users sequence
		userSequence.Clear ();
	}
}




















