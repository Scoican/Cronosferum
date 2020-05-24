using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Slider timeSlider;

	public void ChangeTimeScale()
	{
		Time.timeScale = timeSlider.value;
	}
}