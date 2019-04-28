using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManagerUI : MonoBehaviour {

	public GameObject OptionPanel;
	public GameObject OptionName;
	public GameObject OptionText;
	public GameObject [] Options;
	public GameObject [] OptionsButtons;
	private int selectedOption;

	public GameObject TextPanel;
	public GameObject TextName;
	public GameObject TextText;

	public GameObject EventPanel;
	public GameObject EventText;

	private bool readyForNext = true;

	private void Start() {
		OptionText.GetComponent<Text>().text = "";
		OptionName.GetComponent<Text>().text = "";
		TextText.GetComponent<Text>().text = "";
		TextName.GetComponent<Text>().text = "";
		EventText.GetComponent<Text>().text = "";
		DisableAllPanels();
	}

	// Disable all the panels
	public void DisableAllPanels() {
		OptionPanel.SetActive(false);
		TextPanel.SetActive(false);
		EventPanel.SetActive(false);
	}

	// Option text, e.g. "Give Cigar | Run away"
	public void SetOptions(List<string> options) {
		readyForNext = false;
		DisableAllPanels();
		OptionPanel.SetActive(true);

		Debug.Assert(options.Count <= Options.Length);
		Debug.Assert(OptionsButtons.Length == Options.Length);
		var optionOffset = Options.Length - options.Count; // make sure we start from the bottom;
		for(var i = 0; i < optionOffset; ++i) {
			OptionsButtons[i].SetActive(false);
		}
		for(var i = 0; i < options.Count; ++i) {
			var optIdx = optionOffset + i;
			OptionsButtons[optIdx].SetActive(true);
			Options[optIdx].GetComponent<Text>().text = options[i];
		}
	}

	// Dialog text, e.g. "Hello there!"
	public void SetText(string text, string name = null) {
		readyForNext = false;
		DisableAllPanels();
		TextPanel.SetActive(true);
		OptionText.GetComponent<Text>().text = text;
		TextText.GetComponent<Text>().text = text;
		if(name != null){
			OptionName.GetComponent<Text>().text = name;
			TextName.GetComponent<Text>().text = name;
		}
	}

	// Event Text, e.g. "You picked up Old Frog Card!"
	public void SetEvent(string text){
		readyForNext = false;
		DisableAllPanels();
		EventPanel.SetActive(true);
		EventText.GetComponent<Text>().text = text;
	}

	public void OptionButtonPressed(int index) {
		// hook this up into the Dialog system as the callback
		selectedOption = index;
		DisableAllPanels();
		readyForNext = true;
	}

	public void OKButtonPressed() {
		DisableAllPanels();
		readyForNext = true;
	}

	public bool ReadyForNext() {
		return readyForNext;
	}

	// null is valid response
	public int GetCurrentSelectedOption() {
		return selectedOption;
	}



	public void ExampleSetOptions() {
	}

	public void ExampleSetText() {
		SetText("Hello there!","Old Frog");
	}

	public void ExampleSetEvent() {
		SetEvent("You picked up Old Frog Card!");
	}

}
