using UnityEngine;
using UnityEngine.UI;

public class LifeBarHandler : MonoBehaviour
{
    #region Fields
    private static Image HealthBarImage;
    #endregion
    #region Unity methods
    private void Start()
    {
        HealthBarImage = GetComponent<Image>();
    }
    #endregion
    #region Methods
    public static void SetHealthBarValue(float value)
    {
        HealthBarImage.fillAmount = value;
        if (HealthBarImage.fillAmount < 0.2f)
        {
            SetHealthBarColor(Color.red);
        }
        else
        {
            SetHealthBarColor(Color.cyan);
        }
    }

    public static float GetHealthBarValue()
    {
        return HealthBarImage.fillAmount;
    }

    public static void SetHealthBarColor(Color healthColor)
    {
        HealthBarImage.color = healthColor;
    }
    #endregion
    
}
