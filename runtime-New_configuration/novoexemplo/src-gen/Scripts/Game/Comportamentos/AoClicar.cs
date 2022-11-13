using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
		
public class AoClicar : MonoBehaviour
{
	public void AoClicarContinuarIntro()
	{
		SceneManager.LoadScene("TelaQ1");
	}
	public void AoClicarNovoJogo()
	{
		SceneManager.LoadScene("TelaIntro");
	}
	public void AoClicarSair()
	{
		Application.Quit();
	}
	
	public void Call(string methodReference)
	{
		switch(methodReference)
		{
			case ("AoClicarContinuarIntro"):
				AoClicarContinuarIntro();
				break;
			case ("AoClicarNovoJogo"):
				AoClicarNovoJogo();
				break;
			case ("AoClicarSair"):
				AoClicarSair();
				break;
		}
	}
}
