using UnityEngine;

public class RainChecker : MonoBehaviour
{
    private ParticleSystem rain;
    //public float _startRainChance;
    //public float _stopRainChance;
    //public float RainChance { get => _startRainChance; set => _startRainChance = Mathf.Clamp(value, 0f, 100f); }
    //public float StopRainChance { get => _stopRainChance; set => _stopRainChance = Mathf.Clamp(value, 0f, 100f); }

    //private void Awake()
    //{
    //    rain = GetComponent<ParticleSystem>();
    //}

    //private void OnEnable()
    //{
    //    TimeManager.Instance.OnTimePass += CheckIfRain;
    //}

    //private void OnDisable()
    //{
    //    TimeManager.Instance.OnTimePass -= CheckIfRain;
    //}

    //public void CheckIfRain(int hours, int minutes)
    //{
    //    if(!GameplayManager.instance.globalConfigs.CanChangeClimate_Active)
    //        return;
        
    //    float random = Random.Range(0f, 100f);

    //    if (!TimeManager.Instance.IsRaining && random < RainChance)
    //    {
    //        DoRain(true);
    //    }
    //    else if (TimeManager.Instance.IsRaining && random < StopRainChance)
    //    {
    //        DoRain(false);
    //    }
    //}

    //public void DoRain(bool play)
    //{
    //    if (!play)
    //    {
    //        rain.Stop();
    //        TimeManager.Instance.IsRaining = false;
    //    }
    //    else
    //    {
    //        rain.Play();
    //        TimeManager.Instance.IsRaining = true;
    //    }
    //}
}