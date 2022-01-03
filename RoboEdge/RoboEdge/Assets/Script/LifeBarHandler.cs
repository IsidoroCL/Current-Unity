using UnityEngine;
using UnityEngine.UI;

public class LifeBarHandler : MonoBehaviour
{
    #region Fields
    private static Image HealthBarImage;
    #endregion
    #region Unity methods
    private void Awake()
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
            SetHealthBarColor(new Color(1, 0, 0, 0.5f));
        }
        else
        {
            SetHealthBarColor(new Color(0, 1, 1, 0.5f));
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
