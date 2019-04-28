using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option {
	public string text = "";
	public string next = "";
}

public class DialogManagerUI : MonoBehaviour {

	public GameObject OptionPanel;
	public GameObject OptionName;
	public GameObject OptionText;
	public GameObject [] Options;
	public GameObject [] OptionsButtons;
	private Option [] currentOptions = new Option [4];
	private Option selectedOption;

	public GameObject TextPanel;
	public GameObject TextName;
	public GameObject TextText;

	public GameObject EventPanel;
	public GameObject EventText;

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
	public void SetOptions(Option option0, Option option1 = null, Option option2 = null, Option option3 = null) {
		DisableAllPanels();
		OptionPanel.SetActive(true);

		OptionsButtons [0].SetActive(option0 != null);
		OptionsButtons [1].SetActive(option1 != null);
		OptionsButtons [2].SetActive(option2 != null);
		OptionsButtons [3].SetActive(option3 != null);

		Options [0].GetComponent<Text>().text = option0.text;
		currentOptions [0] = option0;
		if (option1 != null) {
			currentOptions [1] = option1;
			Options [1].GetComponent<Text>().text = option1.text;
		}
		if (option2 != null) {
			currentOptions [2] = option1;
			Options [2].GetComponent<Text>().text = option2.text;
		}
		if (option3 != null) {
			currentOptions [3] = option1;
			Options [3].GetComponent<Text>().text = option3.text;
		}
	}

	// Dialog text, e.g. "Hello there!"
	public void SetText(string text, string name = null) {
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
		DisableAllPanels();
		EventPanel.SetActive(true);
		EventText.GetComponent<Text>().text = text;
	}

	public void OptionButtonPressed(int index) {
		DisableAllPanels();
		// hook this up into the Dialog system as the callback
		Debug.Log("You pressed button " + index);
		Debug.Log("Text: " + currentOptions [index].text);
		Debug.Log("Next: " + currentOptions [index].next);

		selectedOption = currentOptions [index];
	}

	// null is valid response
	public Option GetCurrentSelectedOption() {
		return selectedOption;
	}

	public void ExampleSetOptions() {
		Option option0 = new Option();
		option0.text = "Let's Go!!";
		option0.next = "id-1234-5678-9123";
		Option option1 = new Option();
		option1.text = "Go away!";
		option1.next = "id-ABCD-EFGH-IJKL";
		SetOptions(option0, option1);
	}

	public void ExampleSetText() {
		SetText("Hello there!","Old Frog");
	}

	public void ExampleSetEvent() {
		SetEvent("You picked up Old Frog Card!");
	}

}
