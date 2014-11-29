using UnityEngine;
using System.Collections;

public class sButton : MonoBehaviour 
{
	//Reference to activated sprite (used for switching/animating sprite)
	public Sprite activatedSprite;
	//Buttons ID (-1 = none)
	public int ID = -1;
	//Reference to an audio clip that will be played on button press
	public AudioClip buttonPressSound;

	//Reference to SpriteRenderer that is attached to the button game object
	private SpriteRenderer spriteRenderer;
	//Reference to the de-activated sprite (Set the de-activated sprite as the default sprite)
	private Sprite deactivatedSprite;
	//Reference to the board object
	private sBoard board;
	//Reference to the buttons Audio Source
	private AudioSource audioSource;


	void Awake () 
	{
		//Get the SpriteRenderer attached to this game object
		spriteRenderer = GetComponent <SpriteRenderer> ();
		//Get the de-activated sprite (current default sprite) attached to the SpriteRenderer
		deactivatedSprite = spriteRenderer.sprite;
		//Get the object that has an sBoard script attached to it (should only be one)
		board = FindObjectOfType <sBoard> ();
		//Get AudioSource that is attached to this button game object
		audioSource = GetComponent <AudioSource> ();
	}

	void OnMouseDown ()
	{
		//Checks whether the board is currently animating Simon's sequence
		if (!board.animatingSequence)
		{
			//Add this buttons ID to the User sequence List in board
			board.AddToUserSequence (ID);
			//Start the button animation
			StartCoroutine (AnimateButton ());
		}
	}

	public IEnumerator AnimateButton ()
	{
		//Set the current sprite to the activated sprite
		spriteRenderer.sprite = activatedSprite;
		//Play the buttons pressed sound through the Audio source attached to this game object
		audioSource.PlayOneShot (buttonPressSound);
		//Wait .5 seconds
		yield return new WaitForSeconds (0.5f);
		//Set the current sprite to the de-activated sprite
		spriteRenderer.sprite = deactivatedSprite;
	}
}
